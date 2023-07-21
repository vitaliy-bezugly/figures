using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Figures.Domain;
using Figures.Domain.Args;
using Figures.Domain.Exceptions;
using Figures.Infrastructure;
using Microsoft.Win32;
using Point = System.Drawing.Point;

namespace Figures.UI
{
    public partial class MainWindow
    {
        private const int RedrawTime = 50;
        private const int MoveTime = 50;
        
        private readonly DispatcherTimer _timer = new();
        private readonly UiElementFactory _uiElementFactory = new();
        private readonly FiguresBuilder _figuresBuilder = new();
        private readonly ICollection<Figure> _figures = new List<Figure>();
        private readonly LocalizationManager _localizationManager;
        private int _figureCounter;

        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

        public MainWindow()
        {
            InitializeComponent();

            _localizationManager = new LocalizationManager(Resources);
            _localizationManager.InitializeDefaultLanguage();

            _figureCounter = 0;

            _timer.Tick += TimerOnTick;
            _timer.Interval = TimeSpan.FromMilliseconds(MoveTime);
            _timer.Start();
            
            // Start the drawing thread
            ThreadPool.QueueUserWorkItem(_ => DrawFiguresThreadMethod());
        }

        private void TimerOnTick(object? sender, EventArgs e)
        {
            var bottomRightPoint = new Point(((int)MainCanvas.ActualWidth), ((int)MainCanvas.ActualHeight));
            
            if (_figures.Count == 0)
                return;

            MoveFigures(_figures, bottomRightPoint);
        }
        
        private void DrawFiguresThreadMethod()
        {
            while (true)
            {
                DrawFigures(_figures);
                Thread.Sleep(RedrawTime);
            }
            // ReSharper disable once FunctionNeverReturns
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
            if (selectedTreeView is null)
                return;

            var figure = selectedTreeView.Tag as Figure;
            if (figure is null)
                throw new InvalidOperationException();

            ChangeButtonContentBasedOnFigureStatus(StopOrContinueButton, figure.Stopped);
        }

        private void StopOrContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var figure = GetSelectedFigure();

                figure.Stopped = !figure.Stopped;
                ChangeButtonContentBasedOnFigureStatus(StopOrContinueButton, figure.Stopped);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ChangeLanguageMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem is null)
                throw new InvalidOperationException();

            var language = menuItem.Tag as string;
            if (language is null)
                throw new InvalidOperationException();

            _localizationManager.SwitchLanguage(language);
        }

        private async void SaveFiguresButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using IRepository<Figure> repository = ShowFileDialogAndGetSpecificRepo();
                await repository.SaveManyAsync(_figures);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadFiguresButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using IRepository<Figure> repository = ShowFileDialogAndGetSpecificRepo();
                await LoadFiguresAndAddToCollectionAsync(repository);

                MessageBox.Show("Figures loaded successfully", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void AddSubscriberToFigureEventButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Figure figure = GetSelectedFigure();
                figure.Intersection += FigureOnIntersection;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void RemoveSubscriberFromFigureEventButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Figure figure = GetSelectedFigure();
                figure.Intersection -= FigureOnIntersection;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        
        private void ClearFiguresButton_OnClick(object sender, RoutedEventArgs e)
        {
            _figures.Clear();
            ClearTreeViewItemsFromFigures();
        }

        private IRepository<Figure> ShowFileDialogAndGetSpecificRepo()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json|Xml files (*.xml)|*.xml|Binary files (*.bin)|*.bin",
                InitialDirectory = Environment.CurrentDirectory
            };

            fileDialog.ShowDialog();
            if (string.IsNullOrEmpty(fileDialog.FileName))
                throw new InvalidOperationException("File not selected");

            var resolution = fileDialog.FileName.Split('.').Last();
            return resolution switch
            {
                "json" => new JsonFileRepository<Figure>(fileDialog.FileName),
                "xml" => new XmlFileRepository<Figure>(fileDialog.FileName),
                "bin" => new BinaryFileRepository<Figure>(fileDialog.FileName),
                _ => throw new InvalidOperationException("Unknown file type")
            };
        }

        private async Task LoadFiguresAndAddToCollectionAsync(IRepository<Figure> repository)
        {
            var loadedFigures = await repository.GetAllAsync();

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
            try
            {
                var enumerable = figures as Figure[] ?? figures.ToArray();

                foreach (var figure in enumerable)
                {
                    figure.Move(endPoint);
                    figure.CheckIntersections(enumerable);
                }
            }
            catch (OutOfRegionException e)
            {
                e.Figure.MoveToSpecificLocation(new Point(100, 100));
                Logger.Info(e.Message);
            }
        }
        
        private void DrawFigures(IEnumerable<Figure> figures)
        {
            var enumerable = figures as Figure[] ?? figures.ToArray();
            
            MainCanvas.Dispatcher.Invoke(() =>
            {
                MainCanvas.Children.Clear();
            });

            foreach (var figure in enumerable)
            {
                // Access UI thread here for creating UIElement
                MainCanvas.Dispatcher.Invoke(() =>
                {
                    var geometryFigure = figure.Draw();
                    var uiElement = _uiElementFactory.Create(geometryFigure);
                    MainCanvas.Children.Add(uiElement);
                });
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

        private Figure CreateRectangle() =>
            _figuresBuilder.BuildRectangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateCircle() =>
            _figuresBuilder.BuildCircle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private Figure CreateTriable() =>
            _figuresBuilder.BuildTriangle(new Point((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight));

        private void ChangeButtonContentBasedOnFigureStatus(Button button, bool stopped)
        {
            button.Content = _localizationManager.GetLocaleStringByKey(stopped ? "Continue" : "Stop");
        }

        private void ClearTreeViewItemsFromFigures()
        {
            RectanglesTreeViewItem.Items.Clear();
            TrianglesTreeViewItem.Items.Clear();
            CirclesTreeViewItem.Items.Clear();
        }

        private Figure GetSelectedFigure()
        {
            var selectedTreeView = FiguresThreeView.SelectedItem as TreeViewItem;
            return selectedTreeView?.Tag as Figure ?? new FigureNullObject();
        }
        
        private void FigureOnIntersection(object? sender, IntersectionEventArgs e)
        {
            SystemSounds.Beep.Play();
            Trace.WriteLine("Intersection detected. Intersection point: " + e.IntersectionPoint);
        }
    }
}