using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Country> Countries { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Setting> Settings { get; set; }       
        public DbSet<CompanyType> CompanyTypes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DatabaseFacade GetDatabase();
    }
}
