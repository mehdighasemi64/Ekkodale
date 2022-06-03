using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Ekkodale.Controllers;

[ApiController]
[Route("[controller]")]
public class NeoController : ControllerBase
{
    private readonly GraphClient _GraphClient;
    private readonly ILogger<NeoController> _logger;

    public NeoController(ILogger<NeoController> logger)
    {
        _logger = logger;
        _GraphClient = new GraphClient(new Uri("http://localhost:7474/"), "neo4j", "123456");
    }

    // [HttpPost]
    // public async Task<IActionResult> CreateNode(string name)
    // {
    //     var statementText = new StringBuilder();
    //     statementText.Append("CREATE (person:Person {name: $name})");
    //     var statementParameters = new Dictionary<string, object>
    //     {
    //         {"name", name }
    //     };

    //     var session = this._driver.AsyncSession();
    //     var result = await session.WriteTransactionAsync(tx => tx.RunAsync(statementText.ToString(), statementParameters));
    //     return StatusCode(201, "Node has been created in the database");
    // }

    [HttpPost]
    public async Task<IActionResult> CreatePerson(Person person)
    {
        _GraphClient.ConnectAsync().Wait();
        //var newPerson = new Person { name = Name, family = "family" };
        var newPerson = person;
        await _GraphClient.Cypher
        .Create("(p:Person $newPerson)")
        .WithParam("newPerson", newPerson)
        .ExecuteWithoutResultsAsync();
        return StatusCode(201, "Node has been created in the database");
    }
    [HttpPost("CreateMovie")]
    public async Task<IActionResult> CreateMovie(Movie movie)
    {
        _GraphClient.ConnectAsync().Wait();
        var newMovie = movie;
        await _GraphClient.Cypher
        .Create("(m:Movie $newMovie)")
        .WithParam("newMovie", newMovie)
        .ExecuteWithoutResultsAsync();
        return StatusCode(201, "Node has been created in the database");
    }    
    [HttpPost("CreateActingRelationship")]
    public async Task<IActionResult> CreateActingRelationship(string PersonName, string MovieTitle)
    {
         _GraphClient.ConnectAsync().Wait();
        await _GraphClient.Cypher
        .Match("(p:Person)" , "(m: Movie)")
        .Where((Person p) => p.name == PersonName)
        .AndWhere((Movie m) => m.title == MovieTitle)
        .Create("(p)-[A:Acting_In]->(m)")
        .ExecuteWithoutResultsAsync();
        return StatusCode(201, "Node has been created in the database");
    }    
    [HttpPost("CreateFriendShip")]
    public async Task<IActionResult> CreateFriendShip(int PersonID, int PersonID2)
    {
        _GraphClient.ConnectAsync().Wait();
        await _GraphClient.Cypher
        .Match("(p1:Person)" , "(p2: Person)")
        .Where((Person p1) => p1.personID == PersonID)
        .AndWhere((Person p2) => p2.personID == PersonID2)
        .Create("(p1)-[F:Friends_With]->(p2)")
        .ExecuteWithoutResultsAsync();
        return StatusCode(201, "Node has been created in the database");
    }    
    [HttpGet]
    public async Task<List<Person>> GetAllPerson()
    {
        _GraphClient.ConnectAsync().Wait();
        try
        {
            var results = await _GraphClient.Cypher
                .Match("(p:Person)")
                .Return(p => p.As<Person>())
                .ResultsAsync;

            return new List<Person>(results);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception.Message);
            return new List<Person>();
        }       
    }
    [HttpDelete]
    public async Task<IActionResult> DeletePerson(string Name)
    {
        _GraphClient.ConnectAsync().Wait();
        await _GraphClient.Cypher
        .Match("(p:Person)")
        .Where((Person p) => p.name == Name)
        .Delete("p")
        .ExecuteWithoutResultsAsync();
       return StatusCode(201, "Node has been deleted in the database");
    }

    [HttpPatch]
    public async Task<IActionResult> UpdatePerson(string Name , string NewName)
    {
        _GraphClient.ConnectAsync().Wait();
        await _GraphClient.Cypher
        .Match("(p:Person)")
        .Where((Person p) => p.name == Name)
        .Set("p.name = $NewName")
        .WithParam("NewName", NewName)
        .ExecuteWithoutResultsAsync();
       return StatusCode(201, "Node has been updated in the database");
    }
    [HttpGet("SearchByParam/{Name}/{Family}/{Age}")]
    public async Task<List<Person>> Search(string Name="", string Family="", int Age=0)
    {
      _GraphClient.ConnectAsync().Wait();
        try
        {
            var results = await _GraphClient.Cypher
                .Match("(p:Person)")
                .Where((Person p) => p.name.Contains(Name) || p.family.Contains(Family) || p.age == Age)
                .Return(p => p.As<Person>())
                .ResultsAsync;

            return new List<Person>(results);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception.Message);
            return new List<Person>();
        }      
    }
}
