namespace API_Automation.Constants
{

    public static class ApiEndpoints
    {

        public static class Books
        {
            public const string GetAll = "/BookStore/v1/Books";
            public const string AddBooks = "/BookStore/v1/Books";
            public const string DeleteBook = "/BookStore/v1/Book";
        }


        public static class Account
        {
            public const string CreateUser = "/Account/v1/User";
            public const string GenerateToken = "/Account/v1/GenerateToken";

            public static string GetUser(string userId) => $"/Account/v1/User/{userId}";

            public static string DeleteUser(string userId) => $"/Account/v1/User/{userId}";
        }
    }
}

