

namespace MTGView.Data.Scryfall.Internal;

internal class ScryfallCardService : IScryfallCardService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ILogger<ScryfallCardService> _logger;

    public ScryfallCardService(IHttpClientFactory httpClientFactory, ILogger<ScryfallCardService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ApiResponse<ScryfallCard>> GetScryfallInformationAsync(Guid scryfallId, CancellationToken cancellationToken = default)
    {
        var apiResponse = new ApiResponse<ScryfallCard>();

        try
        {
            using var client = _httpClientFactory.CreateClient("scryfallCardContext");

            var requestUri = $"{client.BaseAddress}{scryfallId}";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using var response = await client.SendAsync(requestMessage, cancellationToken);

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            var scryfallCard = await responseStream.DeserializeFromStreamAsync<ScryfallCard>(cancellationToken);

            if (scryfallCard is not null)
            {
                apiResponse.Data = scryfallCard;

            }
        }
        catch (HttpRequestException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving card with scryfallId: {id}, Exception was: {@ex}", scryfallId, ex);

        }
        catch (HttpListenerException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving card with scryfallId: {id}, Exception was: {@ex}", scryfallId, ex);
        }
        catch (JsonException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving card with scryfallId: {id}, Exception was: {@ex}", scryfallId, ex);
        }
        catch (NullReferenceException ex)
        {
            var errors = new List<String>(2);

            var innermostExceptionMessage = ex.GetInnermostExceptionMessage();

            errors.Add(ex.Message);
            errors.Add(innermostExceptionMessage);

            apiResponse.Outcome = OperationOutcome.ValidationFailureOutcome(errors, "Could not get card deserialized");
        }
        catch (Exception ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving card with scryfallId: {id}, Exception was: {@ex}", scryfallId, ex);
        }

        return apiResponse;
    }

    public async IAsyncEnumerable<ApiResponse<ScryfallCard>> GetScryfallInformationAsync(IEnumerable<Guid> scryfallIds, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var scryfallId in scryfallIds)
        {
            Thread.Sleep(75);

            yield return await GetScryfallInformationAsync(scryfallId, cancellationToken);
        }
    }
}

