using MainEnvironment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Database
{
    public class DownloadInstructions
    {
        public Guid Id { get; set; }
        public Guid ExperimentId { get; set; }
        public EquipmentTypeEnum EquimentType { get; set; }
        public string Instructions { get; set; }
        public InstructionsTypeEnum InstructionsType { get; set; }
        public virtual Experiment ExperimentDetails { get; set; }

    }
}
