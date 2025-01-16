using Microsoft.EntityFrameworkCore;
using TodoBackend.Data.Entities;

namespace TodoBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> users { get; set; }
        public DbSet<TodoEntity> todos { get; set; }
        public DbSet<TodoItemEntity> TodoItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoEntity>()
                .HasMany(t => t.TodoItems)
                .WithOne(t => t.Todo)
                .HasForeignKey(t => t.TodoId)
                .HasPrincipalKey(t => t.Id);
        }
    }
}
