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

    private Chart<Int32> _pieChart = new();

    private Chart<Int32> _doughnutChart = new();

    private readonly BarChartOptions _barChartOptions = new()
    {
        AspectRatio = 1.5,
        Parsing = new ChartParsing
        {
            XAxisKey = "color",
            YAxisKey = "count",
        }
    };

    private readonly PieChartOptions _pieChartOptions = new()
    {
        AspectRatio = 1.5,
        Parsing = new ChartParsing
        {
            XAxisKey = "convertedmanacost",
            YAxisKey = "count",
        }
    };

    private readonly DoughnutChartOptions _doughnutChartOptions = new()
    {
        AspectRatio = 1.5,
        Parsing = new ChartParsing
        {
            XAxisKey = "keyword",
            YAxisKey = "count",
        }
    };

    private bool isAlreadyInitialized;

    private readonly List<string> _backgroundColors = new() { ChartColor.FromRgba(255, 255, 255, 1.0f), ChartColor.FromRgba(54, 84, 235, 0.66f), ChartColor.FromRgba(160, 84, 177, 0.66f), ChartColor.FromRgba(255, 25, 48, 0.8f), ChartColor.FromRgba(0, 253, 47, 0.82f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
    private readonly List<string> _borderColors = new() { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _magicSets = await GetMagicSetsAsync(context).ToListAsync();
    }

    private async IAsyncEnumerable<ColorAnalyticsBySet> GetMagicColorGraphDataBySetAsync(MagicthegatheringDbContext context, String setCode, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initialGroupingQuery = context.Cards
            .Where(c => c.setCode.Equals(setCode))
            .GroupBy(c => c.colorIdentity)
            .Select(c => new { c.Key, Subtotal = c.Count() })
            .AsAsyncEnumerable();

        await foreach (var colorGroup in initialGroupingQuery
                           .Select(c => new ColorAnalyticsBySet(
                               DetermineColorIdentity(c.Key),
                               c.Subtotal
                           ))
                           .GroupBy(c => c.Color)
                           .SelectAwait(async grouping => new ColorAnalyticsBySet
                           (
                               grouping.Key,
                               await grouping.SumAsync(c => c.Count, cancellationToken)
                           )).WithCancellation(cancellationToken))
        {
            yield return colorGroup;
        }

        StateHasChanged();
    }

    private async IAsyncEnumerable<ConvertedManaCostAnalyticsBySet> GetMagicManaCostDataBySetAsync(MagicthegatheringDbContext context, String setCode,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initialGroupingQuery = context.Cards
            .Where(c => c.setCode.Equals(setCode))
            .GroupBy(c => c.manaValue)
            .Select(c => new { c.Key, Subtotal = c.Count() })
            .AsAsyncEnumerable();

        await foreach (var convertedManaCostGroup in initialGroupingQuery
                           .Select(c => new ConvertedManaCostAnalyticsBySet
                           (
                               c.Key?.ToString() ?? String.Empty,
                               c.Subtotal
                           )).WithCancellation(cancellationToken))
        {
            yield return convertedManaCostGroup;
        }

        StateHasChanged();
    }

    private async IAsyncEnumerable<KeywordAnalyticsBySet> GetKeywordDataBySetAsync(MagicthegatheringDbContext context, String setCode,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var initialGroupingQuery = context.Cards
            .Where(c => c.keywords != null && c.setCode.Equals(setCode))
            .GroupBy(c => c.keywords)
            .Select(c => new { c.Key, Subtotal = c.Count() })
            .AsAsyncEnumerable();

        await foreach (var convertedManaCostGroup in initialGroupingQuery
                           .Select(c => new KeywordAnalyticsBySet
                           (
                               c.Key?.ToString() ?? String.Empty,
                               c.Subtotal
                           )).WithCancellation(cancellationToken))
        {
            yield return convertedManaCostGroup;
        }

        StateHasChanged();
    }

    private static async IAsyncEnumerable<MagicSet> GetMagicSetsAsync(MagicthegatheringDbContext context,
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

    private async Task HandleBarChartRedraw()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        var labels = Enum.GetNames(typeof(MtgColors));

        var setToResearch = _selectedSetValue;

        var chartData = await GetMagicColorGraphDataBySetAsync(context, setToResearch).ToListAsync();

        var dataSet = new BarChartDataset<ColorAnalyticsBySet>
        {
            Label = "Color Distributions",
            Data = chartData,
            BackgroundColor = _backgroundColors,
            BorderColor = _borderColors,
        };

        await _barChart.Clear();

        await _barChart.AddLabelsDatasetsAndUpdate(labels, dataSet);

        StateHasChanged();
    }

    private async Task HandlePieChartRedraw()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        var chartData = await GetMagicManaCostDataBySetAsync(context, _selectedSetValue).ToListAsync();

        var labels = chartData.DistinctBy(c => c.ConvertedManaCost)
            .Select(c => c.ConvertedManaCost.ToString());

        var dataSet = new PieChartDataset<Int32>
        {
            Label = "CMC Distributions",
            Data = chartData.Select(c => c.Count).ToList(),
            BorderColor = _borderColors,
            BackgroundColor = _backgroundColors,
            BorderRadius = 1
        };

        await _pieChart.Clear();

        await _pieChart.AddLabelsDatasetsAndUpdate(labels.ToList(), dataSet);

        StateHasChanged();
    }

    private async Task HandleDoughnutChartRedraw()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        var chartData = await GetKeywordDataBySetAsync(context, _selectedSetValue).ToListAsync();

        var labels = chartData.DistinctBy(c => c.Keyword)
            .Select(c =>  c.Keyword);

        var dataSet = new DoughnutChartDataset<Int32>
        {
            Label = "Keyword Distributions",
            Data = chartData.Select(c => c.Count).ToList(),
            BorderColor = _borderColors,
            BackgroundColor = _backgroundColors,
            BorderRadius = 1
        };

        await _doughnutChart.Clear();

        await _doughnutChart.AddLabelsDatasetsAndUpdate(labels.ToList(), dataSet);

        StateHasChanged();
    }

    private async Task<string> OnSelectedValueChanged(object? value)
    {
        _selectedSetValue = value?.ToString() ?? String.Empty;

        await HandleBarChartRedraw();

        await HandlePieChartRedraw();

        await HandleDoughnutChartRedraw();

        await InvokeAsync(StateHasChanged);

        return _selectedSetValue;
    }

    private static String DetermineColorIdentity(string setBasedIndicator)
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

