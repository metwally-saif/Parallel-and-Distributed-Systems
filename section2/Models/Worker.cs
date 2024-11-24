using section2.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace section2.Models
{
    public class Worker(int id, List<Circle> assignedCircles, SimulationManager manager, int paintingDelay, MainWindow mainWindow)
    {
        public int Id { get; } = id;
        public List<Circle> AssignedCircles { get; } = assignedCircles;
        public ConcurrentBag<int> CompletedCircles { get; } = [];
        private readonly SimulationManager _manager = manager;
        private readonly int _paintingDelay = paintingDelay; // in milliseconds

        public async Task PaintCirclesAsync()
        {
            foreach (var circle in AssignedCircles)
            {
                // Check if the circle is already painted
                if (!_manager.IsCirclePainted(circle.Id))
                {
                    // Simulate painting delay
                    await Task.Delay(_paintingDelay);

                    // Mark the circle as painted
                    _manager.MarkCircleAsPainted(circle.Id, this.Id);

                    // Add to completed list
                    CompletedCircles.Add(circle.Id);
                }
            }
        }
    }
}
