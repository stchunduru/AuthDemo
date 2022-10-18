using AuthDemo.Models;

namespace AuthDemo.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
