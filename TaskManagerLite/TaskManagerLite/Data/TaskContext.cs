using Microsoft.EntityFrameworkCore;
using TaskManagerLite.Entities.Tasks;

namespace TaskManagerLite.Data
{
    public class TaskContext : DbContext
    {
        public DbSet<SimpleTask> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseInMemoryDatabase(databaseName: "TestDb");
        }
    }
}
