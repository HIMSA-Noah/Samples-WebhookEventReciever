using System;
using System.Collections.Generic;
using System.Text;

namespace Himsa.Samples.AzureStorageQueueProvider
{
    public class AzureStorageQueueOptions
    {
        /// <summary>
        /// The name f the Azure Storage Queue
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The Connection string to the Azure Storage Account containing the storage queue
        /// </summary>
        public string AzureStorageConectionString { get; set; }
    }
}
