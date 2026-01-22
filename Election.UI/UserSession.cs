using System;

namespace Election.UI
{
    public static class UserSession
    {
        public static int UserId { get; set; } = 0;  // ✅ Added for vote tracking
        public static string Username { get; set; } = "";
        public static string Email { get; set; } = "";
        public static string Region { get; set; } = "";  // ✅ Added for auto-fill
        public static string Role { get; set; } = "";
        public static string Token { get; set; } = "";

        public static void Clear()
        {
            UserId = 0;
            Username = "";
            Email = "";
            Region = "";
            Role = "";
            Token = "";
        }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Token);
    }
}