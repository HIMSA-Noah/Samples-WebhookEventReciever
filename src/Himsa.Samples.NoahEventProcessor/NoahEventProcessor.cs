using System;
using Himsa.Samples.Models.Interface;

namespace Himsa.Samples.NoahEventProcessor
{
    public class NoahEventProcessor : INoahEventProcessor
    {
        protected INoahEventQueue _noahEventQueue;
        protected INoahEventHandler _eventHandler;
        private bool isRunning = false;
        
        private System.Timers.Timer timer = new System.Timers.Timer(1000);

        public NoahEventProcessor(INoahEventQueue noahEventQueue, INoahEventHandler eventHandler)
        {
            _noahEventQueue = noahEventQueue;
            _eventHandler = eventHandler;
        }

        public void Start() {
            if (isRunning) {
                return;
            }

            isRunning = true;
            
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isRunning != false)
            {
                var noahEvent = _noahEventQueue.Dequeue();

                do {
                    
                    if (noahEvent != null)
                    {
                        // Call the Event handler
                        _eventHandler.ProcessEvent(noahEvent);
                    }

                    noahEvent = _noahEventQueue.Dequeue();

                } while(noahEvent != null);
            }
        }

        public void Stop() {
            if (isRunning == false)
            {
                return;
            }
            isRunning = false;
        }

    }
}
