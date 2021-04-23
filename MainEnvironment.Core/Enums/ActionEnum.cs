using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Enums
{
    [Flags]
    public enum ActionEnum
    {
        Translation = 1,
        RotationX = 2,
        RotationY = 4,
        RotationZ = 8,
        Flying = 16,
        UserScaling = 32
    }
}
