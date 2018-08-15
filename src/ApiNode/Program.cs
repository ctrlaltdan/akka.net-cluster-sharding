using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Shared;

namespace ApiNode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Print.Line(ConsoleColor.Magenta);
            Print.Message("API NODE", ConsoleColor.Magenta);
            Print.Line(ConsoleColor.Magenta);

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
