using System;
using Shared;

namespace ClusterNode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Print.Line(ConsoleColor.Cyan);
            Print.Message("CLUSTER NODE", ConsoleColor.Cyan);
            Print.Line(ConsoleColor.Cyan);

            var actors = Actors.Build();

            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                await actors.Shutdown();
            };

            actors
                .StayAlive()
                .Wait();
        }
    }
}
