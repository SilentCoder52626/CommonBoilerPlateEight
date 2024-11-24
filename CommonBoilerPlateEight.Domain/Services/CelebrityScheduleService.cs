using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CelebrityScheduleService : ICelebrityScheduleService
    {
        private readonly IDbContext _db;
        public CelebrityScheduleService(IDbContext db)
        {
            _db = db;
        }

        public async Task Activate(int id)
        {
            var celebritySchedule = await _db.CelebritySchedules.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Schedule not found");
            celebritySchedule.Activate();
            _db.CelebritySchedules.Update(celebritySchedule);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Create(CelebrityScheduleCreateViewModel model, bool isCreatedByAdmin)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == model.CelebrityId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Not Found");
            await ValidateSchedule(model.FromTime, model.ToTime, model.Date, model.CelebrityId).ConfigureAwait(false);
            var newSchedule = new CelebritySchedule
            {
                Celebrity = celebrity,
                Date = model.Date,
                From = model.FromTime,
                To = model.ToTime,
                IsActive = true
            };
            if (isCreatedByAdmin)
            {
                var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
                newSchedule.SetCreatedBy(user);
            }

            await _db.CelebritySchedules.AddAsync(newSchedule).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);

        }

        private async Task ValidateSchedule(TimeOnly fromTime, TimeOnly toTime, DateOnly date, int celebrityId, int celebrityScheduleId = 0)
        {
            if ((toTime - fromTime).TotalHours > 2)
            {
                throw new Exception("A schedule cannot be more than 2 hours.");
            }
            var existingSchedules = await _db.CelebritySchedules.Where(a => a.CelebrityId == celebrityId && a.Date == date).ToListAsync().ConfigureAwait(false);
            if (existingSchedules.Count() >= 3) // use from setting
            {
                throw new CustomException("A celebrity cannot have more than 3 schedules on the same day.");
            }

            if (existingSchedules.Any(s =>s.Date == date && fromTime < s.To && toTime > s.From && s.Id != celebrityScheduleId))
            {
                throw new CustomException("The schedule overlaps with an existing schedule.");
            }
        }


        public async Task Deactivate(int id)
        {
            var celebritySchedule = await _db.CelebritySchedules.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Schedule not found");
            celebritySchedule.Deactivate();
            _db.CelebritySchedules.Update(celebritySchedule);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Edit(CelebrityScheduleUpdateViewModel model)
        {
            var celebritySchedule = await _db.CelebritySchedules.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Schedule Not Found.");
            await ValidateSchedule(model.FromTime, model.ToTime, model.Date, celebritySchedule.CelebrityId, celebritySchedule.Id).ConfigureAwait(!false);
            celebritySchedule.From = model.FromTime;
            celebritySchedule.To = model.ToTime;
            celebritySchedule.Date = model.Date;
            _db.CelebritySchedules.Update(celebritySchedule);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IPagedList<CelebrityScheduleViewModel>> GetAllAsPagedList(CelebrityScheduleFilterViewModel filter)
        {
            var scheduleQUeryable = _db.CelebritySchedules.Where(a => !a.DeletedDate.HasValue).AsQueryable();
            if (filter.CelebrityId > 0)
            {
                scheduleQUeryable = scheduleQUeryable.Where(a => a.CelebrityId == filter.CelebrityId);
            }

            if (filter.From.HasValue)
            {
                var filterDate = DateOnly.FromDateTime(filter.From.Value); 
                 scheduleQUeryable = scheduleQUeryable.Where(a => a.Date >= filterDate);
            }
            if (filter.To.HasValue)
            {
                var filterDate = DateOnly.FromDateTime(filter.To.Value);
                scheduleQUeryable = scheduleQUeryable.Where(a => a.Date <= filterDate);
            }

            var celebritySchedules = await scheduleQUeryable.OrderByDescending(a => a.Id).Select(a => new CelebrityScheduleViewModel
            {
                Id = a.Id,
                Celebrity = a.Celebrity.FullName ?? string.Empty,
                Date = a.Date,
                FromTime = a.From,
                ToTime = a.To,
                IsActive = a.IsActive,
                CelebrityId = a.CelebrityId,
                FormattedSchedule = a.GetSchedule()
            }).ToPagedListAsync(filter.PageNumber, filter.pageSize).ConfigureAwait(false);
            return celebritySchedules;
        }

        public async Task<CelebrityScheduleViewModel> GetById(int id)
        {
            var celebritySchedule = await _db.CelebritySchedules.Include(a => a.Celebrity).FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Schedule Not Found.");
            return new CelebrityScheduleViewModel
            {
                Id = celebritySchedule.Id,
                Celebrity = celebritySchedule.Celebrity.FullName ?? string.Empty,
                Date = celebritySchedule.Date,
                FromTime = celebritySchedule.From,
                ToTime = celebritySchedule.To,
                IsActive = celebritySchedule.IsActive,
                CelebrityId = celebritySchedule.CelebrityId,
                FormattedSchedule = celebritySchedule.GetSchedule()
            };

        }
    }
}
