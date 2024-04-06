using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ProductService.Application.Messaging
{
     public class KafkaEventPublisher : IEventPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaEventPublisher> _logger;
        public KafkaEventPublisher(IConfiguration configuration, ILogger<KafkaEventPublisher> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task PublishAsync(string topic, string message)
        {
            try
            {
                _logger.LogInformation("Publishing Event for topic {0} with message {1}", topic, message);
                var deliveryReport = await _producer.ProduceAsync(topic, new Message<string, string> { Key = null, Value = message });
                _logger.LogInformation("Message Published Successfully for topic {0} with response {1}", topic, deliveryReport.ToString());
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
