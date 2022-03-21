using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceEmail.Infrastructure
{
    public class GetIpAddresHost
    {   
        private static List<string> _listIp = new List<string>();

        public static string GetIpThisHost()
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

            return _listIp[0].ToString();
        }

        private static void AddIpWithDhcpPrefix()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                int indexInterfaceIPv4 = nic.GetIPProperties().UnicastAddresses.Count - 1;

                if (nic.GetIPProperties().UnicastAddresses[indexInterfaceIPv4].PrefixOrigin == System.Net.NetworkInformation.PrefixOrigin.Dhcp)
                {
                    IPAddress ip = nic.GetIPProperties().UnicastAddresses[indexInterfaceIPv4].Address;
                    _listIp.Add(ip.ToString());
                }
            }
        }

        private static void AddIpWithManualPrefix()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface nic in nics)
            {
                int indexInterfaceIPv4 = nic.GetIPProperties().UnicastAddresses.Count - 1;

                if (nic.GetIPProperties().UnicastAddresses[indexInterfaceIPv4].PrefixOrigin == System.Net.NetworkInformation.PrefixOrigin.Manual)
                {
                    IPAddress ip = nic.GetIPProperties().UnicastAddresses[indexInterfaceIPv4].Address;
                    _listIp.Add(ip.ToString());
                }
            }
        }

    }
}
