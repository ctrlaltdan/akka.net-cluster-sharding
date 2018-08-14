using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ApiNode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            
            var actors = Actors.Build();

            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                await actors.Shutdown();
                await host.StopAsync(TimeSpan.FromSeconds(10));
            };
            
            host.Run();

            actors
                .StayAlive()
                .Wait();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
