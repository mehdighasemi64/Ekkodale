using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Ekkodale.Services
{
    public class PersonService : IPersonService
    {
        // GraphClient _GraphClient = new GraphClient(new Uri("http://localhost:7474/"), "neo4j", "123456");
        // public PersonService()
        // {
        //     _GraphClient.ConnectAsync().Wait();
        // }
        private readonly Neo4jService _neo4jService;
        public PersonService(Neo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }
        public async void CreatePerson(Person person)
        {
            var newPerson = person;
            await _neo4jService.client.Cypher
            .Create("(p:Person $newPerson)")
            .WithParam("newPerson", newPerson)
            .ExecuteWithoutResultsAsync();
        }
        public async void DeletePerson(string Name)
        {
            //     await _neo4jService.client.Cypher  // delete a Person without any relationship
            //    .Match("(p:Person)")
            //    .Where((Person p) => p.name == Name)
            //    .Delete("p")
            //    .ExecuteWithoutResultsAsync();

            //  await _neo4jService.client.Cypher // delete a person with incoming relationship
            //    .OptionalMatch("(p:Person)<-[r]-()")
            //    .Where((Person p) => p.name == Name)
            //    .Delete("r, p")
            //    .ExecuteWithoutResultsAsync();

            await _neo4jService.client.Cypher // delete person and movie and relationship between them
             .OptionalMatch("(p)-[r]->(m:Movie)")
             .Where((Person p) => p.Name == Name)
             .Delete("r,p,m")
             .ExecuteWithoutResultsAsync();

        }
        public async void UpdatePerson(string Name, string NewName)
        {
            await _neo4jService.client.Cypher
            .Match("(p:Person)")
            .Where((Person p) => p.Name == Name)
            .Set("p.name = $NewName")
            .WithParam("NewName", NewName)
            .ExecuteWithoutResultsAsync();
        }
        public async Task<List<Person>> GetAllPerson()
        {
            try
            {
                var results = await _neo4jService.client.Cypher
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
                var results = await _neo4jService.client.Cypher
                    .Match("(p:Person)")
                    .Where((Person p) => p.Name.Contains(Name) || p.Family.Contains(Family) || p.Age == Age)
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
            await _neo4jService.client.Cypher
            .Match("(p:Person)", "(m: Movie)")
            .Where((Person p) => p.Name == PersonName)
            .AndWhere((Movie m) => m.Title == MovieTitle)
            .Create("(p)-[A:Acting_In]->(m)")
            .ExecuteWithoutResultsAsync();
        }
        public async void CreateFriendShip(int PersonID, int PersonID2)
        {
            await _neo4jService.client.Cypher
            .Match("(p1:Person)", "(p2: Person)")
            .Where((Person p1) => p1.PersonID == PersonID)
            .AndWhere((Person p2) => p2.PersonID == PersonID2)
            .Create("(p1)-[F:Friends_With]->(p2)")
            .ExecuteWithoutResultsAsync();
        }
        public async void DeleteFriendShip(int PersonID, int PersonID2)
        {
            await _neo4jService.client.Cypher
            .Match("root-[:Friends_With]->user-[r]->friend")
            .Where((Person p1) => p1.PersonID == PersonID)
            .AndWhere((Person p2) => p2.PersonID == PersonID2)
            .Delete("(p1)-[Friends_With]->(p2)")
            .ExecuteWithoutResultsAsync();
        }        
        public async void CreateRelationship(string PersonName, string MovieTitle, string RelationshipType)
        {
            switch (RelationshipType)
            {
                case "Acting":
                    await _neo4jService.client.Cypher
                               .Match("(p:Person)", "(m: Movie)")
                               .Where((Person p) => p.Name == PersonName)
                               .AndWhere((Movie m) => m.Title == MovieTitle)
                               .Create("(p)-[A:Acting_In]->(m)")
                               .ExecuteWithoutResultsAsync();
                    break;
                case "Producing":
                    await _neo4jService.client.Cypher
                                   .Match("(p:Person)", "(m: Movie)")
                                   .Where((Person p) => p.Name == PersonName)
                                   .AndWhere((Movie m) => m.Title == MovieTitle)
                                   .Create("(p)-[c:Producing_It]->(m)")
                                   .ExecuteWithoutResultsAsync();
                    break;
                case "ِDirecting":
                    await _neo4jService.client.Cypher
                                   .Match("(p:Person)", "(m: Movie)")
                                   .Where((Person p) => p.Name == PersonName)
                                   .AndWhere((Movie m) => m.Title == MovieTitle)
                                   .Create("(p)-[d:Directing_It]->(m)")
                                   .ExecuteWithoutResultsAsync();
                    break;
                default: break;
            }
        }

        // ~PersonService()
        // {
        //     _neo4jService.client.Dispose();
        // }
    }
}


