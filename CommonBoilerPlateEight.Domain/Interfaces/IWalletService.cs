using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IWalletService
    {
        Task AddBalanceFromAdminAsync(WalletCreateViewModel model);
        Task DeductFromWallet(int customerId, decimal amount);
        Task<IPagedList<WalletDetailViewModel>> LoadWalletDetailsByFilterAsync(WalletFilterViewModel model);
        Task<WalletBasicViewModel> LoadWalletOfCustomerAsync(int customerId);
    }
}
