using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoxConsole
{
    class Program
    {
        [DllImport("MessageBoxAlpha.dll")]
        public static extern void ShowMessageBoxAlpha();
        [DllImport("MessageBoxBeta.dll")]
        static public extern IntPtr CreateMessageBoxBeta();
        [DllImport("MessageBoxBeta.dll")]
        static public extern void DisposeMessageBoxBeta(IntPtr messageBoxBeta);
        [DllImport("MessageBoxBeta.dll")]
        static public extern void ShowMessageBoxBeta(IntPtr messageBoxBeta);
        static void Main(string[] args)
        {
            ShowMessageBoxAlpha();
            IntPtr messageBoxBeta = CreateMessageBoxBeta();
            ShowMessageBoxBeta(messageBoxBeta);
            DisposeMessageBoxBeta(messageBoxBeta);
        }
    }
}
