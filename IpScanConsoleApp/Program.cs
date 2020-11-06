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
            var hostIpAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var hostIpAddress in hostIpAddresses)
            {
                if (hostIpAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    var subnetIpAddress = GetIpv4SubnetMask(hostIpAddress);
                    byte[] hostAddress = hostIpAddress.GetAddressBytes();
                    byte[] subnetAddress = subnetIpAddress.GetAddressBytes();
                    byte[] startAddress = new byte[hostAddress.Length];
                    byte[] endAddress = new byte[hostAddress.Length];
                    for (int i = 0; i < hostAddress.Length; i++)
                    {
                        startAddress[i] = (byte)(hostAddress[i] & subnetAddress[i]);
                        endAddress[i] = (byte)(hostAddress[i] | ~subnetAddress[i]);
                    }
                    IPAddress startIPAddress = new IPAddress(startAddress);
                    IPAddress endIPAddress = new IPAddress(endAddress);
                    Console.WriteLine($"Hello Subnet {subnetIpAddress}!");

                    for (int i = startAddress[0]; i < endAddress[0] + 1; i++)
                    {
                        for (int j = startAddress[1]; j < endAddress[1] + 1; j++)
                        {
                            for (int k = startAddress[2]; k < endAddress[2] + 1; k++)
                            {
                                Parallel.For(startAddress[3], endAddress[3] + 1, l =>
                                {
                                    byte[] address = new byte[4];
                                    address[0] = (byte)i;
                                    address[1] = (byte)j;
                                    address[2] = (byte)k;
                                    address[3] = (byte)l;
                                    Ping ping = new Ping();
                                    var pingReply = ping.Send(new IPAddress(address));
                                    if (pingReply.Status == IPStatus.Success) Console.WriteLine($"Hello IPAddress {new IPAddress(address)}!");
                                });
                            }
                        }
                    }
                }
                else if (hostIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    var ipAddressBytes = hostIpAddress.GetAddressBytes();
                    var prefixLength = GetIpv6PrefixLength(hostIpAddress);
                    prefixLength = 62;
                    double zeroHostHextets = (128 - prefixLength) / 8d;
                    int fullSubnetHextets = (int)(16 - zeroHostHextets);
                    int breakPoint = fullSubnetHextets;
                    int reverseBytePosition = -1;
                    byte[] subnetMaskAddressBytes = new byte[16];

                    for (int i = 0; i < fullSubnetHextets; i++)
                    {
                        subnetMaskAddressBytes[i] = 255;
                    }
                    var difference = (double)zeroHostHextets - (int)zeroHostHextets;
                    if (difference > 0)
                    {
                        int part = (int)((1 - difference) * 8);
                        subnetMaskAddressBytes[fullSubnetHextets] = (byte)(255 << part);
                        breakPoint++;
                        reverseBytePosition = (int)(zeroHostHextets - 1);
                    }
                    byte[] startIPBytes = new byte[ipAddressBytes.Length];
                    //byte[] endIPBytes = new byte[ipAddressBytes.Length];
                    //byte[] hostBytes = new byte[subnetMaskAddressBytes.Length];
                    //for (int i = 0; i < hostBytes.Length; i++)
                    //{
                    //    hostBytes[i] = (byte)~subnetMaskAddressBytes[i];
                    //    if (reverseBytePosition > -1 && reverseBytePosition == i) hostBytes[i] = MirrorByte(hostBytes[i]);
                    //}
                    for (int i = 0; i < ipAddressBytes.Length; i++)
                    {
                        startIPBytes[i] = (byte)(ipAddressBytes[i] & subnetMaskAddressBytes[i]);
                        //endIPBytes[i] = (byte)(ipAddressBytes[i] | ~hostBytes[i]);
                    }
                    
                    IPAddress startIPAddress = new IPAddress(startIPBytes);
                    //IPAddress endIPAddress = new IPAddress(endIPBytes);
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
            throw new ArgumentException(string.Format("Can't find prefix length for IPv6 address '{0}'", iPAddress));
        }
    }
}
