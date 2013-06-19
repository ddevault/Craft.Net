using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Craft.Net.Client
{
    internal class NetworkHelper
    {
        internal static IPEndPoint ParseEndPoint(string str)
        {
            IPEndPoint ep;
            if (TryParseEndPoint(str, out ep))
                return ep;
            throw new FormatException();
        }

        private static readonly Regex endPointSplitter = new Regex("^(?<ip>.*):(?<port>[0-9]+)*$");

        internal static bool TryParseEndPoint(string str, out IPEndPoint value)
        {
            value = null;
            if (string.IsNullOrEmpty(str))
                return false;
            var splitResult = endPointSplitter.Match(str);
            if (!splitResult.Success) return false;
            IPAddress adr;
            if (!IPAddress.TryParse(splitResult.Groups["ip"].Value, out adr))
                adr =
                    (Dns.GetHostEntry(splitResult.Groups["ip"].Value)
                        .AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork)).FirstOrDefault();
            if (adr == null)
                return false;
            int port;
            if (!int.TryParse(splitResult.Groups["port"].Value, NumberStyles.None, NumberFormatInfo.CurrentInfo,
                              out port))
                return false;

            value = new IPEndPoint(adr, port);
            return true;
        }
    }
}