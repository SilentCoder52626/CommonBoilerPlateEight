using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IQuestionSettingService
    {
        Task Create(QuestionSettingCreateViewModel model);
        Task Edit(QuestionSettingEditViewModel model);
        Task<QuestionSettingResponseViewModel> GetById(int id);
        Task<IPagedList<QuestionSettingResponseViewModel>> GetAllAsPagedList(QuestionSettingFilterViewModel model);
        Task Activate(int id);
        Task Deactivate(int id);
    }
}
