using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Dokan;
using WikiTools.Access;

namespace MediawikiFSAlpha
{
    class MWFS : DokanOperations
    {

        #region DokanOperations member

        private Wiki wiki;
        private Dictionary<string, string> TopDirectory;
        private Dictionary<string, Dictionary<string,int> > DirCache; 

        public MWFS(string serverUrl, string serverUser, string serverPassword)
        {
            wiki = new Wiki("http://"+serverUrl);
            if (!String.IsNullOrEmpty(serverUser))
            {
                wiki.Login(serverUser, serverPassword);
            }
            //Init directories
            TopDirectory = new Dictionary<string, string>();
            TopDirectory["Categories"] = "Categories";
            TopDirectory["Main namespace"] = "Main namespace";
            TopDirectory["Templates"] = "Templates";
            TopDirectory["Forms"] = "Forms";
            TopDirectory["Properties"] = "Properties";

            DirCache = new Dictionary<string, Dictionary<string, int>>();
            DirCache["Categories"] = new Dictionary<string, int>();
            DirCache["Main namespace"] = new Dictionary<string, int>();
            DirCache["Templates"] = new Dictionary<string, int>();
            DirCache["Forms"] = new Dictionary<string, int>();
            DirCache["Properties"] = new Dictionary<string, int>();
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {

            //Clean
            DirCache = new Dictionary<string, Dictionary<string, int>>();
            DirCache["Categories"] = new Dictionary<string, int>();
            DirCache["Main namespace"] = new Dictionary<string, int>();
            DirCache["Templates"] = new Dictionary<string, int>();
            DirCache["Forms"] = new Dictionary<string, int>();
            DirCache["Properties"] = new Dictionary<string, int>();

            return 0;
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            return 0;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            return -1;
        }

        public int CreateFile(
            string filename,
            System.IO.FileAccess access,
            System.IO.FileShare share,
            System.IO.FileMode mode,
            System.IO.FileOptions options,
            DokanFileInfo info)
        {
            return 0;
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            return -1;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            return -1;
        }

        public int FlushFileBuffers(
            string filename,
            DokanFileInfo info)
        {
            return -1;
        }

        public int FindFiles(
            string filename,
            System.Collections.ArrayList files,
            DokanFileInfo info)
        {
            //Root folder
            if (filename == "\\")
            {
                foreach (string name in TopDirectory.Keys)
                {
                    FileInformation finfo = new FileInformation();
                    finfo.FileName = name;
                    finfo.Attributes = System.IO.FileAttributes.Directory;
                    finfo.LastAccessTime = DateTime.Now;
                    finfo.LastWriteTime = DateTime.Now;
                    finfo.CreationTime = DateTime.Now;
                    files.Add(finfo);
                }
                return 0;
            }
            else
            {
                //Some directory
                Console.WriteLine("Filename: "+filename);
                switch (filename)
                {
                    case "\\Main namespace":
                        Dictionary<string, int> pages;
                        /*if (DirCache["Main namespace"].Any())
                        {
                            //Console.WriteLine("Load from cache");
                            pages = DirCache["Main namespace"];
                        }
                        else
                        {*/
                        pages = wiki.GetAllPagesEx("0", 100000, PageTypes.NonRedirects, 0);
                        //}
                        foreach (KeyValuePair<string,int> pair in pages)
                        {
                            FileInformation finfo = new FileInformation();
                            finfo.FileName = pair.Key.Replace(':', '_');
                            finfo.Attributes = System.IO.FileAttributes.Normal;
                            finfo.LastAccessTime = DateTime.Now;
                            finfo.LastWriteTime = DateTime.Now;
                            finfo.CreationTime = DateTime.Now;
                            finfo.Length = pair.Value;
                            files.Add(finfo);
                            DirCache["Main namespace"].Add(pair.Key,pair.Value);
                        }
                        break;
                    case "\\Templates":
                        //Get all main namespace pages
                        //Console.WriteLine("Getting pages...");
                        Dictionary<string, int> templates = wiki.GetAllPagesEx("0", 100000, PageTypes.NonRedirects, 10);
                        //Console.WriteLine("Pages got");
                        foreach (KeyValuePair<string, int> pair in templates)
                        {
                            FileInformation finfo = new FileInformation();
                            finfo.FileName = pair.Key.Replace(':', '_');
                            finfo.Attributes = System.IO.FileAttributes.Normal;
                            finfo.LastAccessTime = DateTime.Now;
                            finfo.LastWriteTime = DateTime.Now;
                            finfo.CreationTime = DateTime.Now;
                            finfo.Length = pair.Value;
                            files.Add(finfo);
                            //Console.WriteLine(pair.Key);
                        }
                        break;
                    case "\\Categories":
                        //Get all main namespace pages
                        //Console.WriteLine("Getting pages...");
                        Dictionary<string, int> cats = wiki.GetAllPagesEx("0", 100000, PageTypes.NonRedirects, 14);
                        //Console.WriteLine("Pages got");
                        foreach (KeyValuePair<string, int> pair in cats)
                        {
                            FileInformation finfo = new FileInformation();
                            finfo.FileName = pair.Key.Replace(':', '_');
                            finfo.Attributes = System.IO.FileAttributes.Normal;
                            finfo.LastAccessTime = DateTime.Now;
                            finfo.LastWriteTime = DateTime.Now;
                            finfo.CreationTime = DateTime.Now;
                            finfo.Length = pair.Value;
                            files.Add(finfo);
                            //Console.WriteLine(pair.Key);
                        }
                        break;
                    case "\\Forms":
                        //Get all main namespace pages
                        //Console.WriteLine("Getting pages...");
                        Dictionary<string, int> forms = wiki.GetAllPagesEx("0", 100000, PageTypes.NonRedirects, 106);
                        //Console.WriteLine("Pages got");
                        foreach (KeyValuePair<string, int> pair in forms)
                        {
                            FileInformation finfo = new FileInformation();
                            finfo.FileName = pair.Key.Replace(':', '_');
                            finfo.Attributes = System.IO.FileAttributes.Normal;
                            finfo.LastAccessTime = DateTime.Now;
                            finfo.LastWriteTime = DateTime.Now;
                            finfo.CreationTime = DateTime.Now;
                            finfo.Length = pair.Value;
                            files.Add(finfo);
                            //Console.WriteLine(pair.Key);
                        }
                        break;
                    case "\\Properties":
                        //Get all main namespace pages
                        //Console.WriteLine("Getting pages...");
                        Dictionary<string, int> props = wiki.GetAllPagesEx("0", 100000, PageTypes.NonRedirects, 102);
                        //Console.WriteLine("Pages got");
                        foreach (KeyValuePair<string, int> pair in props)
                        {
                            FileInformation finfo = new FileInformation();
                            finfo.FileName = pair.Key.Replace(':', '_');
                            finfo.Attributes = System.IO.FileAttributes.Normal;
                            finfo.LastAccessTime = DateTime.Now;
                            finfo.LastWriteTime = DateTime.Now;
                            finfo.CreationTime = DateTime.Now;
                            finfo.Length = pair.Value;
                            files.Add(finfo);
                            //Console.WriteLine(pair.Key);
                        }
                        break;
                    default:
                        return -1;
                        break;
                }
                return 0;
            }
        }


        public int GetFileInformation(
            string filename,
            FileInformation fileinfo,
            DokanFileInfo info)
        {
            //Root directory
            if (filename == "\\" || TopDirectory.ContainsKey( filename.Replace("\\","") ) )
            {
                fileinfo.Attributes = System.IO.FileAttributes.Directory | System.IO.FileAttributes.ReadOnly;
                fileinfo.LastAccessTime = DateTime.Now;
                fileinfo.LastWriteTime = DateTime.Now;
                fileinfo.CreationTime = DateTime.Now;

                return 0;
            }
            else
            {
                fileinfo.LastAccessTime = DateTime.Now;
                fileinfo.LastWriteTime = DateTime.Now;
                fileinfo.CreationTime = DateTime.Now;
                fileinfo.Length = 0;

                string[] nameParts = filename.Split('\\');
                if (nameParts.Count() < 1)
                {

                    return -1;
                }
                string rootFileName = nameParts[1].Trim();
                string fileNameLast = nameParts[nameParts.Count() - 1].Trim().Replace('_', ':');

                if (fileNameLast.StartsWith(".") || fileNameLast == "autorun.inf" || fileNameLast == "AutoRun.inf" || fileNameLast == "folder.jpg" || fileNameLast == "desktop.ini")
                {
                    return -1;
                }

                //Some directory or file
                if (wiki.CurrentUserInfo.CanEdit)
                {
                    fileinfo.Attributes = System.IO.FileAttributes.Normal;
                }
                else
                {
                    fileinfo.Attributes = System.IO.FileAttributes.Normal | System.IO.FileAttributes.ReadOnly;
                }

                Page p = new Page(wiki, fileNameLast);
                fileinfo.Length = p.Length;
                fileinfo.LastAccessTime = p.Touched.Date;
                fileinfo.LastWriteTime = p.Touched.Date;
                fileinfo.CreationTime = p.Touched.Date;
                
                Console.WriteLine("["+p.Touched+"] Got fileinfo for "+fileNameLast+" len: "+p.Length);

                return 0;
            }
        }

        public int LockFile(
            string filename,
            long offset,
            long length,
            DokanFileInfo info)
        {
            return 0;
        }

        public int MoveFile(
            string filename,
            string newname,
            bool replace,
            DokanFileInfo info)
        {
            return -1;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            return 0;
        }

        public int ReadFile(
            string filename,
            byte[] buffer,
            ref uint readBytes,
            long offset,
            DokanFileInfo info)
        {

            string[] nameParts = filename.Split('\\');
            string rootFileName = nameParts[1].Trim();
            string fileNameLast = nameParts[nameParts.Count()-1].Trim().Replace('_',':');

            if (fileNameLast.StartsWith(".") || fileNameLast == "folder.jpg" || fileNameLast == "desktop.ini")
            {
                return 0;
            }

            Console.WriteLine("Root: "+rootFileName);
            Console.WriteLine("Read file: "+filename);
            Console.WriteLine("offset:"+offset);
            Console.WriteLine("readBytes: "+readBytes);

            try
            {

                switch (rootFileName)
                {
                    case "Main namespace":
                    case "Categories":
                    case "Properties":
                    case "Forms":
                    case "Templates":

                        //Get page
                        Console.WriteLine("Opening page " + fileNameLast);

                        Page p = new Page(wiki, fileNameLast);
                        p.LoadInfo();
                        p.LoadText();

                        string pText = p.Text;
                        
                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms);
                        sw.Write(pText);
                        sw.Flush();
                        ms.Position = 0;
                        StreamReader sr = new StreamReader(ms);

                        //Read
                        sr.BaseStream.Seek(offset, SeekOrigin.Begin);
                        readBytes = (uint) sr.BaseStream.Read(buffer, 0, buffer.Length);

                        break;
                }

                return 0;

            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            return -1;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            return -1;
        }

        public int SetFileAttributes(
            string filename,
            System.IO.FileAttributes attr,
            DokanFileInfo info)
        {
            return -1;
        }

        public int SetFileTime(
            string filename,
            DateTime ctime,
            DateTime atime,
            DateTime mtime,
            DokanFileInfo info)
        {
            return -1;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            return 0;
        }

        public int Unmount(DokanFileInfo info)
        {
            return 0;
        }

        public int GetDiskFreeSpace(
           ref ulong freeBytesAvailable,
           ref ulong totalBytes,
           ref ulong totalFreeBytes,
           DokanFileInfo info)
        {
            freeBytesAvailable = 512 * 1024 * 1024;
            totalBytes = 1024 * 1024 * 1024;
            totalFreeBytes = 512 * 1024 * 1024;
            return 0;
        }

        public int WriteFile(
            string filename,
            byte[] buffer,
            ref uint writtenBytes,
            long offset,
            DokanFileInfo info)
        {

            //Extract file name and root directory
            string[] nameParts = filename.Split('\\');
            string rootFileName = nameParts[1].Trim();
            string fileNameLast = nameParts[nameParts.Count() - 1].Trim().Replace('_', ':');

            if (fileNameLast.StartsWith(".") || fileNameLast.EndsWith(".bak"))
            {
                Console.WriteLine("Skip creating temp file");
                return 0;
            }

            try
            {
                switch (rootFileName)
                {
                    case "Main namespace":
                    case "Categories":
                    case "Properties":
                    case "Forms":
                    case "Templates":

                        //Get page
                        Console.WriteLine("Writing page " + fileNameLast);
                        Page p = new Page(wiki, fileNameLast);

                        if (!wiki.CurrentUserInfo.CanEdit)
                        {
                            Console.WriteLine("User have no edit rights!");
                            return -1;
                        }

                        string pText = p.Text;
                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms);
                        sw.Write(pText);
                        sw.Flush();
                        ms.Position = 0;
                        StreamReader sr = new StreamReader(ms);

                        //Read
                        sr.BaseStream.Seek(offset, SeekOrigin.Begin);
                        writtenBytes = (uint)buffer.Length;
                        sr.BaseStream.Write(buffer, 0, buffer.Length);
                        sr.BaseStream.Seek(0, SeekOrigin.Begin);
                        pText = sr.ReadToEnd();

                        Console.WriteLine(pText);

                        p.PrepareToEdit();
                        p.SetText(pText);

                        break;
                }

                return 0;

            }
            catch (Exception)
            {
                return -1;
            }
        }

        #endregion

    }
}
