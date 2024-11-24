using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace section2.Services
{
    public class SimulationManager
    {
        private readonly ConcurrentDictionary<int, int> _paintedCircles; // CircleId -> WorkerId

        public SimulationManager()
        {
            _paintedCircles = new ConcurrentDictionary<int, int>();
        }

        public bool IsCirclePainted(int circleId)
        {
            return _paintedCircles.ContainsKey(circleId);
        }

        public void MarkCircleAsPainted(int circleId, int workerId)
        {
            _paintedCircles.TryAdd(circleId, workerId);
        }

        public bool AllCirclesPainted(int totalCircles)
        {
            return _paintedCircles.Count >= totalCircles;
        }
    }
}
