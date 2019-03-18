using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NativePrimes
{
    class Program
    {
        [DllImport("PrimesLib.dll")]
        public static extern int GetPrimes();

        static void Main(string[] args)
        {
            Console.WriteLine(GetPrimes());
            Console.ReadKey();
        }
    }
}
