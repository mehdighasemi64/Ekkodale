using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Ekkodale
{
    public interface INeo4jDatabaseSettings
    {
        public string Uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Neo4jDatabaseSettings : INeo4jDatabaseSettings
    {
        public string Uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }  
}
