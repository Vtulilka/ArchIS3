using System.Data.Entity;
using UpdServer;

namespace Server.DataBase
{
    class SchoolContext : DbContext
    {
        public SchoolContext() : base("DbConnection")
        { }
        public DbSet<School> Students { get; set; }
    }
}
