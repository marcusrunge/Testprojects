using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lambda
{    
    class Program
    {
        //Delegatenerklärung
        delegate string MakeString(string StringA, string StringB);
        //Delegatenvariable
        static MakeString makeString;
        static void Main(string[] args)
        {
            //Dem Delegaten zugewiesener Lambda-Ausdruck
            makeString = (a, b) => a + b;
            //Konsolenausgabe "AB"
            Console.WriteLine(makeString("A","B"));
            Console.ReadKey();
        }
    }
}
