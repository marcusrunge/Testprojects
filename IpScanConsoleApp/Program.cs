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
                    var subnetMask = GetIpv4SubnetMask(iPAddress);
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
                else if (iPAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    var ipAddressBytes = iPAddress.GetAddressBytes();
                    var prefixLength = GetIpv6PrefixLength(iPAddress);
                    prefixLength = 62;
                    double zeroHextets = (128 - prefixLength) / 8d;
                    int fullHextets = (int)(16 - zeroHextets);
                    
                    byte[] subnetMaskAddressBytes = new byte[16];
                    
                    for (int i = 0; i < fullHextets; i++)
                    {
                        subnetMaskAddressBytes[i] = 255;
                    }
                    var difference = (double)zeroHextets - (int)zeroHextets;
                    if (difference > 0)
                    {
                        int part = (int)((1 - difference)*8);
                        subnetMaskAddressBytes[fullHextets] = (byte)(255 << part);                        
                    }
                    byte[] startIPBytes = new byte[ipAddressBytes.Length];
                    byte[] endIPBytes = new byte[ipAddressBytes.Length];
                    for (int i = 0; i < ipAddressBytes.Length; i++)
                    {
                        startIPBytes[i] = (byte)(ipAddressBytes[i] & subnetMaskAddressBytes[i]);
                        endIPBytes[i] = (byte)(ipAddressBytes[i] | ~subnetMaskAddressBytes[i]);
                    }
                    IPAddress startIPAddress = new IPAddress(startIPBytes);
                    IPAddress endIPAddress = new IPAddress(endIPBytes);                    
                }
            }
        }

        public static IPAddress GetIpv4SubnetMask(IPAddress iPAddress)
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (iPAddress.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", iPAddress));
        }

        public static int GetIpv6PrefixLength(IPAddress iPAddress)
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        if (iPAddress.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.PrefixLength;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", iPAddress));
        }
    }
}
