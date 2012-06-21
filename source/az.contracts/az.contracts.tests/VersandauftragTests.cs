using NUnit.Framework;

namespace az.contracts.tests
{
    [TestFixture]
    public class VersandauftragTests
    {
        [Test]
        public void Id_ist_gesetzt() {
            Assert.That(new Versandauftrag().Id, Is.Not.Empty);
        }
    }
}