using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using az.contracts;
using az.gui;
using az.ironmqapi;
using az.security;
using az.serialization;
using az.tinyurlapi;
using az.twitterapi;
using npantarhei.runtime;
using npantarhei.runtime.patterns;

namespace az.application
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args) {
            var twitterops = new TwitterOperations();
            var gui = new MainWindow();
            var ironmq = new IronMQOperations("AppZwitschern", TokenRepository.LoadFrom("ironmq.credentials.txt"));
            var serialisieren = new Serialization<Versandauftrag>();
            var urlShortener = new TinyUrlOperations();
            var compressor = new TextCompressor();

            FlowRuntimeConfiguration.SynchronizationFactory = () => new SyncWithWPFDispatcher();

            var config = new FlowRuntimeConfiguration()
                .AddStreamsFrom("az.application.flows.flow", Assembly.GetExecutingAssembly())
                .AddFunc<Versandauftrag, Versandauftrag>("versandauftrag_schnueren", twitterops.Versandauftrag_um_access_token_erweitern)
                .AddFunc<Versandauftrag, string>("serialisieren", serialisieren.Serialize)
                .AddAction<string>("enqueue", ironmq.Enqueue, true)
                .AddFunc<string, string[]>("extract_urls", compressor.Extract_Urls)
                .AddFunc<Tuple<string, Tuple<string,string>[]>,string>("replace_urls", compressor.Replace_Urls)
                .AddFunc<IEnumerable<string>, Tuple<String,string>[]>("shorten_urls", urlShortener.ShortenMany)
                .AddOperation(new AutoResetJoin<string, Tuple<string,string>[]>("join"))
                .AddAction("versandstatus_anzeigen", () => gui.Versandstatus("Versendet!")).MakeSync();

            using (var fr = new FlowRuntime(config)) {
                fr.UnhandledException += ex => MessageBox.Show(ex.InnerException.Message);

                gui.Versenden += fr.CreateEventProcessor<Versandauftrag>(".versenden");


                var app = new Application { MainWindow = gui };
                app.Run(gui);
            }
        }
    }
}