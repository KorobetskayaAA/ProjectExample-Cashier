using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

// Только для учебных целей!
// Не помещайте строки подключения в код, даже в комментарии!
/* Scaffolded by
 * Scaffold-DbContext "Server=ANASTASY-ONB;Database=Cashier;Trusted_Connection=True;" \
 * Microsoft.EntityFrameworkCore.SqlServer \
 * -OutputDir ModelDB \
 * -Context CashierDbContext
 */

namespace CashierDbContext.ModelDB
{
    public partial class CashierDbContext : DbContext
    {
        public CashierDbContext()
        {
        }

        public CashierDbContext(DbContextOptions<CashierDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillStatus> BillStatuses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VwBill> VwBills { get; set; }
        public DbSet<VwBillsSearchByNumber> VwBillsSearchByNumbers { get; set; }
        public DbSet<VwBillsSearchByStatus> VwBillsSearchByStatuses { get; set; }
        public DbSet<VwProductSearchByName> VwProductSearchByNames { get; set; }
        public DbSet<VwProductsSearchByBarcode> VwProductsSearchByBarcodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Только для учебных целей!
                // Не помещайте строки подключения в код, даже в комментарии!
                //setx CashierDb_ConnectionString "Server=ANASTASY-ONB;Database=Cashier;Trusted_Connection=True;"
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("CashierDb_ConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__Bill__78A1A19CC6C2BB67");

                entity.ToTable("Bill");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bill__Status__4316F928");
            });

            modelBuilder.Entity<BillStatus>(entity =>
            {
                entity.ToTable("BillStatus");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Item");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.BarcodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Barcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__Barcode__44FF419A");

                entity.HasOne(d => d.BillNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.BillNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__BillNumber__440B1D61");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Barcode)
                    .HasName("PK__Product__177800D2C00DAA8A");

                entity.ToTable("Product");

                entity.Property(e => e.Barcode).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(15, 2)");
            });

            modelBuilder.Entity<VwBill>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwBills");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<VwBillsSearchByNumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwBillsSearchByNumber");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<VwBillsSearchByStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwBillsSearchByStatus");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<VwProductSearchByName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwProductSearchByName");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(15, 2)");
            });

            modelBuilder.Entity<VwProductsSearchByBarcode>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwProductsSearchByBarcode");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(15, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
