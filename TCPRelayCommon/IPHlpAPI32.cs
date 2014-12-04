using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace TCPRelayCommon
{
    public class SystemException : Exception
    {
        public readonly uint ErrorCode;

        public SystemException(uint errorCode) : base(GetAPIErrorMessageDescription(errorCode))
        {
            this.ErrorCode = errorCode;
        }

        private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        
        [DllImport("kernel32", SetLastError = true)]
        private static extern int FormatMessage(int flags, IntPtr source, uint messageId,
            int languageId, StringBuilder buffer, int size, IntPtr arguments); 

        private static string GetAPIErrorMessageDescription(uint ApiErrNumber)
        {
            System.Text.StringBuilder sError = new System.Text.StringBuilder(512);
            int lErrorMessageLength = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, (IntPtr)0, ApiErrNumber, 0, sError, sError.Capacity, (IntPtr)0);

            if (lErrorMessageLength > 0)
            {
                string strgError = sError.ToString();
                strgError = strgError.Substring(0, strgError.Length - 2);
                return strgError + " (" + ApiErrNumber.ToString() + ")";
            }
            return "none";
        }
    }

	#region TCP
	public class TcpConnection
	{
		public TcpState State;
		public IPEndPoint LocalEndPoint;
		public IPEndPoint RemoteEndPoint;
		public Process Process;
	}
    #endregion

    #region Wrapper
    [StructLayout(LayoutKind.Sequential)]
    struct MIB_TCPTABLE_OWNER_PID
    {
        public int dwNumEntries;
        public MIB_TCPROW_OWNER_PID[] table;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MIB_TCPROW_OWNER_PID
    {
        public int dwState;
        public uint dwLocalAddr;
        public int dwLocalPort;
        public uint dwRemoteAddr;
        public int dwRemotePort;
        public int dwOwningPid;
    }

    enum TCP_TABLE_CLASS
    {
        TCP_TABLE_BASIC_LISTENER = 0,
        TCP_TABLE_BASIC_CONNECTIONS = 1,
        TCP_TABLE_BASIC_ALL = 2,
        TCP_TABLE_OWNER_PID_LISTENER = 3,
        TCP_TABLE_OWNER_PID_CONNECTIONS = 4,
        TCP_TABLE_OWNER_PID_ALL = 5,
        TCP_TABLE_OWNER_MODULE_LISTENER = 6,
        TCP_TABLE_OWNER_MODULE_CONNECTIONS = 7,
        TCP_TABLE_OWNER_MODULE_ALL = 8
    }

    static class IPHlpAPI32Wrapper
	{
		private const int ERROR_SUCCESS = 0;
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

		private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;

        private const int TCP_TABLE_OWNER_PID_ALL = 5;

        private const int AF_INET = 2;   // IPv4
        private const int AF_INET6 = 10; // IPv6

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwSize, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, int reserved);
        
        public static MIB_TCPROW_OWNER_PID[] GetExtendedTcpTable(bool order)
        {
            IntPtr lpTable = IntPtr.Zero;
            try
            {
                lpTable = Marshal.AllocHGlobal(4 + 6 * 4);
                int dwSize = 4 + 6 * 4;
                uint hRes1 = GetExtendedTcpTable(lpTable, ref dwSize, order, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                if (hRes1 != ERROR_SUCCESS && hRes1 != ERROR_INSUFFICIENT_BUFFER) throw new SystemException(hRes1);
                Marshal.FreeHGlobal(lpTable);

                lpTable = Marshal.AllocHGlobal(dwSize);
                uint hRes2 = GetExtendedTcpTable(lpTable, ref dwSize, order, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                if (hRes2 != ERROR_SUCCESS && hRes2 != ERROR_INSUFFICIENT_BUFFER) throw new SystemException(hRes1);
                IntPtr ptr = lpTable;

                MIB_TCPTABLE_OWNER_PID table;
                table.dwNumEntries = Marshal.ReadInt32(ptr);
                table.table = new MIB_TCPROW_OWNER_PID[table.dwNumEntries];
                ptr += 4;
                for (int i = 0; i < table.dwNumEntries; i++)
                {
                    table.table[i].dwState = Marshal.ReadInt32(ptr);
                    table.table[i].dwLocalAddr = (uint)Marshal.ReadInt32(ptr + 4);
                    table.table[i].dwLocalPort = ReverseBytes16(Marshal.ReadInt32(ptr + 8));
                    table.table[i].dwRemoteAddr = (uint)Marshal.ReadInt32(ptr + 12);
                    table.table[i].dwRemotePort = ReverseBytes16(Marshal.ReadInt32(ptr + 16));
                    table.table[i].dwOwningPid = Marshal.ReadInt32(ptr + 20);

                    ptr += 24;
                }
                Marshal.FreeHGlobal(lpTable);
                lpTable = IntPtr.Zero;
                return table.table;
            }
            finally
            {
                if (lpTable != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpTable);
            }
        }

        private static int ReverseBytes16(int value)
        {
            return ((value & 0xFF) << 8) | ((value & 0xFF00) >> 8);
        }
	}
    #endregion

    public static class TcpConnectionHelper
    {
        public static TcpConnection[] GetTcpConnections()
        {
            MIB_TCPROW_OWNER_PID[] table = IPHlpAPI32Wrapper.GetExtendedTcpTable(false);
            TcpConnection[] connections = new TcpConnection[table.Length];

            for (int i = 0; i < connections.Length; i++)
            {
                try
                {
                    connections[i] = new TcpConnection();
                    connections[i].State = (TcpState)table[i].dwState;
                    connections[i].LocalEndPoint = new IPEndPoint(table[i].dwLocalAddr, table[i].dwLocalPort);
                    connections[i].RemoteEndPoint = new IPEndPoint(table[i].dwRemoteAddr, table[i].dwRemotePort);
                    connections[i].Process = Process.GetProcessById(table[i].dwOwningPid);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return connections;
        }
    }
}

