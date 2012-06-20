using System;
using System.Diagnostics;
using NUnit.Framework;

namespace az.twitter.tests
{
    [TestFixture]
    public class TwitterTests
    {
        private Twitter twitter;

        [SetUp]
        public void Setup() {
            twitter = new Twitter(TokenRepository.LoadFrom("twitter.consumer.token.txt"));
        }

        [Test, Explicit]
        public void Tweet() {
            twitter.SendMessage(
                "Ich möchte so gerne Tweets zu einem Zeitpunkt schedulen statt sie sofort abzusetzen...",
                "xx",
                "yy");
        }

        [Test, Explicit]
        public void Authorization() {
            var url = twitter.GetAuthorizationUrl();
            Console.WriteLine(url);

            Process.Start(url);

            var pin = Console.ReadLine();

            var accessToken = twitter.GetAccessToken(pin);
        }
    }
}