using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using az.contracts;
using az.security;

namespace az.tweetstore.ftp.tests
{
    [TestFixture]
    public class test_Repository
    {
        private const string SERVER = "ftp.ralfw.domainfactory-kunde.de";

        [Test]
        public void Run()
        {
            using (var sut = new Repository(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"), "AppZwitschern_TweetStore"))
            {
                Console.WriteLine("storing...");
                var versandaufträge = new[]
                                          {
                                              new Versandauftrag()
                                                  {Id = "1", Termin = new DateTime(2012, 3, 26, 12, 21, 0), Text = "a"},
                                              new Versandauftrag()
                                                  {Id = "2", Termin = new DateTime(2011, 10, 27, 14, 17, 0), Text = "b"}
                                              ,
                                              null
                                          };
                var versandt = false;
                versandaufträge.ToList().ForEach(_ => sut.Store(_, () => versandt = true));
                Console.WriteLine("  stored!");
                Assert.IsTrue(versandt);

                Console.WriteLine("listing...");
                var filenames = new List<string>();
                sut.List(filenames.Add);
                filenames.ForEach(fn => Console.WriteLine("list {0}", fn));
                Assert.AreEqual(3, filenames.Count);

                Console.WriteLine("loading...");
                var results = new List<Versandauftrag>();
                filenames.ForEach(fn => sut.Load(fn, results.Add));
                Console.WriteLine("  loaded!");
                Assert.That(results.Select(_ => _ == null ? null : _.Id).ToArray(),
                            Is.EquivalentTo(new[] {"1", "2", null}));

                Console.WriteLine("deleting...");
                var deleted = false;
                results.ForEach(va => sut.Delete(va, () => deleted = true));
                Console.WriteLine("  deleted!");
                Assert.IsTrue(deleted);
            }
        }
    }
}
