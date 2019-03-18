using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;

namespace Quantum.Bell
{
    class Driver
    {
        static void Main(string[] args)
        {
            //Erzeugen des Quantumsimulators
            using (var quantumSimulator = new QuantumSimulator())
            {
                //Erzeugung zweier anfänglicher Messergebnisse(Resultate) von Null und Eins
                Result[] initials = new Result[] { Result.Zero, Result.One };
                //Für jedes Messergebnis(Resultat), zwei an der Zahl, wird der Quantensimulator gestartet
                foreach (Result initial in initials)
                {
                    var result = BellTest.Run(quantumSimulator, 1000, initial).Result;
                    var (numZeros, numOnes, agree) = result;
                    System.Console.WriteLine($"Init:{initial,-4} 0s={numZeros,-4} 1s={numOnes,-4} agree={agree, -4}");
                }
            }
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}