using System;
using System.Reflection;
using az.contracts;
using az.ironmqapi;
using az.security;
using az.serialization;
using npantarhei.runtime;

namespace az.receiver.application
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (var repo = new az.tweetstore.ftp.Repository())
            {
                var frc = new FlowRuntimeConfiguration()
                    .AddStreamsFrom("az.receiver.application.root.flow", Assembly.GetExecutingAssembly())

                    .AddAction<string>("dequeue",
                                       new IronMQOperations("AppZwitschern",
                                                            TokenRepository.LoadFrom("ironmq.credentials.txt")).Dequeue)

                    .AddFunc<string, Versandauftrag>("deserialize", new Serialization<Versandauftrag>().Deserialize)
                    .AddAction<Versandauftrag>("speichern", repo.Store);

                using (var fr = new FlowRuntime(frc))
                {
                    fr.Message += Console.WriteLine;
                    fr.UnhandledException += e =>
                                                 {
                                                     Console.WriteLine(e.InnerException);
                                                     fr.Process(".stop");
                                                 };

                    fr.Process(".start");
                    fr.WaitForResult();
                }
            }
        }
    }
}