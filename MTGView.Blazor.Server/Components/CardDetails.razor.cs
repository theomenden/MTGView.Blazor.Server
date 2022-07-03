using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;
using MTGView.Data.Scryfall.Internal;

namespace MTGView.Blazor.Server.Components;

public partial class CardDetails : ComponentBase
{
    #region Injected Members
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> MtgContextFactory { get; init; }

    [Inject] public ScryfallCardService ScryfallCardService { get; init; }

    [Inject] public NavigationManager NavigationManager { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }
    #endregion
    private readonly Lazy<Regex> _regex = new(() => new(@"\{.\}", RegexOptions.Compiled | RegexOptions.IgnoreCase));

    private readonly Dictionary<String, Color> _backgroundMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        {"Banned", Color.Danger },
        {"Restricted", Color.Warning},
        {"Legal", Color.Success}
    };

    private IEnumerable<Ruling>? _rulings;

    private MagicCard? _magicCardToReview;

    private IDictionary<String, ScryfallSetDetails> _setValuePairs = new Dictionary<String, ScryfallSetDetails>(15);

    #region Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("cardId", out var cardId)
            && Guid.TryParse(cardId, out var magicGuid))
        {
            await using var context = await MtgContextFactory.CreateDbContextAsync();

            _magicCardToReview = context.Cards.AsQueryable().First(card => card.uuid == magicGuid);

            _magicCardToReview.Legalities = await context.Legalities
                .Where(legality => legality.uuid == magicGuid)
                .AsAsyncEnumerable()
                .ToArrayAsync();

            _rulings = await context.Rulings
                .Where(r => r.RulingGuid == _magicCardToReview.uuid)
                .AsAsyncEnumerable()
                .ToArrayAsync();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.WhenAll(
                GetScryfallCardApiInformation(),
                AddManaCostVisibleSymbols(),
                AddVisibleSetSymbols(),
                GetOtherPrintingSetSymbolUris());

            await InvokeAsync(StateHasChanged);
        }
    }
    #endregion
    #region Private Methods
    private async Task GetScryfallCardApiInformation()
    {
        var scryfallDataResponse = await ScryfallCardService.GetContentAsync(_magicCardToReview?.scryfallId.ToString());

        var scryfallData = scryfallDataResponse.Data;

        if (scryfallData.image_uris is not null)
        {
            _magicCardToReview.ScryfallImageUri = scryfallData.image_uris.Normal ?? String.Empty;

            _magicCardToReview.ScryfallImagesAsSizes = scryfallData.image_uris.GetAllImagesAsSizes();
        }

        if (scryfallData.purchase_uris is not null)
        {
            _magicCardToReview.purchaseUrls = scryfallData.purchase_uris.tcgplayer;
        }

        if (scryfallData.prices is not null)
        {
            _magicCardToReview.CurrentPrice = $"${scryfallData.prices.usd}";
        }
    }

    //THINDAL Provided Guidance :) 4/17/2022
    private async Task AddManaCostVisibleSymbols()
    {
        var regex = _regex.Value;

        var matches = regex.Matches(_magicCardToReview.manaCost ?? String.Empty)
            .SelectMany(match => match.Groups.Values);

        foreach (var match in matches)
        {
            var symbolToAdd = await SymbologyRepository.GetById(match.Value);

            _magicCardToReview.ManaCostSvgUris.Add(symbolToAdd?.SvgUri ?? String.Empty);
        }
    }

    private async Task AddVisibleSetSymbols()
    {
        var symbolToAdd = await SetInformationRepository.GetBySetCode(_magicCardToReview?.setCode ?? String.Empty);

        _magicCardToReview.ScryfallSetIconUri = symbolToAdd?.IconUri ?? String.Empty;
    }

    private async Task GetOtherPrintingSetSymbolUris()
    {
        if (_magicCardToReview is not null && !String.IsNullOrWhiteSpace(_magicCardToReview.printings))
        {
            var setCodes = _magicCardToReview.printings.Split(',');

            _setValuePairs = await SetInformationRepository.GetBySetCodes(setCodes);
        }
    }

    private Color DetermineBackgroundFromStatus(String status)
    {
        if (String.IsNullOrWhiteSpace(status))
        {
            status = "Legal";
        }
        return _backgroundMappings[status];
    }
    #endregion
}