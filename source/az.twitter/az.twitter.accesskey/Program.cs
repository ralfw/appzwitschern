﻿using System;
using System.Diagnostics;

namespace az.twitter.accesskey
{
    internal class Program
    {
        private static void Main(string[] args) {
            var twitter = new Twitter("UZMoA54gkfW4Csrn4FOCQ", "sQywJpTP5ZNve59r2wgUWOwShgPfDmWzhvWlhbTiBM");

            var url = twitter.GetAuthorizationUrl();
            Console.WriteLine(url);

            Process.Start(url);

            Console.Write("Bitte geben Sie die PIN aus dem Browserfenster ein: ");
            var pin = Console.ReadLine();

            var accessToken = twitter.GetAccessToken(pin);
            Console.Write("Access key:     ");
            Console.WriteLine(accessToken.AccessKey);
            Console.Write("Access secret: ");
            Console.WriteLine(accessToken.AccessSecret);

            Console.WriteLine("Mit Enter beenden...");
            Console.ReadLine();
        }
    }
}