using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IApplicationSettingService
    {
        Task SetEmailSettings(EmailSetupViewModel model);
        Task<EmailSetupViewModel> GetEmailSettings();
    }
}
