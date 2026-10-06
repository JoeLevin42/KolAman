
using NotificationsGate.Models;
using NotificationsGate.Services;
using System.Text.Json;

namespace YourProject.Services;

public class FileWatcherService  //IDisposable
{
    private readonly KafkaService _kafkaService;
    private readonly LoggerService _loggerService;
    private  FileSystemWatcher _watcher;

    public FileWatcherService(
        KafkaService kafkaService,
        LoggerService loggerService)
    {
        _kafkaService = kafkaService;
        _loggerService = loggerService;

        _watcher = new FileSystemWatcher("C:\\Users\\JL202\\Desktop\\KolAman\\alert-simulator\\alerts") // need to be changed
           
        {
            Filter = "*.ready",
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName,
            EnableRaisingEvents = true
            
        };
        _watcher.InternalBufferSize = 65536; // NEED TO CHECK THIS
        _watcher.Created += OnCreated;
    }

    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        await ProcessFileAsync(e.FullPath);
        Console.WriteLine($"Proccessed! {e.FullPath}");
    }

    private async Task ProcessFileAsync(string readyFilePath)
    {
        try
        {
            
            string jsonFilePath = Path.Combine(
                Path.GetDirectoryName(readyFilePath)!,
                Path.GetFileNameWithoutExtension(readyFilePath) + ".json");

            if (!File.Exists(jsonFilePath))
            {
                return;
            }

            string json = await File.ReadAllTextAsync(jsonFilePath);

            //validation
            JsonDocument.Parse(json);
            var jsonObj = JsonSerializer.Deserialize<Alert>(json);
            if (jsonObj == null)
            {
                throw new Exception();
            }

            await _kafkaService.SendToKafkaAsync(jsonObj); //this is for vlidation

            await _loggerService.LogAsync("INFO", 
                $"Processed alert: {jsonFilePath}");
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("ERROR",
                $"Error processing {readyFilePath}: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _watcher.Created -= OnCreated;
        _watcher.Dispose();
    }
}

