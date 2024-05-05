using AVS.Models;
using AVS.Repository;
using AVS.Tools.Hasher;
using AVS.Tools.JWT_Tokens;

namespace AVS.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository = null!;
        private readonly IPasswordHasher _passwordHasher = null!;
        private readonly IJWTProvider _jWTProvider = null!;

        public UserService(IUserRepository repository, IJWTProvider jWTProvider) 
        {
            this._userRepository = repository;
            this._passwordHasher = new PasswordHasher();
            this._jWTProvider = jWTProvider;
        }

        public async Task Registration(User newUser)
        {
            if (newUser is null)
               return;

            var user = await _userRepository.GetUserByEmail(newUser.Email);
            if (user is not null) return;

            newUser.Password = _passwordHasher.Generate(newUser.Password);
            await _userRepository.AddNewUser(newUser);
        }

        public async Task<string> Login(User checkingUser) 
        {
            var user = await _userRepository.GetUserByEmail(checkingUser.Email);

            if(user is null)
                return null;

            if (!_passwordHasher.Verify(checkingUser.Password, user.Password))
                return null;

            return _jWTProvider.GenerateToken(user);
        }
    }
}
