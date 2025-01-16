namespace TodoBackend.Utils
{
    public static class PasswordUtils
    {
        public static string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ComparePassword(string password, string hashedPassword)
        { return BCrypt.Net.BCrypt.Verify(password, hashedPassword); }
    }
}