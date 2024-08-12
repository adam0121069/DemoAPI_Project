using Microsoft.EntityFrameworkCore;

namespace CharlieApi.Models.EFModels;

public partial class Mssql2022dbContext : DbContext
{
    public Mssql2022dbContext()
    {
    }

    public Mssql2022dbContext(DbContextOptions<Mssql2022dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK__Users__7AFBF48DAAE4ADB8");

            entity.Property(e => e.AccountNumber).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PassWordStr).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
