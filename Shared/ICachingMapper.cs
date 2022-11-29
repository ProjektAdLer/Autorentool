namespace Shared;

public interface ICachingMapper
{
    void Map<T1, T2>( T1 entity, T2 viewModel);
}