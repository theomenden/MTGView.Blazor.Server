using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace MTGView.Blazor.Server.Components;

public partial class CardDetails : ComponentBase
{
    #region Injected Members
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> MtgContextFactory { get; init; }

    [Inject] public IScryfallCardService ScryfallCardService { get; init; }

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

    private MagicCard? _magicCardToReview;

    #region Lifecycle Methods
    protected override void OnInitialized()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("cardId", out var cardId)
            && Guid.TryParse(cardId, out var magicGuid))
        {
            using var context = MtgContextFactory.CreateDbContext();

            _magicCardToReview = context.Cards.First(card => card.uuid == magicGuid);

            _magicCardToReview.Legalities = context.Legalities.Where(legality => legality.uuid == magicGuid).ToArray();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetScryfallImageInformation();
            await AddManaCostVisibleSymbols();
            await AddVisibleSetSymbols();
        }
    }
    #endregion
    #region Private Methods
    private async Task GetScryfallImageInformation()
    {
        var scryfallDataResponse = await ScryfallCardService.GetScryfallInformationAsync(_magicCardToReview.scryfallId);

        var scryfallData = scryfallDataResponse.Data;

        _magicCardToReview.ScryfallImageUri = scryfallData.image_uris.HighResolution;

        _magicCardToReview.ScryfallImagesAsSizes = scryfallData.image_uris.GetAllImagesAsSizes();

        StateHasChanged();
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

        StateHasChanged();
    }

    private async Task AddVisibleSetSymbols()
    {
        var symbolToAdd = await SetInformationRepository.GetBySetCode(_magicCardToReview.setCode ?? String.Empty);

        _magicCardToReview.ScryfallSetIconUri = symbolToAdd?.IconUri ?? String.Empty;

        StateHasChanged();
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