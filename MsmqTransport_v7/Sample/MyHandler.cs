﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"HostId = ${context.MessageHeaders[Headers.OriginatingHostId]}");
        log.Info("Hello from MyHandler");
        return Task.CompletedTask;
    }
}