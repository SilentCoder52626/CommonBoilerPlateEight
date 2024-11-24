using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Celebrity;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CelebrityService : ICelebrityService
    {
        private readonly IDbContext _db;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IPasswordHasher _hasher;
        public CelebrityService(IDbContext db,
            IFileUploaderService fileUploaderService,
            IPasswordHasher hasher)
        {
            _db = db;
            _fileUploaderService = fileUploaderService;
            _hasher = hasher;

        }

        public async Task<IPagedList<CelebrityBasicDetailViewModel>> GetFilteredPageCelebrities(CelebrityFilterPageViewModel model, int CustomerId)
        {
            var query = _db.Celebrities.Include(a => a.CelebrityToTypes).ThenInclude(a => a.CelebrityType).Include(a => a.Country).AsQueryable();


            if (!string.IsNullOrEmpty(model.Name))
            {
                var search = model.Name.ToLower().Trim();
                query = query.Where(a => !string.IsNullOrEmpty(a.FullName) && a.FullName.ToLower().Trim().Contains(search) || a.Email.ToLower().Trim().Equals(search) || (!string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(search)));
            }

            if (model.CelebrityTypes.Any())
            {
                var celebrityTypeIds = new HashSet<int>(model.CelebrityTypes);
                query = query
                    .Where(a => a.CelebrityToTypes.Any(x => celebrityTypeIds.Contains(x.CelebrityTypeId)));
            }

            if (!string.IsNullOrEmpty(model.Gender))
            {
                var gender = model.Gender.ToEnum<GenderTypeEnum>();
                query = query.Where(a => a.Gender == gender);
            }
            if (model.FromPrice > 0)
            {
                query = query.Where(a => a.PricePerPost >= model.FromPrice || a.PricePerEvent >= model.FromPrice || a.PricePerDelivery >= model.FromPrice);
            }
            if (model.ToPrice > 0)
            {
                query = query.Where(a => a.PricePerPost <= model.ToPrice || a.PricePerEvent <= model.ToPrice || a.PricePerDelivery <= model.ToPrice);
            }

            if (model.Rating > 0)
            {
                query = query.Where(a => _db.CelebrityReviews
                    .Where(r => r.CelebrityAdvertisement.CelebrityId == a.Id)
                    .Average(r => r.Rating) >= model.Rating);
            }


            var baseImageUrl = _fileUploaderService.GetImageBaseUrl();
            var celebrities = await query.Select(a => new CelebrityBasicDetailViewModel
            {
                Id = a.Id,
                DialCode = a.Country != null ? a.Country.DialCode : string.Empty,
                MobileNumber = a.MobileNumber ?? string.Empty,
                FullName = a.FullName ?? string.Empty,
                ProfileImageURL = baseImageUrl + a.ProfilePictureURL,
                TimeToCall = a.TimeToCall,
                Email = a.Email,
                Status = a.Status.ToString(),
                PricePerAd = a.PricePerPost,
                PricePerDelivery = a.PricePerDelivery,
                PricePerEvent = a.PricePerEvent,
                IsActive = a.IsActive,
                CelebrityType = a.CelebrityToTypes.Any() ? a.CelebrityToTypes.First().CelebrityType.Name : string.Empty,
                AverageRating = _db.CelebrityReviews
                           .Where(r => r.CelebrityAdvertisement.CelebrityId == a.Id)
                            .Average(ad => ad.Rating)

            }).ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);
            return celebrities;
        }


        public async Task<IPagedList<CelebrityBasicDetailViewModel>> GetRecommendedCelebrities(int customerId, int pageNumber, int pageSize)
        {
            // Step 1: Get the top searched/booked celebrity types by the customer
            var mostSearchedCelebrityTypeIds = await _db.CustomerToCelebrityTypes
                .Where(ct => ct.CustomerId == customerId)
                .GroupBy(ct => ct.CelebrityTypeId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3) // Get top 3 most searched types
                .ToListAsync();

            // Step 2: Determine the most common gender the customer has booked in past advertisements (Commented Out)
            // var mostCommonGender = await _db.CelebrityAdvertisements
            //     .Where(ad => ad.CustomerId == customerId)
            //     .Join(_db.Celebrities, ad => ad.CelebrityId, c => c.Id, (ad, c) => c.Gender)
            //     .GroupBy(gender => gender)
            //     .OrderByDescending(g => g.Count())
            //     .Select(g => g.Key)
            //     .FirstOrDefaultAsync();

            // Step 3: Query celebrities based on the most searched types and most common gender (Partially Commented Out)
            var query = _db.Celebrities
                .Include(a => a.CelebrityToTypes).ThenInclude(ct => ct.CelebrityType)
                .Include(a => a.Country)
                .Where(c => mostSearchedCelebrityTypeIds.Contains(c.CelebrityToTypes.Select(ct => ct.CelebrityTypeId).FirstOrDefault()))
                .AsQueryable();

            // Apply the gender filter if a common gender is determined (Commented Out)
            // if (mostCommonGender != null)
            // {
            //     query = query.Where(c => c.Gender == mostCommonGender);
            // }

            // Step 4: Order celebrities by the number of times the customer has booked advertisements with each celebrity (Commented Out)
            // query = query.OrderByDescending(c => c.CelebrityAdvertisements.Count(ad => ad.CustomerId == customerId));

            // Step 5: Select required fields and paginate results

            var baseImageUrl = _fileUploaderService.GetImageBaseUrl();
            // Step 5: Select required fields and paginate results
            var result = await query
                .Select(a => new CelebrityBasicDetailViewModel
                {
                    Id = a.Id,
                    DialCode = a.Country != null ? a.Country.DialCode : string.Empty,
                    MobileNumber = a.MobileNumber ?? string.Empty,
                    FullName = a.FullName ?? string.Empty,
                    ProfileImageURL = baseImageUrl + a.ProfilePictureURL,
                    TimeToCall = a.TimeToCall,
                    Email = a.Email,
                    Status = a.Status.ToString(),
                    PricePerAd = a.PricePerPost,
                    PricePerDelivery = a.PricePerDelivery,
                    PricePerEvent = a.PricePerEvent,
                    IsActive = a.IsActive,
                    CelebrityType = a.CelebrityToTypes.Any() ? a.CelebrityToTypes.First().CelebrityType.Name : string.Empty,
                })
                .ToPagedListAsync(pageNumber, pageSize);

            return result;
        }


        public async Task<int> CreateFromAdmin(CelebrityCreateViewModel model)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            await ValidateCelebrity(model.Email, model.CountryId, model.MobileNumber).ConfigureAwait(false);
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
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
                IsCreatedByAdmin = true,
                TimeToCall = model.TimeToCall,
                PricePerPost = model.PricePerPost,
                PricePerDelivery = model.PricePerDelivery,
                PricePerEvent = model.PricePerEvent,
                Description = model.Description,
                Gender = gender,
                IsActive = true,
                IsOnline = true,
            };
            celebrity.SetCreatedBy(user);
            celebrity.MarkAsApproved(user);
            celebrity.SetPriceRange();
            celebrity.SetPassword(_hasher.HashPassword(model.Password));
            if (model.ProfileImageFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
                celebrity.ProfilePictureURL = imageFilePath;
            }
            celebrity.AddCelebrityTypes(celebrityType);
            AddUpdateCelebritySocialLinks(celebrity, model.FacebookLink, model.InstagramLink, model.SnapchatLink, model.TwitterLink, model.ThreadsLink, model.YoutubeLink);
            if (model.CivilIdFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.CivilIdFile, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
                celebrity.AddCelebrityAttachment(CelebrityAttachmentTypeEnum.CivilId, imageFilePath, model.CivilIdFile.FileName, model.CivilIdFile.ContentType);
            }
            if (model.ContractFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.ContractFile, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
                celebrity.AddCelebrityAttachment(CelebrityAttachmentTypeEnum.Contract, imageFilePath, model.ContractFile.FileName, model.ContractFile.ContentType);
            }

            await _db.Celebrities.AddAsync(celebrity).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
            return celebrity.Id;
        }

        private void AddUpdateCelebritySocialLinks(
         Celebrity celebrity,
         string? facebookUrl,
         string? instagramUrl,
         string? snapchatUrl,
         string? twitterUrl,
         string? threadsUrl,
         string? youtubeUrl)
        {

            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.Facebook, facebookUrl ?? string.Empty);
            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.Instagram, instagramUrl ?? string.Empty);
            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.Snapchat, snapchatUrl ?? string.Empty);
            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.Twitter, twitterUrl ?? string.Empty);
            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.Thread, threadsUrl ?? string.Empty);
            celebrity.AddOrUpdateSocialLink(SocialLinkEnum.YouTube, youtubeUrl ?? string.Empty);

        }


        public async Task<IPagedList<CelebrityBasicDetailViewModel>> GetAllAsPagedList(CelebrityFilterViewModel model)
        {
            var celebrityQueryable = _db.Celebrities.Include(a => a.CelebrityToTypes).ThenInclude(a => a.CelebrityType).Include(a => a.Country).AsQueryable();
            if (!string.IsNullOrEmpty(model.Name))
            {
                var search = model.Name.ToLower().Trim();
                celebrityQueryable = celebrityQueryable.Where(a => !string.IsNullOrEmpty(a.FullName) && a.FullName.ToLower().Trim().Contains(search) || a.Email.ToLower().Trim().Equals(search) || (!string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(search)));
            }
            if (model.CelebrityTypes.Any())
            {
                var celebrityTypeIds = new HashSet<int>(model.CelebrityTypes);
                celebrityQueryable = celebrityQueryable
                    .Where(a => a.CelebrityToTypes.Any(x => celebrityTypeIds.Contains(x.CelebrityTypeId)));
            }
            if (!string.IsNullOrEmpty(model.Status))
            {
                var status = model.Status.ToEnum<StatusTypeEnum>();
                celebrityQueryable = celebrityQueryable.Where(a => a.Status == status);
            }
            if (!string.IsNullOrEmpty(model.Gender))
            {
                var gender = model.Gender.ToEnum<GenderTypeEnum>();
                celebrityQueryable = celebrityQueryable.Where(a => a.Gender == gender);
            }
            if (model.FromPrice > 0)
            {
                celebrityQueryable = celebrityQueryable.Where(a => a.PricePerPost >= model.FromPrice || a.PricePerEvent >= model.FromPrice || a.PricePerDelivery >= model.FromPrice);
            }
            if (model.ToPrice > 0)
            {
                celebrityQueryable = celebrityQueryable.Where(a => a.PricePerPost <= model.ToPrice || a.PricePerEvent <= model.ToPrice || a.PricePerDelivery <= model.ToPrice);
            }
            var baseImageUrl = _fileUploaderService.GetImageBaseUrl();
            var celebrities = await celebrityQueryable.Select(a => new CelebrityBasicDetailViewModel
            {
                Id = a.Id,
                DialCode = a.Country != null ? a.Country.DialCode : string.Empty,
                MobileNumber = a.MobileNumber ?? string.Empty,
                FullName = a.FullName ?? string.Empty,
                ProfileImageURL = baseImageUrl + a.ProfilePictureURL,
                TimeToCall = a.TimeToCall,
                Email = a.Email,
                Status = a.Status.ToString(),
                PricePerAd = a.PricePerPost,
                PricePerDelivery = a.PricePerDelivery,
                PricePerEvent = a.PricePerEvent,
                IsActive = a.IsActive,
                CelebrityType = a.CelebrityToTypes.Any() ? a.CelebrityToTypes.First().CelebrityType.Name : string.Empty,

            }).ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);
            return celebrities;
        }

        public async Task<CelebrityDetailResponseViewModel> GetById(int id)
        {
            var celebrity = await _db.Celebrities
                .Include(a => a.CelebrityToTypes)
                    .ThenInclude(a => a.CelebrityType)
                .Include(a => a.Country)
                .Include(a => a.CelebrityToSocialLinks)
                .Include(a => a.CelebrityToAttachments)
                .Include(a => a.CreatedByUser)
                .Include(a => a.ApprovedByUser)
                .Include(a => a.RejectedByUser)
                .FirstOrDefaultAsync(a => a.Id == id)
                .ConfigureAwait(false) ?? throw new CustomException("Celebrity not found.");

            var baseImageUrl = _fileUploaderService.GetImageBaseUrl();

            var response = new CelebrityDetailResponseViewModel
            {
                Id = celebrity.Id,
                CountryDialCode = celebrity.Country?.DialCode ?? string.Empty,
                MobileNumber = celebrity.MobileNumber ?? string.Empty,
                FullName = celebrity.FullName ?? string.Empty,
                Email = celebrity.Email ?? string.Empty,
                Gender = celebrity.Gender.HasValue ? celebrity.Gender.ToString() : string.Empty,
                ProfileImage = baseImageUrl + celebrity.ProfilePictureURL,
                TimeToCall = celebrity.TimeToCall,
                CountryId = celebrity.Country?.Id ?? 0,
                Status = celebrity.Status.ToString(),
                PricePerAdPost = celebrity.PricePerPost,
                PricePerDelivery = celebrity.PricePerDelivery,
                PriceRange = celebrity.PriceRange,
                PricePerEvent = celebrity.PricePerEvent,
                Description = celebrity.Description,
                IsCreatedByAdmin = celebrity.IsCreatedByAdmin,
                IsActive = celebrity.IsActive,
                CreatedBy = celebrity.IsCreatedByAdmin ? celebrity.CreatedByUser?.FullName : "Self",
                ApprovedBy = celebrity.ApprovedByUser?.FullName ?? string.Empty,
                RejectedBy = celebrity.RejectedByUser?.FullName ?? string.Empty,
                RejectionComment = celebrity.RejectionRemarks,
                CelebrityType = celebrity.CelebrityToTypes.FirstOrDefault()?.CelebrityType?.Name ?? string.Empty,
                CelebrityTypeId = celebrity.CelebrityToTypes.FirstOrDefault()?.CelebrityType?.Id ?? 0,
                SocialLink = new CelebritySocialLinkViewModel
                {
                    FacebookLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.Facebook)?.Url,
                    YoutubeLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.YouTube)?.Url,
                    ThreadsLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.Thread)?.Url,
                    InstagramLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.Instagram)?.Url,
                    SnapchatLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.Snapchat)?.Url,
                    TwitterLink = celebrity.CelebrityToSocialLinks
                        .SingleOrDefault(a => a.Platform == SocialLinkEnum.Twitter)?.Url,
                }
            };
            var civilIdAttachment = celebrity.CelebrityToAttachments
                .SingleOrDefault(a => a.AttachmentType == CelebrityAttachmentTypeEnum.CivilId);
            response.CivilIdAttachment = new CelebrityAttachmentViewModel
            {
                Type = CelebrityAttachmentTypeEnum.CivilId.ToString()
            };
            if (civilIdAttachment != null)
            {
                response.CivilIdAttachment.Id = civilIdAttachment.Id;
                response.CivilIdAttachment.FileName = civilIdAttachment.FileName;
                response.CivilIdAttachment.FilePath = baseImageUrl + civilIdAttachment.FilePath;
            }
            var contractAttachment = celebrity.CelebrityToAttachments
                .SingleOrDefault(a => a.AttachmentType == CelebrityAttachmentTypeEnum.Contract);
            response.ContractAttachment = new CelebrityAttachmentViewModel
            {
                Type = CelebrityAttachmentTypeEnum.Contract.ToString()
            };
            if (contractAttachment != null)
            {
                response.ContractAttachment.Id = contractAttachment.Id;
                response.ContractAttachment.FileName = contractAttachment.FileName;
                response.ContractAttachment.FilePath = baseImageUrl + contractAttachment.FilePath;
            }

            return response;
        }

        private async Task ValidateCelebrity(string email, int countryId, string mobileNumber, int celebrityId = 0)
        {
            var existingEmail = await _db.Celebrities.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && a.Email.ToLower().Trim().Equals(email.ToLower().Trim())).ConfigureAwait(false);
            if (existingEmail != null && existingEmail.Id != celebrityId) throw new CustomException($"Duplicate Email {email}");

            var existingMobileNumber = await _db.Celebrities.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && !string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(mobileNumber.ToLower().Trim()) && a.CountryId == countryId).ConfigureAwait(false);
            if (existingMobileNumber != null && existingMobileNumber.Id != celebrityId) throw new CustomException($"Duplicate MobileNumber {mobileNumber}");
        }

        public async Task DeleteAttachment(int id)
        {
            var celebrityToAttachment = await _db.CelebrityToAttachments.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Attachment not found.");
            _fileUploaderService.RemoveFile(celebrityToAttachment.FilePath);
            _db.CelebrityToAttachments.Remove(celebrityToAttachment);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UploadAttachment(IFormFile file, string type, int celebrityId)
        {
            var attachmentType = type.ToEnum<CelebrityAttachmentTypeEnum>();
            var celebrity = await _db.Celebrities.Include(a => a.CelebrityToAttachments).Where(a => a.Id == celebrityId).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            _fileUploaderService.ValidateImageFiles(new List<IFormFile> { file });
            var existingAttachment = celebrity.CelebrityToAttachments.Where(a => a.AttachmentType == attachmentType).FirstOrDefault();
            if (existingAttachment != null)
            {
                _fileUploaderService.RemoveFile(existingAttachment.FilePath);
                _db.CelebrityToAttachments.Remove(existingAttachment);
            }
            var imageFilePath = await _fileUploaderService.SaveFileAsync(file, FileDirectoryConstants.Celebrity).ConfigureAwait(false);
            celebrity.AddCelebrityAttachment(attachmentType, imageFilePath, file.FileName, file.ContentType);
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task EditBasicDetail(CelebrityEditBasicDetailViewModel model)
        {

            using var tx = TransactionScopeHelper.GetInstance();
            var celebrity = await _db.Celebrities.Include(a => a.CelebrityToTypes).FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");

            await ValidateCelebrity(model.Email, model.CountryId, model.MobileNumber, celebrity.Id).ConfigureAwait(false);
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
            var celebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Id == model.CelebrityTypeId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Type not found.");
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");

            celebrity.Country = country;
            celebrity.MobileNumber = model.MobileNumber;
            celebrity.TimeToCall = model.TimeToCall;
            celebrity.Email = model.Email;
            celebrity.FullName = model.FullName;
            celebrity.PricePerDelivery = model.PricePerDelivery;
            celebrity.PricePerEvent = model.PricePerEvent;
            celebrity.PricePerPost = model.PricePerPost;
            celebrity.Gender = model.Gender.ToEnum<GenderTypeEnum>();
            celebrity.Description = model.Description;
            celebrity.SetPriceRange();
            if (model.ProfileImageFile != null)
            {
                if (!string.IsNullOrEmpty(celebrity.ProfilePictureURL))
                {
                    _fileUploaderService.RemoveFile(celebrity.ProfilePictureURL);
                }
                var imageFileName = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity);
                celebrity.ProfilePictureURL = imageFileName;
            }
            _db.CelebrityToTypes.RemoveRange(celebrity.CelebrityToTypes.ToList());
            celebrity.AddCelebrityTypes(celebrityType);
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            tx.Complete();
        }

        public async Task EditSocialLink(CelebritySocialLinkUpdateViewModel model)
        {
            var celebrity = await _db.Celebrities.Include(a => a.CelebrityToSocialLinks).FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Celebrity Not Found");
            AddUpdateCelebritySocialLinks(celebrity, model.FacebookLink, model.InstagramLink, model.SnapchatLink, model.TwitterLink, model.ThreadsLink, model.YoutubeLink);
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Approve(int id)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
            if (celebrity.Status == StatusTypeEnum.Approved) throw new CustomException("Celebrity is already approved.");
            if (celebrity.Status == StatusTypeEnum.Rejected) throw new CustomException("Celebrity is already rejected.");
            celebrity.MarkAsApproved(user);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ToogleConnectivity(int id)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");

            if (celebrity.IsOnline)
            {
                celebrity.IsOnline = false;
            }
            else
            {
                celebrity.IsOnline = true;
            }
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string> GetCelebrityConnectivityStatus(int id)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            return celebrity.IsOnline ? "Online" : "Offline";
        }

        public async Task SetDeviceId(int id, string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) throw new CustomException("DeviceId not found");
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            celebrity.DeviceId = deviceId;
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Reject(int id, string comment)
        {
            if (string.IsNullOrEmpty(comment)) throw new CustomException("Comment is required");
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            if (celebrity.Status == StatusTypeEnum.Approved) throw new CustomException("Celebrity is already approved.");
            if (celebrity.Status == StatusTypeEnum.Rejected) throw new CustomException("Celebrity is already rejected.");
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
            celebrity.MarkAsRejected(comment, user);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Activate(int id)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            celebrity.Activate();
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Deactivate(int id)
        {
            var celebrity = await _db.Celebrities.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");
            celebrity.Deactivate();
            _db.Celebrities.Update(celebrity);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<List<DropdownDto>> GetCelebrityForDropdown()
        {
            var celebrities = await _db.Celebrities.Where(a => a.IsActive).Select(a => new DropdownDto
            {
                Id = a.Id,
                Name = a.FullName,
            }).ToListAsync().ConfigureAwait(false);
            return celebrities;

        }

        public async Task<GridResponseViewModel> GetFilteredCelebrity(int skip, int take, string? search)
        {
            var celebrityQueryable = _db.Celebrities.Where(a => !a.DeletedDate.HasValue);
            if (!string.IsNullOrEmpty(search))
            {
                celebrityQueryable = celebrityQueryable.Where(a => !string.IsNullOrWhiteSpace(a.FullName) && a.FullName.ToLower().Contains(search.ToLower()) || a.MobileNumber == search || a.Email.ToLower().Equals(search.ToLower()));
            }

            var totalCount = celebrityQueryable.Count();
            var baseImageUrl = _fileUploaderService.GetImageBaseUrl();
            var celebrities = await celebrityQueryable.OrderByDescending(a => a.FullName).Skip(skip).Take(take).Select(a => new CelebrityBasicDetailViewModel
            {
                Id = a.Id,
                DialCode = a.Country != null ? a.Country.DialCode : string.Empty,
                MobileNumber = a.MobileNumber ?? string.Empty,
                FullName = a.FullName ?? string.Empty,
                ProfileImageURL = baseImageUrl + a.ProfilePictureURL,
                TimeToCall = a.TimeToCall,
                Email = a.Email,
                Status = a.Status.ToString(),
                PricePerAd = a.PricePerPost,
                PricePerDelivery = a.PricePerDelivery,
                PricePerEvent = a.PricePerEvent,
            }).ToListAsync();

            return new GridResponseViewModel
            {
                TotalCount = totalCount,
                Data = celebrities
            };
        }
    }
}
