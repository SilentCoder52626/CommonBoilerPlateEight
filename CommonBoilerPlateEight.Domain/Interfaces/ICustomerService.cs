using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<IPagedList<CustomerBasicDetailResponseViewModel>> GetAllAsPagedList(CustomerFilterViewModel model);
        Task<int> Create(CustomerCreateViewModel model);
        Task Edit(CustomerEditViewModel model);
        Task AddEditInterests(CustomerEditTypesViewModel model);
        Task EditBasicDetails(CustomerEditBasicDetailViewModel model);
        Task<CustomerResponseViewModel> GetById(int id);
        Task<bool> HasInterestAdded(int customerId);

        Task ToogleConnectivity(int id);
        Task<string> GetCustomerConnectivityStatus(int id);
        Task SetDeviceId(int id, string deviceId);

        Task Activate(int id);
        Task Deactivate(int id);
        Task<List<DropdownDto>> GetCustomerForDropdown();
    }
}
