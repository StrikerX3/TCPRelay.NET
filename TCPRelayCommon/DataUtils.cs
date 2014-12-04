using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPRelayCommon
{
    public sealed class DataUtils
    {
        private DataUtils() { }

        private static readonly string[] ByteUnits = { "bytes", "KB"  , "MB"  , "GB"  , "TB"   };
        private static readonly string[]  BitUnits = { "bits" , "Kbit", "Mbit", "Gbit", "Tbit" };

        private static readonly string[] ByteSpeedUnits = { "bytes/s", "KB/s", "MB/s", "GB/s", "TB/s" };
        private static readonly string[]  BitSpeedUnits = { "bps"    , "Kbps", "Mbps", "Gbps", "Tbps" };

        public static string      ToByteUnits(ulong  amount) { return Reduce(amount    , "byte",      ByteUnits); }
        public static string       ToBitUnits(ulong  amount) { return Reduce(amount    , "bit" ,       BitUnits); }
        public static string ToByteSpeedUnits(double amount) { return Reduce(amount * 8, "Bps" , ByteSpeedUnits); }
        public static string  ToBitSpeedUnits(double amount) { return Reduce(amount * 8, "bps" ,  BitSpeedUnits); }

        private static string Reduce(ulong value, string singularUnit, string[] units)
        {
            if (value == 1) return "1 " + singularUnit;
            if (value < 1000) return value + " " + units[0];
            int unit = 0;
            ulong val = value;
            double div = 1;
            while (val >= 1000 && unit < BitSpeedUnits.Length - 1)
            {
                unit++;
                val /= 1024;
                div *= 1024;
            }
            double reducedAmt = value / div;

            return reducedAmt.ToString("#.#") + " " + units[unit];
        }

        private static string Reduce(double value, string singularUnit, string[] units)
        {
            if (value < 1) return "0" + value.ToString("#.#") + " " + singularUnit;
            if (value == 1) return "1 " + singularUnit;
            if (value < 10000) return value.ToString("#.#") + " " + units[0];
            int unit = 0;
            double val = value;
            double div = 1;
            while (val >= 10000 && unit < BitSpeedUnits.Length - 1)
            {
                unit++;
                val /= 1024;
                div *= 1024;
            }
            double reducedAmt = value / div;

            return reducedAmt.ToString("#.#") + " " + units[unit];
        }
    }
}
