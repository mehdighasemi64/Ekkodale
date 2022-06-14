using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Ekkodale.Services
{
    public class MovieService : IMovieService
    {       
        private readonly Neo4jService _neo4jService;
        public MovieService(Neo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }
        public async void CreateMovie(Movie movie)
        {
            var newMovie = movie;
            await _neo4jService.client.Cypher
            .Create("(m:Movie $newMovie)")
            .WithParam("newMovie", newMovie)
            .ExecuteWithoutResultsAsync();
        }
        public async void DeleteMovie(string Title)
        {
            await _neo4jService.client.Cypher  // delete a Person without any relationship
           .Match("(m:Movie)")
           .Where((Movie m) => m.Title == Title)
           .Delete("m")
           .ExecuteWithoutResultsAsync();        
        }        
        public async Task<List<Movie>> GetAllMovies()
        {
            try
            {
                var results = await _neo4jService.client.Cypher
                    .Match("(m:Movie)")
                    .Return(m => m.As<Movie>())
                    .ResultsAsync;

                return new List<Movie>(results);
            }
            catch (Exception exception)
            {
                return new List<Movie>();
            }
        }
        public async Task<List<Movie>> Search(string Title = "", int Year = 0)
        {
            try
            {
                var results = await _neo4jService.client.Cypher
                    .Match("(m:Movie)")
                    .Where((Movie m) => m.Title.Contains(Title) || m.Year == Year)
                    .Return(m => m.As<Movie>())
                    .ResultsAsync;

                return new List<Movie>(results);
            }
            catch (Exception exception)
            {
                return new List<Movie>();
            }
        }      
    }
}


