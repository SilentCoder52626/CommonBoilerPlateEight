using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICompanyTypeService
    {
        Task<IList<CompanyTypeResponseModel>> GetAllAsync();
        Task Create(CompanyTypeCreateViewModel dto);
        Task<IPagedList<CompanyTypeResponseModel>> GetAllAsPagedList(string? name, int pageNumber = 1, int pageSize = 10);
        Task<CompanyTypeResponseModel> GetById(int id);
        Task Edit(CompanyTypeEditViewModel dto);
        Task Activate(int id);
        Task Deactivate(int id);
    }
}
