using System.Net;
using System.Net.NetworkInformation;

namespace WorkerServiceEmail.Infrastructure
{
    public class IpAddressHelper
    {
        private static List<string> _listIp = new List<string>();

        public static string? GetIpThisHost()
        {
            try
            {
                AddIpWithDhcpPrefix();
                AddIpWithManualPrefix();
            }
            catch (Exception ex)
            {
                return "No information";
            }

            return _listIp.FirstOrDefault()?.ToString();
        }

        private static void AddIpWithDhcpPrefix()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if (nic.GetIPProperties().UnicastAddresses.FirstOrDefault()?.PrefixOrigin == System.Net.NetworkInformation.PrefixOrigin.Dhcp)
                {
                    IPAddress? ip = nic.GetIPProperties().UnicastAddresses.FirstOrDefault()?.Address;
                    _listIp.Add(ip.ToString());
                }
            }
        }

        private static void AddIpWithManualPrefix()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                if (nic.GetIPProperties().UnicastAddresses.FirstOrDefault()?.PrefixOrigin == System.Net.NetworkInformation.PrefixOrigin.Manual)
                {
                    IPAddress? ip = nic.GetIPProperties().UnicastAddresses.FirstOrDefault()?.Address;
                    _listIp.Add(ip.ToString());
                }
            }
        }
    }
}
