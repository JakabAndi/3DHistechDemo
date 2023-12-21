using GalaSoft.MvvmLight.Command;
using Global;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3DHistechDemo
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<IEngine> engineList;
        private ITable myTable;
        private double tableSpeed = 1;
        private MyCanvas myCanvas = new MyCanvas();
        private ICommand leftButtonCommand;
        private ICommand rightButtonCommand;
        private Point tableCoordinate = new Point(0, 0);

        internal MainWindowViewModel(List<IEngine> engines, ITable table)
        {
            engineList = engines;
            myTable = table;        

            foreach (var engine in engineList)
            {
                if (engine.Axis == AxisEnum.X)
                {
                    engine.MoveTo(TableWidth / 2, CanvasWidth);
                }
                if (engine.Axis == AxisEnum.Y)
                {
                    engine.MoveTo(TableHeight / 2, CanvasHeight);
                }
            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public static readonly DependencyProperty ReferenceProperty =
      DependencyProperty.RegisterAttached("Reference", typeof(bool),
                                          typeof(FrameworkElement),
                                          new UIPropertyMetadata(false, OnReferenceChanged));
        public static bool GetReference(DependencyObject obj) => (bool)obj.GetValue(ReferenceProperty);
        public static void SetReference(DependencyObject obj, bool value) => obj.SetValue(ReferenceProperty, value);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool XAxisEnable => engineList.Count > 0;
        public string XAxisPos => XAxisEnable ? engineList[0].Axis.ToString() + " " + engineList[0].GetEngineStep().ToString("F2") + "%" : string.Empty;
        public bool YAxisEnable => engineList.Count > 1;
        public string YAxisPos => YAxisEnable ? engineList[1].Axis.ToString() + " " + engineList[1].GetEngineStep().ToString("F2") + "%" : string.Empty;
        public bool ZAxisEnable => engineList.Count > 2;
        public string ZAxisPos => ZAxisEnable ? engineList[2].Axis.ToString() + " " + engineList[2].GetEngineStep().ToString("F2") + "%" : string.Empty;

        public string TablePosition => "Table position (" + TableCenter.X.ToString("F2") + ";" + TableCenter.Y.ToString("F2") + ";" + TableCenter.Z.ToString("F2") + ")";

        public Coordinate TableCenter => myTable.GetPosition();

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
        public MyCanvas MyCanvas
        {
            get { return myCanvas; }
            set { myCanvas = value; myCanvas.Clicked += HandlePointChanged; }
        }

        public ICommand LeftButtonCommand
        {
            get
            {
                if (leftButtonCommand == null)
                    leftButtonCommand = new RelayCommand<AxisEnum>(LeftButtonClick);
                return leftButtonCommand;
            }
        }

        public ICommand RightButtonCommand
        {
            get
            {
                if (rightButtonCommand == null)
                    rightButtonCommand = new RelayCommand<AxisEnum>(RightButtonClick);
                return rightButtonCommand;
            }
        }
        public Point TableCoordinateOnUI
        {
            get { return tableCoordinate; }
            private set
            {
                var limitedValue = CheckValue(value);
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

                OnPropertyChanged(nameof(TablePosition));
                OnPropertyChanged(nameof(XAxisPos));
                OnPropertyChanged(nameof(YAxisPos));
                OnPropertyChanged(nameof(ZAxisPos));

                OnPropertyChanged();
            }
        }
        private static void OnReferenceChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var vp = depObj as Canvas;
            if (vp == null || !(e.NewValue is Boolean)) return;
            var vm = vp.DataContext as MainWindowViewModel;
            vm.MyCanvas = new MyCanvas() { Canva = vp };
        }

        private void RightButtonClick(AxisEnum axisEnum)
        {
            MoveEngine(axisEnum, true);
        }
        private void LeftButtonClick(AxisEnum axisEnum)
        {
            MoveEngine(axisEnum, false);
        }

        private void MoveEngine(AxisEnum axisEnum, bool direction)
        {
            Coordinate newTablecoordinates = TableCenter;
            switch (axisEnum)
            {
                case AxisEnum.X:
                    {
                        if (XAxisEnable)
                        {
                            engineList[0].MakeStep(direction);
                            newTablecoordinates.X = ConvertBack(engineList[0].GetEngineStep(), CanvasWidth);

                        }
                        break;
                    }
                case AxisEnum.Y:
                    {
                        if (YAxisEnable)
                        {
                            engineList[1].MakeStep(direction);
                            newTablecoordinates.Y = ConvertBack(engineList[1].GetEngineStep(), CanvasHeight);

                        }
                        break;
                    }
                case AxisEnum.Z:
                    {
                        if (ZAxisEnable)
                        {
                            engineList[2].MakeStep(direction);
                            newTablecoordinates.Z = ConvertBack(engineList[2].GetEngineStep(), 100);
                        }
                        break;
                    }
            }

            myTable.MoveTo(newTablecoordinates);
            tableCoordinate.X = newTablecoordinates.X - TableWidth / 2;
            tableCoordinate.Y = newTablecoordinates.Y - TableHeight / 2;
            OnPropertyChanged(nameof(XAxisPos));
            OnPropertyChanged(nameof(YAxisPos));
            OnPropertyChanged(nameof(ZAxisPos));
            OnPropertyChanged(nameof(TableCoordinateOnUI));

            OnPropertyChanged(nameof(TableCenter));
            OnPropertyChanged(nameof(TablePosition));
        }

        private double ConvertBack(double value, double scale)
        {
            return value / 100 * scale;
        }
        private Point CheckValue(Point value)
        {
            Point tempPoint = value;
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
        private void HandlePointChanged(object? sender, PointEventArgs e)
        {
            if (e != null)
            {
                TableCoordinateOnUI = e.Point;
            }
        }
    }
}    