using AVS.Models.UserModels;

namespace AVS.Tools.JWT_Tokens
{
    public interface IJWTProvider
    {
        public string GenerateToken(User user);
    }
}
