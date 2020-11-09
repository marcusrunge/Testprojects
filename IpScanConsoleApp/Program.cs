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
                    byte[] subAddress = subnetIpAddress.GetAddressBytes();
                    byte[] startAddress = new byte[hostAddress.Length];
                    byte[] endAddress = new byte[hostAddress.Length];
                    for (int i = 0; i < hostAddress.Length; i++)
                    {
                        startAddress[i] = (byte)(hostAddress[i] & subAddress[i]);
                        endAddress[i] = (byte)(hostAddress[i] | ~subAddress[i]);
                    }
                    IPAddress startIPAddress = new IPAddress(startAddress);
                    IPAddress endIPAddress = new IPAddress(endAddress);
                    Console.WriteLine($"Hello Subnet {subnetIpAddress}!");
                    Console.WriteLine($"Hello Start IP Address {startIPAddress}!");
                    Console.WriteLine($"Hello End IP Address {endIPAddress}!");

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
                    var hostAddress = hostIpAddress.GetAddressBytes();
                    var prefixLength = GetIpv6PrefixLength(hostIpAddress);
                    byte[] startAddress = new byte[16];
                    byte[] endAddress = new byte[16];
                    byte[] subAddress = new byte[16];

                    double subLength = prefixLength / 8d;

                    for (int i = 0; i < (int)subLength; i++)
                    {
                        subAddress[i] = 255;
                    }

                    int transition = (int)((subLength - (int)subLength) * 8);

                    if (transition > 0)
                    {
                        byte fullByte = 255;
                        subAddress[(int)subLength] = (byte)(fullByte << 8 - transition);
                    }

                    for (int i = 0; i < hostAddress.Length; i++)
                    {
                        startAddress[i] = (byte)(hostAddress[i] & subAddress[i]);
                        endAddress[i] = (byte)(hostAddress[i] | ~subAddress[i]);
                    }

                    IPAddress startIPAddress = new IPAddress(startAddress);
                    IPAddress endIPAddress = new IPAddress(endAddress);

                    Console.WriteLine($"Hello Subnet {new IPAddress(subAddress)}!");
                    Console.WriteLine($"Hello Start IP Address {startIPAddress}!");
                    Console.WriteLine($"Hello End IP Address {endIPAddress}!");

                    for (int i = startAddress[0]; i < endAddress[0] + 1; i++)
                    {
                        for (int j = startAddress[1]; j < endAddress[1] + 1; j++)
                        {
                            for (int k = startAddress[2]; k < endAddress[2] + 1; k++)
                            {
                                for (int l = startAddress[3]; l < endAddress[3] + 1; l++)
                                {
                                    for (int m = startAddress[4]; m < endAddress[4] + 1; m++)
                                    {
                                        for (int n = startAddress[5]; n < endAddress[5] + 1; n++)
                                        {
                                            for (int o = startAddress[6]; o < endAddress[6] + 1; o++)
                                            {
                                                for (int p = startAddress[7]; p < endAddress[7] + 1; p++)
                                                {
                                                    for (int q = startAddress[8]; q < endAddress[8] + 1; q++)
                                                    {
                                                        for (int r = startAddress[9]; r < endAddress[9] + 1; r++)
                                                        {
                                                            for (int s = startAddress[10]; s < endAddress[10] + 1; s++)
                                                            {
                                                                for (int t = startAddress[11]; t < endAddress[11] + 1; t++)
                                                                {
                                                                    for (int u = startAddress[12]; u < endAddress[12] + 1; u++)
                                                                    {
                                                                        for (int v = startAddress[13]; v < endAddress[13] + 1; v++)
                                                                        {
                                                                            for (int w = startAddress[14]; w < endAddress[14] + 1; w++)
                                                                            {
                                                                                Parallel.For(startAddress[15], endAddress[15] + 1, x =>
                                                                                {
                                                                                    byte[] address = new byte[16];
                                                                                    address[0] = (byte)i;
                                                                                    address[1] = (byte)j;
                                                                                    address[2] = (byte)k;
                                                                                    address[3] = (byte)l;
                                                                                    address[3] = (byte)m;
                                                                                    address[5] = (byte)n;
                                                                                    address[6] = (byte)o;
                                                                                    address[7] = (byte)p;
                                                                                    address[8] = (byte)q;
                                                                                    address[9] = (byte)r;
                                                                                    address[10] = (byte)s;
                                                                                    address[11] = (byte)t;
                                                                                    address[12] = (byte)u;
                                                                                    address[13] = (byte)v;
                                                                                    address[14] = (byte)w;
                                                                                    address[15] = (byte)x;
                                                                                    Ping ping = new Ping();
                                                                                    var pingReply = ping.Send(new IPAddress(address));
                                                                                    if (pingReply.Status == IPStatus.Success) Console.WriteLine($"Hello IPAddress {new IPAddress(address)}!");
                                                                                });
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
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
