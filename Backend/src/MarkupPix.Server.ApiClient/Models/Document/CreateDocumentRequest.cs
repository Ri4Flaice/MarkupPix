using System.ComponentModel.DataAnnotations;

namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Request to create a document.
/// </summary>
public class CreateDocumentRequest
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
    [Required]
    public int NumberPages { get; set; }

    /// <summary>
    /// Document description.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string? DocumentDescription { get; set; }
}