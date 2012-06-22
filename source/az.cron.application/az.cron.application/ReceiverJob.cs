using System.Diagnostics;
using NCron;

namespace az.cron.application
{
    public class ReceiverJob : CronJob
    {
        public override void Execute() {
            Process.Start("az.receiver.application.exe");
        }
    }
}