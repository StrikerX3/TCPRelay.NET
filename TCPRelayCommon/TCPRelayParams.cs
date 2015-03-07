using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TCPRelayCommon
{
    public class TCPRelayParams
    {
        public int InternalBufferSize;

        // optional overrides
        public int? SendBufferSizeApp;
        public int? SendBufferSizeRemote;
        public int? RecvBufferSizeApp;
        public int? RecvBufferSizeRemote;
        public int? ConnectTimeout;

        public bool NoDelayApp;
        public bool NoDelayRemote;

        public IPAddress BindIP;

        public TCPRelayParams()
        {
            // load settings from registry if available
            InternalBufferSize = RegistryUtils.GetDWord("InternalBufferSize", 64);

            SendBufferSizeApp = RegistryUtils.GetDWord("SendBufferApp");
            SendBufferSizeRemote = RegistryUtils.GetDWord("SendBufferRemote");
            RecvBufferSizeApp = RegistryUtils.GetDWord("ReceiveBufferApp");
            RecvBufferSizeRemote = RegistryUtils.GetDWord("ReceiveBufferRemote");
            ConnectTimeout = RegistryUtils.GetDWord("ConnectTimeoutRemote");

            NoDelayApp = RegistryUtils.GetBoolean("NoDelayApp", false);
            NoDelayRemote = RegistryUtils.GetBoolean("NoDelayRemote", false);

            string bindIPAddr = RegistryUtils.GetString("BindIP");
            if (!IPAddress.TryParse(bindIPAddr, out BindIP))
            {
                BindIP = IPAddress.Any;
            }
        }
    }
}
