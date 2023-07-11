using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Figures.Domain;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private ICollection<Figure> _figures = new List<Figure>();
        
        public MainWindow()
        {
            InitializeComponent();
            
            FiguresThreeView.ItemsSource = Figures;
            
            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Start();
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            var partiallyTransparentSolidColorBrush = new SolidColorBrush(Colors.Green)
            {
                Opacity = 0.55
            };

            int width = 50;

            var rectangle =
                new Figures.Domain.Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), width);
            
            var bottomRightPoint = new System.Drawing.Point(((int)MainCanvas.ActualWidth) - width, ((int)MainCanvas.ActualHeight) - width);
            
            var points = rectangle.Draw(bottomRightPoint);
            
            var mySquare = new Polygon
            {
                Fill = partiallyTransparentSolidColorBrush,
                Points = new PointCollection(points.Select(x => new Point(x.X, x.Y)))
            };

            MainCanvas.Children.Add(mySquare);
        }

        private IEnumerable<string> Figures
        {
            get
            {
                return new[] { "Circle", "Rectangle", "Triangle" };
            }
        }

        private void DrawRectangle()
        {

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DrawRectangle();
        }
    }
}