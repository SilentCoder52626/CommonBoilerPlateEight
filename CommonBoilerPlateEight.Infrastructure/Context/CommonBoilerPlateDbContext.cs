using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Interfaces;

namespace CommonBoilerPlateEight.Infrastructure.Context
{
    public class CommonBoilerPlateEightDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        private readonly IConfiguration _configuration;

        public CommonBoilerPlateEightDbContext(DbContextOptions<CommonBoilerPlateEightDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DatabaseFacade GetDatabase()
        {
            return Database;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommonBoilerPlateEightDbContext).Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conString = _configuration.GetConnectionString("CommonBoilerPlateEightConnection");

            // Fall back to appsettings.json if the environment variable is not set
            if (string.IsNullOrEmpty(conString))
            {
                conString = Environment.GetEnvironmentVariable("CommonBoilerPlateEightConnection");
            }
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(conString, options => options.UseNetTopologySuite());
            }
        }
    }
}
