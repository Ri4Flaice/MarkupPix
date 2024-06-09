namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// JWT options.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Secret key.
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// The period of operation of the token.
    /// </summary>
    public int ExpiresHours { get; set; }
}