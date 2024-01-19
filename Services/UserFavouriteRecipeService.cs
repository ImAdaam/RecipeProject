using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.UnitOfWork;

namespace RecipeProject.Services
{
    public interface IUserFavouriteRecipeService
    {
        public IQueryable<UserFavouriteRecipe> GetAll();
        public Task<UserFavouriteRecipe> AddUserFavouriteRecipe(UserFavouriteRecipe userFavouriteRecipe);
        public Task UpdateUserFavouriteRecipe(UserFavouriteRecipe userFavouriteRecipe);
        public Task DeleteUserFavouriteRecipe(int recipeId, int userId);
        public UserFavouriteRecipe? GetById(int recipeId, int userId);
    }
    public class UserFavouriteRecipeService : IUserFavouriteRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly DbSet<UserFavouriteRecipe> DbSet;
        public UserFavouriteRecipeService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            DbSet = dbContext.Set<UserFavouriteRecipe>();
        }
        public IQueryable<UserFavouriteRecipe> GetAll()
        {
            return (IQueryable<UserFavouriteRecipe>)_unitOfWork.Context()
                              .Set<UserFavouriteRecipe>()
                              .ToList();
        }
        public async Task<UserFavouriteRecipe> AddUserFavouriteRecipe(UserFavouriteRecipe userFavouriteRecipe)
        {
            var savedEntity = await DbSet.AddAsync(userFavouriteRecipe);

            UserFavouriteRecipe savedUserFavouriteRecipe = savedEntity.Entity;
            await _unitOfWork.SaveChangesAsync();

            return savedUserFavouriteRecipe;
        }
        public Task UpdateUserFavouriteRecipe(UserFavouriteRecipe userFavouriteRecipe)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteUserFavouriteRecipe(int recipeId, int userId)
        {
            var userFavouriteRecipeToDelete = this.GetById(recipeId, userId);
            if (userFavouriteRecipeToDelete != null)
            {
                DbSet.Remove(userFavouriteRecipeToDelete);
            }
            await _unitOfWork.SaveChangesAsync();

        }
        public UserFavouriteRecipe? GetById(int recipeId, int userId)
        {
            return _unitOfWork.Context()
                              .Set<UserFavouriteRecipe>()
                              .IgnoreQueryFilters()
                              .Where(e => e.RecipeId == recipeId && e.UserId == userId)
                              .FirstOrDefault();
        }
    }
}
