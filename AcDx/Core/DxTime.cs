using System.Diagnostics;

namespace AcDx.Core
{
    public class DxTime
    {
        private Stopwatch _stopwatch;
        private double _lastUpdate;

        public DxTime()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
            _lastUpdate = 0;
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public double Update()
        {
            double now = ElapseTime;
            double updateTime = now - _lastUpdate;
            _lastUpdate = now;
            return updateTime;
        }

        public double ElapseTime => _stopwatch.ElapsedMilliseconds * 0.001;
    }
}
