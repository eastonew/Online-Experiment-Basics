using MainEnvironment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class ExportDefinition
    {
        public Guid Identifier { get; set; }
        public List<ShapeDefinition> CalculatedPoints { get; set; }
        public ShapeTypeEnum BaseGeom { get; set; }
        public Colour BackgroundColour { get; set; }
        public int TotalItems { get; set; }
    }
}
