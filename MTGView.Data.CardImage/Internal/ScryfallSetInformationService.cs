
namespace MTGView.Data.Scryfall.Internal;

internal class ScryfallSetInformationService : IScryfallSetInformationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ILogger<ScryfallSetInformationService> _logger;

    public ScryfallSetInformationService(IHttpClientFactory httpClientFactory, ILogger<ScryfallSetInformationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<ScryfallSetDetails>>> GetAllSetsAsync(CancellationToken cancellationToken = default)
    {
        var apiResponse = new ApiResponse<IEnumerable<ScryfallSetDetails>>();

        try
        {
            using var client = _httpClientFactory.CreateClient("scryfallSetContext");


            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            using var response = await client.SendAsync(requestMessage, cancellationToken);

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var scryfallSetRootInformation = await responseStream.DeserializeFromStreamAsync<ScryfallSetRootInformation>(cancellationToken);

            if (scryfallSetRootInformation is not null && scryfallSetRootInformation.Data.Any())
            {
                apiResponse.Data = scryfallSetRootInformation.Data;
            }

            apiResponse.Outcome = OperationOutcome.SuccessfulOutcome;
        }
        catch (HttpRequestException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving Sets from scryfall, Exception was: {@ex}", ex);

        }
        catch (HttpListenerException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving Sets from scryfall, Exception was: {@ex}", ex);
        }
        catch (JsonException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving Sets from scryfall, Exception was: {@ex}", ex);
        }
        catch (NullReferenceException ex)
        {
            var errors = new List<String>(2);

            var innermostExceptionMessage = ex.GetInnermostExceptionMessage();

            errors.Add(ex.Message);
            errors.Add(innermostExceptionMessage);

            apiResponse.Outcome = OperationOutcome.ValidationFailureOutcome(errors, "Could not get Sets deserialized");
        }
        catch (Exception ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving Sets from scryfall, Exception was: {@ex}", ex);
        }

        return apiResponse;
    }

}

