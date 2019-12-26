using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string topicEndpoint = "https://az-test-event.westus2-1.eventgrid.azure.net/api/events";

            // TODO: Enter value for <topic-key>. You can find this in the "Access Keys" section in the
            // "Event Grid Topics" blade in Azure Portal.
            string topicKey = "zK0vz8uFI0+gcUk1uG4jrArzRvJwdWUEdhfDxEm1odA=";

            string topicHostname = new Uri(topicEndpoint).Host;
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            await client.PublishEventsAsync(topicHostname, GetEventsList());
            Console.Write("Published events to Event Grid.");

            static IList<EventGridEvent> GetEventsList()
            {
                List<EventGridEvent> eventsList = new List<EventGridEvent>();
                for (int i = 0; i < 2; i++)
                {
                    eventsList.Add(new EventGridEvent()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventType = "event-http-test",
                        Data = new ContosoItemReceivedEventData()
                        {
                            ItemSku = $"Dukati SKu {i}",
                            Model = 1998,
                            Color = "Red"
                        },

                        EventTime = DateTime.Now,
                        Subject = "Door1",
                        DataVersion = "2.0"
                    });
                }
                return eventsList;
            }
        }
    }

    class ContosoItemReceivedEventData
    {
        [JsonProperty(PropertyName = "itemSku")]
        public string ItemSku { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "model")]
        public int Model { get; set; }
    }

}
