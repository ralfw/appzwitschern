﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using az.security;
using io.iron.ironmq;

namespace az.ironmqapi.tests
{
    [TestFixture]
    public class spike
    {
        [Test]
        public void Read_from_empty_queue()
        {
            var credentials = TokenRepository.LoadFrom("ironmq.credentials.txt");
            var client = new Client(credentials.Key, credentials.Secret);
            var queue = client.Queue("AppZwitschern");

            var msg = queue.Dequeue();

            Assert.IsNull(msg);
        }
    }
}
