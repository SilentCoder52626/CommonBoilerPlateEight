using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
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
    public class CountryService : ICountryService
    {
        private readonly IDbContext _db;
        public CountryService(IDbContext db)
        {
            _db = db;
        }
        public async Task Create(CountryCreateViewModel dto)
        {
            await ValidateCountry(dto.Name, dto.Code, dto.DialCode);
            var country = new Country(dto.Name, dto.FlagCode, dto.Code, dto.DialCode);
            await _db.Countries.AddAsync(country).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Edit(CountryEditViewModel dto)
        {
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == dto.Id).ConfigureAwait(false) ?? throw new CustomException("Country not found");
            await ValidateCountry(dto.Name, dto.Code, dto.DialCode, dto.Id).ConfigureAwait(false);
            country.Update(dto.Name, dto.FlagCode, dto.Code, dto.DialCode);
            _db.Countries.Update(country);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IPagedList<CountryResponseViewModel>> GetAllAsPagedList(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var countryQueryable = _db.Countries.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                countryQueryable = countryQueryable.Where(a => a.Name.ToLower().Trim().Contains(search.ToLower().Trim()) || a.Code.ToLower().Trim().Equals(search.ToLower().Trim()) || a.DialCode.ToLower().Trim().Equals(search.ToLower().Trim()));
            }
            var countries = await countryQueryable.OrderBy(a => a.Name).Select(a => new CountryResponseViewModel
            {
                Name = a.Name,
                Code = a.Code,
                DialCode = a.DialCode,
                FlagCode = a.FlagCode,
                Id = a.Id
            }).ToPagedListAsync(pageNumber, pageSize).ConfigureAwait(false);
            return countries;
        }

        public async Task<IList<CountryResponseViewModel>> GetAllAsync()
        {
            var countries = await _db.Countries.OrderBy(a => a.Name).Select(a => new CountryResponseViewModel
            {
                Name = a.Name,
                Code = a.Code,
                DialCode = a.DialCode,
                FlagCode = a.FlagCode,
                Id = a.Id
            }).ToListAsync().ConfigureAwait(false);
            return countries;
        }

        public async Task<CountryResponseViewModel> GetById(int id)
        {
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Country not found");
            return new CountryResponseViewModel
            {
                Name = country.Name,
                Code = country.Code,
                DialCode = country.DialCode,
                FlagCode = country.FlagCode,
                Id = country.Id
            };
        }

        private async Task ValidateCountry(string name, string code, string dialCode, int countryId = 0)
        {
            var countryWithSameNameCodeOrDialCode = await _db.Countries.FirstOrDefaultAsync(a => a.Name.ToLower().Trim().Equals(name.ToLower().Trim()) || code.ToLower().Trim().Equals(code.ToLower().Trim()) || a.DialCode.ToLower().Trim().Equals(dialCode.ToLower().Trim())).ConfigureAwait(false);

            if (countryWithSameNameCodeOrDialCode != null && countryWithSameNameCodeOrDialCode.Id != countryId)
            {
                throw new CustomException("Duplicate Code Or Name Or DialCode");
            }
        }
    }
}
