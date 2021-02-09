using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Database
{
    public class ConsentFormClause
    {
        public Guid Id { get; set; }
        public Guid ExperimentId { get; set; }
        public virtual Experiment Experiment { get; set; }
        public string Clause { get; set; }
    }
}
