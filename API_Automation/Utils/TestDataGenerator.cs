

namespace API_Automation.Utils
{
    public static class TestDataGenerator
    {
        public static string RandomUsername()
            => $"user_{Guid.NewGuid():N}".Substring(0, 10);

        public static string RandomPassword()
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 5);
            return $"Pa@{guid}1aA!";
        }
    }
}
