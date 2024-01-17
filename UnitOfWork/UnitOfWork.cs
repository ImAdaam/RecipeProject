using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.Repository;

namespace RecipeProject.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeDbContext _recipeDbContext;
        private Dictionary<Type, object> _repositories;


        public UnitOfWork(RecipeDbContext recipeDbContext)
        {
            _recipeDbContext = recipeDbContext;
        }

        public RecipeDbContext Context()
        {
            return _recipeDbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(_recipeDbContext);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges()
        {
            return _recipeDbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _recipeDbContext.Dispose();
            }
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : AbstractEntity
        {
            return _recipeDbContext.Set<TEntity>();
        }

        public Task SaveChangesAsync()
        {
            return _recipeDbContext.SaveChangesAsync();
        }
    }
}
