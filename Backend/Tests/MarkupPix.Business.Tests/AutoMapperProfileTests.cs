using AutoMapper;

using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests;

/// <summary>
/// AutoMapper tests.
/// </summary>
[TestFixture]
public class AutoMapperProfileTests
{
    private IMapper _mapper;

    /// <summary>
    /// Test settings.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mapper = MockHelper.CreateMapper();
    }

    /// <summary>
    /// A test of the correctness of the configuration.
    /// </summary>
    [Test]
    public void AssertConfigurationIsValidMustPass()
    {
        // Act
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}