using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.ApplicationContext;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
    
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<HistoryEntity> History { get; set; }

    public DbSet<GenerationEntity> Generation { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserEntity>()
            .HasOne(user => user.Role);
        builder.Entity<UserEntity>()
            .HasMany(user => user.History);
        builder.Entity<HistoryEntity>()
            .HasOne(history => history.User);
        builder.Entity<GenerationEntity>()
            .HasOne(generation => generation.User);
        builder.Entity<UserEntity>()
            .HasMany(user => user.Generation);
        
        
        builder.Entity<RoleEntity>().HasData(new List<RoleEntity>()
        {
            new RoleEntity()
            {
                Id = 1,
                Title = "ROLE_USER"
            },
            new RoleEntity()
            {
                Id = 2,
                Title = "ROLE_ADMIN"
            }
        });
    }
}