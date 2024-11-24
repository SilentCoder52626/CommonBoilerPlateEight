using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CustomerAuthService : ICustomerAuthService
    {
        private readonly IDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IFileUploaderService _fileUploaderService;
        public CustomerAuthService(IDbContext db,
            ITokenService tokenService,
             IPasswordHasher passwordHasher,
             IEmailService emailService,
             IFileUploaderService fileUploaderService)
        {
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _db = db;
            _emailService = emailService;
            _fileUploaderService = fileUploaderService;
        }
        public async Task GenerateOtpAndSendForgotPasswordEmail(CustomerForgotPasswordViewModel model)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (string.IsNullOrWhiteSpace(customer.Email)) throw new CustomException("Email not found for this user.");
            customer.SetOtp();
            await _emailService.SendEmailAsync(new SendEmailViewModel
            {
                ToEmails = new List<string> { customer.Email },
                Subject = "Forgot Password",
                Body = GenerateEmailTemplate(customer.FullName, customer.OTP)
            });
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<CustomerTokenResponseViewModel> Login(CustomerLoginRequestViewModel model)
        {
            bool isEmail = model.MobileNumberOrEmail.Contains("@");
            var customerQueryable = _db.Customers.Include(a => a.Country).AsQueryable();
            if (isEmail)
            {
                customerQueryable = customerQueryable.Where(a => a.Email.ToLower().Trim().Equals(model.MobileNumberOrEmail.ToLower().Trim()));
            }
            else
            {
                customerQueryable = customerQueryable.Where(a => !string.IsNullOrWhiteSpace(a.MobileNumber) && a.MobileNumber.ToLower().Trim() == model.MobileNumberOrEmail.ToLower().Trim() && a.CountryId == model.CountryId);
            }
            var customer = await customerQueryable.FirstOrDefaultAsync() ?? throw new CustomException("Invalid Credentials");

            var isPasswordValid = _passwordHasher.ValidatePassword(model.Password, customer.Password);
            if (!isPasswordValid) throw new CustomException("Invalid Credentials");

            if (customer.Status == StatusTypeEnum.Pending) throw new CustomException("User is in pending state.Please contact to administrator");
            if (customer.Status == StatusTypeEnum.Rejected) throw new CustomException("User is in rejected state.Please contact to administrator");
            customer.DeviceId = model.DeviceId;
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var returnModel = GenerateCustomerTokenResponse(customer);
            return returnModel;
        }

        private CustomerTokenResponseViewModel GenerateCustomerTokenResponse(Customer customer)
        {
            var tokenModel = new TokenModel
            {
                Id = customer.Id,
                PhoneNumber = customer.MobileNumber ?? string.Empty,
                CountryId = customer.CountryId ?? 0,
                Email = customer.Email,
                UserType = ClaimTypeConstant.UserTypeCustomer
            };
            return new CustomerTokenResponseViewModel
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.MobileNumber,
                Token = _tokenService.GenerateAccessToken(tokenModel)
            };
        }

        public async Task<CustomerTokenResponseViewModel> Register(CustomerRegisterViewModel model)
        {
            await ValidateCustomer(model.Email, model.CountryId, model.MobileNumber);
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");
            var gender = model.Gender.ToEnum<GenderTypeEnum>();
            var customer = new Customer
            {
                FullName = model.FullName,
                Country = country,
                MobileNumber = model.MobileNumber,
                Email = model.Email,
                AuthenticationType = AuthenticationTypeEnum.Default,
                IsCreatedByAdmin = false,
                Gender = gender,
                DeviceId = model.DeviceId,
                IsOnline = true,
                IsActive = true,
                Description = model.Description
            };
            customer.MarkAsPending();
            customer.SetPassword(_passwordHasher.HashPassword(model.Password));

            if (model.ProfileImageFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
                customer.ProfilePictureURL = imageFilePath;
            }
            await _db.Customers.AddAsync(customer).ConfigureAwait(false);
            customer.SetOtp();
            await _emailService.SendEmailAsync(new SendEmailViewModel
            {
                ToEmails = new List<string> { customer.Email },
                Subject = "Verify Account",
                Body = GenerateAccountVerificationEmailTemplate(customer.FullName, customer.OTP)
            });
            await _db.SaveChangesAsync().ConfigureAwait(false);

            var returnModel = GenerateCustomerTokenResponse(customer);
            return returnModel;
        }


        public async Task ResetPassword(CustomerResetPasswordViewModel model)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (model.Password != model.ConfirmPassword) throw new CustomException("Password do not match");
            customer.Password = _passwordHasher.HashPassword(model.Password);
            customer.ResetOtp();
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<CustomerTokenResponseViewModel> VerifyAccount(string email, string otp)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {email}");
            if (customer.OTP == otp && customer.OTPCreatedOn.HasValue)
            {
                if (customer.OTPCreatedOn.Value.AddMinutes(10) < DateTime.Now) throw new CustomException("OTP is already expired");
            }
            else
            {
                throw new CustomException("Invalid OTP.");
            }
            customer.MarkAsApproved(null);
            customer.ResetOtp();
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var returnModel = GenerateCustomerTokenResponse(customer);
            return returnModel;
        }
        public async Task ValidateOtp(CustomerValidateOtpViewModel model)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (customer.OTP == model.OtpCode && customer.OTPCreatedOn.HasValue)
            {
                if (customer.OTPCreatedOn.Value.AddMinutes(10) < DateTime.Now) throw new CustomException("OTP is already expired");
            }
            else
            {
                throw new CustomException("Invalid OTP.");
            }
        }
        public string GenerateAccountVerificationEmailTemplate(string customerName, string otp)
        {
            return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Account Verification</title>
            </head>
            <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
                <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);"">
                    <div style=""text-align: center; padding: 10px 0;"">
                        <h1 style=""margin: 0; color: #333333;"">Verify Account</h1>
                    </div>
                    <div style=""padding: 20px;"">
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">Hi <strong>{customerName}</strong>,</p>
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">You have successfully registered. Use the following OTP code to verify your acccount:</p>
                        <p style=""font-size: 24px; font-weight: bold; color: #333333;"">{otp}</p>
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">OTP will expired within 10 minutes.</p>
                    </div>
                    <div style=""text-align: center; padding: 10px 0; font-size: 14px; color: #777777;"">
                        <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        public string GenerateEmailTemplate(string customerName, string otp)
        {
            return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Forgot Password</title>
            </head>
            <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
                <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);"">
                    <div style=""text-align: center; padding: 10px 0;"">
                        <h1 style=""margin: 0; color: #333333;"">Forgot Password</h1>
                    </div>
                    <div style=""padding: 20px;"">
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">Hi <strong>{customerName}</strong>,</p>
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">You have requested to reset your password. Use the following OTP code to proceed:</p>
                        <p style=""font-size: 24px; font-weight: bold; color: #333333;"">{otp}</p>
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">OTP will expired within 10 minutes.</p>
                    </div>
                    <div style=""text-align: center; padding: 10px 0; font-size: 14px; color: #777777;"">
                        <p>&copy; {DateTime.Now.Year} Your Company. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        private async Task ValidateCustomer(string email, int countryId, string mobileNumber)
        {
            var existingEmail = await _db.Customers.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && a.Email.ToLower().Trim().Equals(email.ToLower().Trim())).ConfigureAwait(false);
            if (existingEmail != null) throw new CustomException($"Duplicate Email {email}");

            var existingMobileNumber = await _db.Customers.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && !string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(mobileNumber.ToLower().Trim()) && a.CountryId == countryId).ConfigureAwait(false);
            if (existingMobileNumber != null) throw new CustomException($"Duplicate MobileNumber {mobileNumber}");
        }
    }
}
