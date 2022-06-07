namespace MTGView.Blazor.Server.Pages;

public partial class Index: ComponentBase
{
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public IScryfallCardService ScryfallCardService { get; init; }

    [Inject] public IScryfallSetInformationService SetInformationService { get; init; }

    [Inject] public IScryfallSymbologyService SymbologyService { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    private IEnumerable<ScryfallSetDetails> _setDetails = new List<ScryfallSetDetails>(800);
    private IEnumerable<SymbologyDatum> _symbols = new List<SymbologyDatum>(800);

    private Int32 _cardsStored;
    private Int32 _setsStored;
    private Int32 _symbolsStored;

    private MagicCard? _magicCard;
    private String? _setName;
    private readonly Random _random = new();

    protected override async Task OnInitializedAsync()
    {
        await PopulateIndexedDbOnLoad();

        await PopulateRandomCardInformation();

        await PopulateRandomCardScryfallImage();
    }

    private async Task PopulateRandomCardInformation()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _cardsStored = await context.Cards.CountAsync();

        var seedValue = _random.Next(1, _cardsStored);

        _magicCard = await context.Cards.FirstOrDefaultAsync(card => card.index == seedValue);
    }

    private async Task PopulateRandomCardScryfallImage()
    {
        var scryfallDataResponse = await ScryfallCardService.GetScryfallInformationAsync(_magicCard.scryfallId);

        var scryfallData = scryfallDataResponse.Data;

        _magicCard.ScryfallImageUri = scryfallData.image_uris?.HighResolution ?? String.Empty;

        _magicCard.ScryfallImagesAsSizes = scryfallData.image_uris?.GetAllImagesAsSizes() ?? new List<String>(1);
    }

    private async Task PopulateIndexedDbOnLoad()
    {
        var apiSetResponse = await SetInformationService.GetAllSetsAsync();

        var apiSymbolsResponse = await SymbologyService.GetAllSymbolsFromScryfall();

        if (apiSetResponse.Outcome.OperationResult is OperationResult.Success)
        {
            _setDetails = apiSetResponse.Data;
            _setsStored = apiSetResponse.Data.Count();
        }

        if (apiSymbolsResponse.Outcome.OperationResult is OperationResult.Success)
        {
            _symbols = apiSymbolsResponse.Data;
            _symbolsStored = apiSymbolsResponse.Data.Count();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            await SetInformationRepository.CreateOrUpdateMany(_setDetails);
            await SymbologyRepository.CreateOrUpdateMany(_symbols);


            if (!String.IsNullOrWhiteSpace(_magicCard?.setCode))
            {
                var setInformation = await SetInformationRepository.GetBySetCode(_magicCard.setCode);

                _setName = $"<strong>{setInformation?.Name}</strong><br />Released: <em>{setInformation?.ReleasedAt:d}</em>";
            }

            StateHasChanged();
        }
    }
}