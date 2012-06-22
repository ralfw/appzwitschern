using System.Diagnostics;
using System.IO;
using System.Reflection;
using NCron;

namespace az.cron.application
{
    public class PublisherJob : CronJob
    {
        public override void Execute() {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var filename = Path.Combine(path, "az.receiver.application.exe");
            var startInfo = new ProcessStartInfo {
                WorkingDirectory = path,
                FileName = filename
            };
            Process.Start(startInfo);
        }
    }
}