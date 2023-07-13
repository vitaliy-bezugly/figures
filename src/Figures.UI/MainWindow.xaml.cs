using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Figures.Domain;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer = new();
        private readonly UiElementFactory _uiElementFactory = new();
        private readonly FiguresBuilder _figuresBuilder = new();
        private readonly ICollection<Figure> _figures = new List<Figure>();
        private int _figureCounter;
        
        public MainWindow()
        {
            InitializeComponent();

            _figureCounter = 0;

            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Start();
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            var bottomRightPoint = new Point(((int)MainCanvas.ActualWidth), ((int)MainCanvas.ActualHeight));
            
            if (_figures.Count == 0)
                return;
            
            MainCanvas.Children.Clear();
            MoveFigures(_figures, bottomRightPoint);
        }

        private void MoveFigures(IEnumerable<Figure> figures, Point endPoint)
        {
            foreach (var figure in figures)
            {
                figure.Move(endPoint);
                
                var geometryFigure = figure.Draw();
                
                var uiElement = _uiElementFactory.Create(geometryFigure);
                MainCanvas.Children.Add(uiElement);
            }
        }
        
        private void TreeViewItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var child = sender as TreeViewItem;
            if (child is null)
            {
                throw new InvalidOperationException();
            }
            
            Func<Figure> creator = child.Name switch
            {
                "RectanglesTreeViewItem" => CreateRectangle,
                "TrianglesTreeViewItem" => CreateTriable,
                "CirclesTreeViewItem" => CreateCircle,
                _ => throw new InvalidOperationException()
            };

            CreateFigureAndToTreeView(creator, child);
        }
        
        private void CreateFigureAndToTreeView(Func<Figure> creator, TreeViewItem treeViewItem)
        {
            Figure figure = creator.Invoke();
            _figures.Add(figure);
            
            var child = new TreeViewItem
            {
                Header = "Figure #" + _figureCounter++,
                Tag = figure
            };

            treeViewItem.Items.Add(child);
        }
        
        private Figure CreateRectangle() => _figuresBuilder.BuildRectangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateCircle() => _figuresBuilder.BuildCircle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateTriable() => _figuresBuilder.BuildTriangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));
    }
}