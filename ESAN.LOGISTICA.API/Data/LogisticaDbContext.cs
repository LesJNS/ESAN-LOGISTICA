using Microsoft.EntityFrameworkCore;

namespace ESAN.LOGISTICA.API.Data;

public partial class LogisticaDbContext : DbContext
{
    public LogisticaDbContext()
    {
    }

    public LogisticaDbContext(DbContextOptions<LogisticaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(
            "Server=LJNS;Database=LogisticaDB;Integrated Security=True;TrustServerCertificate=True"
        );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.Property(e => e.IdCategory)
                .HasColumnName("Id_Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.Property(e => e.IdProducto)
                .HasColumnName("Id_Producto");

            entity.Property(e => e.Name)
                .HasMaxLength(100);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.IdCategory)
                .HasColumnName("Id_Category");

            entity.HasOne(d => d.IdCategoryNavigation)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_Products_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}