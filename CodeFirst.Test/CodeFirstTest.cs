using NUnit.Framework;
using SQLite.CodeFirst;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace CodeFirst.Test
{
    [TestFixture]
    public class CodeFirstTest : InMemoryDbTest<TestDbContext>
    {
        [Test]
        public void ShouldCreateRecord()
        {
            //var connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=SchoolDbTest; Integrated Security=SSPI";
            //var connectionString = @"Data Source=C:\Users\leo\projects\CodeFirst\db\school_db_2.db";

            using (var ctx = (TestDbContext) GetDbContext())
            {
                var student = new Student { Name = "BoJack" };
                ctx.Students.Add(student);
                ctx.SaveChanges();

                var first = ctx.Students.First(x => x.Name == "BoJack");
                Assert.NotNull(first);
            }
        }

        [Test]
        public void TestA()
        {
            using (var ctx = (TestDbContext) GetDbContext())
            {
                for (int i = 0; i < 1000; i++)
                {
                    ctx.Students.Add(new Student {Name = "Student #" + i});
                }
                ctx.SaveChanges();

                Assert.AreEqual(1000, ctx.Students.Count());
            }
        }

        [Test]
        public void TestB()
        {
            using (var ctx = (TestDbContext) GetDbContext())
            {
                for (int i = 0; i < 1000; i++)
                {
                    ctx.Students.Add(new Student {Name = "Student #" + i});
                }
                ctx.SaveChanges();

                Assert.AreEqual(1000, ctx.Students.Count());
            }
        }
    }

    public class TestDbContext : DefaultDbContext
    {
        public TestDbContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SqliteDropCreateDatabaseAlways<TestDbContext>(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }

    public abstract class InMemoryDbTest<TDbContext> 
        where TDbContext : DbContext
    {
        private bool _dbInitialized;

        protected DbConnection Connection { get; private set; }

        protected DbContext GetDbContext()
        {
            TDbContext context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), Connection, false);
            if (!_dbInitialized)
            {
                context.Database.Initialize(true);
                _dbInitialized = true;
            }
            return context;
        }

        [SetUp]
        public void Initialize()
        {
            Connection = new SQLiteConnection("data source=:memory:");

            // This is important! Else the in memory database will not work.
            Connection.Open();

            _dbInitialized = false;
        }

        [TearDown]
        public void Cleanup()
        {
            Connection.Dispose();
        }
    }
}
