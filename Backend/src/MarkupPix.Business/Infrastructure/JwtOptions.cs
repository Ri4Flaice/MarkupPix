namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// JWT options.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The service that issued the token.
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// The scope of the token.
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Secret key.
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// The period of operation of the token.
    /// </summary>
    public int ExpiresHours { get; set; }
}