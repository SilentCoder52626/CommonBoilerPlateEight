using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class WalletIndexViewModel
    {
        public WalletFilterViewModel Filter { get; set; } = new WalletFilterViewModel();
        public IPagedList<WalletDetailViewModel> Wallets { get; set; }
    }
}
