using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class WalletService : IWalletService
    {
        private readonly IDbContext _db;

        public WalletService(IDbContext db)
        {
            _db = db;
        }

        public async Task DeductFromWallet(int customerId, decimal amount)
        {
            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.CustomerId == customerId) ?? throw new Exception("Wallet is empty");

            if (wallet.Balance < amount) throw new Exception("Insufficient funds in wallet.");

            wallet.Balance -= amount;
            wallet.UpdatedDate = DateTime.UtcNow;

            _db.Wallets.Update(wallet);
            await _db.SaveChangesAsync();
        }

        public async Task<WalletBasicViewModel> LoadWalletOfCustomerAsync(int customerId)
        {
            var wallet = await _db.Wallets
                .Where(w => w.CustomerId == customerId)
                .Select(w => new WalletBasicViewModel
                {
                    WalletId = w.Id,
                    Balance = w.Balance,

                })
                .FirstOrDefaultAsync() ?? throw new Exception("Wallet is empty");

            return wallet;
        }

        //For admin panel
        public async Task<IPagedList<WalletDetailViewModel>> LoadWalletDetailsByFilterAsync(WalletFilterViewModel model)
        {
            var query = _db.Wallets.AsQueryable();

            if (model.CustomerId.HasValue)
            {
                query = query.Where(w => w.CustomerId == model.CustomerId.Value);
            }

            var wallets = await query
                .Select(w => new WalletDetailViewModel
                {
                    WalletId = w.Id,
                    CustomerId = w.CustomerId,
                    Balance = w.Balance,
                    CreatedDate = w.CreatedDate,
                    UpdatedDate = w.UpdatedDate,
                })
                .ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);

            return wallets;
        }


        public async Task AddBalanceFromAdminAsync(WalletCreateViewModel model)
        {
            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.CustomerId == model.CustomerId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    CustomerId = model.CustomerId,
                    Balance = model.Balance,
                    CreatedDate = DateTime.UtcNow,
                };
                _db.Wallets.Add(wallet);
            }
            else
            {
                wallet.Balance += model.Balance;
                wallet.UpdatedDate = DateTime.UtcNow;
                _db.Wallets.Update(wallet);
            }

            await _db.SaveChangesAsync();
        }




    }

}
