using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core
{
    public class RoomModel
    {
        public enum RoomDirectionEnum
        {
            Left,
            Right
        }
        public bool ShowDoorCaps { get; set; }
        public PedestalModel[] Pedestals { get; set; }
        public RoomDirectionEnum? Direction { get; set; }
        public int Order { get; set; }
    }
}
