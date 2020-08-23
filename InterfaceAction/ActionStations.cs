using System;

namespace InterfaceAction
{
    class ActionStations : IActionStations
    {
        public void Run()
        {
            Set?.Invoke();
        }
        public Action Set { get; set; }
    }
}
