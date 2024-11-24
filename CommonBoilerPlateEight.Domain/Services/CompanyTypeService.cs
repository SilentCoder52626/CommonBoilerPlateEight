using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private readonly IDbContext _db;
        public CompanyTypeService(IDbContext db)
        {

            _db = db;

        }
        public async Task Activate(int id)
        {
            var CompanyType = await _db.CompanyTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Company type does not exists.");
            CompanyType.Activate();
            _db.CompanyTypes.Update(CompanyType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Create(CompanyTypeCreateViewModel dto)
        {
            var userId = AppHttpContext.GetAdminCurrentUserId();
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == userId).ConfigureAwait(false) ?? throw new CustomException("User Not Found.");
            await ValiateCompanyType(dto.Name).ConfigureAwait(false);
            var CompanyTYpe = new CompanyType(user, dto.Name);
            await _db.CompanyTypes.AddAsync(CompanyTYpe).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }



        public async Task Deactivate(int id)
        {
            var CompanyType = await _db.CompanyTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Company type does not exists.");
            CompanyType.Deactivate();
            _db.CompanyTypes.Update(CompanyType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Edit(CompanyTypeEditViewModel dto)
        {
            var CompanyType = await _db.CompanyTypes.Where(a => a.Id == dto.Id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Company type does not exists.");
            await ValiateCompanyType(dto.Name, CompanyType.Id).ConfigureAwait(false);
            CompanyType.Update(dto.Name);
            _db.CompanyTypes.Update(CompanyType);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IPagedList<CompanyTypeResponseModel>> GetAllAsPagedList(string? name, int pageNumber = 1, int pageSize = 10)
        {
            var CompanyTypeQueryable = _db.CompanyTypes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                CompanyTypeQueryable = CompanyTypeQueryable.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }

            var CompanyTypes = await CompanyTypeQueryable.Select(a => new CompanyTypeResponseModel
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive
            }).ToPagedListAsync(pageNumber, pageSize);
            return CompanyTypes;
        }

        public async Task<IList<CompanyTypeResponseModel>> GetAllAsync()
        {
            var CompanyTypes = await _db.CompanyTypes.Where(a => a.IsActive).Select(a => new CompanyTypeResponseModel
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync().ConfigureAwait(false);
            return CompanyTypes;
        }

        public async Task<CompanyTypeResponseModel> GetById(int id)
        {
            var CompanyType = await _db.CompanyTypes.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Company type does not exists.");
            return new CompanyTypeResponseModel
            {
                Id = CompanyType.Id,
                Name = CompanyType.Name,
                IsActive = CompanyType.IsActive
            };
        }

        private async Task ValiateCompanyType(string name, int CompanyId = 0)
        {
            var existingCompanyType = await _db.CompanyTypes.FirstOrDefaultAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim() && a.Id != CompanyId).ConfigureAwait(false);
            if (existingCompanyType != null) { throw new CustomException($"Duplicate CompanyType {name}"); }
        }
    }
}
