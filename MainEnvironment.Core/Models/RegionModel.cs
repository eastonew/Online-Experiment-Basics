using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core
{
    public class RegionModel
    {
        public Guid Identifier { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string RegionName { get; set; }
        public int Order { get; set; }
    }
}
