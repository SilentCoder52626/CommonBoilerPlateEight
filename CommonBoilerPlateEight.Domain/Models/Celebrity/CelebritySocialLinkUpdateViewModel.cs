using CommonBoilerPlateEight.Domain.Models.Celebrity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebritySocialLinkUpdateViewModel
    {
        public int Id { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? SnapchatLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? ThreadsLink { get; set; }
        public string? YoutubeLink { get; set; }
    }

    public class CelebritySocialLinkRequestViewModel
    {
        [RegularExpression(@"^(https?:\/\/)?(www\.)?facebook\.com\/[A-Za-z0-9._-]+\/?$", ErrorMessage = "Please enter a valid Facebook link.")]
        public string? FacebookLink { get; set; }

        [RegularExpression(@"^(https?:\/\/)?(www\.)?instagram\.com\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Instagram link.")]
        public string? InstagramLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?snapchat\.com\/add\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Snapchat link.")]
        public string? SnapchatLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?twitter\.com\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Twitter link.")]
        public string? TwitterLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?threads\.net\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Threads link.")]
        public string? ThreadsLink { get; set; }

        [RegularExpression(@"^(https?:\/\/)?(www\.)?(youtube\.com|youtu\.be)\/(watch\?v=[A-Za-z0-9._-]+|channel\/[A-Za-z0-9._-]+|c\/[A-Za-z0-9._-]+|user\/[A-Za-z0-9._-]+|@[\w.-]+)\/?$",
            ErrorMessage = "Please enter a valid YouTube link.")]
        public string? YoutubeLink { get; set; }
    }
}
