using System;   
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using TCPRelayCommon;
using System.Globalization;

namespace TCPRelayWindow
{
    public static class TCPRelayWindow
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            // Localize to a specific language
            if (args.Length > 0)
            {
                try
                {
                    CultureInfo culture = new CultureInfo(args[0]);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    Application.CurrentCulture = culture;
                }
                catch (CultureNotFoundException)
                {
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TCPRelayForm());
        }
    }
}
