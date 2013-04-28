using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangInformGUI.Controls
{
    /// <summary>
    /// Interaction logic for CustomSlider.xaml
    /// </summary>
    public partial class CustomSlider : UserControl
    {
        public CustomSlider()
        {
            InitializeComponent();
        }

        static CustomSlider()
        {
            PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(CustomSlider),
              new FrameworkPropertyMetadata(new PropertyChangedCallback(ChangeValue)));

        }

        public static readonly DependencyProperty PositionProperty;


        private static void ChangeValue(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {

            (source as CustomSlider).UpdateValue(Convert.ToDouble(e.NewValue));

        }

        private void UpdateValue(double newVal)
        {
            Position = newVal;
        }



        static void valueChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            CustomSlider directoryBox = (CustomSlider)property;
            directoryBox.Position = (double)args.NewValue;
        }

        public double Position1
        {
            get { return (double)GetValue(PositionProperty); }
            set
            {
                SetValue(PositionProperty, value);
            }
        }






        double _position;
        /// <summary>
        /// Postion of slider. Postion can not be greater than MaxValue.
        /// </summary>
        public double Position
        {
            get
            {
                return (double)GetValue(PositionProperty);
            }
            set
            {
                double oldValue = _position;
                _position = value;
                SetValue(PositionProperty, value);
                if (_position >= MaxValue)
                {
                    _position = MaxValue;
                    SetValue(PositionProperty, _position);
                }
                ChangePosition();
                if (PositionChanged != null)
                {
                    PositionChanged(this, new PositionChangedEventArgs() { OldPosition = oldValue, NewPosition = _position });
                }
            }
        }

        void ChangePosition()
        {
            lblProgress.Width = (_position * length) / MaxValue;
        }

        public event EventHandler PositionChanged;

        public event EventHandler PositionChangedManually;

        public double MaxValue { get; set; }

        double length = 0;

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            length = this.Width;
            if (Double.IsNaN(this.Width))
                length = this.ActualWidth;
            Point p = Mouse.GetPosition(this);
            double oldValue = _position;
            double _newPosition = (p.X * MaxValue) / length;
            Position = _newPosition;
            StartAnimation((int)_newPosition, (int)MaxValue);
            if (PositionChangedManually != null)
                PositionChangedManually(this, new PositionChangedEventArgs() { OldPosition = oldValue, NewPosition = _position });
        }

        public void StartAnimation(int start, int end)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = start;
            anim.To = end;
            anim.Duration = TimeSpan.FromMilliseconds(end - start);
            this.BeginAnimation(CustomSlider.PositionProperty, anim);
        }

    }
    public class PositionChangedEventArgs : EventArgs
    {
        public double OldPosition { get; set; }
        public double NewPosition { get; set; }
    }

}
