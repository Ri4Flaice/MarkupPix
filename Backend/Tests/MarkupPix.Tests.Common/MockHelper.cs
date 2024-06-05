using AutoMapper;

namespace MarkupPix.Tests.Common;

/// <inheritdoc />
public static class MockHelper
{
    /// <summary>
    /// Create AutoMapper with configuration.
    /// </summary>
    /// <returns>AutoMapper with configuration.</returns>
    public static IMapper CreateMapper()
    {
        var mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<Business.Feature.User.AutoMapperProfile>();
        });

        mapper.AssertConfigurationIsValid();
        return mapper.CreateMapper();
    }
}