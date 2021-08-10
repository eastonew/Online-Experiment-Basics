using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class LevelModel
    {
        public int LevelId { get; set; }
        public string[] LevelInstructions { get; set; }
        /// <summary>
        /// Defines the sculptures which will be available to the participant - each sculpture is an Id and a Group Id - participant is assigned a group Id when the sign into the experiment
        /// </summary>
        public SculptureModel[] Sculptures { get; set; }
    }
}
