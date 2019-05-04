
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeltaLoopUnitTest
{
    [TestClass]
    public class DeltaLoopUnitTest
    {
        string _loopMessage = "UnitTestSource";
        string _loopReturnMessage;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DeltaMessageLoopCallback(IntPtr deltaMessagePtr);
        private DeltaMessageLoopCallback _deltaMessageLoopCallback;

        [DllImport("DeltaLoop.dll", EntryPoint = "DelegateLoopBackDeltaMessage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void DelegateLoopBackDeltaMessage(string deltaMessage, DeltaMessageLoopCallback deltaMessageLoopCallback);

        [TestMethod]
        public void DeltaLoopTest()
        {
            _deltaMessageLoopCallback = new DeltaMessageLoopCallback(x =>
            {
                var deltaMessage = (delta_message)Marshal.PtrToStructure(x, typeof(delta_message));
                _loopReturnMessage = Marshal.PtrToStringAnsi(deltaMessage.message);
                return 0;
            });
            //Task.Run(() => DelegateLoopBackDeltaMessage(_loopMessage, _deltaMessageLoopCallback)).Wait();
            DelegateLoopBackDeltaMessage(_loopMessage, _deltaMessageLoopCallback);
            Assert.AreEqual(_loopMessage, _loopReturnMessage);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct delta_message
        {
            public int id;
            public IntPtr message;
        }
    }
}