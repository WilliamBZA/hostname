using System;
using System.Net;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "MsmqSimple";
        #region ConfigureMsmqEndpoint

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Simple");

        endpointConfiguration.UseTransport(new MsmqTransport());

        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "Particular.ServiceControl",
            frequency: TimeSpan.FromSeconds(15),
            timeToLive: TimeSpan.FromSeconds(30));

        var machineName = global::NServiceBus.Support.RuntimeEnvironment.MachineName;
        Console.WriteLine(machineName);

        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        var metrics = endpointConfiguration.EnableMetrics();

        metrics.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            interval: TimeSpan.FromMinutes(1));

        // 41cde9cc05e12dc509da7014191d4d7b

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(0));

        

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        endpointConfiguration.UniquelyIdentifyRunningInstance().UsingHostName(Dns.GetHostEntry(Environment.MachineName).HostName);
        machineName = global::NServiceBus.Support.RuntimeEnvironment.MachineName;
        Console.WriteLine(machineName);

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);








        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
