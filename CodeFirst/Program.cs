using System.Data.Common;
using System.Data.Entity;

namespace CodeFirst
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(string connectionString = "name=DefaultConnection") : base(connectionString)
        {
        }

        public DefaultDbContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
        }

        public DbSet<Student> Students { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
