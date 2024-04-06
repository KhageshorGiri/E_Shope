using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace ProductService.Application.Messaging
{
     public class KafkaEventPublisher : IEventPublisher
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string message)
        {
            try
            {
                var deliveryReport = await _producer.ProduceAsync(topic, new Message<string, string> { Key = null, Value = message });
            }
            catch (ProduceException<string, string> ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }

}
