using AnnaBot.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class AnnaBotDbContext : DbContext
{
    public AnnaBotDbContext(DbContextOptions<AnnaBotDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomResponse> CustomResponse { get; set; } = default!;
}
