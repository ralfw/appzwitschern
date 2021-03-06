﻿using NCron.Fluent.Crontab;
using NCron.Service;

namespace az.cron.application
{
    internal static class Program
    {
        private static void Main(string[] args) {
            Bootstrap.Init(args, ServiceSetup);
        }

        static void ServiceSetup(ISchedulingService service) {
            service.At("*/5 * * * *").Run(() => new ReceiverJob());
            service.At("*/5 * * * *").Run(() => new PublisherJob());
        }
    }
}