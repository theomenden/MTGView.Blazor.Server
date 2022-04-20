using System.Runtime.CompilerServices;
using Blazorise.DataGrid;
using Blazorise.DataGrid.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MTGView.Blazor.Server.Pages;

public partial class Rulings : ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public ILogger<Rulings> Logger { get; init; }

    private IEnumerable<Ruling> _rulings = new List<Ruling>(500);

    private Int32 _rulingsCount = 0;

    private DataGrid<Ruling> _dataGrid = new();

    private VirtualizeOptions _virtualizeOptions = new()
    {
        DataGridMaxHeight = "80%",
        OverscanCount = 15
    };

    public int CurrentPage { get; set; } = 1;

    private async IAsyncEnumerable<Ruling> GetRulingsFromDb(Int32 virtualizeOffset, Int32 virtualizeCount, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var mtgContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        await foreach (var ruling in mtgContext.Rulings
                           .AsAsyncEnumerable()
                           .Skip(virtualizeOffset)
                           .Take(virtualizeCount)
                           .OrderBy(r => r.Index)
                           .ThenBy(r => r.Id)
                           .WithCancellation(cancellationToken))
        {
            yield return ruling;
        }
    }

    private async Task<Int32> GetTotalRulingsFromDb(CancellationToken cancellationToken = default) 
    {
        await using var mtgContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        return await mtgContext.Rulings.CountAsync(cancellationToken);
    }

    private async Task OnReadData(DataGridReadDataEventArgs<Ruling> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            var rulings = GetRulingsFromDb(e.VirtualizeOffset, e.VirtualizeCount);

            if (!e.CancellationToken.IsCancellationRequested)
            {
                _rulings = await rulings.ToListAsync(e.CancellationToken);

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
