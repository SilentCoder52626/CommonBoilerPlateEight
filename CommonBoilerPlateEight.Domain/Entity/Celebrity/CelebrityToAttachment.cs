using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityToAttachment : BaseEntity
    {
        protected CelebrityToAttachment()
        {
            
        }
        public CelebrityToAttachment(Celebrity celebrity,string filePath, string fileName, CelebrityAttachmentTypeEnum type, string contentType)
        {
            Celebrity = celebrity;
            FilePath = filePath;
            FileName = fileName;
            AttachmentType = type;
            ContentType = contentType;
        }
        public int CelebrityId { get; set; }
        public Celebrity Celebrity { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public CelebrityAttachmentTypeEnum AttachmentType { get; set; }
    }
}
