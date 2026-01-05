using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}

    public DbSet<Medicamento> Medicamentos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}

