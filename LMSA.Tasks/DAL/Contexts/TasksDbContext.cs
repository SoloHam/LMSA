using LMSA.Tasks.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSA.Tasks.DAL.Contexts
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Task> Tasks { get; set; }
    }
}