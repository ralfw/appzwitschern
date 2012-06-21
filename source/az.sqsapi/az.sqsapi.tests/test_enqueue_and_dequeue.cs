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
            var enqueue = new SQSEnqueue(QUEUE_NAME, credentials);
            var dequeue = new SQSDequeue(QUEUE_NAME, credentials);

            enqueue.Implementation(new Message("x", "hello " + DateTime.Now), null, null);
            enqueue.Implementation(new Message("x", "world " + DateTime.Now), null, null);
            for (var i = 0; i < 23; i++) {
                enqueue.Implementation(new Message("x", "world " + i + " " + DateTime.Now), null, null);
            }

            dequeue.Implementation(new Message("x", null), _ => Console.WriteLine(_.Data), null);
        }
    }
}