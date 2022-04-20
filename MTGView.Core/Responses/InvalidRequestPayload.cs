namespace MTGView.Core.Responses;

/// <summary>
/// A holder that reflects the failure to process a model (command, query, etc) that cannot pass internal validation.
/// </summary>
public class InvalidRequestPayload
{
    public string Title { get; } = "One or more validation errors have occurred.";

    /// <value>
    /// A keyed collection of the validation failures on the model
    /// </value>
    public IDictionary<string, IEnumerable<string>> Errors { get; set; }
}

