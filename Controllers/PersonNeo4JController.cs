using Ekkodale.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Ekkodale;
using Neo4jClient;

namespace Ekkodale.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonNeo4JController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonNeo4JController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost]
    public IActionResult CreatePerson(Person person)
    {
        var newPerson = person;
        try
        {
            _personService.CreatePerson(newPerson);
             return StatusCode(201, "Person Created Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpPost("CreateActingRelationship")]
    public IActionResult CreateActingRelationship(string PersonName, string MovieTitle)
    {
        try
        {
            _personService.CreateActingRelationship(PersonName, MovieTitle);
            return StatusCode(201,"Actor and Movie Relationship Created Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpPost("CreateFriendShip")]
    public IActionResult CreateFriendShip(int PersonID, int PersonID2)
    {
        try
        {
            _personService.CreateFriendShip(PersonID, PersonID);
            return StatusCode(201, "Friendship Created Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpPost("CreateRelationship")]
    public IActionResult CreateRelationship(String PersonName, string MovieTitle, string RelationshipType)
    {
        try
        {
            _personService.CreateRelationship(PersonName, MovieTitle, RelationshipType);
            return StatusCode(201, "Relationship Created Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpDelete("DeleteFriendShip")]
    public IActionResult DeleteFriendShip(int PersonID, int PersonID2)
    {
        try
        {
            _personService.DeleteFriendShip(PersonID, PersonID);
            return StatusCode(201, "Friendship Removed Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }
    [HttpGet]
    public async Task<List<Person>> GetAllPerson()
    {
        try
        {
            List<Person> lstPerson = await _personService.GetAllPerson();
            return lstPerson;
        }
        catch (System.Exception e)
        {
            return new List<Person>();
        }
    }

    [HttpDelete]
    public IActionResult DeletePerson(string Name)
    {
        try
        {
            _personService.DeletePerson(Name);
            return StatusCode(201, "Person Removed Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpPatch]
    public IActionResult UpdatePerson(string Name, string NewName)
    {
        try
        {
            _personService.UpdatePerson(Name, NewName);
            return StatusCode(201 , "Person Updated Successfully");
        }
        catch (System.Exception e)
        {
            return StatusCode(501, e.Message);
        }
    }

    [HttpGet("SearchByParam/{Name}/{Family}/{Age}")]
    public async Task<List<Person>> Search(string Name = "", string Family = "", int Age = 0)
    {
        try
        {
            List<Person> lstPerson = await _personService.Search(Name, Family, Age);
            return lstPerson;
        }
        catch (System.Exception e)
        {
            return new List<Person>();
        }
    }
}
