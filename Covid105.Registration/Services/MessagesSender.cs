using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid105.Registration.Services
{
    public class MessagesSender
    {
        private ServiceBusClient client;

        public MessagesSender(IConfiguration configuration)
        {
            client = new ServiceBusClient(configuration.GetConnectionString("ServiceBusConnection"));
        }

        public async Task SendMessage(MessagePayload payload)
        {
            await client.CreateSender("messages").SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(payload)));
        }
    }

    public class MessagePayload
    {
        public string EventName { get; set; }
        public string PatientEmail { get; set; }
        public string Content { get; set; }
    }
}
