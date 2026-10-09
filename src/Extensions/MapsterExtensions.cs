// MapsterExtensions.cs

using Mapster;

namespace Yggdrasil.Extensions;

/// <summary>Shared Mapster helpers</summary>
public static class MapsterExtensions{
    private static readonly TypeAdapterConfig PatchConfig = CreatePatchConfig();

    private static TypeAdapterConfig CreatePatchConfig(){
        var config = new TypeAdapterConfig();
        config.Default.IgnoreNullValues(true);
        return config;
    }

    /// <summary>Copies non-null values from <paramref name="source"/> to <paramref name="destination"/> ignoring null values</summary>
    /// <param name="source">Object holding the new values</param>
    /// <param name="destination">Existing object to update</param>
    /// <returns>The updated destination</returns>
    public static TDestination Patch<TSource, TDestination>(this TSource source, TDestination destination){
        return source.Adapt(destination, PatchConfig);
    }
}
