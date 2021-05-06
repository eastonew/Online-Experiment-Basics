using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models.DataAnalysis
{
    public class SculptureAnalysisLogData : AnalysisLogDataBase
    {
        public enum LeanDirection
        {
            Left,
            Right
        };
        public float LeftSculptureSymmetryValue { get; set; }
        public float RightSculptureSymmetryValue { get; set; }
        public int LeftSculptureId { get; set; }
        public int RightSculptureId { get; set; }
        public LeanDirection ChosenSculpture { get; set; }
        public int Index { get; set; }
        public bool FirstRunThrough { get; set; }
    }
}
