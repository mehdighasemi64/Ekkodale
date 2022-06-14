using Ekkodale.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Ekkodale;
using Neo4jClient;

namespace Ekkodale.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieNeo4JController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieNeo4JController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public IActionResult CreateMovie(Movie movie)
    {
        var newMovie = movie;
        try
        {
            _movieService.CreateMovie(newMovie);
            return StatusCode(201, "Movie Created Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }
    [HttpDelete]
    public IActionResult DeleteMovie(string Title)
    {
        try
        {
            _movieService.DeleteMovie(Title);
            return StatusCode(201, "Movie Removed Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpGet]
    public async Task<List<Movie>> GetAllMovie()
    {
        try
        {
            List<Movie> lstMovie = await _movieService.GetAllMovies();
            return lstMovie;
        }
        catch (System.Exception e)
        {
            return new List<Movie>();
        }
    }
   
    [HttpGet("SearchByParam/{Name}/{Family}/{Age}")]
    public async Task<List<Movie>> Search(string Title = "", int Year = 0)
    {
        try
        {
            List<Movie> lstMovie = await _movieService.Search(Title , Year);
            return lstMovie;
        }
        catch (System.Exception e)
        {
            return new List<Movie>();
        }
    }
}
