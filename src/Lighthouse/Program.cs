using System;
using Shared;

namespace Lighthouse
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lighthouseService = new LighthouseService(actorSystemName: Constants.SystemName);
            lighthouseService.Start();

            Console.WriteLine("Press Control + C to terminate.");
            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                await lighthouseService.StopAsync();
            };
            lighthouseService.TerminationHandle.Wait();
        }
    }
}
