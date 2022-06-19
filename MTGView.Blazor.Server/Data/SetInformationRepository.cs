namespace MTGView.Blazor.Server.Data;

public class SetInformationRepository
{
    private readonly MtgIndexedDb _db;

    public SetInformationRepository(IModuleFactory jsModuleFactory)
    {
        _db = new MtgIndexedDb(jsModuleFactory);
    }

    public async Task<List<ScryfallSetDetails>> GetAll()
    {
        return await _db.ScryfallSetInformationStore
            .ToList();
    }
        
    public async Task<ScryfallSetDetails?> GetById(Guid id)
    {
        return await _db.ScryfallSetInformationStore.Get(id);
    }

    public async Task<ScryfallSetDetails?> GetBySetCode(string setCode)
    {
        var result = await _db.ScryfallSetInformationStore.Where(nameof(ScryfallSetDetails.Code))
            .EqualIgnoreCase(setCode)
            .ToArray();

        return result.FirstOrDefault();
    }

    public async Task<IDictionary<String,ScryfallSetDetails>> GetBySetCodes(IEnumerable<String> setCodes)
    {
        var result = await _db.ScryfallSetInformationStore.Where(nameof(ScryfallSetDetails.Code))
            .AnyOfIgnoreCase(setCodes)
            .ToArray();

        return result.ToDictionary(r => r.Code, r=> r);
    }

    public async Task<ScryfallSetDetails> CreateOrUpdate(ScryfallSetDetails setDetail)
    {
        await _db.ScryfallSetInformationStore.Put(setDetail);
        return await Task.FromResult(setDetail);
    }

    public async Task CreateOrUpdateMany(IEnumerable<ScryfallSetDetails> setDetails, CancellationToken cancellationToken = default)
    {
        if (await _db.ScryfallSetInformationStore.Count(cancellationToken) == 0)
        {
            await _db.ScryfallSetInformationStore.BulkAdd(setDetails, cancellationToken);
            return;
        }

        await _db.ScryfallSetInformationStore.BulkPut(setDetails, cancellationToken);
    }

    public async Task Delete(ScryfallSetDetails setDetail)
    {
        await _db.ScryfallSetInformationStore.Delete(setDetail.Id);
    }
}