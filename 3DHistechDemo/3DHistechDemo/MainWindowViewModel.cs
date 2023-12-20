using GalaSoft.MvvmLight.Command;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3DHistechDemo
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<IEngine> engineList;
        private ITable myTable;
        private double tableXCoordinate = 0;
        private double tableYCoordinate = 0;

        public event PropertyChangedEventHandler? PropertyChanged;
      
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal MainWindowViewModel(List<IEngine> engines, ITable table)
        {
            engineList = engines;
            myTable = table;
        }

        private MyCanvas myCanvas = new MyCanvas();
        public MyCanvas MyCanvas 
        {
            get { return myCanvas; }
            set { myCanvas = value; }
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

        public static System.Windows.Point EndPoint = new System.Windows.Point(0, 0);

        public System.Windows.Point Coordinate 
        {
            get { return new System.Windows.Point(EndPoint.X, EndPoint.Y); }
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public System.Windows.Point ClickedPoint 
        {
            get => clickedPoint;
            private set 
            {
                clickedPoint = value;
                OnPropertyChanged();
            }
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickedPoint = e.GetPosition(canvas);
        }
    }
}