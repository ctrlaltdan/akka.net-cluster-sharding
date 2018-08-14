using System;

namespace ClusterNode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
