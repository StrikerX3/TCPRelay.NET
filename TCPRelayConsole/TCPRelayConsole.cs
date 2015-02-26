using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using TCPRelayCommon;

namespace TCPRelayConsole
{
    static class TCPRelayConsole
    {
        static Uri RtmpUri;
        static bool Debug;

        // TODO localize

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            AssemblyInfoHelper helper = new AssemblyInfoHelper(typeof(TCPRelayConsole));
            Console.WriteLine("TCPRelay v" + helper.AssemblyInformationalVersion);
            Console.WriteLine("  " + helper.Copyright);
            if (args.Length == 0)
            {
                Console.WriteLine("Type \"tcprelay -?\" for instructions.");
            }
            Console.WriteLine();

            // initialize configuration
            TCPRelay relay = new TCPRelay();

            // parse parameters from command line
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                // first, try parsing the parameter as an URL
                try
                {
                    RtmpUri = new Uri(arg);
                    continue;
                }
                catch (UriFormatException)
                {
                    // ignore; don't even log this error.
                }

                // if this did not work, try the other parameters
                if (EqualsIgnoreCase("-h", arg) || EqualsIgnoreCase("-help", arg) || EqualsIgnoreCase("-?", arg))
                {
                    // help
                    PrintHelpAndExit();
                }
                else if (arg.StartsWith("-twitch.tv") || arg.StartsWith("-ttv"))
                {
                    // get twitch.tv ingest server list from http://api.justin.tv/api/ingest/xsplit.xml
                    ListTwitchTvServersAndExit();
                }
                else if (arg.StartsWith("-p:") || arg.StartsWith("-rp:") || arg.StartsWith("-port:") || arg.StartsWith("-relay-port:"))
                {
                    // set relay port
                    relay.ListenPort = ToInt(arg);
                }
                else if (arg.StartsWith("-th:") || arg.StartsWith("-target-host:"))
                {
                    // set target host
                    relay.TargetHost = ToString(arg);
                }
                else if (arg.StartsWith("-tp:") || arg.StartsWith("-target-port:"))
                {
                    // set target port
                    relay.TargetPort = ToInt(arg);
                }
                else if (arg.StartsWith("-sbs:") || arg.StartsWith("-socket-buffer-size:"))
                {
                    // set socket buffer size (for debugging/tweaking purposes)
                    relay.SocketBufferSize = ToInt(arg) * 1024;
                    Console.WriteLine("Socket buffer size set to " + relay.SocketBufferSize / 1024 + " KB");
                }
                else if (EqualsIgnoreCase("-debug", arg))
                {
                    // set debug flag
                    Debug = true;
                    Console.WriteLine("Running in debug mode");
                }
                else
                {
                    Console.WriteLine("Invalid parameter: " + arg);
                    Console.WriteLine();
                    PrintHelpAndExit();
                }
            }

            string localHostName = Dns.GetHostName();
            try
            {
                IPAddress targetAddr = Dns.GetHostEntry(relay.TargetHost).AddressList[0];
                bool isLoopback = IPAddress.IsLoopback(targetAddr);
                bool isLocalhost = relay.TargetHost.Equals(localHostName, StringComparison.CurrentCultureIgnoreCase);
                if ((isLoopback || isLocalhost) && relay.TargetPort == relay.ListenPort)
                {
                    Console.WriteLine("Invalid relay target: " + relay.TargetHost + ":" + relay.TargetPort);
                    Console.WriteLine("You should not relay data to the relay server itself!");
                    return;
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.HostNotFound)
                {
                    Console.WriteLine("Unknown host: " + relay.TargetHost);
                    DebugTrace(e);
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine(e);
                }
            }

