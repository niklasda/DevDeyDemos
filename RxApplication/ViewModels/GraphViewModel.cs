using System;
using System.Threading;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace RxApplication.ViewModels
{
    public class GraphViewModel : ViewModelBase
    {
        public GraphViewModel()
        {
            var plotModel = new PlotModel { Title = "Example 1", Subtitle = "Graph" };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1 });
            plotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid });

            GraphData = plotModel;

            _timer = new Timer(OnTimerElapsed, null, 500, 20);
        }

        private PlotModel _graphData;
        public PlotModel GraphData
        {
            get { return _graphData; }
            set
            {
                if (_graphData != value)
                {
                    _graphData = value;
                    RaisePropertyChanged();
                }
            }
        }

        private readonly Timer _timer;

        // private Func<double, double, double, double> Function { get; set; }
        // Function = (t, x, a) => Math.Cos(t * a) * (x == 0 ? 1 : Math.Sin(x * a) / x);

        private void OnTimerElapsed(object state)
        {
            lock (GraphData.SyncRoot)
            {
                Update();
            }

            GraphData.InvalidatePlot(true);
        }

        private void Update()
        {
            var s = (LineSeries)GraphData.Series[0];

            double x = s.Points.Count > 0 ? s.Points[s.Points.Count - 1].X + 1 : 0;
            if (s.Points.Count >= 200)
                s.Points.RemoveAt(0);

            var r = new Random(DateTime.Now.Millisecond);
            double y = r.Next(-1, 2)*r.NextDouble();

            s.Points.Add(new DataPoint(x, y));
        }
    }
}