using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Himsa.Samples.WebHookEventReciever
{
    public class AppSettings
    {
        public string WebHookOrigin { get; set; }

        public string WebHookSharedSecret { get; set; }
    }
}
