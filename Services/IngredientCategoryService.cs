using RecipeProject.DbContext;
using RecipeProject.Entity;
using Microsoft.EntityFrameworkCore;
using RecipeProject.UnitOfWork;
using RecipeProject.Exceptions;

namespace RecipeProject.Services
{
    public interface IIngredientCategoryService
    {
        public IQueryable<IngredientCategory> GetAll(bool includeDeleted);
        public Task<IngredientCategory> AddIngredientCategory(IngredientCategory ingredientCategory);
        public Task UpdateIngredientCategory(IngredientCategory ingredientCategory);
        public Task DeleteIngredientCategory(int id);
        public IngredientCategory? GetById(int id);
        public IQueryable<Ingredient> GetAllIngredient(string categoryName);
    }
    public class IngredientCategoryService : IIngredientCategoryService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public IngredientCategoryService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public IQueryable<IngredientCategory> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<IngredientCategory>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<IngredientCategory>().GetAll();
        }
        public async Task<IngredientCategory> AddIngredientCategory(IngredientCategory ingredientCategory)
        {
            IngredientCategory savedIngredientCategory = await _unitOfWork.GetRepository<IngredientCategory>().Create(ingredientCategory);
            await _unitOfWork.SaveChangesAsync();

            return savedIngredientCategory;
        }
        public async Task UpdateIngredientCategory(IngredientCategory ingredientCategory)
        {
            var ingredientCategoryToUpdate = await _unitOfWork.GetRepository<IngredientCategory>().GetById(ingredientCategory.Id);
            ingredientCategoryToUpdate.Name = ingredientCategory.Name;
            _unitOfWork.GetRepository<IngredientCategory>().Update(ingredientCategoryToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteIngredientCategory(int id)
        {
            var ingredientCategoryToDelete = await _unitOfWork.GetRepository<IngredientCategory>().GetById(id);
            if(ingredientCategoryToDelete.Ingredients != null)
            {
                ingredientCategoryToDelete.Deleted = true;
                _unitOfWork.GetRepository<IngredientCategory>().Update(ingredientCategoryToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new MethodNotAllowedException("Cannot delete IngredientCategory due to foreign key contraint");
            }
        }

        public IngredientCategory? GetById(int id)
        {
            return _unitOfWork.Context()
                              .Set<IngredientCategory>()
                              .IgnoreQueryFilters()
                              .Where(r => r.Id == id)
                              .FirstOrDefault();
        }

        public IQueryable<Ingredient> GetAllIngredient(string categoryName)
        {
            return (IQueryable<Ingredient>)_unitOfWork.Context()
                             .Set<IngredientCategory>()
                             .IgnoreQueryFilters()
                             .Where(r => r.Name == categoryName)
                             .Include(r => r.Ingredients)
                             .ToList();
        }

    }
}
