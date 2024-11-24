using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Extensions;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityToSocialLink : BaseEntity
    {
        protected CelebrityToSocialLink()
        {

        }

        public CelebrityToSocialLink(Celebrity celebrity, SocialLinkEnum platform, string url)
        {
            Celebrity = celebrity;
            Platform = platform;
            Url = url;
            Icon = SocialLinkIconMapper.GetIconPath(platform);
        }

        public void Update(string url)
        {
            Url = url;
        }
        public int CelebrityId { get; set; }
        public Celebrity Celebrity { get; set; }
        public SocialLinkEnum Platform { get; set; }
        public string? Icon { get; set; }
        public string Url { get; set; }
    }
}
