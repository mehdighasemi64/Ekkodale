using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;

namespace Ekkodale.Services
{
    public partial interface IPersonService
    {    
        void CreatePerson(Person person);      
        void DeletePerson(string Name);
        void UpdatePerson(string Name , string NewName);
        Task<List<Person>> GetAllPerson();
        Task<List<Person>> Search(string Name="", string Family="", int Age=0);
        void CreateActingRelationship(string PersonName, string MovieTitle);
        void CreateFriendShip(int PersonID, int PersonID2);
    }
}
