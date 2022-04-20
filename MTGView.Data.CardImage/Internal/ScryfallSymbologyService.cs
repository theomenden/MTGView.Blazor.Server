namespace MTGView.Data.Scryfall.Internal;

public class ScryfallSymbologyService: IScryfallSymbologyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ILogger<ScryfallSymbologyService> _logger;

    public ScryfallSymbologyService(IHttpClientFactory httpClientFactory, ILogger<ScryfallSymbologyService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<SymbologyDatum>>> GetAllSymbolsFromScryfall(CancellationToken cancellationToken = default)
    {
        var apiResponse = new ApiResponse<IEnumerable<SymbologyDatum>>();

        try
        {
            using var client = _httpClientFactory.CreateClient("scryfallSymbologyContext");

            
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            using var response = await client.SendAsync(requestMessage, cancellationToken);

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var scryfallSymbology = await responseStream.DeserializeFromStreamAsync<Symbology>(cancellationToken);

            if (scryfallSymbology is not null && scryfallSymbology.Data.Any())
            {
                apiResponse.Data = scryfallSymbology.Data;

                apiResponse.Outcome = OperationOutcome.SuccessfulOutcome;
            }
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

