using Himsa.Samples.Models;
using Himsa.Samples.Models.Interface;
using System;

namespace Himsa.Samples.AzureStorageQueueProvider
{
    public class NoahAzureStorageQueue : INoahEventQueue
    {
        
        public NoahAzureStorageQueue()
        {
        }

        public void Enqueue(NoahEvent noahEvent)
        {
            throw new NotImplementedException();
        }

        public NoahEvent Dequeue()
        {
            throw new NotImplementedException();
        }
    }
}
