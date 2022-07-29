using Blazorise.DataGrid;

namespace MTGView.Blazor.Server.Pages
{
    public partial class Keywords: ComponentBase
    {
        [Inject] public IDbContextFactory<MagicthegatheringDbContext> DbContextFactory { get; init; }

        [Inject] public ILogger<Keywords> Logger { get; init; }

        private IEnumerable<Keyword> _keywords = new List<Keyword>(500);

        private Int32 _keywordsCount;

        private DataGrid<Keyword> _dataGrid = new();

        public int CurrentPage { get; set; } = 1;

        private static Task<List<Keyword>> LoadKeywords(MagicthegatheringDbContext context, DataGridReadDataEventArgs<Keyword> eventArgs)
        {
            var keywords = context.Keywords
                .DynamicSort(eventArgs)
                .Paging(eventArgs)
                .ToListAsync(eventArgs.CancellationToken);

            return keywords;
        }

        private async Task<Int32> GetTotalRulingsFromDb(CancellationToken cancellationToken = default)
        {
            await using var mtgContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            return await mtgContext.Rulings.CountAsync(cancellationToken);
        }

        private async Task OnReadData(DataGridReadDataEventArgs<Keyword> e)
        {
            await using var context = await DbContextFactory.CreateDbContextAsync(e.CancellationToken);

            if (!e.CancellationToken.IsCancellationRequested)
            {
                if (!e.CancellationToken.IsCancellationRequested)
                {
                    _keywords = await LoadKeywords(context, e);

                    _keywordsCount = await GetTotalRulingsFromDb(e.CancellationToken);
                }
            }

            StateHasChanged();
        }

        private String DisplayKeywordNamedType(String keywordType)
        {
            return keywordType switch
            {
                "AbilityWords" => "Ability",
                "KeywordActions" => "Action",
                "KeywordAbilities" => "Keyworded Ability",
                _ => String.Empty
            };
        }

        private Task Reset()
        {
            return _dataGrid.Reload();
        }
    }
}
