using System;
using System.Collections;
using System.Collections.Generic;
using Himsa.Samples.Models;
using Himsa.Samples.Models.Interface;

namespace Himsa.Samples.MemoryQueueProvider
{
    public class NoahMemoryQueue : INoahEventQueue
    {
        private Queue<NoahEvent> _eventQueue;

        public NoahMemoryQueue()
        {
            _eventQueue = new Queue<NoahEvent>();
        }

        public void Enqueue(NoahEvent noahEvent)
        {
            _eventQueue.Enqueue(noahEvent);
        }

        public NoahEvent Dequeue()
        {
            if (_eventQueue.Count > 0)
            {
                return _eventQueue.Dequeue();
            } else {
                return null;
            }
        }
    }
}
