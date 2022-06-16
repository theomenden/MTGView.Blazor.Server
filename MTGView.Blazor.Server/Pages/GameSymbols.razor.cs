namespace MTGView.Blazor.Server.Pages;

public partial class GameSymbols: ComponentBase
{
    [Inject] public IScryfallSymbologyService ScryfallSymbologyService { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    private List<SymbologyDatum> _symbols = new (200);

    private List<SymbologyDatum> _funnySymbols = new(50);

    private List<SymbologyDatum> _manaSymbols = new(20);

    private List<SymbologyDatum> _manaCostSymbols = new(40);

    private List<SymbologyDatum> _transposableSymbols = new(40);

    private List<SymbologyDatum> _otherSymbols = new(40);

    private string selectedTab = "manaCosts";


    protected override async Task OnInitializedAsync()
    {
        var symbolEndPointResponse = await ScryfallSymbologyService.GetAllSymbolsFromScryfall();

        if (symbolEndPointResponse.Outcome.OperationResult is OperationResult.Success)
        {
            _symbols = symbolEndPointResponse.Data.Select(symbolInformation => symbolInformation).ToList();
            
            _funnySymbols.AddRange(_symbols.Where(symbol => symbol.IsFunny));

            _manaCostSymbols.AddRange(_symbols.Where(symbol => symbol.CanBeInManaCost));

            _manaSymbols.AddRange(_symbols.Where(symbol => symbol.IsManaRepresentative));

            _transposableSymbols.AddRange(_symbols.Where(symbol => symbol.IsTransposable));

            _otherSymbols.AddRange(_symbols.Where(symbol => !symbol.IsFunny && !symbol.CanBeInManaCost && !symbol.IsManaRepresentative && !symbol.IsTransposable));
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
           await SymbologyRepository.CreateOrUpdateMany(_symbols);
        }
    }

    private Task OnSelectedTabChanged(string name)
    {
        selectedTab = name;

        return Task.CompletedTask;
    }
}
