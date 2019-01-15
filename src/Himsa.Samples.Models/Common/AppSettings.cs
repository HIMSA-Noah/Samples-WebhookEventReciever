using System;
using System.Collections.Generic;
using System.Text;

namespace Himsa.Samples.Models.Common
{
    public class AppSettings
    {
        public string WebHookOrigin { get; set; }

        public string WebHookSharedSecret { get; set; }

        public AzureStorageQueueOptions AzureStorageQueueOptions { get; set; }
    }
}
