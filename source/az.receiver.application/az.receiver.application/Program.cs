using System;
using System.Reflection;
using az.security;
using az.serialization;
using az.sqsapi;
using az.twitterapi;
using npantarhei.runtime;

namespace az.receiver.application
{
    internal class Program
    {
        private static void Main(string[] args) {
            var frc = new FlowRuntimeConfiguration();
            frc.AddStreamsFrom("az.receiver.application.root.flow", Assembly.GetExecutingAssembly());
            frc.AddOperation(new SQSDequeue("dequeue", "AppZwitschern", TokenRepository.LoadFrom("aws.credentials.txt")));
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