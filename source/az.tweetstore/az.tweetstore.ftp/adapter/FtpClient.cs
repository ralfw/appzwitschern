using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using az.security;

namespace az.tweetstore.ftp.adapter
{
    /// <summary>
    /// General, easy-to-use FTP class.
    /// </summary>
    public class FtpClient
    {
        protected FtpDirectory _host;

        /// <summary>
        /// Gets or sets the current FTP domain and optional directory
        /// </summary>
        public string Host
        {
            set { _host.SetUrl(value); }
            get { return _host.GetUrl(); }
        }

        /// <summary>
        /// Gets or sets the login username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Indicates if the current directory is the
        /// root directory.
        /// </summary>
        public bool IsRootDirectory
        {
            get { return _host.IsRootDirectory; }
        }

        // Construction
        public FtpClient() { _host = new FtpDirectory(); }
        public FtpClient(string host, Token token) : this()
        {
            Host = host;
            Username = token.Key;
            Password = token.Secret;
        }


        /// <summary>
        /// Returns a directory listing of the current working directory.
        /// </summary>
        public List<FtpDirectoryEntry> ListDirectory()
        {
            var request = GetRequest();
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            string listing;
            using (var response = request.GetResponse() as FtpWebResponse)
            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                listing = sr.ReadToEnd();
            }
            return ParseDirectoryListing(listing);
        }

        /// <summary>
        /// Changes the current working directory. If directory starts with "/" then it
        /// is relative to the root directory. If directory is ".." then it refers to
        /// the parent directory.</param>
        /// </summary>
        /// <param name="directory">Directory to make active.</param>
        public void ChangeDirectory(string directory)
        {
            _host.CurrentDirectory = directory;
        }

        /// <summary>
        /// Indicates if the specified directory exists. This function returns false
        /// if a filename existing with the given name.
        /// </summary>
        /// <param name="directory">Directory to test. May be relative or absolute.</param>
        /// <returns></returns>
        public bool DirectoryExists(string directory)
        {
            try
            {
                var request = GetRequest(directory);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                using (var response = request.GetResponse() as FtpWebResponse)
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                {
                    if (sr.ReadLine().Trim() == "") return false;
                }
                return true;
            }
            catch {}
            return false;
        }

