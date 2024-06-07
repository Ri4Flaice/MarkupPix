using AutoMapper;

using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

namespace MarkupPix.Business.Feature.User;

/// <inheritdoc />
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="AutoMapperProfile"/>.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<CreateUserRequest, UserEntity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.EmailAddress))
            .ForMember(d => d.Block, o => o.MapFrom(s => false))
            .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
            .ForMember(d => d.AccountType, o => o.MapFrom(s => s.AccountType))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.EmailAddress))
            .ForMember(d => d.NormalizedUserName, o => o.Ignore())
            .ForMember(d => d.Email, o => o.Ignore())
            .ForMember(d => d.NormalizedEmail, o => o.Ignore())
            .ForMember(d => d.EmailConfirmed, o => o.Ignore())
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.SecurityStamp, o => o.Ignore())
            .ForMember(d => d.ConcurrencyStamp, o => o.Ignore())
            .ForMember(d => d.PhoneNumber, o => o.Ignore())
            .ForMember(d => d.PhoneNumberConfirmed, o => o.Ignore())
            .ForMember(d => d.TwoFactorEnabled, o => o.Ignore())
            .ForMember(d => d.LockoutEnd, o => o.Ignore())
            .ForMember(d => d.LockoutEnabled, o => o.Ignore())
            .ForMember(d => d.AccessFailedCount, o => o.MapFrom(s => 0));

        CreateMap<UserEntity, GetUserResponse>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));
    }
}