using System;
using System.Collections.Generic;
using System.Text;

namespace Himsa.Samples.Models.Interface
{
    public interface INoahEventHandler
    {
        void ProcessEvent(NoahEvent noahEvent);
    }
}
