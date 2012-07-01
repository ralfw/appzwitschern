using System;

namespace az.tweetstore.ftp.adapter
{
    public class FtpDirectoryEntry
    {
        public string Name;
        public DateTime CreateTime;
        public bool IsDirectory;
        public Int64 Size;
        public string Group;    // UNIX only
        public string Owner;
        public string Flags;
    }
}