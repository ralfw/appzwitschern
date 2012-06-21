using System;
using System.Reflection;
using az.security;
using az.serialization;
using az.sqsapi;
using az.twitterapi;
using npantarhei.runtime;

namespace az.receiver.application
{
    internal static class Program
    {
        private static void Main(string[] args) {
            var frc = new FlowRuntimeConfiguration();
            frc.AddStreamsFrom("az.receiver.application.root.flow", Assembly.GetExecutingAssembly());
            frc.AddAction<string>("dequeue", new SQSOperations("AppZwitschern", TokenRepository.LoadFrom("aws.credentials.txt")).Dequeue);
            frc.AddFunc<string, Versandauftrag>("deserialize", new Serialization<Versandauftrag>().Deserialize);
            frc.AddFunc<Versandauftrag, string>("versenden", new TwitterOperations().Versenden);
            
            using(var fr = new FlowRuntime(frc)) {
                fr.Message += Console.WriteLine;
                fr.UnhandledException += e => Console.WriteLine(e.InnerException.Message);

                fr.Process(".start");
                fr.WaitForResult();
            }
        }
    }
}