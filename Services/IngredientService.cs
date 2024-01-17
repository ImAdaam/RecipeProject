using RecipeProject.DbContext;
using RecipeProject.Entity;
using Microsoft.EntityFrameworkCore;
using RecipeProject.UnitOfWork;

namespace RecipeProject.Services
{
    public interface IIngredientService
    {
        public IQueryable<Ingredient> GetAllRecipesContaining(int id);
        public IQueryable<Ingredient> GetAll(bool includeDeleted);
        public Task<Ingredient> AddIngredient(Ingredient ingredient);
        public Task UpdateIngredient(Ingredient ingredient);
        public Task DeleteIngredient(int id);
        public Ingredient? GetById(int id);
    }
    public class IngredientService : IIngredientService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public IngredientService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Ingredient> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<Ingredient>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<Ingredient>().GetAll();
        }

        public async Task<Ingredient> AddIngredient(Ingredient ingredient)
        {
            Ingredient savedIngredient = await _unitOfWork.GetRepository<Ingredient>().Create(ingredient);
            await _unitOfWork.SaveChangesAsync();

            return savedIngredient;
        }
        public async Task UpdateIngredient(Ingredient ingredient)
        {
            var ingredientToUpdate = await _unitOfWork.GetRepository<Ingredient>().GetById(ingredient.Id);
            ingredientToUpdate.Name = ingredient.Name;
            _unitOfWork.GetRepository<Ingredient>().Update(ingredientToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteIngredient(int id)
        {
            var ingredientToDelete = await _unitOfWork.GetRepository<Ingredient>().GetById(id);
            if (ingredientToDelete.IngredientAllergens != null && ingredientToDelete.Materials != null) 
            {
                ingredientToDelete.Deleted = true;
                _unitOfWork.GetRepository<Ingredient>().Update(ingredientToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotImplementedException(); 
            }
        }
        public Ingredient? GetById(int id)
        {
            return _unitOfWork.Context()
                              .Set<Ingredient>()
                              .IgnoreQueryFilters()
                              .Where(r => r.Id == id)
                              .FirstOrDefault();
        }

        public IQueryable<Ingredient> GetAllRecipesContaining(int id)
        {
            return (IQueryable<Ingredient>)_unitOfWork.Context()
                              .Set<Ingredient>()
                              .IgnoreQueryFilters()
                              .Where(i => i.Id == id)
                              .Include(i => i.Materials)
                              .ThenInclude(m => m.Recipe)
                              .Include(i => i.IngredientAllergens)
                              .ThenInclude(i => i.Allergen)
                              .OrderBy(i => i.Materials.Select(m => m.Recipe.Cooking_time).FirstOrDefault())
                              .ToList();
        }
    }
}
