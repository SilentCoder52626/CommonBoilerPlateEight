using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailViewModel model);
    }
}
