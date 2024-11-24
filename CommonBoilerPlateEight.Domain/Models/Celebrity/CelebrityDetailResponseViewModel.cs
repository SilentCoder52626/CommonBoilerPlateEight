using Microsoft.AspNetCore.Http;
using CommonBoilerPlateEight.Domain.Models.Celebrity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityDetailResponseViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string CountryDialCode { get; set; }
        public string MobileNumber { get; set; }
        public string Gender { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime? TimeToCall { get; set; }
        public decimal PricePerAdPost { get; set; }
        public decimal PricePerEvent { get; set; }
        public decimal PricePerDelivery { get; set; }
        public string PriceRange { get; set; }
        public int CelebrityTypeId { get; set; }
        public string CelebrityType { get; set; }
        public string? Description { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectedBy { get; set; }
        public string? RejectionComment { get; set; }
        public string Status { get; set; }
        public CelebritySocialLinkViewModel SocialLink { get; set; } = new CelebritySocialLinkViewModel();
        public CelebrityAttachmentViewModel CivilIdAttachment { get; set; } = new CelebrityAttachmentViewModel();
        public CelebrityAttachmentViewModel ContractAttachment { get; set; } = new CelebrityAttachmentViewModel();
    }
}
