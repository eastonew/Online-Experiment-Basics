using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MainEnvironment.Core
{
    public class PedestalModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public SculptureModel Sculpture { get; set; }
    }
}
