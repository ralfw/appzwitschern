using System;
using System.Reflection;
using az.contracts;
using az.security;
using az.serialization;
using az.sqsapi;
using az.tweetstore;
using npantarhei.runtime;

namespace az.receiver.application
{
    internal static class Program
    {
        private static void Main(string[] args) {
            var frc = new FlowRuntimeConfiguration()
                       .AddStreamsFrom("az.receiver.application.root.flow", Assembly.GetExecutingAssembly())

                       .AddAction<string>("dequeue", new SQSOperations("AppZwitschern", TokenRepository.LoadFrom("aws.credentials.txt")).Dequeue)
                       .AddFunc<string, Versandauftrag>("deserialize", new Serialization<Versandauftrag>().Deserialize)
                       .AddAction<Versandauftrag>("speichern", new Repository().Store);
            
            using(var fr = new FlowRuntime(frc)) {
                fr.Message += Console.WriteLine;
                fr.UnhandledException += e => Console.WriteLine(e.InnerException.Message);

                fr.Process(".start");
                fr.WaitForResult();
            }
        }
    }
}