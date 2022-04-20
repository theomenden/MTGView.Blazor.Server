﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable

namespace MTGView.Data.EFCore.Contexts.Configurations
{
    public partial class RulingConfiguration : IEntityTypeConfiguration<Ruling>
    {
        public void Configure(EntityTypeBuilder<Ruling> entity)
        {
            entity.ToTable("rulings", "MTG");

            entity.HasIndex(e => e.Index, "IX_Rulings_Index");

            entity.HasIndex(e => e.RulingGuid, "IX_Rulings_Uuid");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.Index)
                .HasColumnName("index")
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("date")
                .HasColumnType("date");

            entity.Property(e => e.RuleText)
                .HasColumnName("text")
                .HasMaxLength(2000);

            entity.Property(e => e.RulingGuid)
                .HasColumnName("uuid");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Ruling> entity);
    }
}
