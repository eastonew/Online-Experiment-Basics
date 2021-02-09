using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Database
{
    public class EnvironmentContext : DbContext
    {

        public EnvironmentContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ConsentFormClause> ConsentFormClauses { get; set; }
    }

}
