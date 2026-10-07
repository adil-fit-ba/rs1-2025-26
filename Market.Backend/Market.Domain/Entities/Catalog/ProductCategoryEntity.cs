using Market.Domain.Common;

namespace Market.Domain.Entities.Catalog;

/// <summary>
/// Represents a product category.
/// </summary>
public class ProductCategoryEntity : BaseEntity
{
    /// <summary>
    /// Name of the category.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Descripion of the category.
    /// </summary>
    public string? DescriptionCategoriy { get; set; }

    /// <summary>
    /// Indicates whether the category is active (enabled).
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Lista proizvoda koji pripadaju ovoj kategoriji.
    ///
    /// **Napomena za studente:**
    /// Ova kolekcija se koristi prvenstveno za čitanje (query),
    /// a ne za izmjene. Koristimo <see cref="IReadOnlyCollection{T}"/>
    /// sa <c>private set</c> da onemogućimo direktno manipulisanje
    /// sadržajem liste u kodu (npr. <c>category.Products.Add(...)</c>).
    ///
    /// EF Core i dalje može učitati proizvode pomoću <c>Include</c>,
    /// ali neće pratiti promjene u ovoj kolekciji prilikom
    /// <c>SaveChanges</c>.
    ///
    /// Tehnički, moguće je koristiti običnu <see cref="ICollection{T}"/>
    /// i dodavati proizvode preko navigacije, ali to često donosi
    /// komplikacije:
    /// - EF Core može izgubiti praćenje stanja entiteta (tracking),
    /// - može pokušati dvostruko kreirati relaciju,
    /// - te otežava validaciju i poslovna pravila (npr. zabranu dodavanja
    ///   proizvoda u onemogućenu kategoriju).
    ///
    /// Zato se ovdje koristi "read-only" pristup: kategorija zna
    /// koji proizvodi su s njom povezani, ali se izmjene vrše isključivo
    /// kroz <see cref="ProductEntity"/> koji ima <c>CategoryId</c>.
    /// </summary>
    public IReadOnlyCollection<ProductEntity> Products { get; private set; } = new List<ProductEntity>();

    /// <summary>
    /// Single source of truth for technical/business constraints.
    /// Used in validators and EF configuration.
    /// </summary>
    public static class Constraints
    {
        public const int NameMaxLength = 100;
    }
}