using AuthDemo.Models;
using AuthDemo.Repositories;

namespace AuthDemo.Services
{
    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {
            movie.Id = MovieRepository.Movies.Count + 1;
            MovieRepository.Movies.Add(movie);
            return movie;
        }

        public bool Delete(int id)
        {
            Movie rMovie = MovieRepository.Movies.FirstOrDefault(o => o.Id == id);
            if(rMovie != null){
                MovieRepository.Movies.Remove(rMovie);
                return true;
            }
            return false;
           
        }

        public Movie Get(int id)
        {
            Movie gMovie = MovieRepository.Movies.FirstOrDefault(o => o.Id == id);
            return gMovie == null ? null : gMovie;
        }

        public List<Movie> List()
        {
            return MovieRepository.Movies.ToList();
        }

        public Movie Update(Movie movie)
        {
            Movie eMovie = MovieRepository.Movies.FirstOrDefault(o => o.Id == movie.Id);
            if (eMovie != null)
            {
                eMovie.Id = movie.Id;
                eMovie.Title = movie.Title;
                eMovie.Description = movie.Description;
                eMovie.Rating = movie.Rating;
                return eMovie;
            }
            return null;
        }
    }
}
