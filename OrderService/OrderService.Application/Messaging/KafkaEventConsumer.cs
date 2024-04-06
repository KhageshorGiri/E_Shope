using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace OrderService.Application.Messaging
{
    public class KafkaEventConsumer : IEventConsumer
    {
        private readonly IConsumer<string, string> _consumer;

        public KafkaEventConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:ConsumerGroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(configuration["Kafka:Topic"]);
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = await Task.Run(() => _consumer.Consume(cancellationToken));
                    var temp = consumeResult.Message;
                }
                catch (OperationCanceledException)
                {
                    // Ignore cancellation exception
                }
                catch (ConsumeException e)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task StopConsumingAsync()
        {
            _consumer.Close();
            await Task.CompletedTask;
        }
    }

}
