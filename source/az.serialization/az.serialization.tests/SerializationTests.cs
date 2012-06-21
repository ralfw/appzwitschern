using NUnit.Framework;

namespace az.serialization.tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void Zusammengesetzter_Typ() {
            var sut = new Serialization<Person>();
            var serialized = sut.Serialize(new Person { Name = "Peter", Alter = 42 });

            var person = sut.Deserialize(serialized);
            Assert.That(person.Name, Is.EqualTo("Peter"));
            Assert.That(person.Alter, Is.EqualTo(42));
        }

        private class Person
        {
            public string Name { get; set; }

            public int Alter { get; set; }
        }
    }
}