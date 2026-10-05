using System;
using System.IO;
using System.Text.Json;

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


         void OnCreated(object sender, FileSystemEventArgs e)
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
}//load  public class JsonLoader
{
    public List<JsonElement> ReadJson(string path)
{
    var json = File.ReadAllText(path);

    return JsonSerializer.Deserialize<List<JsonElement>>(json) ?? [];
}
}

//
foreach (var item in items)
{
    var message = item.GetRawText();

    // send message to Kafka or anywhere else 
}