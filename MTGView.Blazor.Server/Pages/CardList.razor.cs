using System.Text.RegularExpressions;
using Blazorise.DataGrid;
using MTGView.Data.Scryfall.Internal;

namespace MTGView.Blazor.Server.Pages;
public partial class CardList : ComponentBase
{
    #region Injected Members
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> ContextFactory { get; init; }

    [Inject] public ScryfallCardService ScryfallCardService { get; init; }

    [Inject] public ScryfallSetInformationService ScryfallSetInformationService { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }
    #endregion
    #region Private Fields
    private List<MagicCard> _magicCards = Enumerable.Empty<MagicCard>().ToList();

    private List<String> _setsToSearch = Enumerable.Empty<String>().ToList();
    private List<String> _multipleSelectionSets = Enumerable.Empty<String>().ToList();
    private List<String> _keywordsToSearch = Enumerable.Empty<String>().ToList();
    private List<String> _multipleSelectionKeywords = Enumerable.Empty<String>().ToList();
    private string _selectedComparisonOperator = String.Empty;
    
    private Int32 _magicCardCount;

    private decimal _magicCardManaCost;

    private MagicCard? _selectedCard;

    private string _cardName = String.Empty;

    private readonly List<MagicSet> _availableSets = new(800);

    private readonly List<Keyword> _availableKeywords = new(300);

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
        
        var keywordAbilities = KeywordTypes.KeywordAbilities.ToString();

        await foreach (var keyword in context.Keywords
                           .Where(c => c.RecordType == keywordAbilities)
                           .OrderBy(k => k.Id)
                           .AsAsyncEnumerable())
        {
            _availableKeywords.Add(keyword);
        }
    }
    #endregion
    #region Private Methods
    private IAsyncEnumerable<MagicCard> LoadCards(MagicthegatheringDbContext context, DataGridReadDataEventArgs<MagicCard> eventArgs)
    {
        var queryableCards = context.Cards
            .AsNoTracking()
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(_cardName))
        {
            var queryString = $"%{_cardName}%";

            queryableCards = queryableCards
                .Where(c => EF.Functions.Like(c.name!, queryString));
        }

        if (_keywordsToSearch.Any())
        {
            var keywordsToSearch = $"%{String.Join(',',_keywordsToSearch)}%";

            queryableCards = queryableCards.Where(c => EF.Functions.Like(c.keywords!, keywordsToSearch));
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

        var cards = queryableCards
            .Paging(eventArgs)
            .ToAsyncEnumerable();

        return cards;
    }

    private async Task OnReadData(DataGridReadDataEventArgs<MagicCard> e)
    {
        if (!e.CancellationToken.IsCancellationRequested)
        {
            await using var context = await ContextFactory.CreateDbContextAsync(e.CancellationToken);

            await MaterializeCardInformation(e, context);
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task MaterializeCardInformation(DataGridReadDataEventArgs<MagicCard> e, MagicthegatheringDbContext context)
    {
        await foreach (var magicCard in LoadCards(context, e).WithCancellation(e.CancellationToken))
        {
            var scryfallDataResponse = await ScryfallCardService.GetContentAsync(magicCard.scryfallId.ToString(), e.CancellationToken);

            var scryfallData = scryfallDataResponse.Data;

            if (scryfallData.image_uris is not null)
            {
                magicCard.ScryfallImageUri = scryfallData.image_uris.BorderCropped;

                magicCard.ScryfallImagesAsSizes = scryfallData.image_uris.GetAllImagesAsSizes();
            }
            await AddVisibleSetSymbols(magicCard);

            await AddManaCostVisibleSymbols(magicCard);

            await GetSetName(magicCard);

            _magicCards.Add(magicCard);
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
        _keywordsToSearch.Clear();
        _setsToSearch = new();
        _keywordsToSearch = new();
        _multipleSelectionSets.Clear();
        _multipleSelectionKeywords.Clear();
        _multipleSelectionSets = new();
        _multipleSelectionKeywords = new();

        return context.ClearFilterCommand.Clicked;
    }
    #endregion
}