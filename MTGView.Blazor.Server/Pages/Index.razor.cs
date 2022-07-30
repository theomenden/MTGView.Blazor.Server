using System.Text.RegularExpressions;

namespace MTGView.Blazor.Server.Pages;

public partial class Index : ComponentBase
{
    #region Injected Properties
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public ScryfallCardService ScryfallCardService { get; init; }

    [Inject] public ScryfallSetInformationService SetInformationService { get; init; }

    [Inject] public ScryfallSymbologyService SymbologyService { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }
    
    [Inject] public IPageProgressService PageProgressService { get; init; }
    #endregion
    #region Fields
    private IEnumerable<ScryfallSetDetails> _setDetails = new List<ScryfallSetDetails>(800);
    private IEnumerable<SymbologyDatum> _symbols = new List<SymbologyDatum>(800);

    private Int32 _cardsStored;
    private Int32 _setsStored;
    private Int32 _symbolsStored;

    private readonly Lazy<Regex> _regex = new(() => new(@"([A-Z])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline));

    private MagicCard? _magicCard;
    private String? _setName;
    private readonly Random _random = new();
    #endregion
    #region Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        await PageProgressService.Go(null, options => { options.Color = Color.Warning; });

        await PopulateIndexedDbOnLoad();

        await PopulateRandomCardInformation();

        await PopulateRandomCardScryfallImage();

        await PageProgressService.Go(-1, options => { options.Color = Color.Warning; });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await SetInformationRepository.CreateOrUpdateMany(_setDetails);

            await SymbologyRepository.CreateOrUpdateMany(_symbols);

            await AddSetInformation();

            await AddColorIdentitySymbols();

            StateHasChanged();
        }
    }
    #endregion
    #region Private Methods
    private async Task PopulateRandomCardInformation()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _cardsStored = await context.Cards.CountAsync();

        if (_cardsStored > 0)
        {
            var seedValue = _random.Next(1, _cardsStored);

            _magicCard = await context.Cards.FirstOrDefaultAsync(card => card.index == seedValue);
        }
    }

    private async Task PopulateRandomCardScryfallImage()
    {
        if (_magicCard is not null)
        {
            var scryfallCardDataResponse = await ScryfallCardService.GetContentAsync(_magicCard.scryfallId.ToString());

            var materializedCardData = scryfallCardDataResponse.Data;

            _magicCard.ScryfallImageUri = materializedCardData.image_uris?.HighResolution ?? String.Empty;

            _magicCard.ScryfallImagesAsSizes =
                materializedCardData.image_uris?.GetAllImagesAsSizes() ?? new List<String>(1);
        }
    }

    private async Task PopulateIndexedDbOnLoad()
    {
        var apiSetResponse = await SetInformationService.GetSetDetailsAsync();

        var apiSymbolsResponse = await SymbologyService.GetAllSymbolsFromScryfall();

        if (apiSetResponse.Outcome.OperationResult == OperationResult.Success)
        {
            _setDetails = apiSetResponse.Data;
            _setsStored = apiSetResponse.Data.Count();
        }

        if (apiSymbolsResponse.Outcome.OperationResult == OperationResult.Success)
        {

            _symbols = apiSymbolsResponse.Data;
            _symbolsStored = apiSymbolsResponse.Data.Count();
        }

    }

    //THINDAL Provided Guidance :) 4/17/2022
    private Task AddColorIdentitySymbols()
    {
        var regex = _regex.Value;

        if (_magicCard is null)
        {
            return Task.CompletedTask;
        }

        var matches = regex.Matches(_magicCard.colorIdentity ?? String.Empty)
            .SelectMany(match => match.Groups.Values);

        foreach (var match in matches)
        {
            var symbolToAdd = _symbols.First(l => !String.IsNullOrWhiteSpace(l.LooseVariant)
                                                  && l.LooseVariant.Equals(match.Value,
                                                      StringComparison.OrdinalIgnoreCase));

            _magicCard.ColorIdentitySvgUris.TryAdd(match.Value, symbolToAdd.SvgUri);
        }

        return Task.CompletedTask;
    }

    private Task AddSetInformation()
    {
        if (_magicCard is null || !String.IsNullOrWhiteSpace(_magicCard.setCode))
        {
            return Task.CompletedTask;
        }

        var setInformation =
            _setDetails.First(s => s.Code.Equals(_magicCard.setCode, StringComparison.OrdinalIgnoreCase));

        _setName = $"<strong>{setInformation.Name}</strong><br />Released: <em>{setInformation.ReleasedAt:d}</em>";

        return Task.CompletedTask;
    }
    #endregion
}