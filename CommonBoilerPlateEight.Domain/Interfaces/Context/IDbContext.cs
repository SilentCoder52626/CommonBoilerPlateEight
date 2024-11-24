using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerToCelebrityType> CustomerToCelebrityTypes { get; set; }
        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<CelebrityToAttachment> CelebrityToAttachments { get; set; }
        public DbSet<CelebrityToSocialLink> CelebrityToSocialLinks { get; set; }
        public DbSet<CelebrityToType> CelebrityToTypes { get; set; }
        public DbSet<CelebrityType> CelebrityTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<QuestionSetting> QuestionSettings { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<CelebritySchedule> CelebritySchedules { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<CelebrityLocation> CelebrityLocations { get; set; }
        DbSet<CelebrityReview> CelebrityReviews { get; set; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<CelebrityAdvertisement> CelebrityAdvertisements { get; set; }
        DbSet<CelebrityAdvertismentQuestion> CelebrityAdvertismentQuestions { get; set; }
        DbSet<CelebrityAdHistory> CelebrityAdHistories { get; set; }
        DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemQuestion> CartItemQuestions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DatabaseFacade GetDatabase();
    }
}
