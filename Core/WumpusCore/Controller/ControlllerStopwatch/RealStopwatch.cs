using System;

namespace WumpusCore.Controller.Stopwatch
{
    public class RealStopwatch : System.Diagnostics.Stopwatch, IStopwatch
    {
        public TimeSpan GetElapsed()
        {
            return Elapsed;
        }
    }
}