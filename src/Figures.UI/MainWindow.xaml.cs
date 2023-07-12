using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Figures.Domain;
using Rectangle = Figures.Domain.Rectangle;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer = new();
        private readonly ICollection<Figure> _figures = new List<Figure>();
        
        public MainWindow()
        {
            InitializeComponent();
            
            FiguresThreeView.ItemsSource = Figures;
            
            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Start();
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            var bottomRightPoint = new System.Drawing.Point(((int)MainCanvas.ActualWidth), ((int)MainCanvas.ActualHeight));
            
            ClearCanvas();
            MoveFigures(_figures, bottomRightPoint);
        }

        private void MoveFigures(IEnumerable<Figure> figures, System.Drawing.Point endPoint)
        {
            foreach (var figure in figures)
            {
                figure.Move(endPoint);
                var points = figure.Draw(endPoint);
                var polygon = GetPolygon(points);

                polygon.Tag = figure;
                MainCanvas.Children.Add(polygon);
            }
        }

        private void ClearCanvas()
        {
            MainCanvas.Children.Clear();
        }

        private Polygon GetPolygon(IEnumerable<System.Drawing.Point> points)
        {
            var partiallyTransparentSolidColorBrush = new SolidColorBrush(Colors.Green)
            {
                Opacity = 0.55
            };
            
            var mySquare = new Polygon
            {
                Fill = partiallyTransparentSolidColorBrush,
                Points = new PointCollection(points.Select(x => new Point(x.X, x.Y)))
            };

            return mySquare;
        }

        private IEnumerable<string> Figures
        {
            get
            {
                return new[] { "Circle", "Rectangle", "Triangle" };
            }
        }

        private void AddRectangle()
        {
            var randomPoint = new System.Drawing.Point(Random.Shared.Next(0, (int)MainCanvas.ActualWidth), Random.Shared.Next(0, (int)MainCanvas.ActualHeight));
            var width = Random.Shared.Next(10, 100);
            var height = Random.Shared.Next(10, width);
            
            var rectangle = new Rectangle(randomPoint, Random.Shared.Next(10, 25), Random.Shared.Next(10, 25), width, height);
            _figures.Add(rectangle);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            AddRectangle();
        }
    }
}