            try
            {
                if (RtmpUri != null)
                {
                    relay.TargetHost = RtmpUri.Host;
                    relay.TargetPort = RtmpUri.Port == -1 ? 1935 : RtmpUri.Port;
                }

                relay.Listeners.Add(new ConsoleConnectionListener());

                relay.Start();

                if (RtmpUri != null)
                {
                    Console.WriteLine("RTMP URL provided: " + RtmpUri);
                    Console.WriteLine("Use the following RTMP URL in XSplit: rtmp://" + localHostName + (relay.ListenPort == 1935 ? "" : ":" + relay.ListenPort) + RtmpUri.PathAndQuery);
                    Console.WriteLine();
                }

                AppDomain.CurrentDomain.ProcessExit += (s, e) =>
                {
                    Console.WriteLine("Shutting down server...");
                    try
                    {
                        relay.Stop();
                    }
                    catch (SocketException ex)
                    {
                        DebugTrace(ex);
                    }
                };
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    Console.WriteLine("Port " + relay.ListenPort + " already in use. Check if TCPRelay is already running.");
                }
            }
        }

        static bool EqualsIgnoreCase(string str1, string str2)
        {
            return str1.Equals(str2, StringComparison.CurrentCultureIgnoreCase);
        }

        public static void DebugTrace(Exception e)
        {
            if (Debug)
            {
                Console.WriteLine(e);
            }
        }

        static string ToString(string arg)
        {
            return arg.Substring(arg.IndexOf(':') + 1);
        }

        static void PrintHelpAndExit()
        {
            Console.WriteLine("Syntax: tcprelay [-h | -help | -?]");
            Console.WriteLine("                 [-twitch.tv | -ttv]");
            Console.WriteLine("                 [-p:<port> | -rp:<port> | -port:<port> | -relay-port:<port>]");
            Console.WriteLine("                 [-th:<host> | -target-host:<host>]");
            Console.WriteLine("                 [-tp:<port> | -target-port:<port>]");
            Console.WriteLine("                 [-debug]");
            Console.WriteLine("                 [<url>]");
            Console.WriteLine();
            Console.WriteLine("  -h | -help | -?");
            Console.WriteLine("      Prints this help");
            Console.WriteLine("  -twitch.tv | -ttv");
            Console.WriteLine("      Lists all available Twitch.tv ingest servers");
            Console.WriteLine("  -p:<port> | -rp:<port> | -port:<port> | -relay-port:<port>");
            Console.WriteLine("      Specifies the port the relay will listen to (default: 1935)");
            Console.WriteLine("  -th:<host> | -target-host:<host>");
            Console.WriteLine("      Specifies the target host address (default: live.justin.tv)");
            Console.WriteLine("  -tp:<port> | -target-port:<port>");
            Console.WriteLine("      Specifies the target port (default: 1935)");
            Console.WriteLine("  -sbs:<size> | -socket-buffer-size:<size>");
            Console.WriteLine("      Specifies the socket send buffer size in KB (default: 8)");
            Console.WriteLine("  -debug");
            Console.WriteLine("      Enters debug mode, which enables additional logging and tracing");
            Console.WriteLine("      (use this if you're having issues with TCPRelay)");
            Console.WriteLine("  <url>");
            Console.WriteLine("      Extracts the target address from an URL (e.g. rtmp://live.justin.tv/app)");
            Console.WriteLine("      The target host:port is the address where all data will be relayed to.");
            Console.WriteLine("      This will override both -th and -tp");
            Console.WriteLine();
            Console.WriteLine("Usage examples:");
            Console.WriteLine("  tcprelay");
            Console.WriteLine("    Starts a relay server listening to port 1935 targeting live.justin.tv");
            Console.WriteLine();
            Console.WriteLine("  tcprelay -target-host:live-jfk.justin.tv -target-port:1935");
            Console.WriteLine("    Starts a relay server listening to port 1935 targeting the New York");
            Console.WriteLine("    Justin.tv server");
            Console.WriteLine();
            Console.WriteLine("  tcprelay rtmp://live.use.own3d.tv/live");
            Console.WriteLine("    Starts a relay server listening to port 1935 targeting the own3d.tv server");

            Environment.Exit(0);
        }

        static void ListTwitchTvServersAndExit()
        {
            Console.WriteLine("Listing Twitch.tv ingest servers...");
            Console.WriteLine();

            try
            {
                List<TwitchTvIngestServerData> servers = TwitchTvIngestServerData.Retrieve();
                servers.Sort();
                
                int maxNameLen = 0;
                servers.ForEach((server) => maxNameLen = Math.Max(maxNameLen, server.Name.Length));
                servers.ForEach((server) => Console.WriteLine(Pad(server.Name, maxNameLen) + " | " + server.Uri));
            }
            catch (WebException e)
            {
                Console.WriteLine("Failed to retrieve data from Twitch.tv: " + e.Message);
                DebugTrace(e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to retrieve Twitch.tv server list: " + e.Message);
                DebugTrace(e);
            }

            Environment.Exit(0);
        }

        private static string Pad(string text, int length)
        {
            string sb = text;
            for (int i = text.Length; i < length; i++)
            {
                sb += ' ';
            }
            return sb;
        }

        private static int ToInt(string arg)
        {
            try
            {
                string str = ToString(arg);
                return Int32.Parse(str);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid parameter: " + arg);
                DebugTrace(e);
                Environment.Exit(0);
            }
            return 0;
        }
    }

    class ConsoleConnectionListener : IConnectionListener
    {
        public void Started(TCPRelay relay)
        {
            Console.WriteLine("Server up at " + Dns.GetHostName() + ":" + relay.ListenPort);
            Console.WriteLine("Relaying to " + relay.TargetHost + ":" + relay.TargetPort);
        }
        public void Stopped(TCPRelay relay) { }

        public void ConnectionAttempt(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort)
        {
            Console.WriteLine("Attempting to connect to " + targetHost + ":" + targetPort + "...");
        }

        public void ConnectionAccepted(TCPRelay relay, Connection c)
        {
            Console.WriteLine("Connection established between " + c.SrcEndPoint + " and " + c.DstEndPoint);
        }

        public void ConnectionClosed(TCPRelay relay, Connection c)
        {
            Console.WriteLine("Connection between " + c.SrcHostName + ":" + c.SrcPort + " and " + c.DstHostName + ":" + c.DstPort + " closed");
        }

        public void ConnectionRefused(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort, SocketException e)
        {
            Console.WriteLine("Could not connect to " + targetHost + ":" + targetPort);
            if (src != null)
            {
                Console.WriteLine("Connection to " + src + " closed");
            }
            TCPRelayConsole.DebugTrace(e);
        }

        public void ConnectionFailed(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort, SocketException e)
        {
            Console.WriteLine("Listener stopped.");
            TCPRelayConsole.DebugTrace(e);
        }

        public void StartFailed(TCPRelay relay, Exception e)
        {
            SocketException se = e as SocketException;
            if (se != null && se.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                Console.WriteLine("Port " + relay.ListenPort + " already in use. Check if TCPRelay is already running.");
            }
        }

        public void OpenPipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest)
        {
            Print("Piping bytes");
        }

        public void ClosePipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest)
        {
            Print("Connection closed");
        }

        public void PipeFailure(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest, Exception e)
        {
            if (!(e is IOException)) Print(e.ToString());
        }

        private void Print(string message)
        {
            Console.WriteLine("[" + Thread.CurrentThread.Name + "] " + message);
        }
    }
}
