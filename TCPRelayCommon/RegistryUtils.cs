using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TCPRelayCommon
{
    public sealed class RegistryUtils
    {
        private const string RegKey = "Software\\StrikerX3\\TCPRelay";

        private RegistryUtils() { }

        private static object GetValue(string valueName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(RegKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
            
            object ret = key.GetValue(valueName);
            key.Close();
            
            return ret;
        }

        public static string GetString(string valueName)
        {
            return (string)GetValue(valueName);
        }

        public static int? GetDWord(string valueName)
        {
            return (int?)GetValue(valueName);
        }



        private static void SetValue(String valueName, object value, RegistryValueKind kind)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (key == null)
            {
                key = Registry.CurrentUser.CreateSubKey(RegKey);
            }
            key.SetValue(valueName, value, kind);
            key.Close();
        }

        public static void SetString(string valueName, string value)
        {
            SetValue(valueName, value, RegistryValueKind.String);
        }

        public static void SetDWord(string valueName, int value)
        {
            SetValue(valueName, value, RegistryValueKind.DWord);
        }
    }
}
