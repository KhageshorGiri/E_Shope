namespace ProductService.Application.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(string topic, string message);
    }
}
