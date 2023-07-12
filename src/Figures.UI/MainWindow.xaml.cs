using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Figures.Domain;
using Rectangle = Figures.Domain.Rectangle;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer = new();
        private readonly UiElementFactory _uiElementFactory = new();
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
            
            if (_figures.Count == 0)
                return;
            
            MainCanvas.Children.Clear();
            MoveFigures(_figures, bottomRightPoint);
        }

        private void MoveFigures(IEnumerable<Figure> figures, System.Drawing.Point endPoint)
        {
            foreach (var figure in figures)
            {
                figure.Move(endPoint);
                var geometryFigure = figure.Draw();
                
                var uiElement = _uiElementFactory.Create(geometryFigure);
                MainCanvas.Children.Add(uiElement);
            }
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
            var width = Random.Shared.Next(10, 100);
            var height = Random.Shared.Next(10, width);
            
            var randomPoint = new System.Drawing.Point(Random.Shared.Next(0, (int)MainCanvas.ActualWidth - width), Random.Shared.Next(0, (int)MainCanvas.ActualHeight) - height);
            var rectangle = new Rectangle(randomPoint, Random.Shared.Next(10, 25), Random.Shared.Next(10, 25), width, height);
            
            _figures.Add(rectangle);
        }

        private void AddCircle()
        {
            int radius = Random.Shared.Next(10, 30);
            
            var randomPoint = new System.Drawing.Point(Random.Shared.Next(0, (int)MainCanvas.ActualWidth - radius), Random.Shared.Next(0, (int)MainCanvas.ActualHeight) - radius);
            var circle = new Circle(randomPoint, Random.Shared.Next(10, 25), Random.Shared.Next(10, 25), radius);
            
            _figures.Add(circle);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(Random.Shared.Next(1, 3) == 1)
            {
                AddRectangle();
            }
            else
            {
                AddCircle();
            }
        }
    }
}