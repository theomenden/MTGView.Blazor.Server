using Nosthy.Blazor.DexieWrapper.Database;

namespace MTGView.Blazor.Server.Data
{
    public class MtgIndexedDbVersion: DbVersion
    {
        public MtgIndexedDbVersion()
        :base(1)
        {
            
        }
    }
}
