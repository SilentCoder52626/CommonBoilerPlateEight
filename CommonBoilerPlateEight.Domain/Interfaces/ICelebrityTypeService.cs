using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityTypeService
    {
        Task<IList<CelebrityTypeResponseModel>> GetAllAsync();
        Task Create(CelebrityTypeCreateViewModel dto);
        Task<IPagedList<CelebrityTypeResponseModel>> GetAllAsPagedList(string? name,int pageNumber = 1,int pageSize= 10 );
        Task<CelebrityTypeResponseModel> GetById(int id);
        Task Edit(CelebrityTypeEditViewModel dto);
        Task Activate(int id);
        Task Deactivate(int id);
    }
}
