using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.UnitOfWork;

namespace RecipeProject.Services
{
    public interface IIngredientAllergenService
    {
        public IQueryable<IngredientAllergen> GetAll();
        public Task<IngredientAllergen> AddIngredientAllergen(IngredientAllergen ingredientAllergen);
        public Task UpdateIngredientAllergen(IngredientAllergen ingredientAllergen);
        public Task DeleteIngredientAllergen(int ingredientId, int allergenId);
        public IngredientAllergen? GetById(int ingredientId, int allergenId);
    }
    public class IngredientAllergenService : IIngredientAllergenService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<IngredientAllergen> DbSet;
        public IngredientAllergenService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            DbSet = dbContext.Set<IngredientAllergen>();
        }
        public IQueryable<IngredientAllergen> GetAll()
        {
            return (IQueryable<IngredientAllergen>)_unitOfWork.Context()
                              .Set<IngredientAllergen>()
                              .ToList();
        }
        public async Task<IngredientAllergen> AddIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            var savedEntity = await DbSet.AddAsync(ingredientAllergen);

            IngredientAllergen savedIngredientAllergen = savedEntity.Entity;
            await _unitOfWork.SaveChangesAsync();

            return savedIngredientAllergen;
        }
        public Task UpdateIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteIngredientAllergen(int ingredientId, int allergenId)
        {
            var ingredientAllergenToDelete = this.GetById(ingredientId,allergenId);
            if (ingredientAllergenToDelete != null)
            {
                DbSet.Remove(ingredientAllergenToDelete);
            }
            await _unitOfWork.SaveChangesAsync();

        }
        public IngredientAllergen? GetById(int ingredientId, int allergenId)
        {
            return _unitOfWork.Context()
                              .Set<IngredientAllergen>()
                              .IgnoreQueryFilters()
                              .Where(e => e.IngredientId == ingredientId && e.AllergenId == allergenId)
                              .FirstOrDefault();
        }
    }
}
