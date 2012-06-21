using NUnit.Framework;

namespace az.serialization.tests
{
    [TestFixture]
    public class SerializationTests
    {
        private Serialization<Person> sut;

        [SetUp]
        public void Setup() {
            sut = new Serialization<Person>();
        }

        [Test]
        public void Zusammengesetzter_Typ() {
            var serialized = sut.Serialize(new Person { Name = "Peter", Alter = 42 });

            var person = sut.Deserialize(serialized);
            Assert.That(person.Name, Is.EqualTo("Peter"));
            Assert.That(person.Alter, Is.EqualTo(42));
        }

        [Test]
        public void Null_wird_durchgereicht_zur_end_of_stream_Erkennung() {
            Assert.That(sut.Deserialize(null), Is.Null);
        }

        private class Person
        {
            public string Name { get; set; }

            public int Alter { get; set; }
        }
    }
}