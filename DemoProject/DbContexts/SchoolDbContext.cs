using DemoProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DemoProject.DbContexts
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
