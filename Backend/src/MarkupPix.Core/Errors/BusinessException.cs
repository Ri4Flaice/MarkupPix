namespace MarkupPix.Core.Errors;

/// <summary>
/// An error message in the business logic.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="BusinessException"/>.
    /// </summary>
    /// <param name="innerException">Inner exception.</param>
    /// <param name="errorNumber">Error number.</param>
    /// <param name="message">Error message.</param>
    public BusinessException(Exception? innerException, string errorNumber, string message)
        : base(message, innerException)
    {
        ErrorNumber = errorNumber;
    }

    /// <summary>
    /// Initializes a new instance of the class <see cref="BusinessException"/>.
    /// </summary>
    /// <param name="errorNumber">Error number.</param>
    /// <param name="message">Error message.</param>
    public BusinessException(string errorNumber, string message)
        : this(null, errorNumber, message)
    {
    }

    /// <summary>
    /// Error number (string).
    /// </summary>
    public string ErrorNumber { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string ErrorText => base.Message;
}