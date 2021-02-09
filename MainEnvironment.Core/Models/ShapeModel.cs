using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MainEnvironment.Core
{
    public class ShapeModel
    {
        public enum ShapeModelBaseEnum
        {
            Sphere,
            Cube,
            Custom
        }
        public ShapeModelBaseEnum BaseShape { get; set; }
        public Vector3 Scale { get; set; }
    }
}
