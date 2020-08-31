using LMSA.Projects.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSA.Projects.DAL.Contexts
{
    public class ProjectsDbContext : DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }
        public DbSet<Project> Projects { get; set; }
    }
}