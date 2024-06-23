using System.ComponentModel.DataAnnotations;

namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Request to create a page.
/// </summary>
public class CreatePageRequest
{
    /// <summary>
    /// Document name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? DocumentName { get; set; }
}