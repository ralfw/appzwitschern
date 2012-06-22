using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace az.application.tests
{
    [TestFixture]
    public class test_TextCompressor
    {
        [Test]
        public void Extract_urls()
        {
            var sut = new TextCompressor();

            var urls = sut.Extract_Urls("a http://x.de b http://y.com");

            Assert.That(urls, Is.EqualTo(new[]{"http://x.de", "http://y.com"}));
        }


        [Test]
        public void Replace_urls()
        {
            var sut = new TextCompressor();

            var text = sut.Replace_Urls(new Tuple<string, Tuple<string, string>[]>("a http://x.de b http://y.com",
                                                                                   new[]
                                                                                       {
                                                                                           new Tuple<string, string>("http://x.de", "x"),
                                                                                           new Tuple<string, string>("http://y.com", "y") 
                                                                                       }));
            Assert.AreEqual("a x b y", text);
        }
    }
}
