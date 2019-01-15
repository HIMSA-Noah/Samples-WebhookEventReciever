using Himsa.Samples.Models;
using System;
using Himsa.Samples.Models.Interface;

namespace Himsa.Samples.ConsolNoahEventHandler
{
    public class ConsoleEventHandler : INoahEventHandler
    {
        public void ProcessEvent(NoahEvent noahEvent)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(noahEvent);
            Console.Write(jsonString);
        }
    }
}
