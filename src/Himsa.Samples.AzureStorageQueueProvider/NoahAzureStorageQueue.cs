using System;
using Microsoft.Extensions.Options;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Queue;
using Himsa.Samples.Models;
using Himsa.Samples.Models.Common;
using Himsa.Samples.Models.Interface;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace Himsa.Samples.AzureStorageQueueProvider
{
    public class NoahAzureStorageQueue : INoahEventQueue
    {
        protected CloudStorageAccount _storageAccount;
        protected CloudQueueClient _queueClient;
        protected CloudQueue _queue;

        private readonly AppSettings _settings;

        public NoahAzureStorageQueue(IOptions<AppSettings> options)
        {
            _settings = options.Value;

            _storageAccount = CloudStorageAccount.Parse(_settings.AzureStorageQueueOptions.AzureStorageConectionString);
            _queueClient = _storageAccount.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference(_settings.AzureStorageQueueOptions.QueueName);

            // Create the queue if it doesn't already exist.
            _queue.CreateIfNotExists();
        }

        public void Enqueue(NoahEvent noahEvent)
        {
            var messageBody = JsonConvert.SerializeObject(noahEvent); 

            CloudQueueMessage message = new CloudQueueMessage(messageBody);
            _queue.AddMessage(message);

            //throw new NotImplementedException();
        }

        public NoahEvent Dequeue()
        {
            CloudQueueMessage retrievedMessage = _queue.GetMessage();

            if (retrievedMessage != null)
            {
                NoahEvent noahEvent = JsonConvert.DeserializeObject<NoahEvent>(retrievedMessage.AsString);

                _queue.DeleteMessage(retrievedMessage);

                return noahEvent;
            }

            return null;
        }
    }
}
