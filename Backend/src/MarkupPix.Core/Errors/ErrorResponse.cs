using System.ComponentModel.DataAnnotations;

namespace MarkupPix.Core.Errors;

/// <summary>
/// The response model with error data.
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="ErrorResponse"/>.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="errorMessage">Error message.</param>
    public ErrorResponse(string? errorCode, string? errorMessage)
    {
        ErrorCode = errorCode ?? nameof(Errors.MPX500);
        ErrorMessage = errorMessage ?? nameof(Errors.MPX500);
    }

    /// <summary>
    /// Error code.
    /// </summary>
    [MaxLength(6)]
    public string ErrorCode { get; set; }

    /// <summary>
    /// Error message.
    /// </summary>
    [MaxLength(250)]
    public string ErrorMessage { get; set; }
}