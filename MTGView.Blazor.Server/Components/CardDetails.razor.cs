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
    
    private MagicCard? _magicCardToReview;
    #region Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("cardId", out var cardId))
        {
            var magicCardId = Convert.ToInt32(cardId);

            await using var context = await MtgContextFactory.CreateDbContextAsync();

            _magicCardToReview = context.Cards.FirstOrDefault(card => card.id == magicCardId);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
                await GetScryfallImageInformation();
                await AddManaCostVisibleSymbols();
                await AddVisibleSetSymbols();

                StateHasChanged();
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
        var symbolToAdd = await SetInformationRepository.GetBySetCode(_magicCardToReview.setCode ?? String.Empty);

        _magicCardToReview.ScryfallSetIconUri = symbolToAdd?.IconUri ?? String.Empty;
    }
    #endregion
}