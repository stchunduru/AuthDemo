using AuthDemo.Models;
using AuthDemo.Repositories;

namespace AuthDemo.Services
{
    public class UserService : IUserService
    {
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals
            (userLogin.Username, StringComparison.OrdinalIgnoreCase) && o.Password.Equals
            (userLogin.Password));

            return user;
        }
    }
}
