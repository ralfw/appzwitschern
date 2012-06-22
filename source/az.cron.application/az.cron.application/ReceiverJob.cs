using System.Diagnostics;
using System.IO;
using System.Reflection;
using NCron;

namespace az.cron.application
{
    public class ReceiverJob : CronJob
    {
        public override void Execute() {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Log.Info(() => path);
            var filename = Path.Combine(path, "az.receiver.application.exe");
            Log.Info(() => filename);
            Process.Start(filename);
        }
    }
}