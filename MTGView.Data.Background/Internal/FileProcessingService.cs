using System.IO.Compression;

namespace MTGView.Data.Background.Internal;

internal sealed class FileProcessingService : IUnzippingService
{
    private readonly IHttpClientFactory _mtgJsonClientFactory;
    private readonly ILogger<FileProcessingService> _logger;

    private const string AllPrintingsFileName = "AllPrintingsCSVFiles";
    private const string AllPrintingsCompressedFileName = $"{AllPrintingsFileName}{FileExtensions.ZipExtension}";
    private string _filePath = String.Empty;

    public FileProcessingService(IHttpClientFactory httpClientFactory, ILogger<FileProcessingService> logger)
    {
        _mtgJsonClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task InitiateFileDownloadProcess(CancellationToken cancellationToken = default)
    {
        using var client = _mtgJsonClientFactory.CreateClient("MtgJsonClient");

        using var message = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{AllPrintingsCompressedFileName}");

        using var response = await client.SendAsync(message, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            await using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            await DeserializeStreamToFileAsync(AllPrintingsCompressedFileName, content, cancellationToken);

            await UnzipDownloadedFile();
        }
    }

    private Task UnzipDownloadedFile()
    {
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(currentDirectory))
            {
                var directoryNotFound = new DirectoryNotFoundException("Could not find requested directory");
                
                _logger.LogError("{@ex}", directoryNotFound);
                
                return Task.FromException(directoryNotFound);
            }

            if (!File.Exists(_filePath))
            {
                var fileNotFound = new FileNotFoundException($"{_filePath} did not contain the file!");
                
                _logger.LogError("Could not process file: {@ex}", fileNotFound);

                return Task.FromException(fileNotFound);
            }

            ZipFile.ExtractToDirectory(_filePath, currentDirectory, true);
         
            File.Delete($"{currentDirectory}\\{AllPrintingsCompressedFileName}");
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception Occurred while extracting: {@ex}", ex);
        }

        return Task.CompletedTask;
    }

    private async Task DeserializeStreamToFileAsync(String fileName, Stream stream, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        if (!stream.CanRead)
        {
            var exception = new IOException($"Stream was unable to be read {nameof(stream)}");

            _logger.LogError("Could read stream into file for {fileName}: {@ex}", fileName, exception);

            await Task.FromException(exception);

            return;
        }

        var fileInfo = new FileInfo($"{fileName}");

        await using var fileStream = File.Create(fileInfo.FullName);

        stream.Seek(0, SeekOrigin.Begin);

        await stream.CopyToAsync(fileStream, cancellationToken);

        _filePath = fileInfo.FullName;
    }
}
