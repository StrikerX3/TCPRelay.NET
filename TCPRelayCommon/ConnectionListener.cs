using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using TCPRelayCommon;

namespace TCPRelayCommon
{
    public interface IConnectionListener
    {
        void Started(TCPRelay relay);
        void Stopped(TCPRelay relay);
        void StartFailed(TCPRelay relay, Exception e);

        void ConnectionAttempt(TCPRelay relay, IPEndPoint endPoint, string TargetHost, int TargetPort);
        void ConnectionAccepted(TCPRelay relay, Connection c);
        void ConnectionRefused(TCPRelay relay, IPEndPoint endPoint, string TargetHost, int TargetPort, SocketException e);
        void ConnectionFailed(TCPRelay relay, IPEndPoint endPoint, string TargetHost, int TargetPort, SocketException e);
        void ConnectionClosed(TCPRelay relay, Connection c);

        void OpenPipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest);
        void ClosePipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest);
        void PipeFailure(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest, Exception e);
    }
}
