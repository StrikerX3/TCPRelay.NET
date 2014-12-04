using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TCPRelayCommon
{
    public class TCPRelay
    {
        public int ListenPort;

        public string TargetHost;
        public int TargetPort;

        public int ConnectTimeout;
        public int SocketBufferSize;

        private bool Running;

        public bool IsRunning
        {
            get { return Running; }
            private set { }
        }

        private TcpListener svr;

        public readonly List<IConnectionListener> Listeners = new List<IConnectionListener>();

        public readonly List<Connection> Connections = new List<Connection>();

        public TCPRelay()
        {
            TargetHost = "live.justin.tv";
            TargetPort = 1935;

            ListenPort = 1935;

            ConnectTimeout = 15;
            SocketBufferSize = 32 * 1024;
        }

        private void SetCultures(params Thread[] ts)
        {
            foreach(Thread t in ts)
            {
                t.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                t.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            }
        }

        public void Start()
        {
            Thread t = new Thread(this.Run);
            SetCultures(t);
            t.Start();
        }

        public void Run()
        {
            Thread.CurrentThread.Name = "TCPRelay Listener [" + ListenPort + "]";
            try
            {
                svr = new TcpListener(IPAddress.Any, ListenPort);
                svr.Start();

                Running = true;
                Listeners.ForEach((listener) => listener.Started(this));
                while (Running)
                {
                    TcpClient sSrc = null;
                    try
                    {
                        sSrc = svr.AcceptTcpClient();

                        Listeners. ForEach((listener) => listener.ConnectionAttempt(this, sSrc.Client.RemoteEndPoint as IPEndPoint, TargetHost, TargetPort));
                        TcpClient sTarget = CreateTcpClient(TargetHost, TargetPort, ConnectTimeout, SocketBufferSize);
                        Connection c = new Connection(sSrc, sTarget, TargetHost, TargetPort);
                        Connections.Add(c);
                        Listeners.ForEach((listener) => listener.ConnectionAccepted(this, c));

                        Thread t1 = new Thread(new Pipe(sSrc, sTarget, c, this).Run);
                        Thread t2 = new Thread(new Pipe(sTarget, sSrc, c, this).Run);
                        SetCultures(t1, t2);
                        t1.Start();
                        t2.Start();
                    }
                    catch (SocketException e)
                    {
                        if (e.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            Listeners.ForEach((listener) => listener.ConnectionRefused(this, sSrc == null ? null : sSrc.Client.RemoteEndPoint as IPEndPoint, TargetHost, TargetPort, e));
                        }
                        else
                        {
                            if (e.SocketErrorCode != SocketError.Interrupted)
                            {
                                Listeners.ForEach((listener) => listener.ConnectionFailed(this, sSrc == null ? null : sSrc.Client.RemoteEndPoint as IPEndPoint, TargetHost, TargetPort, e));
                            }
                            Stop();
                        }
                        if (sSrc != null) sSrc.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Listeners.ForEach((listener) => listener.StartFailed(this, e));
            }
            finally
            {
                Listeners.ForEach((listener) => listener.Stopped(this));
            }
        }

        private TcpClient CreateTcpClient(string TargetHost, int TargetPort, int timeoutSeconds, int bufferSize)
        {
            TcpClient tcp = new TcpClient();
            // apparently setting a large buffer causes dropped frames
            //tcp.SendBufferSize = bufferSize;
            //tcp.ReceiveBufferSize = bufferSize;
            IAsyncResult ar = tcp.BeginConnect(TargetHost, TargetPort, null, null);
            WaitHandle wh = ar.AsyncWaitHandle;
            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeoutSeconds), false))
                {
                    tcp.Close();
                    throw new TimeoutException();
                }

                tcp.EndConnect(ar);
            }
            finally
            {
                wh.Close();
            }  
            return tcp;
        }

        public void Stop()
        {
            Running = false;
            if (svr != null) svr.Stop();
            lock (Connections)
            {
                Connections.ForEach((c) =>
                {
                    c.Close();
                    Listeners.ForEach((listener) => listener.ConnectionClosed(this, c));
                });
                Connections.Clear();
            }
        }
    }


    class Pipe
    {
        private TcpClient src;
        private TcpClient dest;
        private Connection c;
        private TCPRelay relay;

        public Pipe(TcpClient src, TcpClient dest, Connection c, TCPRelay relay)
        {
            this.src = src;
            this.dest = dest;
            this.c = c;
            this.relay = relay;
        }

        public void Run()
        {
            IPEndPoint srcEndPoint = src.Client.RemoteEndPoint as IPEndPoint;
            IPEndPoint destEndPoint = dest.Client.RemoteEndPoint as IPEndPoint;
            Thread.CurrentThread.Name = "TCPRelay Pipe Thread [" + srcEndPoint + " - " + destEndPoint + "]";
            relay.Listeners.ForEach((listener) => listener.OpenPipe(relay, c, srcEndPoint, destEndPoint));

            byte[] buf = new byte[relay.SocketBufferSize];
            try
            {
                do
                {
                    if (!src.GetStream().CanRead) break;
                    int len = src.GetStream().Read(buf, 0, buf.Length);
                    if (!dest.GetStream().CanWrite) break;
                    dest.GetStream().Write(buf, 0, len);
                    if (len == 0) break;
                    if (srcEndPoint == c.SrcEndPoint)
                    {
                        c.IncSrcReadCount((ulong)len);
                        c.IncDstWriteCount((ulong)len);
                    }
                    else
                    {
                        c.IncDstReadCount((ulong)len);
                        c.IncSrcWriteCount((ulong)len);
                    }
                    //Print("Transferred " + len + " bytes");
                }
                while (src.Connected);
            }
            catch (System.IO.IOException e)
            {
                relay.Listeners.ForEach((listener) => listener.PipeFailure(relay, c, srcEndPoint, destEndPoint, e));
            }
            catch (InvalidOperationException e)
            {
                relay.Listeners.ForEach((listener) => listener.PipeFailure(relay, c, srcEndPoint, destEndPoint, e));
            }
            c.Close();
            lock (relay.Connections)
            {
                bool closed = relay.Connections.Remove(c);
                relay.Listeners.ForEach((listener) =>
                {
                    if (closed) listener.ConnectionClosed(relay, c);
                    listener.ClosePipe(relay, c, srcEndPoint, destEndPoint);
                });
            }
        }
    }
}
