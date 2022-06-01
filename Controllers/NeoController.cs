using System.Text;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
namespace Ekkodale.Controllers;

[ApiController]
[Route("[controller]")]
public class NeoController : ControllerBase  
{   private readonly IDriver _driver;
    private readonly ILogger<WeatherForecastController> _logger;

    public NeoController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123456"));   
    }          
    
    [HttpPost]
    public async Task<IActionResult> CreateNode(string name)
    {
        var statementText = new StringBuilder();
        statementText.Append("CREATE (person:Person {name: $name})");
        var statementParameters = new Dictionary<string, object>
        {
            {"name", name }
        };

        var session = this._driver.AsyncSession();
        var result = await session.WriteTransactionAsync(tx => tx.RunAsync(statementText.ToString(),  statementParameters));
        return StatusCode(201, "Node has been created in the database");
    }
}
