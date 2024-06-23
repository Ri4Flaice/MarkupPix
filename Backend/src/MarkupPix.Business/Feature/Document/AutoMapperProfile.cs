using AutoMapper;

using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Feature.Document;

/// <inheritdoc />
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="AutoMapperProfile"/>.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<CreateDocumentRequest, DocumentEntity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.File, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.DocumentName, o => o.MapFrom(s => s.DocumentName))
            .ForMember(d => d.DocumentDescription, o => o.MapFrom(s => s.DocumentDescription))
            .ForMember(d => d.NumberPages, o => o.MapFrom(s => s.NumberPages))
            .ForMember(d => d.Pages, o => o.Ignore());
    }
}