using System;
using System.IO;
using System.Reflection;
using az.contracts;
using az.tweetstore;
using az.twitterapi;
using npantarhei.runtime;

namespace az.publisher.application
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var publisher = new Publisher(new Repository(), new TwitterOperations());
            publisher.Run();
        }
    }


    class Publisher
    {
        private readonly FlowRuntimeConfiguration _config;

        public Publisher(IRepository repository, ITwitterOperations twitterOperations)
        {
            _config = new FlowRuntimeConfiguration()
                .AddStreamsFrom("az.publisher.application.root.flow", Assembly.GetExecutingAssembly())

                .AddAction<string>("list", repository.List)
                .AddAction<string, Versandauftrag>("load", repository.Load)
                .AddFunc<Versandauftrag, string>("versenden", twitterOperations.Versenden)
                .AddAction<string>("delete", repository.Delete)

                .AddAction<Versandauftrag, Versandauftrag>("filtern", this.Filtern);
        }


        public void Run()
        {
            using (var fr = new FlowRuntime(_config)) {
                fr.UnhandledException += e => Console.WriteLine(e.InnerException.Message);

                fr.Process(".start");
                fr.WaitForResult();
            }
        }


        private void Filtern(Versandauftrag versandauftrag, Action<Versandauftrag> continueWith)
        {
            if (versandauftrag == null || versandauftrag.Termin <= DateTime.Now)
                continueWith(versandauftrag);
        }
    }
}