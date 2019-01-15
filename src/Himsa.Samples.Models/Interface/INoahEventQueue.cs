using System;
using System.Collections.Generic;
using System.Text;

namespace Himsa.Samples.Models.Interface
{
    public interface INoahEventQueue
    {
        void Enqueue(NoahEvent noahEvent);

        NoahEvent Dequeue();
    }
}
