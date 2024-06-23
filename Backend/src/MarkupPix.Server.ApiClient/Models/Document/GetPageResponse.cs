namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Get page response.
/// </summary>
public class GetPageResponse
{
    /// <summary>
    /// Number page.
    /// </summary>
    public int NumberPage { get; set; }

    /// <summary>
    /// Indicates that the page is recognized.
    /// </summary>
    public bool IsRecognize { get; set; }

    /// <summary>
    /// Use's email address.
    /// </summary>
    public string? UserEmailAddress { get; set; }

    /// <summary>
    /// Date of recognition.
    /// </summary>
    public DateTime DateRecognize { get; set; }

    /// <summary>
    /// Page.
    /// </summary>
    public byte[]? Page { get; set; }
}