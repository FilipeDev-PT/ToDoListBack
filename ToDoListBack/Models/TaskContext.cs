using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ToDoListBack.Models
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) => Database.EnsureCreated();
        public DbSet<Tasks> Tasks { get; set; } = null!;
        public DbSet<Categories> Categories { get; set; }
                
    }
}
