using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Ekkodale.Services
{
    public partial class PersonService : IPersonService
    {
        GraphClient _GraphClient = new GraphClient(new Uri("http://localhost:7474/"), "neo4j", "123456");
        public PersonService()
        {
            _GraphClient.ConnectAsync().Wait();
        }
        public async void CreatePerson(Person person)
        {
            var newPerson = person;
            await _GraphClient.Cypher
            .Create("(p:Person $newPerson)")
            .WithParam("newPerson", newPerson)
            .ExecuteWithoutResultsAsync();
        }
        public async void DeletePerson(string Name)
        {
            await _GraphClient.Cypher
           .Match("(p:Person)")
           .Where((Person p) => p.name == Name)
           .Delete("p")
           .ExecuteWithoutResultsAsync();
        }
        public async void UpdatePerson(string Name, string NewName)
        {
            await _GraphClient.Cypher
            .Match("(p:Person)")
            .Where((Person p) => p.name == Name)
            .Set("p.name = $NewName")
            .WithParam("NewName", NewName)
            .ExecuteWithoutResultsAsync();
        }
        public async Task<List<Person>> GetAllPerson()
        {
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
                return new List<Person>();
            }
        }
        public async Task<List<Person>> Search(string Name = "", string Family = "", int Age = 0)
        {
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
                return new List<Person>();
            }
        }
        public async void CreateActingRelationship(string PersonName, string MovieTitle)
        {
            await _GraphClient.Cypher
            .Match("(p:Person)", "(m: Movie)")
            .Where((Person p) => p.name == PersonName)
            .AndWhere((Movie m) => m.title == MovieTitle)
            .Create("(p)-[A:Acting_In]->(m)")
            .ExecuteWithoutResultsAsync();
        }
        public async void CreateFriendShip(int PersonID, int PersonID2)
        {
            await _GraphClient.Cypher
            .Match("(p1:Person)", "(p2: Person)")
            .Where((Person p1) => p1.personID == PersonID)
            .AndWhere((Person p2) => p2.personID == PersonID2)
            .Create("(p1)-[F:Friends_With]->(p2)")
            .ExecuteWithoutResultsAsync();
        }
    }
}


