namespace MTGView.Data.Background.Helpers;

internal static class RelationalNames
{
    private const string Relational = "Relational";

    private const string Schema = "Schema";

    private const string Table = "TableName";

    public const string RelationSchema = $"{Relational}:{Schema}";

    public const string RelationalTable = $"{Relational}:{Table}";
}

