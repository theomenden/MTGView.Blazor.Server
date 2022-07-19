using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MTGView.Data.Background.Helpers;

public static class AnnotationHelper
{
    private static string GetName(IEntityType entityType, string defaultSchemaName = "dbo")
    {
        var schema = entityType.FindAnnotation(RelationalNames.RelationSchema)?.Value ?? defaultSchemaName;

        var tableName = entityType.GetAnnotation(RelationalNames.RelationalTable)?.Value ?? String.Empty;

        var name = $"[{schema}].[{tableName}]";

        return name;
    }
    
    public static string TableName<T>(DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.EntityType;
        
        return GetName(entityType);
    }
}

