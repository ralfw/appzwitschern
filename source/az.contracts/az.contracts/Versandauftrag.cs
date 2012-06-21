using System;
using az.security;

namespace az.contracts
{
    public class Versandauftrag
    {
        public string Id { get; set; }
        
        public string Text { get; set; }

        public Token Credentials { get; set; }

        public DateTime Termin { get; set; }
    }
}