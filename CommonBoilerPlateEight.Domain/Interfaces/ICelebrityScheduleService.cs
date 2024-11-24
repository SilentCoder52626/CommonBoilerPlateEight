using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityScheduleService
    {
        Task<IPagedList<CelebrityScheduleViewModel>> GetAllAsPagedList(CelebrityScheduleFilterViewModel filter);
        Task Create(CelebrityScheduleCreateViewModel model,bool isCreatedByAdmin);
        Task Edit(CelebrityScheduleUpdateViewModel model);
        Task<CelebrityScheduleViewModel> GetById(int id);
        Task Activate(int id);
        Task Deactivate(int id);
       
    }
}
