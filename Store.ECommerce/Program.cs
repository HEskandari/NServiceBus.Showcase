namespace Store.ECommerce.Core
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using Store.Shared;

    public class Program
    {
        public static void Main(string[] args)
        {
            SerilogConfigurer.Configure();
            BuildWebHost(args).Build().Run();
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
                .UseNServiceBus(c =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Store.ECommerce");
                    
                    endpointConfiguration.PurgeOnStartup(true);
                    endpointConfiguration.ApplyCommonConfiguration(transport =>
                    {
                        var routing = transport.Routing();
                        routing.RouteToEndpoint(typeof(Messages.Commands.SubmitHaveOrder).Assembly, "Store.Messages.Commands", "Store.Sales");
                    });

                    return endpointConfiguration;
                });
    }
}
