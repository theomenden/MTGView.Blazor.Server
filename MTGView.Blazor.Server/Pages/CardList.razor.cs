using System.Text;
using System.Text.RegularExpressions;
using Blazorise.DataGrid;
using MTGView.Data.Scryfall.Services;

namespace MTGView.Blazor.Server.Pages
{
    public partial class CardList : ComponentBase
    {
        [Inject] public IDbContextFactory<MagicthegatheringDbContext> ContextFactory { get; init; }

        [Inject] public IScryfallCardService ScryfallCardService { get; init; }

        [Inject] public IScryfallSetInformationService ScryfallSetInformationService { get; init; }

        [Inject] public SymbologyRepository SymbologyRepository { get; init; }

        [Inject] public SetInformationRepository SetInformationRepository { get; init; }

        private IEnumerable<MagicCard> _magicCards = new List<MagicCard>(30_000);

        private Int32 _magicCardCount = 0;

        private MagicCard? _selectedCard;

        private Regex _regex = new(@"\{.\}", RegexOptions.IgnoreCase);

        private async Task<IEnumerable<MagicCard>> LoadCards(MagicthegatheringDbContext context, CancellationToken cancellationToken)
        {
            var cards = await context.Cards
                .OrderBy(c => c.id)
                .ThenBy(c => c.index)
                .AsQueryable()
                .ToListAsync(cancellationToken);

            return cards;
        }

        private async Task<Int32> GetCardCount(MagicthegatheringDbContext context,
            CancellationToken cancellationToken = default) => await context.Cards.CountAsync(cancellationToken);

        private async Task OnReadData(DataGridReadDataEventArgs<MagicCard> e)
        {
            await using var context = await ContextFactory.CreateDbContextAsync(e.CancellationToken);

            if (!e.CancellationToken.IsCancellationRequested)
            {
                _magicCardCount = await GetCardCount(context, e.CancellationToken);

                if (!e.CancellationToken.IsCancellationRequested)
                {
                    _magicCards = await LoadCards(context, e.CancellationToken);

                    _magicCards = _magicCards
                        .DynamicFilter(e)
                        .DynamicSort(e)
                        .Paging(e);

                    foreach (var magicCard in _magicCards)
                    {
                        var scryfallDataResponse = await ScryfallCardService.GetScryfallInformationAsync(magicCard.scryfallId, e.CancellationToken);

                        var scryfallData = scryfallDataResponse.Data;

                        magicCard.ScryfallImageUri = scryfallData.image_uris.border_crop;

                        magicCard.ScryfallImagesAsSizes = scryfallData.image_uris.GetAllImagesAsSizes();

                        await AddVisibleSetSymbols(magicCard);

                        await AddManaCostVisibleSymbols(magicCard);
                    }

                    StateHasChanged();
                }
            }
        }

        //THINDAL Provided Guidance :) 4/17/2022
        private async Task AddManaCostVisibleSymbols(MagicCard magicCard)
        {
            var matches = _regex.Matches(magicCard.manaCost)
                .SelectMany(match => match.Groups.Values);

            foreach (var match in matches)
            {
                var symbolToAdd = await SymbologyRepository.GetById(match.Value);

                magicCard.ManaCostSvgUris.Add(symbolToAdd?.SvgUri ?? String.Empty);
            }
        }

        private async Task AddVisibleSetSymbols(MagicCard magicCard)
        {
            var symbolToAdd = await SetInformationRepository.GetBySetCode(magicCard.setCode);

            magicCard.ScryfallSetIconUri = symbolToAdd?.IconUri ?? String.Empty;
        }
    }
}
