using MainEnvironment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core
{
    public class SculptureModel
    {
        public Guid SculptureId { get; set; }
        public int GroupId { get; set; }
        public int? LevelId { get; set; }
    }
}
