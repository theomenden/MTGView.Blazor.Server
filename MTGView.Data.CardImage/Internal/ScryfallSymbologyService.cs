using Microsoft.Extensions.Options;

namespace MTGView.Data.Scryfall.Internal;

public sealed class ScryfallSymbologyService: ApiServiceBase<Symbology>
{
    private readonly ILogger<ScryfallSymbologyService> _logger;

    private const string SymbologyEndpoint = $"symbology/";

    public ScryfallSymbologyService(IHttpClientFactory httpClientFactory, IOptions<HttpClientConfiguration> options, ILogger<ScryfallSymbologyService> logger)
    :base(httpClientFactory, options)
    {
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<SymbologyDatum>>> GetAllSymbolsFromScryfall(CancellationToken cancellationToken = default)
    {
        var apiResponse = new ApiResponse<IEnumerable<SymbologyDatum>>();

        try
        {
            var content = await GetContentAsync(SymbologyEndpoint, cancellationToken);
            apiResponse.Outcome = OperationOutcome.SuccessfulOutcome;
            apiResponse.Data = content.Data.Data;
        }
        catch (HttpRequestException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}", ex);

        }
        catch (HttpListenerException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}",  ex);
        }
        catch (JsonException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}",  ex);
        }
        catch (NullReferenceException ex)
        {
            var errors = new List<String>(2);

            var innermostExceptionMessage = ex.GetInnermostExceptionMessage();

            errors.Add(ex.Message);
            errors.Add(innermostExceptionMessage);

            apiResponse.Outcome = OperationOutcome.ValidationFailureOutcome(errors, "Could not get symbols deserialized");
        }
        catch (Exception ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}",  ex);
        }

        return apiResponse;
    }
}

