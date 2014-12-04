using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TCPRelayCommon
{
    public class Connection
    {
        private DateTime _StartTime;
        private DateTime _CloseTime;

        private bool _Closed;

        private TcpClient Src;
        private TcpClient Dst;

        private string _SrcHostName;
        private int _SrcPort;
        private string _DstHostName;
        private int _DstPort;

        public IPEndPoint SrcEndPoint { get { return Src.Client.RemoteEndPoint as IPEndPoint; } private set { } }
        public IPEndPoint DstEndPoint { get { return Dst.Client.RemoteEndPoint as IPEndPoint; } private set { } }

        public string SrcHostName { get { return _SrcHostName; } private set { } }
        public int SrcPort { get { return _SrcPort; } private set { } }
        public string DstHostName { get { return _DstHostName; } private set { } }
        public int DstPort { get { return _DstPort; } private set { } }

        private ulong _SrcReadCount, _SrcWriteCount;
        private ulong _DstReadCount, _DstWriteCount;

        public ulong SrcReadCount { get { return _SrcReadCount ; } private set { } }
        public ulong SrcWriteCount{ get { return _SrcWriteCount; } private set { } }
        public ulong DstReadCount { get { return _DstReadCount ; } private set { } }
        public ulong DstWriteCount{ get { return _DstWriteCount; } private set { } }

        public DateTime StartTime { get { return _StartTime; } private set { } }
        public DateTime CloseTime { get { return _Closed ? _CloseTime : DateTime.MaxValue; } private set { } }
        public TimeSpan Uptime    { get { return (_Closed ? _CloseTime : DateTime.Now) - _StartTime; } private set { } }

        public bool Closed { get { return _Closed; } private set { } }

        public Connection(TcpClient src, TcpClient dest, string targetHost, int targetPort)
        {
            this.Src = src;
            this.Dst = dest;
            _SrcHostName = (src.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
            _SrcPort = (src.Client.RemoteEndPoint as IPEndPoint).Port;
            _DstHostName = targetHost;
            _DstPort = targetPort;
            _Closed = false;
            _StartTime = DateTime.Now;
        }

        public void IncSrcReadCount (ulong amount) { _SrcReadCount  += amount; }
        public void IncSrcWriteCount(ulong amount) { _SrcWriteCount += amount; }
        public void IncDstReadCount (ulong amount) { _DstReadCount  += amount; }
        public void IncDstWriteCount(ulong amount) { _DstWriteCount += amount; }

        public void Close()
        {
            if (Src.Connected) { Src.Close(); _Closed = true; }
            if (Dst.Connected) { Dst.Close(); _Closed = true; }
            if (_Closed) { _CloseTime = DateTime.Now; }
        }
    }
}
