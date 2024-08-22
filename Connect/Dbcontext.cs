using Agent_Management_Server.models;
using Microsoft.EntityFrameworkCore;

namespace Agent_Management_Server.Connect
{
    public class Dbcontext : DbContext
    {
        public Dbcontext(DbContextOptions<Dbcontext> options) : base(options)

        {
            Database.EnsureCreated();

        }

        public DbSet<Target> Targets { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Mission> Mission { get; set; }
        //private static DbContextOptions Getoptions(string connectionString)
        //{
        //    return SqlServerDbContextOptionsExtensions.UseSqlServer(new
        //        DbContextOptionsBuilder(), connectionString).Options;
        //}       


        }
    }



