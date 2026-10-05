using System;
using System.IO;

namespace MyNamespace
{
    class MyClassCS
    {
        static void Main()
        {
            using var watcher = new FileSystemWatcher("C:\\Users\\JL202\\Desktop\\KolAman\\alert-simulator\\alerts");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


            watcher.Created += OnCreated;

            watcher.Filter = "*.ready";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;


            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }


        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            string newPath = Path.ChangeExtension(value, ".json");
            Console.WriteLine(newPath);
            //Console.WriteLine(value);
            
           
            //try
            //{
            //    var file = File.ReadAllText(value);
            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

        }


    }
}