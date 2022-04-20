namespace MTGView.Core.Responses;
#nullable disable

/// <summary>
/// Information relevant to our processing of Apis throughout the app
/// </summary>
public class ApiResponse
{
    /// <value>
    /// Indicates if the resulting operation was an Error, or a success
    /// </value>
    public OperationOutcome Outcome { get; set; }

    /// <value>
    /// Status code belonging to a particular response
    /// </value>
    public Int32 StatusCode { get; set; }
}

/// <summary>
/// Generic wrapper for <inheritdoc cref="ApiResponse"/>
/// </summary>
/// <typeparam name="T">The type returned by a successful outcome</typeparam>
public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }
}


