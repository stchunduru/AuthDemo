using AuthDemo.Models;

namespace AuthDemo.Repositories
{
    public class MovieRepository
    {
        public static List<Movie> Movies = new()
        {
            new() {Title = "Halo", Description = "ITS HALO", Rating = 80, Id = 1},
            new() {Title = "Star", Description = "ITS STAR", Rating = 90, Id = 2},
            new() {Title = "Dogs", Description = "ITS DOGS", Rating = 60, Id = 3},
        };
    }
}
