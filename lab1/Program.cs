using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab1
{
    static class Program
    {
        public static bool PrintInfo(string path)
        {
            if (File.Exists(path))
            {
                PrintFileInfo(path, 0);
            }
            else if (Directory.Exists(path))
            {
                PrintDirInfo(path, 0);
            }
            else
            {
                Console.WriteLine("Niepoprawna sciezka!");
                return false;
            }
            return true;
        }
        public static void PrintDirInfo(string path, int depth)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                for (int i = 0; i < depth; i++)
                {
                    Console.Write("    ");
                }
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                int filesAmount = (dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
                Console.WriteLine("{0} ({1}) {2} ", dirInfo.Name, filesAmount, dirInfo.GetRAHS());
                PrintDirInfo(dir, depth + 1);
            }

            foreach (var file in Directory.GetFiles(path))
            {
                PrintFileInfo(file, depth);
            }
        }
        public static void PrintFileInfo(string file, int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                Console.Write("    ");
            }
            FileInfo fileInfo = new FileInfo(file);
            Console.WriteLine("{0} {1} bytes {2}", Path.GetFileName(file), fileInfo.Length, fileInfo.GetRAHS());
        }
        public static string GetRAHS(this FileSystemInfo fsi)
        {
            string output = "";
            FileAttributes fileAttr = fsi.Attributes;
            if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                output += 'r';
            }
            else
            {
                output += '-';
            }
            if ((fileAttr & FileAttributes.Archive) == FileAttributes.Archive)
            {
                output += 'a';
            }
            else
            {
                output += '-';
            }
            if ((fileAttr & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                output += 'h';
            }
            else
            {
                output += '-';
            }
            if ((fileAttr & FileAttributes.System) == FileAttributes.System)
            {
                output += 's';
            }
            else
            {
                output += '-';
            }
            return output;
        }

        public static DateTime GetOldestFile(this DirectoryInfo dir)
        {
            DateTime oldest = new DateTime(2222, 2, 2);
            foreach (var d in dir.GetDirectories())
            {
                if (d.GetOldestFile() < oldest)
                {
                    oldest = d.GetOldestFile();
                }
                d.GetOldestFile();
            }
            foreach (var f in dir.GetFiles())
            {
                if (f.CreationTime < oldest)
                {
                    oldest = f.CreationTime;
                }
            }
            return oldest;
        }

        public static void CreateCollection(string path)
        {
            SortedDictionary<string, int> files = new SortedDictionary<string, int>(new FileNameComparer());
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                files.Add(file.Name, (int)file.Length);
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (var d in dir.GetDirectories())
                {
                    files.Add(d.Name, (d.GetFiles().Length + d.GetDirectories().Length));
                }
                foreach (var f in dir.GetFiles())
                {
                    files.Add(f.Name, (int)f.Length);
                }
            }

            foreach (var f in files)
            {
                Console.WriteLine("{0} -> {1}", f.Key, f.Value);
            }
        }
        static void Main(string[] args)
        {
            string path = args[0];
            PrintInfo(path);
            Console.WriteLine();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DateTime oldestFile = dirInfo.GetOldestFile();
            Console.WriteLine();
            Console.WriteLine("Najstarszy plik: {0}", oldestFile);
            Console.WriteLine();
            CreateCollection(path);    
        }
    }
}
