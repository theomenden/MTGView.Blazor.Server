
using Microsoft.Extensions.Options;
using TheOmenDen.Shared.Configuration;
using TheOmenDen.Shared.Infrastructure;

namespace MTGView.Data.Scryfall.Internal;

public sealed class ScryfallSetInformationService : ApiServiceBase<ScryfallSetRootInformation>
{
    private readonly ILogger<ScryfallSetInformationService> _logger;
    private const string SetsEndpoint = "sets/";

    public ScryfallSetInformationService(IHttpClientFactory httpClientFactory, IOptions<HttpClientConfiguration> options, ILogger<ScryfallSetInformationService> logger)
    : base(httpClientFactory, options)
    {
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<ScryfallSetDetails>>> GetSetDetailsAsync(CancellationToken cancellationToken = default)
    {
        var apiResponse = new ApiResponse<IEnumerable<ScryfallSetDetails>>();

        try
        {
           var content = await base.GetContentAsync(SetsEndpoint, cancellationToken);
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

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}", ex);
        }
        catch (JsonException ex)
        {
            apiResponse.Outcome = OperationOutcome.UnsuccessfulOutcome;

            apiResponse.Outcome.CorrelationId = Guid.NewGuid().ToString();

            apiResponse.Outcome.Message = ex.Message;

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}", ex);
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

            _logger.LogError("Failed retrieving symbols from scryfall, Exception was: {@ex}", ex);
        }

        return apiResponse;
    }

}

