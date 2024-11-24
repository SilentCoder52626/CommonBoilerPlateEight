using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CelebrityTypeService : ICelebrityTypeService
    {
        private readonly IDbContext _db;
        public CelebrityTypeService(IDbContext db)
        {

            _db = db;

        }
        public async Task Activate(int id)
        {
            var celebrityType = await _db.CelebrityTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Celebrity type does not exists.");
            celebrityType.Activate();
            _db.CelebrityTypes.Update(celebrityType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Create(CelebrityTypeCreateViewModel dto)
        {
            var userId = AppHttpContext.GetAdminCurrentUserId();
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == userId).ConfigureAwait(false) ?? throw new CustomException("User Not Found.");
            await ValiateCelebrityType(dto.Name).ConfigureAwait(false);
            var celebrityTYpe = new CelebrityType(user, dto.Name);
            await _db.CelebrityTypes.AddAsync(celebrityTYpe).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Deactivate(int id)
        {
            var celebrityType = await _db.CelebrityTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Celebrity type does not exists.");
            celebrityType.Deactivate();
            _db.CelebrityTypes.Update(celebrityType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Edit(CelebrityTypeEditViewModel dto)
        {
            var celebrityType = await _db.CelebrityTypes.Where(a => a.Id == dto.Id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Celebrity type does not exists.");
            await ValiateCelebrityType(dto.Name, celebrityType.Id).ConfigureAwait(false);
            celebrityType.Update(dto.Name);
            _db.CelebrityTypes.Update(celebrityType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IPagedList<CelebrityTypeResponseModel>> GetAllAsPagedList(string? name, int pageNumber = 1, int pageSize = 10)
        {
            var celebrityTypeQueryable = _db.CelebrityTypes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                celebrityTypeQueryable = celebrityTypeQueryable.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }

            var celebrityTypes = await celebrityTypeQueryable.Select(a => new CelebrityTypeResponseModel
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive
            }).ToPagedListAsync(pageNumber, pageSize);
            return celebrityTypes;
        }

        public async Task<IList<CelebrityTypeResponseModel>> GetAllAsync()
        {
            var celebrityTypes = await _db.CelebrityTypes.Where(a => a.IsActive).Select(a => new CelebrityTypeResponseModel
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync().ConfigureAwait(false);
            return celebrityTypes;
        }

        public async Task<CelebrityTypeResponseModel> GetById(int id)
        {
            var celebrityType = await _db.CelebrityTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Celebrity type does not exists.");
            return new CelebrityTypeResponseModel
            {
                Id = celebrityType.Id,
                Name = celebrityType.Name,
                IsActive = celebrityType.IsActive
            };
        }

        private async Task ValiateCelebrityType(string name, int celebrityId = 0)
        {
            var existingCelebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim() && a.Id != celebrityId).ConfigureAwait(false);
            if (existingCelebrityType != null) { throw new CustomException($"Duplicate celebrityType {name}"); }
        }
    }
}
