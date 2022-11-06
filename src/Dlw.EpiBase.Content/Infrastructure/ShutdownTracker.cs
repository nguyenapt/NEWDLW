using System;
using System.Web.Hosting;
using EPiServer.Logging;
using Newtonsoft.Json;

namespace Dlw.EpiBase.Content.Infrastructure
{
    public class ShutdownTracker : IRegisteredObject
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(ShutdownTracker));

        public static void Start()
        {
            HostingEnvironment.RegisterObject(new ShutdownTracker());
        }

        public void Stop(bool immediate)
        {
            var data = new
            {
                ShutdownReason = HostingEnvironment.ShutdownReason.ToString(),
                ImmediateFlag = immediate,
                ComputerName = Environment.MachineName
            };

            Logger.Warning($"Shutdown triggered: '{JsonConvert.SerializeObject(data, Formatting.Indented)}'.");

            HostingEnvironment.UnregisterObject(this);
        }
    }
}