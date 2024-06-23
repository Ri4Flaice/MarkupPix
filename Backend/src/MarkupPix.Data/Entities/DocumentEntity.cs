using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MarkupPix.Core.Document;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Data.Entities;

/// <summary>
/// Document table.
/// </summary>
[Table("documents")]
[Comment("Documents data")]
public class DocumentEntity : IDocument
{
    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(User))]
    [Comment("User id")]
    public long UserId { get; set; }

    /// <inheritdoc />
    [MaxLength(50)]
    [Comment("Document name")]
    public string? DocumentName { get; set; }

    /// <inheritdoc />
    [Comment("Number of pages")]
    public int NumberPages { get; set; }

    /// <inheritdoc />
    [MaxLength(100)]
    [Comment("Document description")]
    public string? DocumentDescription { get; set; }

    /// <inheritdoc />
    [Comment("Document")]
    public byte[]? File { get; set; }

    /// <summary>
    /// A navigation property for the user.
    /// </summary>
    public UserEntity? User { get; set; }

    /// <summary>
    /// A navigation property for pages.
    /// </summary>
    public List<PageEntity>? Pages { get; set; }
}