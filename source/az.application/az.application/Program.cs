using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using az.gui;
using az.twitterapi;
using npantarhei.runtime;
using npantarhei.runtime.patterns;

namespace az.application
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var twitterops = new TwitterOperations();
            var gui = new MainWindow();

            FlowRuntimeConfiguration.SynchronizationFactory = () => new SyncWithWPFDispatcher();

            var config = new FlowRuntimeConfiguration()
                .AddStreamsFrom("az.application.flows.flow", Assembly.GetExecutingAssembly())

                .AddFunc<string, Versandauftrag>("versandauftrag_schnueren", twitterops.Versandauftrag_schnüren)
                .AddFunc<Versandauftrag, string>("versenden", twitterops.Versenden)
                .AddAction<string>("versandstatus_anzeigen", gui.Versandstatus).MakeSync();

            using (var fr = new FlowRuntime(config))
            {
                fr.UnhandledException += ex => MessageBox.Show(ex.InnerException.Message);

                gui.Versenden += fr.CreateEventProcessor<string>(".versenden");

                var app = new Application {MainWindow = gui};
                app.Run(gui);
            }
        }
    }
}
