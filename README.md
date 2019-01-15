# Samples-WebhookEventReciever
Sample application for receiving webhook events from Noah

The sample application is implemented in .Net Core using Visual Studio 2017.

The sample application consists of the following parts:
*	WebHook endpoint – The webhook endpoint is the Http endpoint that receives the events from Noah. The WebHook endpoint simple receives events and forwards the message to a queue for processing.
*	Queue – Events received by the webhook endpoint are added to a internal queue. The sample application has a simple implementation of a INoahQueue interface with a memory queue. NB!: You must replace the queue implemetation with your preferred queue implementation – the memory queue is not suited for usage in a production environment.
*	Event processor – The Event processor deques the messages from the queue and sends the message to a implementation of an INoahEventHandler interface. The sample contains an implementation of the INoahEventHandler that writes the event to the console window.  
Adding a internal queue allows the WebHook endpoint to accept requests, without requiring processing of each message at time of ingestion, providing better scalability.

## Setting up the Azure Storage Queue Provider
If you download the source code to start working with the Webhook receiver locally it is setup with a MemoryQueueProvider for the internal queue in the event receiver.
MemoryQueueProvider is not a persistent queue and is only usable for development and test.
As part of the sample we have also created a AzureStorageQueueProvider that use the Azure Storage Queue Service.
Before you can use the AzureStorageQueueProvider you will need to setup a Azure Storage Account and copy the Storage Account Credentials to the AppSettings.json file in the Webhook solution.

### Copy Azure Storage credentials from the Azure portal
The sample code needs to authorize access to your storage account. To authorize, you provide the application with your storage account credentials in the form of a connection string. To view your storage account credentials:
*	Navigate to the Azure portal.
*	Locate your storage account or create a new storage Account in the Azure Portal.
*	In the Settings section of the storage account overview, select Access keys. Your account access keys appear, as well as the complete connection string for each key.
*	Find the Connection string value under key1, and click the Copy button to copy the connection string. You will add the connection string value to an environment variable in the next step.
