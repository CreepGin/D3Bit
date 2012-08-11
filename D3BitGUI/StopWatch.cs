using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3BitGUI
{
    public class StopWatch
    {
        private DateTime watchTime;

        public StopWatch()
        {
            watchTime = DateTime.Now;
        }

        public void Lap(string label)
        {
            DateTime now = DateTime.Now;
            GUI.Debug("Lap<{1}>: {0:0}ms", (now - watchTime).TotalMilliseconds, label);
            watchTime = now;
        }
    }
}
