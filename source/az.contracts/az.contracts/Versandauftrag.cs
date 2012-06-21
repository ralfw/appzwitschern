using System;
using az.security;

namespace az.contracts
{
    public class Versandauftrag
    {
        public Versandauftrag() {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
        
        public string Text { get; set; }

        public Token Credentials { get; set; }

        public DateTime Termin { get; set; }
    }
}