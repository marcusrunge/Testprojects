using System;

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
