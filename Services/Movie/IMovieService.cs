using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;

namespace Ekkodale.Services
{
    public interface IMovieService
    {    
        void CreateMovie(Movie movie);      
        void DeleteMovie(string Title);        
        Task<List<Movie>> GetAllMovies();
        Task<List<Movie>> Search(string Title="", int Year=0);
    }
}
