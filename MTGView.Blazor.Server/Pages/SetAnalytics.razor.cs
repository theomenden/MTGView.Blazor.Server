using System.Runtime.CompilerServices;
using MTGView.Blazor.Server.Models;

namespace MTGView.Blazor.Server.Pages;

public partial class SetAnalytics : ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    private IList<MagicCard> _magicCards = new List<MagicCard>(65_000);

    private IList<MagicSet> _magicSets = new List<MagicSet>(600);

    private string? _selectedSetValue;

    private string? selectedAutoCompleteText;

    private MagicSet? _selectedMagicSet;

    private BarChart<ColorAnalyticsBySet> _barChart = new();

    private readonly BarChartOptions _barChartOptions = new()
    {
        Parsing = new ChartParsing
        {
            XAxisKey = "color",
            YAxisKey = "count",
        }
    };


    private readonly List<string> _backgroundColors = new List<string> { ChartColor.FromRgba(255, 255, 255, 1.0f), ChartColor.FromRgba(54, 84, 235, 0.66f), ChartColor.FromRgba(160, 84, 177, 0.66f), ChartColor.FromRgba(255, 25, 48, 0.8f), ChartColor.FromRgba(0, 253, 47, 0.82f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
    private readonly List<string> _borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _magicSets = await GetMagicSetsAsync(context).ToListAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await HandleRedraw();
        }
    }

    private async IAsyncEnumerable<ColorAnalyticsBySet> GetMagicCardsBySetAsync(MagicthegatheringDbContext context, String setCode, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initialGroupingQuery = context.Cards
            .Where(c => c.setCode.Equals(setCode))
            .GroupBy(c => c.colorIdentity)
            .Select(c => new { c.Key, Subtotal = c.Count() });

        await foreach (var colorGroup in initialGroupingQuery
                           .AsAsyncEnumerable()
                           .Select(c => new ColorAnalyticsBySet
                           {
                               Color = DetermineColorIdentity(c.Key),
                               Count = c.Subtotal
                           })
                           .GroupBy(c => c.Color)
                           .Select(async (grouping) => new ColorAnalyticsBySet
                           {
                               Color = grouping.Key,
                               Count = await grouping.SumAsync(c => c.Count, cancellationToken)
                           })
                           .WithCancellation(cancellationToken))
        {.
            yield return await colorGroup;
        }
        StateHasChanged();
    }

    private async IAsyncEnumerable<MagicSet> GetMagicSetsAsync(MagicthegatheringDbContext context,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {

        await foreach (var set in context.Sets
                           .OrderBy(s => s.index)
                           .ThenBy(s => s.id)
                           .AsAsyncEnumerable()
                           .WithCancellation(cancellationToken))
        {
            yield return set;
        }
    }

    private async Task HandleRedraw()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        var labels = Enum.GetNames(typeof(MtgColors));

        var setToResearch = String.IsNullOrWhiteSpace(_selectedSetValue) ? "ODY" : _selectedSetValue;

        var chartData = await GetMagicCardsBySetAsync(context, setToResearch).ToListAsync();

        var dataSet = new BarChartDataset<ColorAnalyticsBySet>
        {
            Label = "Color Distributions",
            Data = chartData,
            BackgroundColor = _backgroundColors,
            BorderColor = _borderColors,
        };

        await _barChart.Clear();

        await _barChart.AddLabelsDatasetsAndUpdate(labels, dataSet);
    }

    private String DetermineColorIdentity(string setBasedIndicator)
    {
        if (String.IsNullOrWhiteSpace(setBasedIndicator))
        {
            return MtgColors.Colorless.ToString();
        }

        var colorIdentity = setBasedIndicator.ToCharArray();

        if (colorIdentity.Length > 1)
        {
            return MtgColors.Gold.ToString();
        }

        return colorIdentity[0] switch
        {
            'W' => MtgColors.White.ToString(),
            'U' => MtgColors.Blue.ToString(),
            'B' => MtgColors.Black.ToString(),
            'R' => MtgColors.Red.ToString(),
            'G' => MtgColors.Green.ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(colorIdentity))
        };
    }

}

