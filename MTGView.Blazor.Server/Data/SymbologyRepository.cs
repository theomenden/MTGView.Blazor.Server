using MTGView.Data.Scryfall.Models;

namespace MTGView.Blazor.Server.Data;
public class SymbologyRepository
    {
        private readonly MtgIndexedDb _db;

        public SymbologyRepository(IModuleFactory jsModuleFactory)
        {
            _db = new MtgIndexedDb(jsModuleFactory);
        }

        public async Task<List<SymbologyDatum>> GetAll()
        {
            return await _db.ScryfallSymbolInformationStore.ToList();
        }

        public async Task<SymbologyDatum?> GetById(String symbolName)
        {
            return await _db.ScryfallSymbolInformationStore.Get(symbolName);
        }

        public async Task<SymbologyDatum> CreateOrUpdate(SymbologyDatum setDetail)
        {
            await _db.ScryfallSymbolInformationStore.Put(setDetail);
            return await Task.FromResult(setDetail);
        }

        public async Task CreateOrUpdateMany(IEnumerable<SymbologyDatum> symbologyDetails, CancellationToken cancellationToken = default)
        {
            if (await _db.ScryfallSymbolInformationStore.Count(cancellationToken) == 0)
            {
                await _db.ScryfallSymbolInformationStore.BulkAdd(symbologyDetails, cancellationToken);
                return;
            }


            await _db.ScryfallSymbolInformationStore.BulkPut(symbologyDetails, cancellationToken);
        }

        public async Task Delete(SymbologyDatum symbologyDetail, CancellationToken cancellationToken = default)
        {
            await _db.ScryfallSymbolInformationStore.Delete(symbologyDetail.Symbol, cancellationToken);
        }
    }