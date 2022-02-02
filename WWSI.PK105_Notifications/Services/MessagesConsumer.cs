using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace WWSI.PK105_Notifications.Services
{
    public class MessagesConsumer
    {
        private readonly ILogger _logger;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;

        public MessagesConsumer(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _client = new ServiceBusClient(configuration.GetConnectionString("ServiceBusConnection"));
        }

        public async Task Register()
        {
            _processor = _client.CreateProcessor("messages");
            _processor.ProcessMessageAsync += _processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += _processor_ProcessErrorAsync;

            await _processor.StartProcessingAsync();
        }

        private Task _processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine("Wystąpił błąd: " + arg.Exception);

            _logger.Error(arg.Exception, "Erorr while handling message");

            return Task.CompletedTask;
        }

        private async Task _processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var payload = arg.Message.Body.ToObjectFromJson<MessagePayload>();

            if (payload.EventName == "PatientCreated")
            {

                throw new InvalidOperationException("Error while sending email");

                await arg.CompleteMessageAsync(arg.Message);
            }
        }
    }

    public class MessagePayload
    {
        public string EventName { get; set; }
        public string PatientEmail { get; set; }
        public string Content { get; set; }
    }
}
