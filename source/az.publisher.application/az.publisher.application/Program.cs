using System;
using System.IO;
using System.Reflection;
using az.contracts;
using az.twitterapi;
using npantarhei.runtime;
using npantarhei.runtime.messagetypes;

namespace az.publisher.application
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var repo = new az.tweetstore.ftp.Repository())
            {
                var publisher = new Publisher(repo, new TwitterOperations());
                publisher.Run();
            }
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
                .AddFunc<Versandauftrag, Versandauftrag>("versenden", twitterOperations.Versenden)
                .AddAction<Versandauftrag>("delete", repository.Delete)

                .AddAction<Versandauftrag, Versandauftrag>("filtern", this.Filtern);
        }


        public void Run()
        {
            using (var fr = new FlowRuntime(_config))
            {
                fr.Message += Console.WriteLine;
                fr.UnhandledException += e => 
                                                { 
                                                    Console.WriteLine(e.InnerException);
                                                    fr.Process(new Message(".stop") {Priority = 99});
                                                };

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