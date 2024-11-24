using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;

using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IDbContext _db;
        private readonly string _baseImageUrl;
        private readonly IPasswordHasher _hasher;
        public CustomerService(IFileUploaderService fileUploaderService,
            IDbContext db,
            IPasswordHasher hasher)
        {
            _fileUploaderService = fileUploaderService;
            _db = db;
            _baseImageUrl = _fileUploaderService.GetImageBaseUrl();
            _hasher = hasher;
        }


        public async Task<int> Create(CustomerCreateViewModel model)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            if (!model.CelebrityTypeIds.Any()) throw new CustomException("No Interest Selected");
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");
            await ValidateCustomer(model.Email, model.CountryId, model.MobileNumber);
            var gender = model.Gender.ToEnum<GenderTypeEnum>();
            var customer = new Customer
            {
                FullName = model.FullName,
                Country = country,
                MobileNumber = model.MobileNumber,
                Gender = gender,
                Email = model.Email,
                AuthenticationType = AuthenticationTypeEnum.Default,
                IsCreatedByAdmin = true,
                IsActive = true,
                Description = model.Description,
            };
            customer.SetCreatedBy(user);
            customer.MarkAsApproved(user);
            customer.SetPassword(_hasher.HashPassword(model.Password));
            if (model.ProfileImageFile != null)
            {
                var imageFilePath = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Customer).ConfigureAwait(false);
                customer.SetProfilePicture(imageFilePath);
            }
            foreach (var celebrityTypeId in model.CelebrityTypeIds)
            {
                var celebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Id == celebrityTypeId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Type not found.");
                customer.AddCelebrityTypes(celebrityType);
            }
            await _db.Customers.AddAsync(customer).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
            return customer.Id;
        }

        public async Task Edit(CustomerEditViewModel model)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            if (!model.CelebrityTypeIds.Any()) throw new CustomException("No Interest Selected");
            var customer = await _db.Customers.Include(a => a.CustomerToCelebrityTypes).FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            await ValidateCustomer(model.Email, model.CountryId, model.MobileNumber, customer.Id).ConfigureAwait(false);
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");
            var gender = model.Gender.ToEnum<GenderTypeEnum>();
            customer.FullName = model.FullName;
            customer.Email = model.Email;
            customer.MobileNumber = model.MobileNumber;
            customer.Country = country;
            customer.Gender = gender;
            customer.Description = model.Description;
            if (model.ProfileImageFile != null)
            {
                if (!string.IsNullOrEmpty(customer.ProfilePictureURL))
                {
                    _fileUploaderService.RemoveFile(customer.ProfilePictureURL);
                }
                var imageFileName = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity);
                customer.ProfilePictureURL = imageFileName;
            }
            _db.CustomerToCelebrityTypes.RemoveRange(customer.CustomerToCelebrityTypes.ToList());
            foreach (var celebrityTypeId in model.CelebrityTypeIds)
            {
                var celebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Id == celebrityTypeId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Type not found.");
                customer.AddCelebrityTypes(celebrityType);
            }
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
        }

        private async Task ValidateCustomer(string email, int countryId, string mobileNumber, int customerId = 0)
        {
            var existingEmail = await _db.Customers.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && a.Email.ToLower().Trim().Equals(email.ToLower().Trim())).ConfigureAwait(false);
            if (existingEmail != null && existingEmail.Id != customerId) throw new CustomException($"Duplicate Email {email}");

            var existingMobileNumber = await _db.Customers.FirstOrDefaultAsync(a => !a.DeletedDate.HasValue && !string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(mobileNumber.ToLower().Trim()) && a.CountryId == countryId).ConfigureAwait(false);
            if (existingMobileNumber != null && existingMobileNumber.Id != customerId) throw new CustomException($"Duplicate MobileNumber {mobileNumber}");
        }

        public async Task<IPagedList<CustomerBasicDetailResponseViewModel>> GetAllAsPagedList(CustomerFilterViewModel model)
        {
            var customerQueryable = _db.Customers.Include(a => a.CustomerToCelebrityTypes).ThenInclude(a => a.CelebrityType).Include(a => a.Country).AsQueryable();
            if (!string.IsNullOrEmpty(model.Name))
            {
                var search = model.Name.ToLower().Trim();
                customerQueryable = customerQueryable.Where(a => !string.IsNullOrEmpty(a.FullName) && a.FullName.ToLower().Trim().Contains(search) || a.Email.ToLower().Trim().Equals(search) || (!string.IsNullOrEmpty(a.MobileNumber) && a.MobileNumber.ToLower().Trim().Equals(search)));
            }
            if (model.CelebrityTypes.Any())
            {
                var celebrityTypeIds = new HashSet<int>(model.CelebrityTypes);
                customerQueryable = customerQueryable
                    .Where(a => a.CustomerToCelebrityTypes.Any(x => celebrityTypeIds.Contains(x.CelebrityTypeId)));
            }
            if (!string.IsNullOrEmpty(model.Status))
            {
                var status = model.Status.ToEnum<StatusTypeEnum>();
                customerQueryable = customerQueryable.Where(a => a.Status == status);
            }
            if (!string.IsNullOrEmpty(model.Gender))
            {
                var gender = model.Gender.ToEnum<GenderTypeEnum>();
                customerQueryable = customerQueryable.Where(a => a.Gender == gender);
            }


            var customers = await customerQueryable.Select(a => new CustomerBasicDetailResponseViewModel
            {
                Id = a.Id,
                CountryDialCode = a.Country != null ? a.Country.DialCode : string.Empty,
                MobileNumber = a.MobileNumber ?? string.Empty,
                FullName = a.FullName ?? string.Empty,
                Status = a.Status.ToString(),
                ProfileImageUrl = _baseImageUrl + a.ProfilePictureURL,
                Email = a.Email,
                IsActive = a.IsActive,
                CelebrityTypes = a.CustomerToCelebrityTypes.Any() ? string.Join(",", a.CustomerToCelebrityTypes.Select(b => b.CelebrityType).Select(a => a.Name).ToList()) : string.Empty,

            }).ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);
            return customers;
        }

        public async Task<CustomerResponseViewModel> GetById(int id)
        {
            var customer = await _db.Customers.Include(a => a.Country).Include(a => a.CustomerToCelebrityTypes).ThenInclude(a => a.CelebrityType)
                 .Include(a => a.CreatedByUser)
                 .Include(a => a.ApprovedByUser)
                 .Include(a => a.RejectedByUser)
                 .Include(a => a.CreatedByUser).Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            var response = new CustomerResponseViewModel
            {
                Id = customer.Id,
                FullName = customer.FullName ?? string.Empty,
                MobileNumber = customer.MobileNumber ?? string.Empty,
                CountryId = customer.CountryId.HasValue ? customer.CountryId.Value : 0,
                CountryDialCode = customer.Country != null ? customer.Country.DialCode : string.Empty,
                Email = customer.Email,
                ProfileImage = !string.IsNullOrEmpty(customer.ProfilePictureURL) ? _baseImageUrl + customer.ProfilePictureURL : string.Empty,
                CelebrityTypeIds = customer.CustomerToCelebrityTypes.Select(a => a.CelebrityTypeId).ToList(),
                CelebrityTypes = string.Join(",", customer.CustomerToCelebrityTypes.Select(a => a.CelebrityType).Select(a => a.Name).ToList()),
                Gender = customer.Gender.HasValue ? customer.Gender.Value.ToString() : string.Empty,
                Description = customer.Description,
                Status = customer.Status.ToString(),
                IsOnline = customer.IsOnline,
                IsCreatedByAdmin = customer.IsCreatedByAdmin,
                IsActive = customer.IsActive,
                CreatedBy = customer.IsCreatedByAdmin ? customer.CreatedByUser.FullName : "self",
                ApprovedBy = customer.ApprovedByUser?.FullName ?? string.Empty,
                RejectedBy = customer.RejectedByUser?.FullName ?? string.Empty,
                RejectionRemarks = customer.RejectionRemarks

            };
            return response;
        }

        public async Task EditBasicDetails(CustomerEditBasicDetailViewModel model)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var customer = await _db.Customers.Include(a => a.CustomerToCelebrityTypes).FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            await ValidateCustomer(model.Email, model.CountryId, model.MobileNumber, customer.Id).ConfigureAwait(false);
            var country = await _db.Countries.FirstOrDefaultAsync(a => a.Id == model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country Not Found");
            var gender = model.Gender.ToEnum<GenderTypeEnum>();
            customer.FullName = model.FullName;
            customer.Email = model.Email;
            customer.MobileNumber = model.MobileNumber;
            customer.Country = country;
            customer.Gender = gender;
            customer.Description = model.Description;
            if (model.ProfileImageFile != null)
            {
                if (!string.IsNullOrEmpty(customer.ProfilePictureURL))
                {
                    _fileUploaderService.RemoveFile(customer.ProfilePictureURL);
                }
                var imageFileName = await _fileUploaderService.SaveFileAsync(model.ProfileImageFile, FileDirectoryConstants.Celebrity);
                customer.ProfilePictureURL = imageFileName;
            }
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();
        }

        public async Task<bool> HasInterestAdded(int customerId)
        {
            var customer = await _db.Customers.Include(a => a.CustomerToCelebrityTypes).FirstOrDefaultAsync(a => a.Id == customerId).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            return customer.CustomerToCelebrityTypes.Any();
        }

        public async Task AddEditInterests(CustomerEditTypesViewModel model)
        {
            var customer = await _db.Customers.Include(a => a.CustomerToCelebrityTypes).FirstOrDefaultAsync(a => a.Id == model.CustomerId).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            _db.CustomerToCelebrityTypes.RemoveRange(customer.CustomerToCelebrityTypes.ToList());
            foreach (var celebrityTypeId in model.CelebrityTyesIds)
            {
                var celebrityType = await _db.CelebrityTypes.FirstOrDefaultAsync(a => a.Id == celebrityTypeId).ConfigureAwait(false) ?? throw new CustomException("Celebrity Type not found.");
                customer.AddCelebrityTypes(celebrityType);
            }
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ToogleConnectivity(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Customer not found");

            if (customer.IsOnline)
            {
                customer.IsOnline = false;
            }
            else
            {
                customer.IsOnline = true;
            }
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task<List<DropdownDto>> GetCustomerForDropdown()
        {
            var customers = await _db.Customers.Where(a => a.IsActive).Select(a => new DropdownDto
            {
                Id = a.Id,
                Name = a.FullName,
            }).ToListAsync().ConfigureAwait(false);
            return customers;

        }


        public async Task<string> GetCustomerConnectivityStatus(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            return customer.IsOnline ? "Online" : "Offline";
        }

        public async Task SetDeviceId(int id, string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) throw new CustomException("DeviceId not found");
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            customer.DeviceId = deviceId;
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Activate(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            customer.Activate();
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Deactivate(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Customer not found");
            customer.Deactivate();
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
