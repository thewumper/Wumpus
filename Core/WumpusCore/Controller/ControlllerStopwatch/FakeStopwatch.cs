using System;
using System.Diagnostics;
using WumpusCore.Controller;

namespace WumpusTesting
{
    public class FakeStopwatch : IStopwatch
    {
        public void Stop()
        {
            throw new InvalidOperationException("This is a fake timer, you can't start or end it");
        }

        public TimeSpan GetElapsed()
        {
            return _desiredTimespan;
        }

        void IStopwatch.Start()
        {
            throw new InvalidOperationException("This is a fake timer, you can't start or end it");
        }

        void IStopwatch.Restart()
        {
            throw new InvalidOperationException("This is a fake timer, you can't restart it");
        }

        private TimeSpan _desiredTimespan;

        public FakeStopwatch(TimeSpan inputTimespan)
        {
            _desiredTimespan = inputTimespan;
        }


    }
}