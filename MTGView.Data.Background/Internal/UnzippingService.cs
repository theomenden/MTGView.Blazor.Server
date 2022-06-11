using System.IO.Compression;
using MTGView.Data.Background.Interfaces;

namespace MTGView.Data.Background.Internal;

internal sealed class UnzippingService: IUnzippingService
{
    private readonly IHttpClientFactory _mtgJsonClientFactory;
    private readonly ILogger<UnzippingService> _logger;

    private const string CompressedExtension = ".zip";
    private const string AllPrintingsFileName = "AllPrintings.json";
    private const string CompleteFileName = $"{AllPrintingsFileName}{CompressedExtension}";

    private string _filePath = String.Empty;

    public UnzippingService(IHttpClientFactory httpClientFactory, ILogger<UnzippingService> logger)
    {
        _mtgJsonClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task InitiateFileDownloadProcess(CancellationToken cancellationToken = default)
    {
        using var client = _mtgJsonClientFactory.CreateClient("MtgJsonClient");

        using var message = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{CompleteFileName}");

        try
        {
            using var response = await client.SendAsync(message, cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

            await DeserializeStreamToFile(content);

            await UnzipDownloadedFile();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while attempting to download file {@ex}", ex);
        }
    }

    private Task UnzipDownloadedFile()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }

        try
        {
            ZipFile.ExtractToDirectory(_filePath, Directory.GetCurrentDirectory());

            File.Delete(CompleteFileName);
        }
        catch (DirectoryNotFoundException ex)
        {
            _logger.LogError("Could not find requested directory: {@ex}", ex);
        }
        catch (IOException ex)
        {
            _logger.LogError("Could not process file: {@ex}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception Occurred while extracting: {@ex}", ex);
        }

        return Task.CompletedTask;
    }

    private async Task DeserializeStreamToFile(Stream stream)
    {
        var fileInfo = new FileInfo($"{CompleteFileName}");

        await using var fileStream = File.Create(fileInfo.FullName);

        stream.Seek(0, SeekOrigin.Begin);

        await stream.CopyToAsync(fileStream);

        _filePath = fileInfo.FullName;
    }
}
