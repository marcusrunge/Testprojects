using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EchoComponent
{
    public sealed class EchoClass
    {
        Socket _socket;
        [DllImport("EchoSocket.dll", EntryPoint = "TestSocket", CallingConvention = CallingConvention.Cdecl)]
        private static extern int TestSocket(IntPtr socket);
        public int CreateAndTestSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            return TestSocket(_socket.Handle);
        }
    }
}
