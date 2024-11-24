using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models.Celebrity
{
    public class CelebrityAttachmentViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
    }

    public class CelebrityAttachmentUploadViewModel
    {
        public IFormFile   Attachment { get; set; }
        public string Type { get; set; }
    }
    

}
