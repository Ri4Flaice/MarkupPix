using System.ComponentModel.DataAnnotations;

namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Update document request.
/// </summary>
public class UpdateDocumentRequest
{
    /// <summary>
    /// Document name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? DocumentName { get; set; }

    /// <summary>
    /// Number of pages.
    /// </summary>
    public int? NumberPages { get; set; }

    /// <summary>
    /// Document description.
    /// </summary>
    [MaxLength(100)]
    public string? DocumentDescription { get; set; }
}