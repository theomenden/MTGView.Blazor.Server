﻿using System.Text.RegularExpressions;

namespace MTGView.Blazor.Server.Pages;

public partial class Index : ComponentBase
{
    #region Injected Properties
    [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

    [Inject] public IScryfallCardService ScryfallCardService { get; init; }

    [Inject] public IScryfallSetInformationService SetInformationService { get; init; }

    [Inject] public IScryfallSymbologyService SymbologyService { get; init; }

    [Inject] public SetInformationRepository SetInformationRepository { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }
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
        await PopulateIndexedDbOnLoad();

        await PopulateRandomCardInformation();

        await PopulateRandomCardScryfallImage();
        
        await AddSetInformation();

        await AddColorIdentitySymbols();
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await SetInformationRepository.CreateOrUpdateMany(_setDetails);
            await SymbologyRepository.CreateOrUpdateMany(_symbols);
        }
    }
    #endregion
    #region Private Methods
    private async Task PopulateRandomCardInformation()
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();

        _cardsStored = await context.Cards.CountAsync();

        var seedValue = _random.Next(1, _cardsStored);

        _magicCard = await context.Cards.FirstOrDefaultAsync(card => card.index == seedValue);
    }

    private async Task PopulateRandomCardScryfallImage()
    {
        var scryfallCardDataResponse = await ScryfallCardService.GetScryfallInformationAsync(_magicCard.scryfallId);

        var materializedCardData = scryfallCardDataResponse.Data;

        _magicCard.ScryfallImageUri = materializedCardData.image_uris?.HighResolution ?? String.Empty;

        _magicCard.ScryfallImagesAsSizes = materializedCardData.image_uris?.GetAllImagesAsSizes() ?? new List<String>(1);
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

    //THINDAL Provided Guidance :) 4/17/2022
    private Task AddColorIdentitySymbols()
    {
        var regex = _regex.Value;

        var matches = regex.Matches(_magicCard.colorIdentity ?? String.Empty)
            .SelectMany(match => match.Groups.Values);

        foreach (var match in matches)
        {
            var symbolToAdd = _symbols.First(l => !String.IsNullOrWhiteSpace(l.LooseVariant)
                                                                                    && l.LooseVariant.Equals(match.Value, StringComparison.OrdinalIgnoreCase));

            _magicCard.ColorIdentitySvgUris.TryAdd(match.Value, symbolToAdd.SvgUri);
        }

        return Task.CompletedTask;
    }

    private Task AddSetInformation()
    {
        if (String.IsNullOrWhiteSpace(_magicCard?.setCode))
        {
            var setInformation =
                _setDetails.First(s => s.Code.Equals(_magicCard.setCode, StringComparison.OrdinalIgnoreCase));

            _setName = $"<strong>{setInformation.Name}</strong><br />Released: <em>{setInformation.ReleasedAt:d}</em>";
        }

        return Task.CompletedTask;
    }
    #endregion
}