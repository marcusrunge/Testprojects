using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IpScanConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {            
            var iPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var iPAddress in iPAddresses)
            {
                if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    var subnetMask = GetSubnetMask(iPAddress);
                    byte[] ipAddressBytes = iPAddress.GetAddressBytes();
                    byte[] subnetMaskAddressBytes = subnetMask.GetAddressBytes();
                    byte[] startIPBytes = new byte[ipAddressBytes.Length];
                    byte[] endIPBytes = new byte[ipAddressBytes.Length];
                    for (int i = 0; i < ipAddressBytes.Length; i++)
                    {
                        startIPBytes[i] = (byte)(ipAddressBytes[i] & subnetMaskAddressBytes[i]);
                        endIPBytes[i] = (byte)(ipAddressBytes[i] | ~subnetMaskAddressBytes[i]);
                    }
                    IPAddress startIPAddress = new IPAddress(startIPBytes);
                    IPAddress endIPAddress = new IPAddress(endIPBytes);
                    Console.WriteLine($"Hello Subnet {subnetMask}!");

                    Parallel.For(startIPBytes[3], endIPBytes[3] + 1, i =>
                       {
                           startIPBytes[3] = (byte)i;
                           Ping ping = new Ping();
                           var pingReply = ping.Send(new IPAddress(startIPBytes));
                           if (pingReply.Status == IPStatus.Success) Console.WriteLine($"Hello IPAddress {new IPAddress(startIPBytes)}!");
                       });
                }
            }
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }
    }
}
