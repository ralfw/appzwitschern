using System;
using System.Diagnostics;

namespace az.twitter.accesskey
{
    internal class Program
    {
        private static void Main(string[] args) {
            var twitter = new Twitter(TokenRepository.LoadFrom("twitter.consumer.token.txt"));

            var url = twitter.GetAuthorizationUrl();
            Console.WriteLine(url);

            Process.Start(url);

            Console.Write("Bitte geben Sie die PIN aus dem Browserfenster ein: ");
            var pin = Console.ReadLine();

            var accessToken = twitter.GetAccessToken(pin);
            Console.Write("Access key:     ");
            Console.WriteLine(accessToken.Key);
            Console.Write("Access secret: ");
            Console.WriteLine(accessToken.Secret);

            TokenRepository.SaveTo("twitter.access.token.txt", accessToken);
            Console.WriteLine("Token gespeichert in twitter.access.token.txt");

            Console.WriteLine("Mit Enter beenden...");
            Console.ReadLine();
        }
    }
}