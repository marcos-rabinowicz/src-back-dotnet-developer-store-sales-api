using AutoMapper;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

internal static class MapperFactory
{
    public static IMapper New(params Profile[] profiles)
    {
        var cfg = new MapperConfiguration(c =>
        {
            foreach (var p in profiles) c.AddProfile(p);
        });
        cfg.AssertConfigurationIsValid();
        return cfg.CreateMapper();
    }
}
