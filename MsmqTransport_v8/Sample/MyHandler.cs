using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        // On v8 before:  ae6b8144b9fa3fa661fc948a1414702c
        Console.WriteLine($"HostId = {context.MessageHeaders[Headers.OriginatingHostId]}");
        log.Info("Hello from MyHandler");
        return Task.CompletedTask;
    }
}