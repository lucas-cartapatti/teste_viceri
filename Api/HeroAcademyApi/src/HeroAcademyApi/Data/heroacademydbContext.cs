using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HeroAcademyApi.Data
{
    public partial class heroacademydbContext : DbContext
    {
        public heroacademydbContext()
        {
        }

        public heroacademydbContext(DbContextOptions<heroacademydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Herois> Herois { get; set; } = null!;
        public virtual DbSet<HeroisSuperpoderes> HeroisSuperpoderes { get; set; } = null!;
        public virtual DbSet<Superpoderes> Superpoderes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=heroacademydb;user=admin;password=admin", ServerVersion.Parse("8.0.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Herois>(entity =>
            {
                entity.ToTable("herois");

                entity.HasIndex(e => e.NomeHeroi, "NomeHeroi")
                    .IsUnique();

                entity.Property(e => e.DataNascimento).HasColumnType("datetime");

                entity.Property(e => e.Nome).HasMaxLength(120);

                entity.Property(e => e.NomeHeroi).HasMaxLength(120);
            });

            modelBuilder.Entity<HeroisSuperpoderes>(entity =>
            {
                entity.ToTable("HeroisSuperpoderes");

                entity.HasIndex(e => e.HeroiId, "HeroiId");

                entity.HasIndex(e => e.SuperpoderId, "SuperpoderId");

                entity.HasOne(d => d.Heroi)
                    .WithMany(p => p.HeroisSuperpoderes)
                    .HasForeignKey(d => d.HeroiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HeroisSuperpoderes_ibfk_1");

                entity.HasOne(d => d.Superpoder)
                    .WithMany(p => p.HeroisSuperpoderes)
                    .HasForeignKey(d => d.SuperpoderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("HeroisSuperpoderes_ibfk_2");
            });

            modelBuilder.Entity<Superpoderes>(entity =>
            {
                entity.ToTable("superpoderes");

                entity.HasIndex(e => e.Superpoder, "Superpoder")
                    .IsUnique();

                entity.Property(e => e.Descricao).HasMaxLength(250);

                entity.Property(e => e.Superpoder).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
