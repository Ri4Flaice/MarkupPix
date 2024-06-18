namespace MarkupPix.Core.Document;

/// <summary>
/// Document data.
/// </summary>
public interface IDocument
{
    /// <summary>
    /// Document id.
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// User id.
    /// </summary>
    long UserId { get; set; }

    /// <summary>
    /// Document name.
    /// </summary>
    string? DocumentName { get; set; }

    /// <summary>
    /// Number of pages.
    /// </summary>
    int NumberPages { get; set; }

    /// <summary>
    /// Document description.
    /// </summary>
    string? DocumentDescription { get; set; }

    /// <summary>
    /// Document.
    /// </summary>
    byte[]? File { get; set; }
}