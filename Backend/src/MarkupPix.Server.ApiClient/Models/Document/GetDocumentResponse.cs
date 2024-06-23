namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Get document response.
/// </summary>
public class GetDocumentResponse
{
    /// <summary>
    /// User's email address.
    /// </summary>
    public string? UserEmailAddress { get; set; }

    /// <summary>
    /// Document name.
    /// </summary>
    public string? DocumentName { get; set; }

    /// <summary>
    /// Document description.
    /// </summary>
    public string? DocumentDescription { get; set; }

    /// <summary>
    /// Number pages.
    /// </summary>
    public int NumberPages { get; set; }

    /// <summary>
    /// File.
    /// </summary>
    public byte[]? File { get; set; }

    /// <summary>
    /// Pages of the document.
    /// </summary>
    public List<GetPageResponse>? Pages { get; set; }
}