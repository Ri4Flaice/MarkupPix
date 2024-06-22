using System.ComponentModel.DataAnnotations;

namespace MarkupPix.Server.ApiClient.Models.Document;

/// <summary>
/// Request to update a page.
/// </summary>
public class UpdatePageRequest
{
    /// <summary>
    /// Document name.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? DocumentName { get; set; }

    /// <summary>
    /// Number page.
    /// </summary>
    [Required]
    public int NumberPage { get; set; }

    /// <summary>
    /// User's email address.
    /// </summary>
    [Required]
    public string? UserEmailAddress { get; set; }
}