using CommonBoilerPlateEight.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Extensions
{
    public static class SocialLinkIconMapper
    {
        private static readonly string BasePath = "/images/Social/";

        private static readonly Dictionary<SocialLinkEnum, string> IconPaths = new()
    {
        { SocialLinkEnum.Facebook, $"{BasePath}facebook.png" },
        { SocialLinkEnum.Instagram, $"{BasePath}instagram.png" },
        { SocialLinkEnum.Snapchat, $"{BasePath}snapchat.png" },
        { SocialLinkEnum.Tiktok, $"{BasePath}tik-tok.png" },
        { SocialLinkEnum.Thread, $"{BasePath}threads.png" },
        { SocialLinkEnum.YouTube, $"{BasePath}youtube.png" },
        { SocialLinkEnum.Twitter, $"{BasePath}twitter.png" },
    };

        public static string GetIconPath(SocialLinkEnum platform)
        {
            return IconPaths.TryGetValue(platform, out var path) ? path : $"{BasePath}default.png";
        }
    }

}
