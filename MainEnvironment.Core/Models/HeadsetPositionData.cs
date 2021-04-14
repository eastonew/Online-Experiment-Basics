using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class HeadsetPositionData
    {
        public Vector3 HeadsetPos { get; set; }
        public Vector3 DectectedOrigin { get; set; }
        public AngleDetails PlayerLean { get; set; }
        public AngleDetails HeadsetRotation { get; set; }
        public string DetectedLean { get; set; }
    }
}
