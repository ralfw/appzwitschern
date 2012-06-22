using System.Diagnostics;
using NCron;

namespace az.cron
{
    public class PublisherJob : CronJob
    {
        public override void Execute() {
            Process.Start("az.publisher.application.exe");
        }
    }
}