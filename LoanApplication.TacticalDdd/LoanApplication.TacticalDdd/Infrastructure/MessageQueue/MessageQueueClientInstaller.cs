using EasyNetQ;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Infrastructure.MessageQueue;

public static class MessageQueueClientInstaller
{
    public static void AddRabbitMqClient(this IServiceCollection services, string brokerAddress)
    {
        services.AddEasyNetQ(brokerAddress).UseSystemTextJson();
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
    }
}