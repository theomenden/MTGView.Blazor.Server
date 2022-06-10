using System.Text.RegularExpressions;
using Blazorise.DataGrid;

namespace MTGView.Blazor.Server.Pages;
public partial class CardList : ComponentBase
{
    #region Injected Members
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> ContextFactory { get; init; }

    [Inject] public IScryfallCardService ScryfallCardService { get; init; }

    [Inject] public IScryfallSetInformationService ScryfallSetInformationService { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }
    #endregion
    #region Private Fields
    private IEnumerable<MagicCard> _magicCards = new List<MagicCard>(70_000);

    private Int32 _magicCardCount;

    private MagicCard? _selectedCard;

    private readonly Lazy<Regex> _regex = new(() => new(@"\{.\}", RegexOptions.Compiled | RegexOptions.IgnoreCase));
    #endregion
    #region Private Methods
    private static Task<List<MagicCard>> LoadCards(MagicthegatheringDbContext context, DataGridReadDataEventArgs<MagicCard> eventArgs)
    {
        var cards = context.Cards
            .DynamicFilter(eventArgs)
            .DynamicSort(eventArgs)
            .Paging(eventArgs)
            .ToListAsync(eventArgs.CancellationToken);

        return cards;
    }

    private static Task<Int32> GetCardCount(MagicthegatheringDbContext context,
        CancellationToken cancellationToken = default) => context.Cards.CountAsync(cancellationToken);

    private async Task OnReadData(DataGridReadDataEventArgs<MagicCard> e)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(e.CancellationToken);

        if (!e.CancellationToken.IsCancellationRequested)
        {
            _magicCardCount = await GetCardCount(context, e.CancellationToken);

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
    #endregion
}