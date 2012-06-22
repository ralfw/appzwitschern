using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using az.security;

namespace az.ironmqapi.tests
{
    [TestFixture]
    public class enqueue_and_dequeue
    {
        [Test]
        public void Run()
        {
            var credentials = TokenRepository.LoadFrom("ironmq.credentials.txt");
            var sut = new IronMQOperations("AppZwitschern", credentials.Key, credentials.Secret);

            sut.Enqueue("hello " + DateTime.Now);
            sut.Enqueue("world " + DateTime.Now);

            for(var i = 0; i<23; i++)
                sut.Enqueue("many " + i + " " + DateTime.Now);

            Console.WriteLine("enqueued!");

            var results = new List<string>();
            sut.Dequeue(results.Add);

            foreach(var r in results)
                Console.WriteLine(r);
        }
    }
}
