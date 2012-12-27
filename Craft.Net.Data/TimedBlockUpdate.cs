using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public struct TimedBlockUpdate
    {
        public TimedBlockUpdate(DateTime time, Vector3 position)
        {
            Time = time;
            Position = position;
        }

        public DateTime Time;
        public Vector3 Position;
    }
}
