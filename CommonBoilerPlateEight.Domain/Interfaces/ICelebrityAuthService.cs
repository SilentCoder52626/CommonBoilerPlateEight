using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityAuthService
    {
        Task<CelebrityTokenResponseViewModel> Login(CelebrityLoginRequestViewModel model);
        Task<CelebrityTokenResponseViewModel> Register(CelebrityRegisterRequestViewModel model);
        Task GenerateOtpAndSendForgotPasswordEmail(CelebrityForgotPasswordRequestViewModel model);
        Task ValidateOtp(CelebrityValidateOtpViewModel model);
        Task ResetPassword(CelebrityResetPasswordViewModel model);
    }
}
