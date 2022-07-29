using TheOmenDen.Shared.Enumerations;

namespace MTGView.Core.Enums;

public sealed record InGameColors : EnumerationBase
{
    private InGameColors(String name, Int32 id)
    :base(name,id)
    {
    }

    /// <value>
    /// Cards that have a White color associated with them in their color identity
    /// </value>
    public static readonly InGameColors White = new(nameof(White),1);
    /// <value>
    /// Cards that have a blue color associated with them in their color identity
    /// </value>
    public static readonly InGameColors Blue = new(nameof(Blue), 2);
    /// <value>
    /// Cards that have a black color associated with them in their color identity 
    /// </value>
    public static readonly InGameColors Black = new(nameof(Black), 3);
    /// <value>
    /// Cards that have a Red color associated with them in their color identity    
    /// </value>
    public static readonly InGameColors Red = new(nameof(Red), 4);
    /// <value>
    /// Cards that have a Green color associated with them in their color identity
    /// </value>
    public static readonly InGameColors Green = new(nameof(Green), 5);
    /// <value>
    /// Cards that have a multiple colors associated with them in their color identity
    /// </value>
    public static readonly InGameColors Gold = new(nameof(Gold), 6);
    /// <value>
    /// Cards that have no colors associated with them in their color identity
    /// </value>
    public static readonly InGameColors Colorless = new(nameof(Colorless), 7);
}