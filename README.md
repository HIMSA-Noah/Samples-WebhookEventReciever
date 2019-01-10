# Samples-WebhookEventReciever
Sample application for receiving webhook events from Noah

The sample application is implemented in .Net Core using Visual Studio 2017.

The sample application consists of the following parts:
*	WebHook endpoint – The webhook endpoint is the Http endpoint that receives the events from Noah. The WebHook endpoint simple receives events and forwards the message to a queue for processing.
*	Queue – Events received by the webhook endpoint are added to a internal queue. The sample application has a simple implementation of a INoahQueue interface with a memory queue. NB!: You must replace the queue implemetation with your preferred queue implementation – the memory queue is not suited for usage in a production environment.
*	Event processor – The Event processor deques the messages from the queue and sends the message to a implementation of an INoahEventHandler interface. The sample contains an implementation of the INoahEventHandler that writes the event to the console window.  
Adding a internal queue allows the WebHook endpoint to accept requests, without requiring processing of each message at time of ingestion, providing better scalability.
