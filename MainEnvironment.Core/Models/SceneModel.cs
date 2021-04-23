using MainEnvironment.Core.Enums;
using MainEnvironment.Core.Models;
using System;

namespace MainEnvironment.Core
{
    public class SceneModel
    {
        public string ApiKey { get; set; }
        public RoomModel[] Rooms { get; set; }
        //public QuestionnaireModel Questionnaire { get; set; }
        public ActionEnum AvailableActions { get; set; }
        public TagModel[] AllTags { get; set; }
    }
}
