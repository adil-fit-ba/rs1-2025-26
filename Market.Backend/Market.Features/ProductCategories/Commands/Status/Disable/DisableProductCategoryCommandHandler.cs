﻿namespace Market.Features.ProductCategories.Commands.Status.Disable;

public sealed class DisableProductCategoryCommandHandler(DatabaseContext ctx)
    : IRequestHandler<DisableProductCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DisableProductCategoryCommand request, CancellationToken ct)
    {
        var cat = await ctx.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (cat is null)
        {
            throw new MarketNotFoundException($"Kategorija (ID={request.Id}) nije pronađena.");
        }

        if (!cat.IsEnabled) return Unit.Value; // idempotentno

        // poslovno pravilo: ne smiješ disable ako postoje aktivni proizvodi
        var hasActiveProducts = await ctx.Products
            .AnyAsync(p => p.CategoryId == cat.Id && p.IsEnabled, ct);

        if (hasActiveProducts)
        {
            throw new MarketBusinessRuleException("category.disable.blocked.activeProducts",
                $"Kategorija {cat.Name} se ne može onemogućiti jer sadrži aktivne proizvode.");
        }

        cat.IsEnabled = false;

        await ctx.SaveChangesAsync(ct);

        // await _bus.PublishAsync(new ProductCategoryDisabledV1IntegrationEvent(cat.Id, ...), ct);
        // await _cache.RemoveAsync(CacheKeys.CategoriesList, ct);

        return Unit.Value;
    }
}
