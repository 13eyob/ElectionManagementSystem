using System;

namespace Election.UI
{
    public static class UserSession
    {
        public static string Username { get; set; } = "";
        public static string Email { get; set; } = "";
        public static string Role { get; set; } = "";

        public static void Clear()
        {
            Username = "";
            Email = "";
            Role = "";
        }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Username);
    }
}