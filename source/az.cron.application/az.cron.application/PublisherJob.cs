﻿using System.Diagnostics;
using NCron;

namespace az.cron.application
{
    public class PublisherJob : CronJob
    {
        public override void Execute() {
            Process.Start("az.publisher.application.exe");
        }
    }
}