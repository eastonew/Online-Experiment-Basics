using MainEnvironment.Core.Interfaces;

namespace MainEnvironment.Core.Models
{
    public class LogDetail : ILogDetail
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}


