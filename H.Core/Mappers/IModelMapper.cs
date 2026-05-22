namespace H.Core.Mappers;

/// <summary>
/// Generic mapper interface for mapping from TSource to TDest.
/// Replaces AutoMapper's IMapper with strongly-typed, DI-friendly contracts.
/// </summary>
/// <remarks>
/// <typeparamref name="TSource"/> is contravariant so a mapper declared for a base/interface
/// source (e.g. <c>IModelMapper&lt;ICropDto, …&gt;</c>) can be used where a derived source is
/// expected (e.g. <c>IModelMapper&lt;CropDto, …&gt;</c>). <typeparamref name="TDest"/> is invariant
/// because the in-place <see cref="Map(TSource, TDest)"/> overload takes it as an input parameter.
/// </remarks>
public interface IModelMapper<in TSource, TDest>
{
    /// <summary>Creates a new <typeparamref name="TDest"/> from <paramref name="source"/>.</summary>
    TDest Map(TSource source);

    /// <summary>
    /// Copies <paramref name="source"/> onto an <b>existing</b> <paramref name="dest"/> instance
    /// (in-place update) rather than creating a new one. The default copies all matching properties
    /// via <see cref="PropertyMapper"/>; mappers that bridge differently-named properties
    /// (e.g. <c>CropDto.WetYield</c> ↔ <c>CropViewItem.Yield</c>) override this so the transfer/edit
    /// path applies the same mapping as the create path. Identity (<c>Guid</c>) is intentionally not
    /// copied here — an in-place update targets an object that already owns its Guid.
    /// </summary>
    void Map(TSource source, TDest dest) => PropertyMapper.CopyTo(source, dest);
}
