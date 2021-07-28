using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using System;

namespace MainEnvironment.Core
{
    public class SceneModel
    {
        public string ApiKey { get; set; }

        /// <summary>
        /// Defines the regions the users can place sculptures in - will not always be used
        /// </summary>
        public RegionModel[] Regions { get; set; }
        /// <summary>
        /// Defines the sculptures which will be available to the participant - each sculpture is an Id and a Group Id - participant is assigned a group Id when the sign into the experiment
        /// </summary>
        public SculptureModel[] Sculptures { get; set; }

        public ActionEnum AvailableActions { get; set; }
        public TagModel[] AllTags { get; set; }
        public string ErrorMessage { get; set; }
    }
}
