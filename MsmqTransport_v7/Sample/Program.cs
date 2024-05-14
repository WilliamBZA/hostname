using System;
using System.Net;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Support;

class Program
{
    static async Task Main()
    {
        Console.Title = "MsmqSimple";
        #region ConfigureMsmqEndpoint

        RuntimeEnvironment.MachineNameAction = () => Dns.GetHostEntry(Environment.MachineName).HostName;

        var endpointConfiguration = new EndpointConfiguration("Samples.Msmq.Simple");



        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "Particular.ServiceControl",
            frequency: TimeSpan.FromSeconds(15),
            timeToLive: TimeSpan.FromSeconds(30));

        const string SERVICE_CONTROL_METRICS_ADDRESS = "particular.monitoring";

        var metrics = endpointConfiguration.EnableMetrics();

        metrics.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: SERVICE_CONTROL_METRICS_ADDRESS,
            interval: TimeSpan.FromSeconds(15));


        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        #endregion
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}