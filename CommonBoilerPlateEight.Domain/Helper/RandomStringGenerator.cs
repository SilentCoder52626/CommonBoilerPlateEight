namespace CommonBoilerPlateEight.Domain.Helper
{
    public class RandomStringGenerator
    {
        public static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}
