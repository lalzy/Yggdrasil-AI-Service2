// SortOrder.cs

namespace Yggdrasil.Models.Enums;

public enum SortOrder
{
    /// <summary>Identifier can be name, an order-int, etc</summary>
    IdentifierAsc,
    /// <inheritdoc cref="IdentifierAsc"/>
    IdentifierDesc,
    CreatedAsc,
    CreatedDesc,
    ModifiedAsc,
    ModifiedDesc,
    IDAsc,
    IDDesc
}
