using MTGView.Core.Enums;

namespace MTGView.Core.Responses;

public class OperationOutcome
{
    public OperationOutcome()
    {
        Message = string.Empty;
        CorrelationId = string.Empty;
        Errors = Enumerable.Empty<string>();
    }

    public OperationResult OperationResult { get; set; }

    public string CorrelationId { get; set; }

    public string Message { get; set; }

    public bool IsError { get; set; }

    public bool IsValidationFailure { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public static OperationOutcome SuccessfulOutcome => new()
    {
        Errors = Enumerable.Empty<string>(),
        CorrelationId = string.Empty,
        IsError = false,
        IsValidationFailure = false,
        Message = string.Empty,
        OperationResult = OperationResult.Success
    };

    public static OperationOutcome UnsuccessfulOutcome => new()
    {
        Errors = Enumerable.Empty<string>(),
        CorrelationId = string.Empty,
        IsError = true,
        IsValidationFailure = false,
        Message = string.Empty,
        OperationResult = OperationResult.Failure
    };

    public static OperationOutcome ValidationFailureOutcome(IEnumerable<string> errors, string message = null) => new()
    {
        Errors = errors ?? Enumerable.Empty<string>(),
        CorrelationId = string.Empty,
        IsError = false,
        IsValidationFailure = true,
        Message = message ?? string.Empty,
        OperationResult = OperationResult.Failure
    };
}

