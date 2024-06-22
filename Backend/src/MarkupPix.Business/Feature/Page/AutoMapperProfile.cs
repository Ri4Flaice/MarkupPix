using AutoMapper;

using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Feature.Page;

/// <inheritdoc />
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="AutoMapperProfile"/>.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<CreatePageRequest, PageEntity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.DocumentId, o => o.Ignore())
            .ForMember(d => d.Document, o => o.Ignore())
            .ForMember(d => d.RecognizeUser, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.IsRecognize, o => o.Ignore())
            .ForMember(d => d.DateRecognize, o => o.Ignore())
            .ForMember(d => d.Page, o => o.Ignore())
            .ForMember(d => d.NumberPage, o => o.Ignore());

        CreateMap<UpdatePageRequest, PageEntity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.DocumentId, o => o.Ignore())
            .ForMember(d => d.IsRecognize, o => o.Ignore())
            .ForMember(d => d.RecognizeUser, o => o.Ignore())
            .ForMember(d => d.DateRecognize, o => o.Ignore())
            .ForMember(d => d.Page, o => o.Ignore())
            .ForMember(d => d.Document, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore());
    }
}