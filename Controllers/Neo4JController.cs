using Ekkodale.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Ekkodale;
using Neo4jClient;

namespace Ekkodale.Controllers;

[ApiController]
[Route("[controller]")]
public class Neo4JController : ControllerBase
{
    private readonly ILogger<NeoController> _logger;
    private readonly IPersonService _personService;

    public Neo4JController(ILogger<NeoController> logger, IPersonService personService)
    {
        _logger = logger;
        _personService = personService;
    }

    [HttpPost]
    public string CreatePerson(Person person)
    {
         var newPerson = person;
        _personService.CreatePerson(newPerson);
        return "Person Created Successfully";
    }

    [HttpPost("CreateActingRelationship")]
    public string CreateActingRelationship(string PersonName, string MovieTitle)
    {
        _personService.CreateActingRelationship(PersonName, MovieTitle);
        return "Actor and Movie Relationship Created Successfully";
    }    

    [HttpPost("CreateFriendShip")]
    public string CreateFriendShip(int PersonID, int PersonID2)
    {
         _personService.CreateFriendShip(PersonID, PersonID);
        return "Friendship Created Successfully";
    }    

    [HttpGet]
    public async Task<List<Person>> GetAllPerson()
    {
        List<Person> lstPerson = await _personService.GetAllPerson();
        return lstPerson;
    }

    [HttpDelete]
    public string DeletePerson(string Name)
    {
        _personService.DeletePerson(Name);
        return "Person Removed Successfully";
    }

    [HttpPatch]
    public string UpdatePerson(string Name , string NewName)
    {
        _personService.UpdatePerson(Name , NewName);
        return "Person Updated Successfully";
    }
    
    [HttpGet("SearchByParam/{Name}/{Family}/{Age}")]
    public async Task<List<Person>> Search(string Name="", string Family="", int Age=0)
    {
        List<Person> lstPerson = await _personService.Search(Name, Family, Age);

        return lstPerson;
    }
}
