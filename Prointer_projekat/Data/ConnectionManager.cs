using Microsoft.EntityFrameworkCore;
using Prointer_projekat.Models;
using Attribute = Prointer_projekat.Models.Attribute;

/*
 * ova klasa sluzi za povezivanje sa bazom podataka (MySQL)
 * automatski generisan fajl pomocu dotnet-ef sa dodacima
 */
namespace Prointer_projekat.Data;

public partial class ConnectionManager : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Attribute> Attributes { get; set; } = null!;
    public DbSet<Relation> Relations { get; set; } = null!;
    public ConnectionManager()
    {
    }

    public ConnectionManager(DbContextOptions<ConnectionManager> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=test_db;user=test_user;password=12345678", ServerVersion.Parse("8.0.31-mysql"));
            }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
