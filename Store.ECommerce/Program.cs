namespace Store.ECommerce.Core
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Serilog;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateLogger();
            BuildWebHost(args).Build().Run();
        }

        static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Seq(serverUrl: "http://localhost:8889")
                .WriteTo.Console()
                .CreateLogger();
            
            LogManager.Use<SerilogFactory>();
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
