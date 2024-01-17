using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.UnitOfWork;
using System.Net;

namespace RecipeProject.Services
{
    public interface IMaterialService
    {
        public IQueryable<Material> GetAll(bool includeDeleted);
        public Task<Material> AddMaterial(Material material);
        public Task UpdateMaterial(Material material);
        public Task DeleteMaterial(int id);
        public Material? GetById(int id);
    }
    public class MaterialService : IMaterialService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public MaterialService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Material> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<Material>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<Material>().GetAll();
        }
        public async Task<Material> AddMaterial(Material material)
        {
            Material savedMaterial = await _unitOfWork.GetRepository<Material>().Create(material);
            await _unitOfWork.SaveChangesAsync();

            return savedMaterial;
        }
        public async Task UpdateMaterial(Material material)
        {
            var materialToUpdate = await _unitOfWork.GetRepository<Material>().GetById(material.Id);
            materialToUpdate.Quantity = material.Quantity;
            materialToUpdate.Measure = material.Measure;
            _unitOfWork.GetRepository<Material>().Update(materialToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteMaterial(int id)
        {
            var materialToDelete = await _unitOfWork.GetRepository<Material>().GetById(id);
            materialToDelete.Deleted = true;
            _unitOfWork.GetRepository<Material>().Update(materialToDelete);
            await _unitOfWork.SaveChangesAsync();
        }

        public Material? GetById(int id)
        {
            return _unitOfWork.Context()
                               .Set<Material>()
                               .IgnoreQueryFilters()
                               .Where(r => r.Id == id)
                               .FirstOrDefault();
        }

    }
}
