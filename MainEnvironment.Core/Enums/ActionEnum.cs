using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Enums
{
    [Flags]
    public enum ActionEnum
    {
        Translation = 0,
        RotationX = 1,
        RotationY = 2,
        RotationZ = 4,
    }
}
