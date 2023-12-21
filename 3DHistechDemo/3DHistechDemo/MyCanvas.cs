using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _3DHistechDemo
{
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

        private Point clickedPoint = new Point(0, 0);

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
        public Point ClickedPoint
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
        public PointEventArgs(Point pt)
        {
            Point = pt;
        }

        public Point Point { get; }
    }
}