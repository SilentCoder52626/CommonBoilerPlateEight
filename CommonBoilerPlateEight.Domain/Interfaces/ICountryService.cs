using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICountryService
    {
        Task<IList<CountryResponseViewModel>> GetAllAsync();
        Task Create(CountryCreateViewModel dto);
        Task<IPagedList<CountryResponseViewModel>> GetAllAsPagedList(string? search, int pageNumber = 1, int pageSize = 10);
        Task<CountryResponseViewModel> GetById(int id);
        Task Edit(CountryEditViewModel dto);
    }
}
