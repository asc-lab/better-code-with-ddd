using EasyNetQ;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.PortsAdapters.MessageQueue;

public static class MessageQueueClientInstaller
{
    public static void AddRabbitMqClient(this IServiceCollection services, string brokerAddress)
    {
        services.AddSingleton<IBus>(_ => RabbitHutch.CreateBus(brokerAddress));
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
    }
}