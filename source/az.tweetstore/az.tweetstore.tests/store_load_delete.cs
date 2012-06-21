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

            var va = new Versandauftrag() { Text = "a", Termin = new DateTime(2012, 6, 21), Id = Guid.NewGuid().ToString() };
            repo.Store(va);

            va = new Versandauftrag() { Text = "b", Termin = new DateTime(2012, 6, 20), Id = Guid.NewGuid().ToString() };
            repo.Store(va);

            var results = new List<Versandauftrag>();
            var endOfLoad = false;
            repo.Load(results.Add, () => endOfLoad = true);

            Assert.That(results.Select(r => r.Text).ToArray(), Is.EquivalentTo(new[] { "a", "b" }));
            Assert.IsTrue(endOfLoad);

            results.ForEach(r => repo.Delete(r.Id));

            Assert.AreEqual(0, Directory.GetFiles(TEST_REPO_PATH).Length);

            Directory.Delete(TEST_REPO_PATH);
        }
    }
}
