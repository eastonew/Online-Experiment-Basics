using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Web.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class LeanLogService : ILeanLogService
    {
        private const string LEAN_LOG_IDENTIFIER = "User Lean";
        private readonly ILogRepo LogRepo;
       

        public LeanLogService(ILogRepo repo)
        {
            this.LogRepo = repo;
        }

        public async Task<List<HeadsetPositionData>> GetLeanDataForParticipant(Guid participantId)
        {
            List<HeadsetPositionData> positionData = new List<HeadsetPositionData>();

            var allLogs = await this.LogRepo.GetLogsForParticipant(participantId);
            var leanLogs = allLogs.Where(l => l.LogMessage == LEAN_LOG_IDENTIFIER).OrderBy(p => p.LogDate);
            var leanPos = leanLogs.Select(p => JsonConvert.DeserializeObject<LogDetail[]>(p.AdditionalDetails));

            foreach (var lean in leanPos)
            {
                try
                {
                    var posData = new HeadsetPositionData()
                    {
                        HeadsetPos = GetPositionData("HeadsetPos", lean),
                        DectectedOrigin = GetPositionData("DetectedOrigin", lean),
                        HeadsetRotation = GetAngleData("HeadsetRotation", lean),
                        PlayerLean = GetAngleData("PlayerLean", lean),
                        DetectedLean = GetStringData("LeanDetected", lean)
                    };
                    positionData.Add(posData);
                }
                catch
                {
                    int t = 5;
                }
            }
            return positionData;
        }
        

        private Vector3 GetPositionData(string dataValue, LogDetail[] details)
        {
            var headsetPos = details.FirstOrDefault(l => l.Key == dataValue);
            string regex = @"^X: ([0-9.\-E]*), Y: ([0-9.\-E]*), Z: ([0-9.\-E]*)$";
            var match = Regex.Match(headsetPos.Value, regex);
            Vector3 pos = new Vector3(float.Parse(match.Groups[1].Value), float.Parse(match.Groups[2].Value), float.Parse(match.Groups[3].Value));
            return pos;
        }

        private AngleDetails GetAngleData(string dataValue, LogDetail[] details)
        {
            var headsetPos = details.FirstOrDefault(l => l.Key == dataValue);
            string regex = @"^Yaw: ([0-9.\-E]*), Pitch: ([0-9.\-E]*), Roll: ([0-9.\-E]*)$";
            var match = Regex.Match(headsetPos.Value, regex);
            AngleDetails pos = new AngleDetails()
            {
                Yaw = float.Parse(match.Groups[1].Value),
                Pitch = float.Parse(match.Groups[2].Value),
                Roll = float.Parse(match.Groups[3].Value)
            };
            return pos;
        }

        private string GetStringData(string dataValue, LogDetail[] details)
        {
            var headsetPos = details.FirstOrDefault(l => l.Key == dataValue);
            return headsetPos.Value;
        }
    }
}
