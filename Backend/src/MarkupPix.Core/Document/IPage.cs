namespace MarkupPix.Core.Document;

/// <summary>
/// Page data.
/// </summary>
public interface IPage
{
    /// <summary>
    /// Page id.
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// Document id.
    /// </summary>
    long DocumentId { get; set; }

    /// <summary>
    /// Indicates that the page is recognized.
    /// </summary>
    bool? IsRecognize { get; set; }

    /// <summary>
    /// The user who recognized the page.
    /// </summary>
    long? RecognizeUser { get; set; }

    /// <summary>
    /// Date of recognition.
    /// </summary>
    DateTime? DateRecognize { get; set; }

    /// <summary>
    /// Page.
    /// </summary>
    byte[]? Page { get; set; }
}