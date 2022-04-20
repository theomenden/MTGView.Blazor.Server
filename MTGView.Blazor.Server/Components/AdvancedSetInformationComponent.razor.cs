

namespace MTGView.Blazor.Server.Components;

public partial class AdvancedSetInformationComponent : ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    private List<MagicSet> _magicSets = new(600);

    private List<MagicCard> _magicCards = new(60_000);
    
    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _magicCards = await context.Cards.ToListAsync();
        _magicSets = await context.Sets.ToListAsync();
    }
}

