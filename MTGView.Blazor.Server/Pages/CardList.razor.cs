using System.Text.RegularExpressions;
using Blazorise.DataGrid;
using MTGView.Blazor.Server.Invariants;
using MTGView.Blazor.Server.UrlHashing;

namespace MTGView.Blazor.Server.Pages;
public partial class CardList : ComponentBase
{
    #region Injected Members
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> ContextFactory { get; init; }

    [Inject] public IScryfallCardService ScryfallCardService { get; init; }

    [Inject] public IScryfallSetInformationService ScryfallSetInformationService { get; init; }

    [Inject] public IUrlHasher UrlHasher { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }
    #endregion
    #region Private Fields
    private IEnumerable<MagicCard> _magicCards = new List<MagicCard>(70_000);

    private List<String> _setsToSearch = new(20);
    private List<String> _multipleSelectionTexts = new(20);
    private string _selectedComparisonOperator = String.Empty;
    private Int32 _magicCardCount;

    private decimal _magicCardManaCost;

    private MagicCard? _selectedCard;

    private string _cardName = String.Empty;

    private readonly List<MagicSet> _availableSets = new(800);

    private readonly Lazy<Regex> _regex = new(() => new(@"\{.\}", RegexOptions.Compiled | RegexOptions.IgnoreCase));
    #endregion
    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        await using var context = await ContextFactory.CreateDbContextAsync();

        await foreach (var set in context.Sets.OrderBy(s => s.code).AsAsyncEnumerable())
        {
            _availableSets.Add(set);
        }
    }
    #endregion
    #region Private Methods
    private async Task<List<MagicCard>> LoadCards(MagicthegatheringDbContext context, DataGridReadDataEventArgs<MagicCard> eventArgs)
    {
        var queryableCards = context.Cards.AsQueryable();

        if (!String.IsNullOrWhiteSpace(_cardName))
        {
            var queryString = $"%{_cardName}%";

            queryableCards = queryableCards
                .Where(c => EF.Functions.Like(c.name!, queryString));
        }

        queryableCards = _selectedComparisonOperator switch
        {
            ">" => queryableCards.WhereInterpolated($"manaValue > {_magicCardManaCost}"),
            "<" => queryableCards.WhereInterpolated($"manaValue < {_magicCardManaCost}"),
            ">=" => queryableCards.WhereInterpolated($"manaValue >= {_magicCardManaCost}"),
            "<=" => queryableCards.WhereInterpolated($"manaValue <= {_magicCardManaCost}"),
            "=" => queryableCards.WhereInterpolated($"manaValue = {_magicCardManaCost}"),
            _ => queryableCards
        };

        if (_setsToSearch.Any())
        {
            queryableCards = queryableCards.Where("setCode in @0", _setsToSearch);
        }

        _magicCardCount = queryableCards.Count();

        var cards = await queryableCards
            .Paging(eventArgs)
            .ToListAsync(eventArgs.CancellationToken);

        return cards;
    }

    private async Task OnReadData(DataGridReadDataEventArgs<MagicCard> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            await using var context = await ContextFactory.CreateDbContextAsync(e.CancellationToken);

            await MaterializeCardInformation(e, context);
        }
    }

    private async Task MaterializeCardInformation(DataGridReadDataEventArgs<MagicCard> e, MagicthegatheringDbContext context)
    {
        if (e.CancellationToken.IsCancellationRequested)
        {
            return;
        }

        _magicCards = await LoadCards(context, e);

        foreach (var magicCard in _magicCards)
        {
            var scryfallDataResponse = await ScryfallCardService.GetScryfallInformationAsync(magicCard.scryfallId, e.CancellationToken);

            var scryfallData = scryfallDataResponse.Data;

            magicCard.ScryfallImageUri = scryfallData.image_uris.BorderCropped;

            magicCard.ScryfallImagesAsSizes = scryfallData.image_uris.GetAllImagesAsSizes();

            await AddVisibleSetSymbols(magicCard);

            await AddManaCostVisibleSymbols(magicCard);

            await GetSetName(magicCard);

            await InvokeAsync(StateHasChanged);
        }
    }

    //THINDAL Provided Guidance :) 4/17/2022
    private async Task AddManaCostVisibleSymbols(MagicCard magicCard)
    {
        var regex = _regex.Value;

        var matches = regex.Matches(magicCard.manaCost ?? String.Empty)
            .SelectMany(match => match.Groups.Values);

        foreach (var match in matches)
        {
            var symbolToAdd = await SymbologyRepository.GetById(match.Value);

            magicCard.ManaCostSvgUris.Add(symbolToAdd?.SvgUri ?? String.Empty);
        }
    }

    private async Task AddVisibleSetSymbols(MagicCard magicCard)
    {
        var symbolToAdd = await SetInformationRepository.GetBySetCode(magicCard.setCode ?? String.Empty);

        magicCard.ScryfallSetIconUri = symbolToAdd?.IconUri ?? String.Empty;
    }

    private async Task GetSetName(MagicCard? magicCard)
    {
        var setCode = magicCard?.setCode ?? String.Empty;

        if (magicCard is not null && !String.IsNullOrWhiteSpace(setCode))
        {
            var setDetails = await SetInformationRepository.GetBySetCode(setCode);

            magicCard.SetName = setDetails?.Name ?? String.Empty;
        }
    }

    private EventCallback ClearFilter(ButtonRowContext<MagicCard> context)
    {
        _selectedComparisonOperator = String.Empty;
        _magicCardManaCost = 0;
        _cardName = String.Empty;
        _setsToSearch.Clear();
        _setsToSearch = new();
        _multipleSelectionTexts.Clear();
        _multipleSelectionTexts = new();

        return context.ClearFilterCommand.Clicked;
    }

    private Task<String> EncodeUrl(Guid guid)
    {
        return Task.FromResult(String.Empty);
    }
    #endregion
}