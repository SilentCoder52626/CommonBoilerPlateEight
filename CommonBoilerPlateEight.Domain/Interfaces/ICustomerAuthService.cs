using CommonBoilerPlateEight.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICustomerAuthService
    {
        Task<CustomerTokenResponseViewModel> Login(CustomerLoginRequestViewModel model);
        Task<CustomerTokenResponseViewModel> Register(CustomerRegisterViewModel model);
        Task GenerateOtpAndSendForgotPasswordEmail(CustomerForgotPasswordViewModel model);
        Task ValidateOtp(CustomerValidateOtpViewModel model);
        Task<CustomerTokenResponseViewModel> VerifyAccount(string email, string otp);
        Task ResetPassword(CustomerResetPasswordViewModel model);
    }
}
