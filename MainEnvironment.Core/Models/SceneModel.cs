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
        
        public LevelModel[] Levels { get; set; }

        public ActionEnum AvailableActions { get; set; }
        public TagModel[] AllTags { get; set; }
        public string ErrorMessage { get; set; }
        public int GroupId { get; set; }
    }
}