        /// <summary>
        /// Creates the specified directory. This method will create multiple levels of
        /// subdirectories as needed.
        /// </summary>
        /// <param name="directory">Directory to create. May be relative or absolute.</param>
        public void CreateDirectory(string directory)
        {
            // Get absolute directory
            directory = _host.ApplyDirectory(directory);

            // Split into path components
            var steps = directory.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // Build list of full paths to each path component
            var paths = new List<string>();
            for (var i = 1; i <= steps.Length; i++)
                paths.Add(FtpDirectory.ForwardSlash + String.Join(FtpDirectory.ForwardSlash, steps, 0, i));

            // Find first path component that needs creating
            int createIndex;
            for (createIndex = paths.Count-1; createIndex >= 0; createIndex--)
                if (DirectoryExists(paths[createIndex]))
                    break;

            // Created needed paths
            for (createIndex++; createIndex >= 0 && createIndex < paths.Count; createIndex++)
            {
                var request = GetRequest(paths[createIndex]);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                var response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// Uploads the given list of files to the current working directory.
        /// </summary>
        /// <param name="paths">List of local files to upload</param>
        public void UploadFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                FtpWebRequest request = GetRequest(Path.GetFileName(path));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;

                FileInfo info = new FileInfo(path);
                request.ContentLength = info.Length;

                // Create buffer for file contents
                int buffLength = 16384;
                byte[] buff = new byte[buffLength];

                // Upload this file
                using (FileStream instream = info.OpenRead())
                {
                    using (Stream outstream = request.GetRequestStream())
                    {
                        int bytesRead = instream.Read(buff, 0, buffLength);
                        while (bytesRead > 0)
                        {
                            outstream.Write(buff, 0, bytesRead);
                            bytesRead = instream.Read(buff, 0, buffLength);
                        }
                        outstream.Close();
                    }
                    instream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// Downloads the given list of files to the specified local target path
        /// </summary>
        /// <param name="path">Location where downloaded files will be saved</param>
        /// <param name="files">Names of files to download from current FTP directory</param>
        public void DownloadFiles(string path, params string[] files)
        {
            foreach (string file in files)
            {
                FtpWebRequest request = GetRequest(file);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;

                using (FileStream outstream = new FileStream(Path.Combine(path, file), FileMode.Create))
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    using (Stream instream = response.GetResponseStream())
                    {
                        int buffLength = 16384;
                        byte[] buffer = new byte[buffLength];

                        int bytesRead = instream.Read(buffer, 0, buffLength);
                        while (bytesRead > 0)
                        {
                            outstream.Write(buffer, 0, bytesRead);
                            bytesRead = instream.Read(buffer, 0, buffLength);
                        }
                        instream.Close();
                    }
                    outstream.Close();
                    response.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the given list of files from the current working directory.
        /// </summary>
        /// <param name="files">List of files to delete.</param>
        public void DeleteFiles(params string[] files)
        {
            foreach (string file in files)
            {
                FtpWebRequest request = GetRequest(file);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// Deletes the specified directory. The directory should be empty.
        /// </summary>
        /// <param name="files">Directory to delete.</param>
        public void DeleteDirectory(string directory)
        {
            FtpWebRequest request = GetRequest(directory);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        #region Protected helper methods

        // Constructs an FTP web request
        protected FtpWebRequest GetRequest()
        {
            return GetRequest("");
        }

        // Constructs an FTP web request with the given filename
        protected FtpWebRequest GetRequest(string filename)
        {
            var url = _host.GetUrl(filename);
            var request = WebRequest.Create(url) as FtpWebRequest;
            request.Credentials = new NetworkCredential(Username, Password);
            request.Proxy = null;
            request.KeepAlive = false;
            return request;
        }

        delegate FtpDirectoryEntry ParseLine(string lines);

        // Converts a directory listing to a list of FtpDirectoryEntrys
        protected List<FtpDirectoryEntry> ParseDirectoryListing(string listing)
        {
            ParseLine parseFunction = null;
            List<FtpDirectoryEntry> entries = new List<FtpDirectoryEntry>();
            string[] lines = listing.Split('\n');
            FtpDirectoryFormats format = GuessDirectoryFormat(lines);

            if (format == FtpDirectoryFormats.Windows)
                parseFunction = ParseWindowsDirectoryListing;
            else if (format == FtpDirectoryFormats.Unix)
                parseFunction = ParseUnixDirectoryListing;

            if (parseFunction != null)
            {
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        FtpDirectoryEntry entry = parseFunction(line);
                        if (entry.Name != "." && entry.Name != "..")
                            entries.Add(entry);
                    }
                }
            }
            return entries; ;
        }

        // Attempts to determine the directory format.
        protected FtpDirectoryFormats GuessDirectoryFormat(string[] lines)
        {
            foreach (string s in lines)
            {
                if (s.Length > 10 && Regex.IsMatch(s.Substring(0, 10),
                    "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FtpDirectoryFormats.Unix;
                }
                else if (s.Length > 8 && Regex.IsMatch(s.Substring(0, 8),
                    "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FtpDirectoryFormats.Windows;
                }
            }
            return FtpDirectoryFormats.Unknown;
        }

        // Parses a line from a Windows-format listing
        // 
        // Assumes listing style as:
        // 02-03-04  07:46PM       <DIR>          Append
        protected FtpDirectoryEntry ParseWindowsDirectoryListing(string text)
        {
            FtpDirectoryEntry entry = new FtpDirectoryEntry();

            text = text.Trim();
            string dateStr = text.Substring(0, 8);
            text = text.Substring(8).Trim();
            string timeStr = text.Substring(0, 7);
            text = text.Substring(7).Trim();
            entry.CreateTime = DateTime.Parse(String.Format("{0} {1}", dateStr, timeStr));
            if (text.Substring(0, 5) == "<DIR>")
            {
                entry.IsDirectory = true;
                text = text.Substring(5).Trim();
            }
            else
            {
                entry.IsDirectory = false;
                int pos = text.IndexOf(' ');
                entry.Size = Int64.Parse(text.Substring(0, pos));
                text = text.Substring(pos).Trim();
            }
            entry.Name = text;  // Rest is name

            return entry;
        }

        // Parses a line from a UNIX-format listing
        // 
        // Assumes listing style as:
        // dr-xr-xr-x   1 owner    group               0 Nov 25  2002 bussys
        protected FtpDirectoryEntry ParseUnixDirectoryListing(string text)
        {
            // Assuming record style as
            // dr-xr-xr-x   1 owner    group               0 Nov 25  2002 bussys
            FtpDirectoryEntry entry = new FtpDirectoryEntry();
            string processstr = text.Trim();
            entry.Flags = processstr.Substring(0, 9);
            entry.IsDirectory = (entry.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            CutSubstringWithTrim(ref processstr, ' ', 0);   //skip one part
            entry.Owner = CutSubstringWithTrim(ref processstr, ' ', 0);
            entry.Group = CutSubstringWithTrim(ref processstr, ' ', 0);
            CutSubstringWithTrim(ref processstr, ' ', 0);   //skip one part
            entry.CreateTime = DateTime.Parse(CutSubstringWithTrim(ref processstr, ' ', 8));
            entry.Name = processstr;   //Rest of the part is name
            return entry;
        }

        // Removes the token ending in the specified character
        protected string CutSubstringWithTrim(ref string s, char c, int startIndex)
        {
            int pos = s.IndexOf(c, startIndex);
            if (pos < 0) pos = s.Length;
            string retString = s.Substring(0, pos);
            s = (s.Substring(pos)).Trim();
            return retString;
        }

        #endregion

    }
}
