using System;

namespace WumpusCore.Controller
{
    public interface IStopwatch
    {
        void Start();

        void Stop();

        void Restart();

        TimeSpan GetElapsed();
    }
}