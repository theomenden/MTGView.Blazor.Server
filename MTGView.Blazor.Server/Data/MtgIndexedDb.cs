using Nosthy.Blazor.DexieWrapper.Database;

namespace MTGView.Blazor.Server.Data;
public class MtgIndexedDb : Db
{
    public Store<ScryfallSetDetails, Guid> ScryfallSetInformationStore { get; set; } = new(nameof(ScryfallSetDetails.Id), nameof(ScryfallSetDetails.Code), nameof(ScryfallSetDetails.Name));

    public Store<SymbologyDatum, String> ScryfallSymbolInformationStore { get; set; } = new(nameof(SymbologyDatum.Symbol));

    public MtgIndexedDb(IModuleFactory jsModuleFactory)
        : base("MtgInformationDb", 2, new DbVersion[] { new MtgIndexedDbVersion() }, jsModuleFactory)
    {
    }
}
