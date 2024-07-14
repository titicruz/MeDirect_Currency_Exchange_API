using MeDirect_Currency_Exchange_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MeDirect_Currency_Exchange_API.Data {
    public class Currency_Exchange_API_Context : DbContext {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public Currency_Exchange_API_Context(DbContextOptions<Currency_Exchange_API_Context> options)
        : base(options) {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Trade>(entity => {
                entity.HasKey(t => t.ID)
                    .HasName("IDX_Trade_ID");

                entity.HasIndex(t => t.ID_Client).HasDatabaseName("IDX_Trade_ID_Client");

                entity.Property(t => t.ID)
                    .ValueGeneratedOnAdd();

                entity.Property(t => t.FromCurrency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(t => t.ToCurrency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(t => t.Amount)
                    .IsRequired()
                    .HasColumnType("decimal(18, 2)");

                entity.Property(t => t.ExchangeRate)
                    .IsRequired()
                    .HasColumnType("decimal(18, 6)");

                entity.Property(t => t.Dt_Create)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(t => t.Client)
                    .WithMany(c => c.Trades)
                    .HasForeignKey(t => t.ID_Client)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Client_Trades_ID_Client");
            });

            modelBuilder.Entity<Client>(entity => {
                entity.HasKey(c => c.ID)
                    .HasName("IDX_Client_ID");

                entity.Property(c => c.ID)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.DT_Create)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
