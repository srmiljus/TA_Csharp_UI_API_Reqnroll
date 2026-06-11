using NUnit.Framework;

namespace API_Automation.Helpers
{

    public static class Logger
    {

        public static void Log(string message)
        {
            TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
        }


        public static void LogError(string message, Exception ex = null)
        {
            TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ERROR: {message}");
            if (ex != null)
            {
                TestContext.WriteLine($"Exception Type: {ex.GetType().Name}");
                TestContext.WriteLine($"Exception Message: {ex.Message}");
                TestContext.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }


        public static void LogWarning(string message)
        {
            TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] WARNING: {message}");
        }


        public static void LogSuccess(string message)
        {
            TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] SUCCESS: {message}");
        }
    }
}

