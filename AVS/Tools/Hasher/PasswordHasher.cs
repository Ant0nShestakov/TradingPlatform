namespace AVS.Tools.Hasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public string? Generate(string password)
        {
            if (password == null)
                return null;
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool Verify(string password, string hashPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
