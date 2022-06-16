using Blazorise.DataGrid;

namespace MTGView.Blazor.Server.Pages;

public partial class Rulings : ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public ILogger<Rulings> Logger { get; init; }

    private IEnumerable<Ruling> _rulings = new List<Ruling>(500);

    private Int32 _rulingsCount = 0;

    private DataGrid<Ruling> _dataGrid = new();
    
    public int CurrentPage { get; set; } = 1;
    
    private static Task<List<Ruling>> LoadRulings(MagicthegatheringDbContext context, DataGridReadDataEventArgs<Ruling> eventArgs)
    {
        var cards = context.Rulings
            .DynamicFilter(eventArgs)
            .DynamicSort(eventArgs)
            .Paging(eventArgs)
            .ToListAsync(eventArgs.CancellationToken);

        return cards;
    }

    private async Task<Int32> GetTotalRulingsFromDb(CancellationToken cancellationToken = default) 
    {
        await using var mtgContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        return await mtgContext.Rulings.CountAsync(cancellationToken);
    }

    private async Task OnReadData(DataGridReadDataEventArgs<Ruling> e)
    {
        await using var context = await DbContextFactory.CreateDbContextAsync(e.CancellationToken);

        if (!e.CancellationToken.IsCancellationRequested)
        {
            if (!e.CancellationToken.IsCancellationRequested)
            {
                _rulings = await LoadRulings(context, e);

                _rulingsCount = await GetTotalRulingsFromDb(e.CancellationToken);
            }
        }

        StateHasChanged();
    }
    private Task Reset()
    {
        return _dataGrid.Reload();
    }

}
