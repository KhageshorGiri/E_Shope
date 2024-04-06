namespace OrderService.Application.Messaging
{
    public interface IEventConsumer
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
        Task StopConsumingAsync();
    }
}
