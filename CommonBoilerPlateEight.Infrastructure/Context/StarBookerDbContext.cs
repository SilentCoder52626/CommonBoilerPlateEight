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

        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<CelebrityToAttachment> CelebrityToAttachments { get; set; }
        public DbSet<CelebrityToSocialLink> CelebrityToSocialLinks { get; set; }
        public DbSet<CelebrityToType> CelebrityToTypes { get; set; }
        public DbSet<CelebrityType> CelebrityTypes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<QuestionSetting> QuestionSettings { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<CustomerToCelebrityType> CustomerToCelebrityTypes { get; set; }
        public DbSet<CelebritySchedule> CelebritySchedules { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<CelebrityLocation> CelebrityLocations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<CelebrityAdvertisement> CelebrityAdvertisements { get; set; }
        public DbSet<CelebrityAdvertismentQuestion> CelebrityAdvertismentQuestions { get; set; }
        public DbSet<CelebrityReview> CelebrityReviews { get; set; }
        public DbSet<CelebrityAdHistory> CelebrityAdHistories { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemQuestion> CartItemQuestions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }


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
