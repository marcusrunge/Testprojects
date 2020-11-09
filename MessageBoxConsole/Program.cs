using System;
using System.Runtime.InteropServices;

namespace MessageBoxConsole
{
    class Program
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int MessageBoxCharlieWrapperCallback(IntPtr charlieBravo);
        private static MessageBoxCharlieWrapperCallback _messageBoxCharlieWrapperCallback;

        [DllImport("MessageBoxAlpha.dll", EntryPoint = "ShowMessageBoxAlpha", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ShowMessageBoxAlpha();

        [DllImport("MessageBoxBeta.dll", EntryPoint = "CreateMessageBoxBeta", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateMessageBoxBeta();

        [DllImport("MessageBoxBeta.dll", EntryPoint = "DisposeMessageBoxBeta", CallingConvention = CallingConvention.StdCall)]
        public static extern void DisposeMessageBoxBeta(IntPtr messageBoxBeta);

        [DllImport("MessageBoxBeta.dll", EntryPoint = "ShowMessageBoxBeta", CallingConvention = CallingConvention.StdCall)]
        public static extern void ShowMessageBoxBeta(IntPtr messageBoxBeta);

        [DllImport("MessageBoxCharlieWrapper.dll", EntryPoint = "DelegateShowMessageBoxCharlie", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DelegateShowMessageBoxCharlie(MessageBoxCharlieWrapperCallback messageBoxCharlieWrapperCallback);

        static void Main(string[] args)
        {
            _messageBoxCharlieWrapperCallback = new MessageBoxCharlieWrapperCallback(MessageBoxCharlieWrapperCallbackHandler);
            ShowMessageBoxAlpha();
            IntPtr messageBoxBeta = CreateMessageBoxBeta();
            ShowMessageBoxBeta(messageBoxBeta);
            DisposeMessageBoxBeta(messageBoxBeta);
            DelegateShowMessageBoxCharlie(_messageBoxCharlieWrapperCallback);
            Console.ReadKey();
        }

        private static int MessageBoxCharlieWrapperCallbackHandler(IntPtr charlieBravo)
        {
            var charlie_bravo = (CHARLIE_BRAVO)Marshal.PtrToStructure(charlieBravo, typeof(CHARLIE_BRAVO));
            MarshalUnmanagedArrayToManagedArray<CHARLIE_ALPHA>(charlie_bravo.charlieAlpha, 2, out CHARLIE_ALPHA[] charlieAlphas);
            var charlieAlphaOne = charlieAlphas[0];
            var charlieAlphaTwo = charlieAlphas[1];
            var charlieAlphaOneMessage = Marshal.PtrToStringAnsi(charlieAlphaOne.message);
            var charlieAlphaTwoMessage = Marshal.PtrToStringAnsi(charlieAlphaTwo.message);
            Console.WriteLine("CHARLIE_BRAVO Id: " + charlie_bravo.id);
            Console.WriteLine("charlieAlphaOne Id: " + charlieAlphaOne.id + ", Message: " + charlieAlphaOneMessage);
            Console.WriteLine("charlieAlphaTwo Id: " + charlieAlphaTwo.id + ", Message: " + charlieAlphaTwoMessage);
            return 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CHARLIE_ALPHA
        {
            public int id;
            public IntPtr message;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CHARLIE_BRAVO
        {
            public int id;
            public IntPtr charlieAlpha;
        }

        private static void MarshalUnmanagedArrayToManagedArray<T>(IntPtr unmanagedArray, int length, out T[] managedArray)
        {
            var size = Marshal.SizeOf(typeof(T));
            managedArray = new T[length];
            for (int i = 0; i < length; i++)
            {
                IntPtr intPtr = new IntPtr(unmanagedArray.ToInt64() + i * size);
                managedArray[i] = Marshal.PtrToStructure<T>(intPtr);
            }
        }
    }
}
