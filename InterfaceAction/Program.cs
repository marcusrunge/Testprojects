using System;

namespace InterfaceAction
{
    class Program
    {
        static void Main(string[] args)
        {
            var actionStations = new ActionStations();
            if (actionStations is IActionStations)
            {
                actionStations.Set += () =>
                {
                    Console.WriteLine("ActionStations!");
                };
            }
            actionStations.Run();
        }
    }
}
