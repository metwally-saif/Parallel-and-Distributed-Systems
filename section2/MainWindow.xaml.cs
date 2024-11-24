using section2.Models;
using section2.Services;
using section2.Utilities;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace section2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<int> _KValues = [5, 20, 100];
        private readonly int _paintingDelay = 20;
        private readonly int _totalCircles = 1000;
        private readonly double _fieldSize = 1000.0;

        private List<Circle> _circles = [];
        private List<Worker> _workers = [];
        private SimulationManager _simulationManager = new();

        public MainWindow()
        {
            InitializeComponent();
            StartSimulation();
        }

        private async void StartSimulation()
        {
            // Generate circles once
            _circles = CircleGenerator.GenerateRandomCircles(_totalCircles, _fieldSize);

            // Initialize a StringBuilder to collect results
            StringBuilder resultsBuilder = new StringBuilder();
            resultsBuilder.AppendLine("Execution Time Comparison:");
            resultsBuilder.AppendLine("---------------------------");

            foreach (var K in _KValues)
            {
                // Clear the Canvas for each simulation
                FieldCanvas.Children.Clear();

                // Reset SimulationManager
                _simulationManager = new SimulationManager();

                // Assign circles to workers
                _workers = [];
                for (int i = 0; i < K; i++)
                {
                    var assigned = _circles.FindAll(c => c.Id % K == i);
                    var worker = new Worker(i, assigned, _simulationManager, _paintingDelay, this);
                    _workers.Add(worker);
                }

                // Start timing
                var stopwatch = Stopwatch.StartNew();

                // Start painting tasks
                var tasks = new List<Task>();
                foreach (var worker in _workers)
                {
                    tasks.Add(PaintAndVisualize(worker));
                }

                await Task.WhenAll(tasks);

                stopwatch.Stop();

                // Collect the time
                resultsBuilder.AppendLine($"K={K}: All circles painted in {stopwatch.ElapsedMilliseconds} ms.");

                // add a small delay between simulations for better visualization
                await Task.Delay(500);
            }

            // Display the results
            MessageBox.Show(resultsBuilder.ToString(), "Execution Time Results");
        }


        private async Task PaintAndVisualize(Worker worker)
        {
            foreach (var circle in worker.AssignedCircles)
            {
                if (!_simulationManager.IsCirclePainted(circle.Id))
                {
                    await Task.Delay(_paintingDelay);

                    _simulationManager.MarkCircleAsPainted(circle.Id, worker.Id);
                    worker.CompletedCircles.Add(circle.Id);

                    // Visualize the painted circle
                    Dispatcher.Invoke(() =>
                    {
                        var ellipse = new Ellipse
                        {
                            Width = 10, // Fixed size for visibility
                            Height = 10,
                            Fill = GetColor(worker.Id)
                        };
                        Canvas.SetLeft(ellipse, circle.X);
                        Canvas.SetTop(ellipse, circle.Y);
                        FieldCanvas.Children.Add(ellipse);
                    });
                }
            }
        }


        private static Brush GetColor(int workerId)
        {
            // Assign a color based on worker ID
            var colors = new List<Brush>
    {
        Brushes.Red, Brushes.Blue, Brushes.Yellow, Brushes.Green, Brushes.Purple,
        Brushes.Orange, Brushes.Brown, Brushes.Pink, Brushes.Gray, Brushes.Cyan
    };
            return colors[workerId % colors.Count];
        }

    }
}