using Microsoft.EntityFrameworkCore;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Todo ve Category arasındaki ilişkiyi tanımlama
            modelBuilder.Entity<Todo>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Todos)
                .HasForeignKey(t => t.CategoryId);

            // Örnek veri ekleme
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Ev İşleri", Description = "Evde yapılması gereken işler" },
                new Category { Id = 2, Name = "İş", Description = "İş ile ilgili görevler" },
                new Category { Id = 3, Name = "Kişisel", Description = "Kişisel görevler" }
            );

            modelBuilder.Entity<Todo>().HasData(
                new Todo { Id = 1, Title = "Alışveriş Yap", Description = "Market alışverişi yapılacak", IsCompleted = false, CategoryId = 1 },
                new Todo { Id = 2, Title = "Rapor Hazırla", Description = "Haftalık rapor hazırlanacak", IsCompleted = false, CategoryId = 2 }
            );
        }
    }
}