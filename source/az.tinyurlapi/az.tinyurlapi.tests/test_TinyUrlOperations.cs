using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace az.tinyurlapi.tests
{
    [TestFixture]
    public class test_TinyUrlOperations
    {
        [Test]
        public void Shorten_a_url()
        {
            var sut = new TinyUrlOperations();

            var shortenedUrl = sut.Shorten("http://www.ralfw.de");

            Assert.AreEqual("http://tinyurl.com/7w6dkks", shortenedUrl);
        }
    }
}
