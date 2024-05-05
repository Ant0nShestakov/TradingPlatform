using AVS.Models;

namespace AVS.Tools.JWT_Tokens
{
    public interface IJWTProvider
    {
        public string GenerateToken(User user);
    }
}
