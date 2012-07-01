using System;

namespace az.tweetstore.ftp.adapter
{
    /// <summary>
    /// Helper class for managing current FTP directory.
    /// </summary>
    public class FtpDirectory
    {
        // Static members
        protected static char[] _slashes = { '/', '\\' };
        public static string BackSlash = "\\";
        public static string ForwardSlash = "/";

        // Member variables
        protected string _domain;    // No trailing slash
        protected string _cwd;        // Leading, no trailing slash
        public string Domain { get { return _domain; } }

        // Construction
        public FtpDirectory()
        {
            _domain = String.Empty;
            _cwd = ForwardSlash;    // Root directory
        }

        /// <summary>
        /// Determines if the current directory is the root directory.
        /// </summary>
        public bool IsRootDirectory
        {
            get { return _cwd == ForwardSlash; }
        }

        /// <summary>
        /// Gets or sets the current FTP directory.
        /// </summary>
        public string CurrentDirectory
        {
            get { return _cwd; }
            set { _cwd = ApplyDirectory(value); }
        }

        /// <summary>
        /// Sets the domain and current directory from a URL.
        /// </summary>
        /// <param name="url">URL to set to</param>
        public void SetUrl(string url)
        {
            // Separate domain from directory
            int pos = url.IndexOf("://");
            pos = url.IndexOfAny(_slashes, (pos < 0) ? 0 : pos + 3);
            if (pos < 0)
            {
                _domain = url;
                _cwd = ForwardSlash;
            }
            else
            {
                _domain = url.Substring(0, pos);
                // Normalize directory string
                _cwd = ApplyDirectory(url.Substring(pos));
            }
        }

        /// <summary>
        /// Returns the domain and current directory as a URL.
        /// </summary>
        public string GetUrl()
        {
            return GetUrl(String.Empty);
        }

        /// <summary>
        /// Returns the domain and specified directory as a URL.
        /// </summary>
        /// <param name="directory">Partial directory or filename applied to the 
        /// current working directory.</param>
        public string GetUrl(string directory)
        {
            if (directory.Length == 0)
                return _domain + _cwd;
            return _domain + ApplyDirectory(directory);
        }

        /// <summary>
        /// Applies the given directory to the current directory and returns the
        /// result.
        /// 
        /// If directory starts with "/", it replaces all of the current directory.
        /// If directory is "..", the top-most subdirectory is removed from
        /// the current directory.
        /// </summary>
        /// <param name="directory">The directory to apply</param>
        public string ApplyDirectory(string directory)
        {
            // Normalize directory
            directory = directory.Trim();
            directory = directory.Replace(BackSlash, ForwardSlash);
            directory = directory.TrimEnd(_slashes);

            if (directory == "..")
            {
                int pos = _cwd.LastIndexOf(ForwardSlash);
                return (pos <= 0) ? ForwardSlash : _cwd.Substring(0, pos);
            }
            else if (directory.StartsWith(ForwardSlash))
            {
                // Specifies complete directory path
                return directory;
            }
            else
            {
                // Relative to current directory
                if (_cwd == ForwardSlash)
                    return _cwd + directory;
                else
                    return _cwd + ForwardSlash + directory;
            }
        }

        /// <summary>
        /// Returns the domain and current directory as a URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetUrl();
        }
    }
}