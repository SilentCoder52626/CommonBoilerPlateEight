using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Application.Contracts.Services;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class SettingService : ISettingService
    {
        private readonly IDbContext _db;
        public SettingService(IDbContext db)
        {
            _db = db;
        }
        public async Task<string?> Get(string key)
        {
            var settingValue = (await _db.Settings.Where(a => a.Key == key).FirstOrDefaultAsync().ConfigureAwait(false))?.Value;
            return settingValue;
        }

        public async Task Set(SettingViewModel model)
        {
            await SetSettingData(model).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task SetInBulk(List<SettingViewModel> model)
        {
            foreach (var data in model)
            {
                await SetSettingData(data).ConfigureAwait(false);
            }
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
        private async Task SetSettingData(SettingViewModel model)
        {
            var existingSetting = await _db.Settings.Where(a => a.Key == model.Key).FirstOrDefaultAsync().ConfigureAwait(false);
            if (existingSetting == null)
            {
                var setting = new Setting()
                {
                    Key = model.Key,
                    Value = model.Value
                };
                await _db.Settings.AddAsync(setting).ConfigureAwait(false);
            }
            else
            {
                existingSetting.Value = model.Value;
                _db.Settings.Update(existingSetting);
            }
        }
    }
}
