using MainEnvironment.Core.Models;
using System;

namespace MainEnvironment.Core
{
    public class SceneModel
    {
        public string ApiKey { get; set; }
        public ConsentClauseModel[] ConsentClauses { get; set; }

        public RoomModel[] Rooms;
        public QuestionnaireModel Questionnaire { get; set; }
    }
}
