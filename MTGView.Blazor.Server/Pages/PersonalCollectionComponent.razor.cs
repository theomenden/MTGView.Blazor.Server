using Microsoft.EntityFrameworkCore;
using MTGView.Data.Personal.EfCore.Contexts;

namespace MTGView.Blazor.Server.Pages;
public partial class PersonalCollectionComponent: ComponentBase
{
    [Inject] public IDbContextFactory<PersonalcollectionsDbContext> CollectionDbContextFactory { get; init; }

    [Inject] public IDbContextFactory<MagicthegatheringDbContext> MagicthegatheringDbContextFactory { get; init; }

    [Inject] public ILogger<PersonalCollectionComponent> Logger { get; init; }

    private PersonalCollection? _personalCollection;

    protected override async Task OnInitializedAsync()
    {
        await using var mtgContext = await MagicthegatheringDbContextFactory.CreateDbContextAsync();

        await using var context = await CollectionDbContextFactory.CreateDbContextAsync();

        _personalCollection = context.Collections
            .Include(c => c.CardMappings)
                .ThenInclude(cm => cm.Card)
            .FirstOrDefault();
    }
}
