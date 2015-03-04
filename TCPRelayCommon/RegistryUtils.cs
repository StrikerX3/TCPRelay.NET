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

        private static object GetValue(string valueName, object defaultValue)
        {
            return GetValue(valueName) ?? defaultValue;
        }

        public static string GetString(string valueName)
        {
            return (string)GetValue(valueName);
        }

        public static int? GetDWord(string valueName)
        {
            return (int?)GetValue(valueName);
        }

        public static int GetDWord(string valueName, int defaultValue)
        {
            return (int)GetValue(valueName, defaultValue);
        }

        public static bool? GetBoolean(string valueName)
        {
            return (bool?)GetValue(valueName);
        }

        public static bool GetBoolean(string valueName, bool defaultValue)
        {
            int? value = GetDWord(valueName);
            return value.HasValue ? value != 0 : defaultValue;
        }


        private static void SetValue(string valueName, object value, RegistryValueKind kind)
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

        public static void SetBoolean(string valueName, bool value)
        {
            SetValue(valueName, value ? 1 : 0, RegistryValueKind.DWord);
        }


        public static void Remove(string valueName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (key != null)
            {
                if (key.GetValue(valueName) != null)
                {
                    key.DeleteValue(valueName);
                }
                key.Close();
            }
        }
    }
}
