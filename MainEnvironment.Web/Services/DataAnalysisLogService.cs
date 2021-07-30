using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Core.Models.DataAnalysis;
using MainEnvironment.Core.Models.Exceptions;
using MainEnvironment.Web.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class DataAnalysisLogService : IDataAnalysisLogService
    {
        private const string SCULPTURE_INFO_MESSAGE = "Built next Sculpture Pair";
        private const string SCULPTURE_CHOICE_MESSAGE = "User Choose Path";
        private const string QUESTION_CHOICE_MESSAGE_PREFIX = "Participant entered";
        private readonly ILogRepo LogRepo;

        public DataAnalysisLogService(ILogRepo repo)
        {
            this.LogRepo = repo;
        }

        public async Task<List<IAnalysisLogData>> GetDataForParticipant(Guid participantId)
        {
            List<IAnalysisLogData> analysisData = new List<IAnalysisLogData>();

            var allLogs = await this.LogRepo.GetLogsForParticipant(participantId);
            var sculptureInfoLogs = allLogs.Where(l => l.LogMessage == SCULPTURE_INFO_MESSAGE)
                .OrderBy(p => p.LogDate)
                .Select(q => new
                {
                    model = new LogModel()
                    {

                        LogDate = q.LogDate,
                        Message = q.LogMessage,
                        ParticipantId = q.ParticipantId.ToString(),
                        AdditionalValues = JsonConvert.DeserializeObject<List<LogDetail>>(q.AdditionalDetails)
                    },
                    Id = q.Id
                }).ToList();

            var sculptureChoiceLogs = allLogs.Where(l => l.LogMessage == SCULPTURE_CHOICE_MESSAGE)
                .OrderBy(p => p.LogDate)
                .Select(q => new
                {
                    model = new LogModel()
                    {

                        LogDate = q.LogDate,
                        Message = q.LogMessage,
                        ParticipantId = q.ParticipantId.ToString(),
                        AdditionalValues = JsonConvert.DeserializeObject<List<LogDetail>>(q.AdditionalDetails)
                    },
                    Id = q.Id
                }).ToList() ;

            var questionChoiceLogs = allLogs.Where(l => l.LogMessage.StartsWith(QUESTION_CHOICE_MESSAGE_PREFIX)).OrderBy(p => p.LogDate)
                .Select(q=>new
                {
                    model = new LogModel()
                    {

                        LogDate = q.LogDate,
                        Message = q.LogMessage,
                        ParticipantId = q.ParticipantId.ToString(),
                        AdditionalValues = JsonConvert.DeserializeObject<List<LogDetail>>(q.AdditionalDetails)
                    },
                    Id = q.Id
                }).ToList();

            foreach(var details in questionChoiceLogs)
            {
                QuestionAnalysisLogData questionData = new QuestionAnalysisLogData()
                {
                    Date = details.model.LogDate,
                    Id = details.Id,
                    Message = details.model.Message,
                    ParticipantId = Guid.Parse(details.model.ParticipantId),
                    QuestionIdentifier = details.model.AdditionalValues.First().Key,
                    SelectedValue = details.model.AdditionalValues.First().Value,
                };
                analysisData.Add(questionData);
            }

            //Expecting 10 entries for both info and choices
            if(sculptureInfoLogs.Count() == sculptureChoiceLogs.Count())
            {
                for(int i = 0; i < sculptureInfoLogs.Count(); i++)
                {
                    var sculptureInfo = sculptureInfoLogs[i];
                    var sculptureChoice = sculptureChoiceLogs[i];

                    var sculptureIndex = int.Parse(sculptureInfo.model.AdditionalValues.First(l => l.Key == "Index").Value);
                    var choiceIndex = int.Parse(sculptureChoice.model.AdditionalValues.First(l => l.Key == "CaveIndex").Value);

                    //during the second run through the ids for the sculptures are reset so the difference will be 7
                    bool isSecondPhase = choiceIndex >= 7;
                    //due to the training caves the choice index will be 2 higher than the info index
                    if ((isSecondPhase && choiceIndex != sculptureIndex + 7) || (!isSecondPhase && choiceIndex != sculptureIndex + 2))
                    {
                        throw new ParticipantException("Incorrect ordering of logs", participantId);
                    }

                    var rightSculptureIndex = int.Parse(sculptureInfo.model.AdditionalValues.First(l => l.Key == "RightSculptureId").Value);
                    var leftSculptureIndex = int.Parse(sculptureInfo.model.AdditionalValues.First(l => l.Key == "LeftSculptureId").Value);

                    float rightSculptureSymLevel = 0;
                    float leftSculptureSymLevel = 0;
                    //hack to prevent comma formatted results causing problems
                    string rightSymLevel = sculptureInfo.model.AdditionalValues.First(l => l.Key == "RightSculptureValue").Value;
                    string leftSymLevel = sculptureInfo.model.AdditionalValues.First(l => l.Key == "LeftSculptureValue").Value;
                    if (rightSymLevel.Contains(",") || leftSymLevel.Contains(","))
                    {
                        var culture = CultureInfo.CreateSpecificCulture("fr-FR");
                        rightSculptureSymLevel = float.Parse(rightSymLevel, culture);
                        leftSculptureSymLevel = float.Parse(leftSymLevel, culture);
                    }
                    else
                    {
                        rightSculptureSymLevel = float.Parse(rightSymLevel);
                        leftSculptureSymLevel = float.Parse(leftSymLevel);
                    }
                    var chosenItem = (SculptureAnalysisLogData.LeanDirection)Enum.Parse(typeof(SculptureAnalysisLogData.LeanDirection), sculptureChoice.model.AdditionalValues.First(l => l.Key == "LeanDirection").Value);

                    SculptureAnalysisLogData sculptureChoiceLog = new SculptureAnalysisLogData()
                    {
                        Date = sculptureInfo.model.LogDate,
                        Id = sculptureInfo.Id,
                        Message = sculptureInfo.model.Message,
                        ParticipantId = Guid.Parse(sculptureInfo.model.ParticipantId),
                        LeftSculptureId = leftSculptureIndex,
                        RightSculptureId = rightSculptureIndex,
                        //use sculpture index as this is 0 based for sculptures only not all caves
                        Index = sculptureIndex + ((isSecondPhase) ? 5 : 0),
                        LeftSculptureSymmetryValue = leftSculptureSymLevel,
                        RightSculptureSymmetryValue = rightSculptureSymLevel,
                        ChosenSculpture = chosenItem,
                        FirstRunThrough = !isSecondPhase
                    };
                    analysisData.Add(sculptureChoiceLog);
                }
            }
            else
            {
                throw new ParticipantException("Incorrect number of choice logs present", participantId);
            }
            return analysisData;
        }
    }
}
