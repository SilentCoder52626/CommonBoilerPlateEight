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
    public class CelebrityAuthService : ICelebrityAuthService
    {
        private readonly IDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IFileUploaderService _fileUploaderService;

        public CelebrityAuthService(ITokenService tokenService,
             IPasswordHasher passwordHasher,
             IDbContext db,
             IEmailService emailService,
             IFileUploaderService fileUploaderService)
        {
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _db = db;
            _emailService = emailService;
            _fileUploaderService = fileUploaderService;
        }

        public async Task<CelebrityTokenResponseViewModel> Login(CelebrityLoginRequestViewModel model)
        {
            bool isEmail = model.MobileNumberOrEmail.Contains("@");
            var celebrityQueryable = _db.Celebrities.Include(a => a.Country).AsQueryable();
            if (isEmail)
            {
                celebrityQueryable = celebrityQueryable.Where(a => a.Email.ToLower().Trim().Equals(model.MobileNumberOrEmail.ToLower().Trim()));
            }
            else
            {
                celebrityQueryable = celebrityQueryable.Where(a => !string.IsNullOrWhiteSpace(a.MobileNumber) && a.MobileNumber == model.MobileNumberOrEmail && a.CountryId == model.CountryId);
            }

            var celebrity = await celebrityQueryable.FirstOrDefaultAsync() ?? throw new CustomException("Invalid Credentials");
            var isPasswordValid = _passwordHasher.ValidatePassword(model.Password, celebrity.Password);
            if (!isPasswordValid) throw new CustomException("Invalid Credentials");

            if (celebrity.Status == StatusTypeEnum.Pending) throw new CustomException("User is in pending state.Please contact to administrator");
            if (celebrity.Status == StatusTypeEnum.Rejected) throw new CustomException("User is in rejected state.Please contact to administrator");
            celebrity.DeviceId = model.DeviceId;
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var returnModel = GenerateCelebrityTokenResponse(celebrity);
            return returnModel;
        }

        public async Task<CelebrityTokenResponseViewModel> Register(CelebrityRegisterRequestViewModel model)
        {
            await ValidateCelebrity(model.Email, model.CountryId, model.MobileNumber);
            var celebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Id == model.CelebrityTypeId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Type not found.");
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");
            var gender = model.Gender.ToEnum<GenderTypeEnum>();
            var celebrity = new Celebrity
            {
                FullName = model.FullName,
                Country = country,
                MobileNumber = model.MobileNumber,
                Email = model.Email,
                AuthenticationType = AuthenticationTypeEnum.Default,
                IsCreatedByAdmin = false,
                TimeToCall = model.TimeToCall,
                PricePerPost = model.PricePerPost,
                PricePerDelivery = model.PricePerDelivery,
                PricePerEvent = model.PricePerEvent,
                Description = model.Description,
                Gender = gender,
                DeviceId = model.DeviceId,
                IsOnline = true,
                IsActive = true,
            };
            celebrity.MarkAsPending();
            celebrity.SetPriceRange();
            celebrity.SetPassword(_passwordHasher.HashPassword(model.Password));
            celebrity.AddCelebrityTypes(celebrityType);
            if (model.ProfileImageFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
                celebrity.ProfilePictureURL = imageFilePath;
            }
            await _db.Celebrities.AddAsync(celebrity).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            var returnModel = GenerateCelebrityTokenResponse(celebrity);
            return returnModel;
        }

        private async Task ValidateCelebrity(string email, int countryId, string mobileNumber)
        {
            var existingEmail = await _db.Celebrities.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && a.Email.ToLower().Trim().Equals(email.ToLower().Trim())).ConfigureAwait(false);
            if (existingEmail != null) throw new CustomException($"Duplicate Email {email}");

            var existingMobileNumber = await _db.Celebrities.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && !string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(mobileNumber.ToLower().Trim()) && a.CountryId == countryId).ConfigureAwait(false);
            if (existingMobileNumber != null) throw new CustomException($"Duplicate MobileNumber {mobileNumber}");
        }
        private CelebrityTokenResponseViewModel GenerateCelebrityTokenResponse(Celebrity celebrity)
        {
            var tokenModel = new TokenModel
            {
                Id = celebrity.Id,
                PhoneNumber = celebrity.MobileNumber ?? string.Empty,
                CountryId = celebrity.CountryId ?? 0,
                Email = celebrity.Email,
                UserType = ClaimTypeConstant.UserTypeCelebrity
            };
            return new CelebrityTokenResponseViewModel
            {
                Id = celebrity.Id,
                FullName = celebrity.FullName,
                Email = celebrity.Email,
                PhoneNumber = celebrity.MobileNumber,
                Token = _tokenService.GenerateAccessToken(tokenModel)
            };
        }

        public async Task GenerateOtpAndSendForgotPasswordEmail(CelebrityForgotPasswordRequestViewModel model)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (string.IsNullOrWhiteSpace(celebrity.Email)) throw new CustomException("Email not found for this user.");
            celebrity.SetOtp();
            await _emailService.SendEmailAsync(new SendEmailViewModel
            {
                ToEmails = new List<string> { celebrity.Email },
                Subject = "Forgot Password",
                Body = GenerateEmailTemplate(celebrity.FullName, celebrity.OTP)
            });
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ValidateOtp(CelebrityValidateOtpViewModel model)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (celebrity.OTP == model.OtpCode && celebrity.OTPCreatedOn.HasValue)
            {
                if (celebrity.OTPCreatedOn.Value.AddMinutes(10) < DateTime.Now) throw new CustomException("OTP is already expired");
            }
            else
            {
                throw new CustomException("Invalid OTP.");
            }
        }

        public async Task ResetPassword(CelebrityResetPasswordViewModel model)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Email.ToLower().Trim() == model.Email.ToLower().Trim()).ConfigureAwait(false) ?? throw new CustomException($"No Registered User Found With Email {model.Email}");
            if (model.Password != model.ConfirmPassword) throw new CustomException("Password do not match");
            celebrity.Password = _passwordHasher.HashPassword(model.Password);
            celebrity.ResetOtp();
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public string GenerateEmailTemplate(string celebrity, string otp)
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
                        <p style=""font-size: 16px; line-height: 1.5; color: #555555;"">Hi <strong>{celebrity}</strong>,</p>
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
    }
}
