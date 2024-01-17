using RecipeProject.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace RecipeProject.DbContext
{
    public class RecipeDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<IngredientAllergen> ingredientAllergens { get; set; }

        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(60);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Recipe>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Material>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Allergen>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Group>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<IngredientCategory>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Ingredient>().HasQueryFilter(e => !e.Deleted);

            modelBuilder.Entity<IngredientAllergen>().HasKey(s => new { s.AllergenId, s.IngredientId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
