namespace CommonBoilerPlateEight.Domain.Helper
{
    public class RandomStringGenerator
    {
        public static string GenerateRandomOrderString()
        {
            Guid guid = Guid.NewGuid();
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string randomString = $"#SHRORD{guid.ToString("N").Substring(0, 6).ToUpper()}{randomNumber}";
            return randomString;
        }

        public static string GenerateRandomOrderitemString()
        {
            Guid guid = Guid.NewGuid();
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string randomString = $"#SHRORIT{guid.ToString("N").Substring(0, 5).ToUpper()}{randomNumber}";
            return randomString;
        }


        public static string GenerateRandomTransactionString()
        {
            Guid guid = Guid.NewGuid();
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string randomString = $"#SHRTRAN{guid.ToString("N").Substring(0, 6).ToUpper()}{randomNumber}";
            return randomString;
        }

        public static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public static string GenerateTrackingId(string prefix)
        {
            var guidPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            var randomPart = new Random().Next(100000, 999999).ToString();
            return $"#{prefix}-{guidPart}{randomPart}";
        }
    }
}
