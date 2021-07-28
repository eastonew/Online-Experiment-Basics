using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class Colour
    {
        public static Colour BLACK = new Colour { R = 0, G = 0, B = 0, A = 255 };
        public static Colour TRANSPARENT = new Colour { R = 0, G = 0, B = 0, A = 0 };
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Colour && ((Colour)obj).A == A && ((Colour)obj).R == R && ((Colour)obj).B == B && ((Colour)obj).G == G;
        }
    }
}
