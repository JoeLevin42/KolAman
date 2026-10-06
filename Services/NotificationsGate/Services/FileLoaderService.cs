using NotificationsGate.Models;
using System.Text.Json;

namespace NotificationsGate.Services;

public class FileLoaderService
{
    public Alert? LoadMessageFile(string path)
    {
        try
        {
            var file = File.ReadAllText(path);

            var jsonFile = JsonSerializer.Deserialize<Alert>(file);
            return jsonFile;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}