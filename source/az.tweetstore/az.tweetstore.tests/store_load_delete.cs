using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using az.contracts;

namespace az.tweetstore.tests
{
    [TestFixture]
    public class store_load_delete
    {
        [Test]
        public void Run()
        {
            const string TEST_REPO_PATH = @"c:\appzwitschern";
            if (Directory.Exists(TEST_REPO_PATH)) Directory.Delete(TEST_REPO_PATH, true);

            var repo = new Repository(TEST_REPO_PATH);

            var va1 = new Versandauftrag() { Text = "a", Termin = new DateTime(2012, 6, 21), Id = Guid.NewGuid().ToString() };
            repo.Store(va1, null);
            var endOfStream = false;
            var va2 = new Versandauftrag() { Text = "b", Termin = new DateTime(2012, 6, 20), Id = Guid.NewGuid().ToString() };
            repo.Store(va2, null);
            repo.Store(null, () => endOfStream = true);
            Assert.IsTrue(endOfStream);
            endOfStream = false;

            var results = new List<string>();
            repo.List(results.Add);

            Assert.That(results.Select(fn => fn==null ? null : Path.GetFileName(fn)).ToArray(), 
                        Is.EquivalentTo(new[] { va1.Id + ".tweet", va2.Id + ".tweet", null }));

            var resultVAs = new List<Versandauftrag>();
            foreach(var fn in results)
                repo.Load(fn, resultVAs.Add);

            Assert.That(resultVAs.Select(_ => _==null ? null : _.Text).ToArray(), 
                                           Is.EquivalentTo(new[]{"a", "b", null}));

            resultVAs.ForEach(va => repo.Delete(va, () => endOfStream = true));

            Assert.AreEqual(0, Directory.GetFiles(TEST_REPO_PATH).Length);
            Assert.IsTrue(endOfStream);

            Directory.Delete(TEST_REPO_PATH);
        }
    }
}
