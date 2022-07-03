using TheOmenDen.Shared.Enumerations;

namespace MTGView.Core.Enums;
public sealed class KeywordTypes: EnumerationBase
{
    private KeywordTypes(String name, Int32 id)
    : base(name, id)
    {
    }

    public static readonly KeywordTypes AbilityWords = new(nameof(AbilityWords), 1);
    public static readonly KeywordTypes KeywordAbilities = new(nameof(KeywordAbilities),2);
    public static readonly KeywordTypes KeywordActions = new(nameof(KeywordActions),3);
}
