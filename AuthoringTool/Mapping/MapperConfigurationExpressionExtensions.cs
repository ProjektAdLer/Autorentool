using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace AuthoringTool.Mapping;

public static class MapperConfigurationExpressionExtensions
{
    static MapperConfigurationExpressionExtensions()
    {
        ConfigureCollectionMappersAdded = new List<IMapperConfigurationExpression>();
    }
    private static readonly List<IMapperConfigurationExpression> ConfigureCollectionMappersAdded;
    
    /// <summary>
    /// Adds collection mappers to the configuration only once.
    /// </summary>
    /// <param name="cfg">The configuration expression.</param>
    public static void AddCollectionMappersOnce(this IMapperConfigurationExpression cfg)
    {
        if (ConfigureCollectionMappersAdded.Contains(cfg)) return;
        cfg.AddCollectionMappers();
        ConfigureCollectionMappersAdded.Add(cfg);
    }
}