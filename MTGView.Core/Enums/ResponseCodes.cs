namespace MTGView.Core.Enums;

/// <summary>
/// A list of codes that are used to help us determine how our Client to API Pipeline is behaving
/// </summary>
public enum ResponseCodes
{
    /// <summary>
    /// HttpError : An Error that occurs during the transmission of the request
    /// </summary>
    HttpError,

    /// <summary>
    /// ApiError : An Error that occurred from within processing the request in the api
    /// </summary>
    ApiError,

    /// <summary>
    /// UnrecognizedError : The api returned a error that we didn't recognize and were unable to process
    /// </summary>
    UnrecognizedError,

    /// <summary>
    /// UnintelligibleResponse: A non-error response from the api that we didn't recognize and were unable to process
    /// </summary>
    UnintelligibleResponse,

    /// <summary>
    /// ApiSuccess : A successful response
    /// </summary>
    ApiSuccess
}