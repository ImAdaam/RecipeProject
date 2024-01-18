using RecipeProject.DbContext;
using RecipeProject.Entity;
using Microsoft.EntityFrameworkCore;
using RecipeProject.UnitOfWork;
using RecipeProject.Exceptions;

namespace RecipeProject.Services
{
    public interface IRecipeService
    {
        public IQueryable<Recipe> GetAll(bool includeDeleted);
        public Task<Recipe> AddRecipe(Recipe recipe);
        public Task UpdateRecipe(Recipe recipe);
        public Task DeleteRecipe(int id);
        public Recipe? GetById(int id);
        public Recipe? GetByIdFull(int id);
    }
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public RecipeService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Recipe> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<Recipe>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<Recipe>().GetAll();
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            Recipe savedRecipe = await _unitOfWork.GetRepository<Recipe>().Create(recipe);
            await _unitOfWork.SaveChangesAsync();

            return savedRecipe;
        }
        public async Task UpdateRecipe(Recipe recipe)
        {
            var recipeToUpdate = await _unitOfWork.GetRepository<Recipe>().GetById(recipe.Id);
            recipeToUpdate.CodeName = recipe.CodeName;
            recipeToUpdate.Title = recipe.Title;
            recipeToUpdate.Description = recipe.Description;
            recipeToUpdate.Preparation_time = recipe.Preparation_time;
            recipeToUpdate.Cooking_time = recipe.Cooking_time;
            _unitOfWork.GetRepository<Recipe>().Update(recipeToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteRecipe(int id)
        {
            var recipeToDelete = await _unitOfWork.GetRepository<Recipe>().GetById(id);
            if( recipeToDelete.Materials != null) 
            {
                recipeToDelete.Deleted = true;
                _unitOfWork.GetRepository<Recipe>().Update(recipeToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new MethodNotAllowedException("Cannot delete Recipe due to foreign key contraint");
            }
        }
        public Recipe? GetById(int id)
        {
            return _unitOfWork.Context()
                              .Set<Recipe>()
                              .IgnoreQueryFilters()
                              .Where(r => r.Id == id)
                              .FirstOrDefault();
        }
        public Recipe? GetByIdFull(int id)
        {
            return _unitOfWork.Context()
                              .Set<Recipe>()
                              .IgnoreQueryFilters()
                              .Where(r => r.Id == id)//recept
                              .Include(r => r.Materials)//hozzávaló
                              .ThenInclude(r => r.Ingredient)
                              .ThenInclude(r => r.IngredientAllergens)
                              .ThenInclude(r => r.Allergen)//allergén
                              .ThenInclude(r => r.Name)
                              .FirstOrDefault();
        }
    }
}
