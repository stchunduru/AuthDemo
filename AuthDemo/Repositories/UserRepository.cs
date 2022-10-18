using AuthDemo.Models;

namespace AuthDemo.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { Username = "luke", EmailAddress ="luke@email.com", Password = "NOMAN", Name = "Luke", Role = "Admin"},
            new() { Username = "bill", EmailAddress ="bill@email.com", Password = "NOMAN", Name = "Bill", Role = "Standard"},
            new() { Username = "kyle", EmailAddress ="kyle@email.com", Password = "NOMAN", Name = "Kyle", Role = "Standard"},
        };
    }
}
