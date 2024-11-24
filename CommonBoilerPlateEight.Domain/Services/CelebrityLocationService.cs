using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CelebrityLocationService : ICelebrityLocationService
    {
        private readonly IDbContext _db;

        public CelebrityLocationService(IDbContext db)
        {
            _db = db;
        }
        public async Task Create(CelebrityLocationCreateViewModel model)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == model.CelebrityId).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            if (model.Longitude == 0 || model.Latitude == 0) { throw new CustomException("Latitude /Longitude is required"); }
            var celebrityLocation = new CelebrityLocation();
            celebrityLocation.Celebrity = celebrity;
            celebrityLocation.SetLocationDetails(model.Latitude, model.Longitude, model.FullAddress, model.Note, model.Area, model.Block, model.Street, model.Governorate, model.GooglePlusCode);
            await _db.CelebrityLocations.AddAsync(celebrityLocation).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var celebrityLocation = await _db.CelebrityLocations.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Location not found");
            celebrityLocation.MarkAsDeleted();
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<CelebrityLocationViewModel>> GetAllLocationsOfcelebrity(int celebrityId)
        {
            var locationsByCelebrity = await _db.CelebrityLocations.Include(a => a.Celebrity).Where(a => a.CelebrityId == celebrityId).Select(celebrityLocation => new CelebrityLocationViewModel
            {
                Id = celebrityLocation.Id,
                CelebrityId = celebrityLocation.CelebrityId,
                Celebrity = celebrityLocation.Celebrity.FullName ?? string.Empty,
                Latitude = celebrityLocation.Latitude,
                Longitude = celebrityLocation.Longitude,
                Area = celebrityLocation.Area,
                Block = celebrityLocation.Block,
                Governorate = celebrityLocation.Governorate,
                Street = celebrityLocation.Street,
                GooglePlusCode = celebrityLocation.GooglePlusCode,
                FullAddress = celebrityLocation.FullAddress
            }).ToListAsync();
            return locationsByCelebrity;
        }

        public async Task<CelebrityLocationViewModel> GetById(int id)
        {
            var celebrityLocation = await _db.CelebrityLocations.Include(a => a.Celebrity).FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Location not found");
            return new CelebrityLocationViewModel
            {
                Id = celebrityLocation.Id,
                CelebrityId = celebrityLocation.CelebrityId,
                Celebrity = celebrityLocation.Celebrity.FullName ?? string.Empty,
                Latitude = celebrityLocation.Latitude,
                Longitude = celebrityLocation.Longitude,
                Area = celebrityLocation.Area,
                Block = celebrityLocation.Block,
                Governorate = celebrityLocation.Governorate,
                Street = celebrityLocation.Street,
                GooglePlusCode = celebrityLocation.GooglePlusCode,
                FullAddress = celebrityLocation.FullAddress,
                Note = celebrityLocation.Note
            };

        }

        public async Task Update(CelebrityLocationEditViewModel model)
        {
            var celebrityLocation = await _db.CelebrityLocations.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Location not found");
            celebrityLocation.SetLocationDetails(model.Latitude, model.Longitude, model.FullAddress, model.Note, model.Area, model.Block, model.Street, model.Governorate, model.GooglePlusCode);
            _db.CelebrityLocations.Update(celebrityLocation);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
