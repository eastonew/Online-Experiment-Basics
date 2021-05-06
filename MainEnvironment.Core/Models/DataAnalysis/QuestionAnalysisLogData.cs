using MainEnvironment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models.DataAnalysis
{
    public class QuestionAnalysisLogData : AnalysisLogDataBase
    { 
        public string QuestionIdentifier { get; set; }
        public string SelectedValue { get; set; }
    }
}
