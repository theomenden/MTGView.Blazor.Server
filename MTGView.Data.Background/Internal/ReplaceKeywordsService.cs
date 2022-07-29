using System.Runtime.CompilerServices;
using System.Text.Json;
using EFCore.BulkExtensions;

namespace MTGView.Data.Background.Internal;

internal sealed class ReplaceKeywordsService : IReplaceKeywordsService
{
    private readonly IDbContextFactory<MagicthegatheringDbContext> _dbContextFactory;

    private readonly IHttpClientFactory _mtgJsonClientFactory;

    private readonly ILogger<ReplaceKeywordsService> _logger;
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public ReplaceKeywordsService(IDbContextFactory<MagicthegatheringDbContext> dbContextFactory,
        IHttpClientFactory httpClientFactory, ILogger<ReplaceKeywordsService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _mtgJsonClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task DownloadKeywordsData(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing Keywords File starting at: {startTime}", DateTime.Now);

        using var client = _mtgJsonClientFactory.CreateClient("MtgJsonClient");

        using var message = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{FileNamesToProcess.Keywords}{FileExtensions.JsonExtension}");

        using var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Could not process response from MTGJson - {@response}", response);
        }

        await using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

        await ClearKeywords(cancellationToken);

        await ProcessStreamToKeywordsAsync(content, cancellationToken);

        File.Delete($"{FileNamesToProcess.Keywords}{ FileExtensions.JsonExtension}");

        _logger.LogInformation("Finished processing and deleted Keywords File at: {endTime}", DateTime.Now);
    }

    private async Task ProcessStreamToKeywordsAsync(Stream? stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            _logger.LogError("Provided stream was unreadable {methodName}", nameof(ProcessStreamToKeywordsAsync));
            return;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<RootKeywordsData>(stream, JsonSerializerOptions, cancellationToken);
        
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var keywords = ConvertKeywordRootToKeywords(searchResult?.Data)
            .ToArray();

        var bulkConfig = new BulkConfig
        {
            BatchSize = 2000,
            EnableStreaming = true
        };

        await context.BulkInsertOrUpdateAsync(keywords, bulkConfig, null, typeof(Keyword), cancellationToken);

        await context.BulkSaveChangesAsync(bulkConfig, null,cancellationToken);
    }

    private async Task ClearKeywords( CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await context.Database.ExecuteSqlRawAsync($"Truncate Table {AnnotationHelper.TableName(context.Keywords)}", cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<Keyword> ConvertKeywordRootToKeywords(Keywords? keywords)
    {
        const string abilityWords = nameof(Keywords.AbilityWords);
        const string keywordAbilities = nameof(Keywords.KeywordAbilities);
        const string keywordActions = nameof(Keywords.KeywordActions);

        if (keywords is null)
        {
            return Enumerable.Empty<Keyword>();
        }

        var abilityWordsList = keywords.AbilityWords.Select(keyword => new Keyword { Name = keyword, RecordType = abilityWords });
        var keywordAbilitiesList = keywords.KeywordAbilities.Select(keyword => new Keyword { Name = keyword, RecordType = keywordAbilities });
        var keywordActionsList = keywords.KeywordActions.Select(keyword => new Keyword { Name = keyword, RecordType = keywordActions });

        return abilityWordsList.Concat(keywordAbilitiesList).Concat(keywordActionsList).ToList();
    }
}