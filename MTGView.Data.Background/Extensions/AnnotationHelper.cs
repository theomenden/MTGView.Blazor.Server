using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MTGView.Data.Background.Extensions;

public static class AnnotationHelper
{
    private static string GetName(IEntityType entityType, string defaultSchemaName = "dbo")
    {
        var schema = entityType.FindAnnotation("Relational:Schema").Value;

        var tableName = entityType.GetAnnotation("Relational:TableName").Value.ToString();

        var schemaName = schema == null ? defaultSchemaName : schema.ToString();

        var name = $"[{schemaName}].[{tableName}]";

        return name;
    }

    public static string TableName<T>(DbContext dbContext) where T : class
    {
        var entityType = dbContext.Model.FindEntityType(typeof(T));
        return GetName(entityType);
    }

    public static string TableName<T>(DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.EntityType;
        return GetName(entityType);
    }
}

