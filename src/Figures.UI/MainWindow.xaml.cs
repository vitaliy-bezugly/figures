using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Figures.Domain;
using Figures.Infrastructure;
using Point = System.Drawing.Point;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer = new();
        private readonly UiElementFactory _uiElementFactory = new();
        private readonly FiguresBuilder _figuresBuilder = new();
        private readonly ICollection<Figure> _figures = new List<Figure>();
        private readonly LocalizationManager _localizationManager;
        private readonly IRepository<Figure> _repository;
        private int _figureCounter;
        
        public MainWindow()
        {
            InitializeComponent();
            
            _localizationManager = new LocalizationManager(Resources);
            _localizationManager.InitializeDefaultLanguage();

            _figureCounter = 0;
            _repository = new BinaryRepository<Figure>();

            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Start();
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            var bottomRightPoint = new Point(((int)MainCanvas.ActualWidth), ((int)MainCanvas.ActualHeight));
            
            MainCanvas.Children.Clear();
            
            if (_figures.Count == 0)
                return;
            
            MoveFigures(_figures, bottomRightPoint);
        }

        private void TreeViewItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                _ => throw new InvalidOperationException("Unknown figure type")
            };

            CreateFigureAndAddToTreeView(creator, child);
        }

        private void RemoveTreeViewItem_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var child = sender as TreeViewItem;
            var parent = child?.Parent as TreeViewItem;
            if (child is null || parent is null)
            {
                throw new InvalidOperationException();
            }

            Figure? figure = child.Tag as Figure;
            if (figure is null)
            {
                throw new InvalidOperationException();
            }

            _figures.Remove(figure);
            parent.Items.Remove(child);
        }

        private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var selectedTreeView = sender as TreeViewItem;
            if(selectedTreeView is null)
                return;
            
            var figure = selectedTreeView.Tag as Figure;
            if(figure is null)
                throw new InvalidOperationException();
            
            ChangeButtonContentBasedOnFigureStatus(StopOrContinueButton, figure.Stopped);
        }
        
        private void StopOrContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedTreeView = FiguresThreeView.SelectedItem as TreeViewItem;
            if(selectedTreeView is null)
                return;
            
            var figure = selectedTreeView.Tag as Figure;
            if (figure is null)
                return;
            
            figure.Stopped = !figure.Stopped;
            ChangeButtonContentBasedOnFigureStatus(StopOrContinueButton, figure.Stopped);
        }
        
        private void ChangeLanguageMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if(menuItem is null)
                throw new InvalidOperationException();
            
            var language = menuItem.Tag as string;
            if(language is null)
                throw new InvalidOperationException();
            
            _localizationManager.SwitchLanguage(language);
        }
        
        private async void SaveFiguresButton_OnClick(object sender, RoutedEventArgs e) => await _repository.SaveManyAsync(_figures);
        
        private async void LoadFiguresButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadFiguresAndAddToCollectionAsync();
                MessageBox.Show("Figures loaded successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadFiguresAndAddToCollectionAsync()
        {
            var loadedFigures = await _repository.GetAllAsync();

            foreach (var figure in loadedFigures)
            {
                TreeViewItem threeView = GetTreeViewItemByObjectType(figure);
                CreateFigureAndAddToTreeView(() => figure, threeView);
            }
        }

        private TreeViewItem GetTreeViewItemByObjectType(Figure figure)
        {
            var threeView = figure.GetType() switch
            {
                { } type when type == typeof(Rectangle) => RectanglesTreeViewItem,
                { } type when type == typeof(Triangle) => TrianglesTreeViewItem,
                { } type when type == typeof(Circle) => CirclesTreeViewItem,
                _ => throw new InvalidOperationException("Unknown figure type")
            };
            return threeView;
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
        
        private void CreateFigureAndAddToTreeView(Func<Figure> creator, TreeViewItem treeViewItem)
        {
            Figure figure = creator.Invoke();
            _figures.Add(figure);
            
            var child = new TreeViewItem
            {
                Header = "Figure #" + _figureCounter++,
                Tag = figure,
            };

            child.MouseRightButtonUp += RemoveTreeViewItem_OnMouseRightButtonUp;
            child.Selected += TreeViewItem_OnSelected;
            
            treeViewItem.Items.Add(child);
        }

        private Figure CreateRectangle() => _figuresBuilder.BuildRectangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateCircle() => _figuresBuilder.BuildCircle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateTriable() => _figuresBuilder.BuildTriangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private void ChangeButtonContentBasedOnFigureStatus(Button button, bool stopped)
        {
            button.Content = _localizationManager.GetLocaleStringByKey(stopped ? "Continue" : "Stop");
        }
    }
}