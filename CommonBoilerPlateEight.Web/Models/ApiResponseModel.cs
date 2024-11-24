namespace CommonBoilerPlateEight.Web.Models
{
    public class ApiResponseModel
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public List<string> Errors { get; set; } =new List<string>();

        
    }
}
