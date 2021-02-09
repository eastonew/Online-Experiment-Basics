using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core
{
    public class QuestionModel
    {
        public enum QuestionTypeEnum
        {
            TrueFalse,
            Numeric,
            Scale,
            TextOption
        }
        public QuestionTypeEnum QuestionType { get; set; }
        public string Prefix { get; set; }
        public string Subject { get; set; }
        public string Suffix { get; set; }
        public string NoEntryText { get; set; }

        public string[] Options { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int DefaultValue { get; set; }

        public int TotalSteps { get; set; }

        public string TrueAnswerText { get; set; }
        public string FalseAnswerText { get; set; }
    }
}
