namespace MTGView.Data.EFCore.Contexts.Configurations;

public partial class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> entity)
    {
        entity.ToTable("Keywords", "MTG");

        entity.HasKey("Id")
            .IsClustered()
            .HasName("PK_Keyword_Id");

        entity.Property(e => e.Id)
            .UseIdentityColumn()
            .IsRequired();

        entity.Property(e => e.Name)
            .HasMaxLength(256)
            .IsRequired();

        entity.Property(e => e.RecordType)
            .HasColumnName("Type")
            .HasMaxLength(256)
            .IsRequired();

        entity.HasIndex(e => e.RecordType)
            .HasDatabaseName("IX_Keywords_Type")
            .IncludeProperties(e => e.Name);
    }
}
