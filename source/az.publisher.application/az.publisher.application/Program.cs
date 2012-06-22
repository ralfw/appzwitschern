using System;
using System.Reflection;
using az.contracts;
using az.tweetstore;
using az.twitterapi;
using npantarhei.runtime;

namespace az.publisher.application
{
    internal class Program
    {
        private static void Main(string[] args) {
            var repository = new Repository();

            var frc = new FlowRuntimeConfiguration();
            frc.AddStreamsFrom("az.publisher.application.root.flow", Assembly.GetExecutingAssembly());
            frc.AddAction<Versandauftrag>("load", repository.Load);
            frc.AddAction<Versandauftrag, Versandauftrag>("filtern", Program.Filtern);
            frc.AddFunc<Versandauftrag, string>("versenden", new TwitterOperations().Versenden);
            frc.AddAction<string>("delete", repository.Delete);

            using(var fr = new FlowRuntime(frc)) {
                fr.UnhandledException += e => Console.WriteLine(e.InnerException.Message);

                fr.Process(".start");
                fr.WaitForResult();
            }
        }


        private static void Filtern(Versandauftrag versandauftrag, Action<Versandauftrag> continueWith)
        {
            if (versandauftrag == null || versandauftrag.Termin <= DateTime.Now)
                continueWith(versandauftrag);
        }
    }
}