using System;
using NUnit.Framework;
using az.security;
using npantarhei.runtime.messagetypes;

namespace az.sqsapi.tests
{
    [TestFixture]
    public class test_enqueue_and_dequeue
    {
        private const string QUEUE_NAME = "npantarhei-communication-tests";

        [Test, Explicit]
        public void Run() {
            var credentials = TokenRepository.LoadFrom("aws.credentials.txt");
            var sqs = new SQSOperations(QUEUE_NAME, credentials);

            sqs.Enqueue("hello " + DateTime.Now);
            sqs.Enqueue("world " + DateTime.Now);
            for (var i = 0; i < 23; i++) {
                sqs.Enqueue("many " + i + " " + DateTime.Now);
            }

            sqs.Dequeue(Console.WriteLine, () => Console.WriteLine("END OF DATA!"));
        }
    }
}