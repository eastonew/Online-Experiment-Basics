using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Database
{
    public class Experiment
    {
        public Guid Id { get; set; }
        public string ExperimentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsLive { get; set; }
        public string ExperimentDefinition { get; set; }
        public virtual ICollection<ConsentFormClause> ConsentFormClauses { get; set; }
    }
}
