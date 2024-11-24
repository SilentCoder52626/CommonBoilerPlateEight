using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityLocationService
    {
        Task Create(CelebrityLocationCreateViewModel model);
        Task<List<CelebrityLocationViewModel>> GetAllLocationsOfcelebrity(int celebrityId);
        Task Update(CelebrityLocationEditViewModel model);
        Task<CelebrityLocationViewModel> GetById(int id);
        Task Delete(int id);
    }
}
