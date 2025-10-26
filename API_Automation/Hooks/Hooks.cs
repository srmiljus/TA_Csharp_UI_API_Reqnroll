

namespace API_Automation.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("=== Starting API tests with Allure.Reqnroll ===");
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("=== API tests finished ===");
        }
    }
}
