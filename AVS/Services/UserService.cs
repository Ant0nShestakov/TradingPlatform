using AVS.Models.UserModels;
using AVS.Repository;
using AVS.Tools.Hasher;
using AVS.Tools.JWT_Tokens;

namespace AVS.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository = null!;
        private readonly RoleRepository _roleRepository = null!;
        private readonly IPasswordHasher _passwordHasher = null!;
        private readonly IJWTProvider _jWTProvider = null!;

        public UserService(UserRepository repository, RoleRepository roleRepository, IJWTProvider jWTProvider) 
        {
            this._userRepository = repository;
            this._roleRepository = roleRepository;
            this._passwordHasher = new PasswordHasher();
            this._jWTProvider = jWTProvider;
        }

        public async Task Registration(User newUser)
        {
            if (newUser is null)
               return;

            newUser.Password = _passwordHasher.Generate(newUser.Password);

            newUser.Roles.Add(await _roleRepository.GetByName("User"));
            await _userRepository.Add(newUser);
        }

        public async Task<string?> Login(User checkingUser) 
        {
            var user = await _userRepository.GetUserByEmail(checkingUser.Email.ToLower());
            if(user is null)
                return null;

            if (!_passwordHasher.Verify(checkingUser.Password, user.Password))
                return null;

            return _jWTProvider.GenerateToken(user);
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User?> GetUserByEmail(string Email)
        {
            return await _userRepository.GetUserByEmail(Email);
        }
    }
}
