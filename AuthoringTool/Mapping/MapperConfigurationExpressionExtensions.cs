using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace AuthoringTool.Mapping;

public static class MapperConfigurationExpressionExtensions
{
    static MapperConfigurationExpressionExtensions()
    {
        CollectionMappersAdded = new List<IMapperConfigurationExpression>();
    }
    private static readonly List<IMapperConfigurationExpression> CollectionMappersAdded;
    
    /// <summary>
    /// Adds collection mappers to the configuration only once.
    /// </summary>
    /// <param name="cfg">The configuration expression.</param>
    public static void AddCollectionMappersOnce(this IMapperConfigurationExpression cfg)
    {
        if (CollectionMappersAdded.Contains(cfg)) return;
        cfg.AddCollectionMappers();
        CollectionMappersAdded.Add(cfg);
    }
}