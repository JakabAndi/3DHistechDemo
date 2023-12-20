using GalaSoft.MvvmLight.Command;
using Global;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace _3DHistechDemo
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<IEngine> engineList;
        private ITable myTable;
        private double tableSpeed = 1;

        public event PropertyChangedEventHandler? PropertyChanged;
      
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal MainWindowViewModel(List<IEngine> engines, ITable table)
        {
            engineList = engines;
            myTable = table;
            Axis = new ObservableCollection<string>();
            

            foreach (var engine in engineList) 
            {
                Axis.Add(engine.Axis.ToString() + " " + engine.GetEngineStep().ToString("F2") + "%");
            }

        }

        private void HandlePointChanged(object? sender, PointEventArgs e)
        {
            if (e != null)
            {
                TableCoordinateOnUI = e.Point;
            }
        }

        public ObservableCollection<string> Axis { get; set; }

        public string TablePosition => "Table position (" + TableCenter.X.ToString("F2") + ";" + TableCenter.Y.ToString("F2") + ";" + TableCenter.Z.ToString("F2") + ")";

        private MyCanvas myCanvas = new MyCanvas();
        public MyCanvas MyCanvas 
        {
            get { return myCanvas; }
            set { myCanvas = value; myCanvas.Clicked += HandlePointChanged; }
        }

        public static readonly DependencyProperty ReferenceProperty =
      DependencyProperty.RegisterAttached("Reference", typeof(bool),
                                          typeof(FrameworkElement),
                                          new UIPropertyMetadata(false, OnReferenceChanged));
        public static bool GetReference(DependencyObject obj) => (bool)obj.GetValue(ReferenceProperty);
        public static void SetReference(DependencyObject obj, bool value) => obj.SetValue(ReferenceProperty, value);

        private static void OnReferenceChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var vp = depObj as Canvas;
            if (vp == null || !(e.NewValue is Boolean)) return;
            var vm = vp.DataContext as MainWindowViewModel;
            vm.MyCanvas = new MyCanvas() { Canva = vp };
        }
                
        public Coordinate TableCenter
        {
            get => myTable.GetPosition();
        }

        public double TableSpeed 
        {
            get => tableSpeed;
            set 
            {
                if (value > 0 && value <= 1)
                {
                    tableSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        public double TableWidth => myTable.GetTableSize().Width;
        public double TableHeight => myTable.GetTableSize().Height;
        public double CanvasWidth = 500;
        public double CanvasHeight = 500;

        private System.Windows.Point tableCoordinate = new System.Windows.Point(0,0);
        public System.Windows.Point TableCoordinateOnUI 
        {
            get { return tableCoordinate; }
            private set 
            {
                var limitedValue = CheckValue(value);
                //tableCoordinate = value;
                
                //Coordinate calucaltedCoordinate = new Coordinate(tableCoordinate.X + myTable.GetTableSize().Width / 2, tableCoordinate.Y + myTable.GetTableSize().Height / 2, myTable.GetPosition().Z);
                foreach (var item in engineList)
                {
                    if (item.Axis == AxisEnum.X)
                    {
                        item.MoveTo(limitedValue.X, CanvasWidth);
                    }
                    if (item.Axis == AxisEnum.Y)
                    {
                        item.MoveTo(limitedValue.Y, CanvasHeight);
                    }
                }

                tableCoordinate.X = limitedValue.X - TableWidth / 2;
                tableCoordinate.Y = limitedValue.Y - TableHeight / 2;                

                myTable.MoveTo(new Coordinate(limitedValue.X, limitedValue.Y, myTable.GetPosition().Z));

                Axis.Clear();
                foreach (var engine in engineList)
                {
                    Axis.Add(engine.Axis.ToString() + " " + engine.GetEngineStep().ToString("F2") + "%");
                }
                OnPropertyChanged(nameof(TablePosition));
                OnPropertyChanged(nameof(Axis));
                OnPropertyChanged();
            }
        }

        private System.Windows.Point CheckValue(System.Windows.Point value)
        {
            System.Windows.Point tempPoint = value;
            if (value.X < TableWidth / 2)
            { 
                tempPoint.X = TableWidth / 2;
            }
            if (value.Y < TableHeight / 2)
            { 
                tempPoint.Y = TableHeight / 2;
            }
            if (value.X > CanvasWidth - TableWidth / 2)
            {
                tempPoint.X = CanvasWidth - TableWidth / 2;
            }
            if (value.Y > CanvasHeight - TableHeight / 2)
            { 
                tempPoint.Y = CanvasHeight - TableHeight / 2;
            }

            return tempPoint;
        }
    }

    public class MyCanvas : INotifyPropertyChanged
    {
        private Canvas canvas;
        public Canvas Canva
        {
            get => canvas;
            set 
            {
                if (canvas == null)
                {
                    canvas = value;
                    canvas.MouseDown += Canvas_MouseDown;
                }
            }
        
        }

        private System.Windows.Point clickedPoint = new System.Windows.Point(0, 0);

        public event EventHandler<PointEventArgs>? Clicked;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnClicked(PointEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
        public System.Windows.Point ClickedPoint 
        {
            get => clickedPoint;
            private set 
            {
                clickedPoint = value;
                OnClicked(new PointEventArgs(clickedPoint));
                OnPropertyChanged();
            }
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickedPoint = e.GetPosition(canvas);
        }
    }
    public class PointEventArgs : EventArgs
    {
        public PointEventArgs(System.Windows.Point pt)
        {
            Point = pt;
        }

        public System.Windows.Point Point { get; }
    }
}