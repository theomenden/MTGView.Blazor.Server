﻿// <auto-generated />
using System;
using MTGView.Data.Personal.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MTGView.Data.Personal.EfCore.Migrations
{
    [DbContext(typeof(PersonalcollectionsDbContext))]
    partial class PersonalcollectionsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MTGView.Core.Models.PersonalCard", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(620)
                        .IsUnicode(false)
                        .HasColumnType("varchar(620)")
                        .HasColumnName("name");

                    b.Property<string>("SetCode")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)")
                        .HasColumnName("setCode");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.HasIndex("SetCode")
                        .HasDatabaseName("IX_Cards_SetCode");

                    b.ToTable("Cards", "Personal");
                });

            modelBuilder.Entity("MTGView.Core.Models.PersonalCardMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newsequentialid())");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<Guid>("PersonalCollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.HasIndex(new[] { "CardId" }, "IX_PersonalCardMapping_CardId");

                    b.HasIndex(new[] { "PersonalCollectionId" }, "IX_PersonalCardMapping_PersonalCollectionId");

                    b.ToTable("Mappings", "Personal");
                });

            modelBuilder.Entity("MTGView.Core.Models.PersonalCollection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newsequentialid())");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(7)
                        .HasColumnType("datetime2(7)")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("Collections", "Personal");
                });

            modelBuilder.Entity("MTGView.Core.Models.PersonalCardMapping", b =>
                {
                    b.HasOne("MTGView.Core.Models.PersonalCard", "Card")
                        .WithMany("CardMappings")
                        .HasForeignKey("CardId")
                        .IsRequired()
                        .HasConstraintName("FK_Mappings_Card_Id");

                    b.HasOne("MTGView.Core.Models.PersonalCollection", "Collection")
                        .WithMany("CardMappings")
                        .HasForeignKey("PersonalCollectionId")
                        .IsRequired()
                        .HasConstraintName("FK_Mappings_Collection_Id");

                    b.Navigation("Card");

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("MTGView.Core.Models.PersonalCard", b =>
                {
                    b.Navigation("CardMappings");
                });

            modelBuilder.Entity("MTGView.Core.Models.PersonalCollection", b =>
                {
                    b.Navigation("CardMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
