using MTGView.Data.Scryfall.Internal;
using TheOmenDen.Shared.Enumerations;
using TheOmenDen.Shared.Extensions;

namespace MTGView.Blazor.Server.Pages;

public partial class GameSymbols: ComponentBase
{
    [Inject] public ScryfallSymbologyService ScryfallSymbologyService { get; init; }

    [Inject] public SymbologyRepository SymbologyRepository { get; init; }

    [Inject] public IMessageService MessageService { get; init; }

    private List<SymbologyDatum> _symbols = new (200);
             
    private readonly List<SymbologyDatum> _funnySymbols = new(50);
             
    private readonly List<SymbologyDatum> _manaSymbols = new(20);
             
    private readonly List<SymbologyDatum> _manaCostSymbols = new(40);
             
    private readonly List<SymbologyDatum> _transposableSymbols = new(40);
             
    private readonly List<SymbologyDatum> _otherSymbols = new(40);

    private string _selectedTab = "manaCosts";

    protected override async Task OnInitializedAsync()
    {
        if (!StringBuilderPoolFactory<GameSymbols>.Exists(nameof(GameSymbols)))
        {
            StringBuilderPoolFactory<GameSymbols>.Create(nameof(GameSymbols));
        }

        var symbolEndPointResponse = await ScryfallSymbologyService.GetAllSymbolsFromScryfall();

        if (symbolEndPointResponse.Outcome.OperationResult == OperationResult.Failure)
        {
            await OnApiFailureAsync();
            return;
        }

        PopulateSymbolInformationAsync(symbolEndPointResponse);
    }

    private async Task OnApiFailureAsync()
    {
        var builder = StringBuilderPoolFactory<GameSymbols>.Get(nameof(GameSymbols));
            builder.Append("<a href='https://www.scryfall.com' class='text-link'>Scryfall Api</a> is down");
            builder.Append("<br /><strong><em>Please refresh the page</em></strong>");
            builder.Append(
            "<br /><hr>If the error still persists, create an issue on our <a href='https://github.com/theomenden/MTGView.Blazor.Server/issues' class='text-link'>GitHub!</a>");

        await MessageService.Error(new MarkupString(builder.ToString()), "Scryfall API could not be reached");
    }

    private void PopulateSymbolInformationAsync(ApiResponse<IEnumerable<SymbologyDatum>> symbolEndPointResponse)
    {
        _symbols = symbolEndPointResponse.Data.Select(symbolInformation => symbolInformation).ToList();

        _funnySymbols.AddRange(_symbols.Where(symbol => symbol.IsFunny));

        _manaCostSymbols.AddRange(_symbols.Where(symbol => symbol.CanBeInManaCost));

        _manaSymbols.AddRange(_symbols.Where(symbol => symbol.IsManaRepresentative));

        _transposableSymbols.AddRange(_symbols.Where(symbol => symbol.IsTransposable));

        _otherSymbols.AddRange(_symbols.Where(symbol =>
            !symbol.IsFunny && !symbol.CanBeInManaCost && !symbol.IsManaRepresentative && !symbol.IsTransposable));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender && _symbols.Any())
        {
           await SymbologyRepository.CreateOrUpdateMany(_symbols);
        }
    }

    private Task OnSelectedTabChanged(string name)
    {
        _selectedTab = name;

        return Task.CompletedTask;
    }
}
