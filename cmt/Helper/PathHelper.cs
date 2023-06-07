using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace cmt.Helper
{
    public class PathHelper
    {
        public static string MakeFilenameValid(string filename)
        {
            if (filename == null)
                return "";

            if (filename.EndsWith("."))
                filename = Regex.Replace(filename, @"\.+$", "");

            if (filename.Length == 0)
                return "";

            if (filename.Length > 245)
                throw new PathTooLongException();

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return filename;
        }

        public static string MakeFoldernameValid(string foldername)
        {
            if (foldername == null)
                return "";

            if (foldername.EndsWith("."))
                foldername = Regex.Replace(foldername, @"\.+$", "");

            if (foldername.Length == 0)
                return "";

            if (foldername.Length > 245)
                throw new PathTooLongException();

            foreach (char c in System.IO.Path.GetInvalidPathChars())
            {
                foldername = foldername.Replace(c, '_');
            }

            return foldername;
        }
    }
}