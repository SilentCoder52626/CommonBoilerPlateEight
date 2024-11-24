using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.CelebrityAdvertisment;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CelebrityAdvertismentService : ICelebrityAdvertismentService
    {
        private readonly IDbContext _db;
        public CelebrityAdvertismentService(IDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AcceptAdvertisment(string trackingId)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var advertisement = await _db.CelebrityAdvertisements.Where(x => x.TrackingId == trackingId).Include(x => x.Booking)
                .FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException($"Advertisment Does not Exist.");

            if (advertisement.Booking.Status == BookingStatusEnum.Pending)
            {
                advertisement.Booking.Status = BookingStatusEnum.InProcess;
                AddBookingHistory(advertisement.Booking, "Booking is in Process", BookingStatusEnum.InProcess);
                _db.Bookings.Update(advertisement.Booking);
            }

            advertisement.Status = BookingStatusEnum.Accepted;
            AddAdvertismentHistory(advertisement, "Ad is Accepted", BookingStatusEnum.Accepted);
            _db.CelebrityAdvertisements.Update(advertisement);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
            return true;
        }

        public async Task<bool> CancelAdvertisment(string trackingId, string comment)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var advertisement = await _db.CelebrityAdvertisements.Where(x => x.TrackingId == trackingId).Include(x => x.Booking)
                .FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException($"Order Not Found. ");

            advertisement.Status = BookingStatusEnum.Cancelled;
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var isBookingCancelled = advertisement.Booking.CelebrityAdvertisements.All(x => x.Status == BookingStatusEnum.Cancelled);

            if (isBookingCancelled)
            {
                advertisement.Booking.Status = BookingStatusEnum.Cancelled;
                AddBookingHistory(advertisement.Booking, comment, BookingStatusEnum.Cancelled);
                _db.Bookings.Update(advertisement.Booking);
            }

            AddAdvertismentHistory(advertisement, comment, BookingStatusEnum.Cancelled);
            _db.CelebrityAdvertisements.Update(advertisement);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
            return true;
        }

        public async Task<bool> CompleteAdvertisment(string trackingId)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var advertisement = await _db.CelebrityAdvertisements.Where(x => x.TrackingId == trackingId).Include(x => x.Booking)
                .FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException($"Order Not Found. ");

            if (DateTime.UtcNow < advertisement.AdDate)
                throw new CustomException($"Advertisment can not be completed before the scheduled time");

            advertisement.Status = BookingStatusEnum.Completed;
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var isBookingCompleted = advertisement.Booking.CelebrityAdvertisements.All(x => x.Status == advertisement.Status);
            if (isBookingCompleted)
            {
                advertisement.Booking.Status = BookingStatusEnum.Completed;
                AddBookingHistory(advertisement.Booking, "Booking is Completed.", BookingStatusEnum.Completed);
                _db.Bookings.Update(advertisement.Booking);
            }

            AddAdvertismentHistory(advertisement, "Ad is Completed", BookingStatusEnum.Completed);
            _db.CelebrityAdvertisements.Update(advertisement);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
            return true;
        }

        public async Task<CelebrityAdvertismentResponseModel> GetCelebrityAdvertismentAsync(string trackingId)
        {
            var celebrityOrder = await _db.CelebrityAdvertisements.Where(x => x.TrackingId == trackingId).Include(x => x.Booking).
            Select(x => new CelebrityAdvertismentResponseModel
            {
                BookingDate = x.Booking.CreatedDate,
                TrackingId = x.TrackingId,
                CelebrityName = x.Celebrity.FullName,
                BookingId = x.Booking.Id,
                CompanyName = x.CompanyName,
                OrderDate = x.CreatedDate,
                OrderId = x.Id,
                Status = x.Status.ToString(),
                Total = x.AdPrice
            })
            .FirstOrDefaultAsync().ConfigureAwait(false);
            if (celebrityOrder == null) throw new CustomException($"Order Not Found.");
            return celebrityOrder;
        }

        public async Task<IPagedList<CelebrityAdvertismentResponseModel>> GetAdvertismentsOfACelebrityAsync(int celebrityId,
            CelebrityAdvertismentFilterViewModel model)
        {
            var query = _db.CelebrityAdvertisements.Where(x => x.CelebrityId == celebrityId)
                 .Include(x => x.Booking).AsQueryable();

            if (model.From.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= model.From.Value);
            }

            if (model.To.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= model.To.Value);
            }

            if (!string.IsNullOrEmpty(model.Status))
            {
                var status = model.Status.ToEnum<BookingStatusEnum>();
                query = query.Where(x => x.Status == status);
            }

            var celebrityOrders = await query.Select(x => new CelebrityAdvertismentResponseModel
            {
                BookingDate = x.Booking.CreatedDate,
                TrackingId = x.TrackingId,
                CelebrityName = x.Celebrity.FullName,
                BookingId = x.Booking.Id,
                CompanyName = x.CompanyName,
                OrderDate = x.CreatedDate,
                OrderId = x.Id,
                Status = x.Status.ToString(),
                Total = x.AdPrice
            }).ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);
            //if (!celebrityOrders.Any()) return Enumerable
            //        .Empty<CelebrityAdvertismentResponseModel>().ToList();
            return celebrityOrders;
        }

        private void AddBookingHistory(Booking booking, string comment, BookingStatusEnum status)
        {
            booking.AddBookingHistory(comment, status);
        }

        private void AddAdvertismentHistory(CelebrityAdvertisement advertisment, string comment, BookingStatusEnum status)
        {
            advertisment.AddAdvertismentHistory(comment, status);
        }
    }
}
