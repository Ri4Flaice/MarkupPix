using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MarkupPix.Core.Document;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Data.Entities;

/// <summary>
/// Page table.
/// </summary>
[Table("pages")]
[Comment("Pages data")]
public class PageEntity : IPage
{
    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(Document))]
    [Comment("Document id")]
    public long DocumentId { get; set; }

    /// <inheritdoc />
    [Comment("Indicates that the page is recognized")]
    public bool? IsRecognize { get; set; }

    /// <inheritdoc />
    [ForeignKey(nameof(User))]
    [Comment("The user who recognized the page.")]
    public long? RecognizeUser { get; set; }

    /// <inheritdoc />
    [Comment("Date of recognition")]
    public DateTime? DateRecognize { get; set; }

    /// <inheritdoc />
    [Comment("Page")]
    public byte[]? Page { get; set; }

    /// <summary>
    /// A navigation property for the document.
    /// </summary>
    public DocumentEntity? Document { get; set; }

    /// <summary>
    /// A navigation property for the user.
    /// </summary>
    public UserEntity? User { get; set; }
}