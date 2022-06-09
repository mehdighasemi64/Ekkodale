using Models.Ekkodale;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Ekkodale.Services
{
    public class Neo4jService
    {
        private readonly GraphClient _GraphClient;
        public Neo4jService(INeo4jDatabaseSettings settings)
        {
            _GraphClient = new GraphClient(new Uri(settings.Uri), settings.UserName, settings.Password);
            client = _GraphClient;
            client.ConnectAsync().Wait();
        }
        public GraphClient client { get; set; }
    }
}
