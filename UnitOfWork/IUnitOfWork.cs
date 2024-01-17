using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.Repository;

namespace RecipeProject.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity;
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : AbstractEntity;
        int SaveChanges();
        Task SaveChangesAsync();
        RecipeDbContext Context();
    }
}
