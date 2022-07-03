using MTGView.Data.Personal.EfCore.Contexts;
using MTGView.Data.Scryfall.Internal;

namespace MTGView.Blazor.Server.Components
{
    public partial class CreateCollectionComponent: ComponentBase
    {
        [Inject] public IDbContextFactory<MagicthegatheringDbContext> MagicDbContextFactory { get; init; }

        [Inject] public IDbContextFactory<PersonalcollectionsDbContext> PersonalCollectionDbContextFactory { get; init; }

        [Inject] public ScryfallCardService ScryfallCardService { get; init; }

        [Inject] public ILogger<CreateCollectionComponent> Logger { get; init; }

        private List<MagicSet> _magicSets = new(200);

        private List<MagicCard> _magicCards = new(400);

        private List<MagicCard> _personalMagicCards = new(40);

        private Int32 _selectedSet;

        private bool _loading;

        private MagicSet _magicSet;

        private MagicCard _selectedMagicCard;

        private MagicCard _selectedPersonalCard;

        private string _selectedPersonalCardName;

        private string? selectedAutoCompleteText;

        private PersonalCardMapping _personalCollectionMapping;
        
        private PersonalCollection _createdCollection = new();

        protected override async Task OnInitializedAsync()
        {
            _createdCollection.CardMappings = new List<PersonalCardMapping>(20);

            await using var context = await MagicDbContextFactory.CreateDbContextAsync();

            await foreach (var set in context.Sets.AsAsyncEnumerable())
            {
                _magicSets.Add(set);
            };
        }

        private async Task OnMagicSetChanged(Int32 setId)
        {
            _selectedSet = setId;

            _magicSet = _magicSets.FirstOrDefault(ms => ms.id == setId);

            _magicCards.Clear();

            await LoadMagicCardsForSet();

        }

        private async Task LoadMagicCardsForSet()
        {
            await using var context = await MagicDbContextFactory.CreateDbContextAsync();

            if (_magicCards.Any())
            {
                _magicCards.Clear();
            }

            await foreach (var card in context.Cards
                               .Where(c => c.setCode.Equals(_magicSet.code))
                               .AsAsyncEnumerable())
            {
                var scryfallDataResponse = await ScryfallCardService.GetContentAsync(card.scryfallId.ToString());

                var scryfallData = scryfallDataResponse.Data;

                if (scryfallData.image_uris is not null)
                {
                    card.ScryfallImageUri = scryfallData.image_uris?.BorderCropped ?? String.Empty;

                    card.ScryfallImagesAsSizes =
                        scryfallData.image_uris?.GetAllImagesAsSizes() ?? new List<String>(1) { };
                }

                _magicCards.Add(card);

                await InvokeAsync(StateHasChanged);
            }
        }

        private Task AddMagicCardToCollection()
        {
            _createdCollection.CardMappings.Add(new ()
            {
                CardId = _selectedMagicCard.id
            });
            
            _personalMagicCards.Add(_selectedMagicCard);

            StateHasChanged();

            return Task.CompletedTask;
        }

        private Task RemoveMagicCardFromCollection()
        {
            _selectedPersonalCard = _magicCards.FirstOrDefault(c => c.name.Equals(_selectedPersonalCardName));
            
            if (_selectedPersonalCard is null)
            {
                return Task.CompletedTask;
            }

            _createdCollection.CardMappings.Remove(_createdCollection.CardMappings.First(c => c.CardId == _selectedPersonalCard.id));

            _personalMagicCards.Remove(_personalMagicCards.First(c => c.id == _selectedPersonalCard.id 
                                                                      && c.setCode.Equals(_selectedPersonalCard.setCode)));

            StateHasChanged();

            return Task.CompletedTask;
        }

        private async Task OnSubmit()
        {
            _loading = true;

            try
            {
                await using var context = await PersonalCollectionDbContextFactory.CreateDbContextAsync();

                context.Collections.Add(_createdCollection);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError("Couldn't save collection for player: {@ex}", ex);
            }

            _loading = false;
        }
    }
}
