using RecipeProject.DbContext;
using RecipeProject.Entity;
using Microsoft.EntityFrameworkCore;
using RecipeProject.UnitOfWork;
using RecipeProject.Exceptions;

namespace RecipeProject.Services
{
    public interface IAllergenService
    {
        public IQueryable<Allergen> GetAll(bool includeDeleted);
        public Task<Allergen> AddAllergen(Allergen allergen);
        public Task UpdateAllergen(Allergen allergen);
        public Task DeleteAllergen(int id);
        public Allergen? GetById(int id);
        public IQueryable<Ingredient> GetAllIngredient(string allergenName);
    }
    public class AllergenService : IAllergenService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public AllergenService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Allergen> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<Allergen>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<Allergen>().GetAll();
        }
        public async Task<Allergen> AddAllergen(Allergen allergen)
        {
            Allergen savedAllergen = await _unitOfWork.GetRepository<Allergen>().Create(allergen);
            await _unitOfWork.SaveChangesAsync();

            return savedAllergen;
        }
        public async Task UpdateAllergen(Allergen allergen)
        {
            var allergenToUpdate = await _unitOfWork.GetRepository<Allergen>().GetById(allergen.Id);
            allergenToUpdate.Name = allergen.Name;
            _unitOfWork.GetRepository<Allergen>().Update(allergenToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAllergen(int id)
        {
            var allergenToDelete = await _unitOfWork.GetRepository<Allergen>().GetById(id);
            if (allergenToDelete.IngredientAllergens != null)
            {
                allergenToDelete.Deleted = true;
                _unitOfWork.GetRepository<Allergen>().Update(allergenToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new MethodNotAllowedException("Cannot delete Allergen due to foreign key contraint");
            }
        }
        public Allergen? GetById(int id)
        {
            return _unitOfWork.Context()
                               .Set<Allergen>()
                               .IgnoreQueryFilters()
                               .Where(r => r.Id == id)
                               .FirstOrDefault();
        }
        public IQueryable<Ingredient> GetAllIngredient(string allergenName)
        {
            return (IQueryable<Ingredient>)_unitOfWork.Context()
                             .Set<Allergen>()
                             .IgnoreQueryFilters()
                             .Where(r => r.Name == allergenName)
                             .Include(r => r.IngredientAllergens)
                             .ThenInclude(r => r.Ingredient)
                             .ToList();
        }
    }
}
