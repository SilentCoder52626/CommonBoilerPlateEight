using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Application.Contracts.Services
{
    public interface ISettingService
    {
        Task Set(SettingViewModel model);
        Task SetInBulk(List<SettingViewModel> model);
        Task<string?> Get(string key);
    }
}
