using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.UnitOfWork;

namespace RecipeProject.Services
{
    public interface IIngredientAllergenService
    {
        public IQueryable<IngredientAllergen> GetAll(bool includeDeleted);
        public Task<IngredientAllergen> AddIngredientAllergen(IngredientAllergen ingredientAllergen);
        public Task UpdateIngredientAllergen(IngredientAllergen ingredientAllergen);
        public Task DeleteIngredientAllergen(int id);
        public IngredientAllergen? GetById(int id);
    }
    public class IngredientAllergenService : IIngredientAllergenService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public IngredientAllergenService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public Task<IngredientAllergen> AddIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredientAllergen(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<IngredientAllergen> GetAll(bool includeDeleted)
        {
            throw new NotImplementedException();
        }

        public IngredientAllergen? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAllergen(IngredientAllergen ingredientAllergen)
        {
            throw new NotImplementedException();
        }
    }
}
