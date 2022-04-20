namespace MTGView.Core.Extensions;
public static class ExceptionExtensions
{
    /// <summary>
    /// Iterates through the provided <paramref name="exception" /> to retrieve the lowest level inner exception message
    /// </summary>
    /// <param name="exception"></param>
    /// <returns>The innermost message for a provided <see cref="Exception"/></returns>
    public static String GetInnermostExceptionMessage(this Exception exception)
    {
        return GetInnermostException(exception).Message;
    }

    /// <summary>
    /// Iterates through the provided <paramref name="exception"/> to retrieve the lowest level inner exception
    /// </summary>
    /// <param name="exception">The provided exception</param>
    /// <returns>The innermost <see cref="Exception"/></returns>
    public static Exception GetInnermostException(this Exception exception)
    {
        var innerExceptionReference = exception;

        while (innerExceptionReference.InnerException is not null)
        {
            if (exception.InnerException is null)
            {
                break;
            }

            innerExceptionReference = innerExceptionReference.InnerException;
        }

        return innerExceptionReference;
    }
}