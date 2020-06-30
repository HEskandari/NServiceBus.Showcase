using System;
using System.Threading.Tasks;
using NServiceBus;
using Store.Shared;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Store.Sales";

        SerilogConfigurer.Configure();

        var endpointConfiguration = new EndpointConfiguration("Store.Sales");
        endpointConfiguration.ApplyCommonConfiguration();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
