using MTGView.Data.EfCore.Contexts;

namespace MTGView.Blazor.Server.Pages;
public partial class PersonalCollectionComponent: ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> MagicthegatheringDbContextFactory { get; init; }

    [Inject] public ILogger<PersonalCollectionComponent> Logger { get; init; }

    private PersonalCollection? _personalCollection;

    protected override async Task OnInitializedAsync()
    {
        await using var mtgContext = await MagicthegatheringDbContextFactory.CreateDbContextAsync();

        _personalCollection = mtgContext.PersonalCollections
            .Include(c => c.CardMappings)
                .ThenInclude(cm => cm.Card)
            .FirstOrDefault();
    }
}
