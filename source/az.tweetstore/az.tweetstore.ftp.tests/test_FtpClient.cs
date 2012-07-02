using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using az.security;
using az.tweetstore.ftp.adapter;

namespace az.tweetstore.ftp.tests
{
    [TestFixture]
    public class test_FtpClient
    {
        private const string SERVER = "ftp://ftp.ralfw.domainfactory-kunde.de";


        [Test]
        public void Create_AppZwitscher_directory()
        {
            var sut = new FtpClient(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"));

            sut.CreateDirectory("/AppZwitschern/TweetStore");

            var fnd = false;
            var dirEntries = sut.ListDirectory();
            foreach (var entry in dirEntries)
            {
                if (entry.Name == "AppZwitschern") fnd = true;
                Console.WriteLine("{0}, {1}", entry.Name, entry.IsDirectory);
            }
            Assert.IsTrue(fnd);
        }

        [Test]
        public void Upload_file()
        {
            var sut = new FtpClient(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"));
            sut.ChangeDirectory("/AppZwitschern/TweetStore");

            sut.UploadFiles("a.txt");

            Assert.IsTrue(sut.ListDirectory().Any(f => f.Name == "a.txt"));

            sut.DeleteFiles("a.txt");
        }

        [Test]
        public void Delete_file()
        {
            var sut = new FtpClient(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"));
            sut.ChangeDirectory("/AppZwitschern/TweetStore");

            sut.UploadFiles("a.txt");
            sut.DeleteFiles("a.txt");

            Assert.AreEqual(0, sut.ListDirectory().Count);   
        }

        [Test]
        public void Download_file()
        {
            if (!Directory.Exists("download")) Directory.CreateDirectory("download");

            var sut = new FtpClient(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"));
            sut.ChangeDirectory("/AppZwitschern/TweetStore");
   
            sut.UploadFiles("a.txt");
            sut.DownloadFiles("download", "a.txt");

            Assert.IsTrue(File.Exists(@"download\a.txt"));

            File.Delete(@"download\a.txt");
        }

        [Test]
        public void List_files()
        {
            var sut = new FtpClient(SERVER, TokenRepository.LoadFrom("ftp.credentials.txt"));
            sut.ChangeDirectory("/AppZwitschern/TweetStore");
   
            sut.UploadFiles("a.txt", "b.txt");

            var entries = sut.ListDirectory();
            Assert.AreEqual(2, entries.Count);
            Assert.IsTrue(entries.Any(e => e.Name == "a.txt"));
            Assert.IsTrue(entries.Any(e => e.Name == "b.txt"));

            sut.DeleteFiles("a.txt", "b.txt");
        }
    }
}
