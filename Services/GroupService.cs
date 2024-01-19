using Microsoft.EntityFrameworkCore;
using RecipeProject.DbContext;
using RecipeProject.Entity;
using RecipeProject.Exceptions;
using RecipeProject.UnitOfWork;

namespace RecipeProject.Services
{
    public interface IGroupService
    {
        public IQueryable<Group> GetAll(bool includeDeleted);
        public Task<Group> AddGroup(Group group);
        public Task UpdateGroup(Group group);
        public Task DeleteGroup(int id);
        public Group? GetById(int id);
    }
    public class GroupService : IGroupService
    {
        private readonly RecipeDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(RecipeDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        public IQueryable<Group> GetAll(bool includeDeleted)
        {
            return includeDeleted ? _unitOfWork.GetRepository<Group>().GetAll().IgnoreQueryFilters() : _unitOfWork.GetRepository<Group>().GetAll();
        }
        public async Task<Group> AddGroup(Group group)
        {
            Group savedGroup = await _unitOfWork.GetRepository<Group>().Create(group);
            await _unitOfWork.SaveChangesAsync();

            return savedGroup;
        }
        public async Task UpdateGroup(Group group)
        {
            var groupToUpdate = await _unitOfWork.GetRepository<Group>().GetById(group.Id);
            groupToUpdate.Name = group.Name;
            _unitOfWork.GetRepository<Group>().Update(groupToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteGroup(int id)
        {
            var groupToDelete = await _unitOfWork.GetRepository<Group>().GetById(id);
            if (groupToDelete.Materials != null)
            {
                groupToDelete.Deleted = true;
                _unitOfWork.GetRepository<Group>().Update(groupToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new MethodNotAllowedException("Cannot delete Group due to foreign key contraint");
            }
        }
        public Group? GetById(int id)
        {
            return _unitOfWork.Context()
                               .Set<Group>()
                               .IgnoreQueryFilters()
                               .Where(r => r.Id == id)
                               .FirstOrDefault();
        }
    }
}
