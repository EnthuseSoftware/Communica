using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CustomSlider : UserControl, INotifyPropertyChanged
    {

        public CustomSlider()
        {
            InitializeComponent();
            this.Loaded += CustomSlider_Loaded;
        }

        //public static readonly DependencyProperty PositionProperty;
        public static readonly DependencyProperty ValueProperty;
        static CustomSlider()
        {
            //PositionProperty = DependencyProperty.Register("Position", typeof(double), typeof(CustomSlider), new FrameworkPropertyMetadata());
            ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CustomSlider), new FrameworkPropertyMetadata(new PropertyChangedCallback(
                (s, e) =>
                {
                    CustomSlider slider = (CustomSlider)s;
                    //if (!slider.animStopped)
                    //{
                    //    //slider.BeginAnimation(CustomSlider.ValueProperty, null);
                    //    slider.animStopped = true;
                    //}
                    slider.length = slider.Width;
                    if (Double.IsNaN(slider.Width))
                        slider.length = slider.ActualWidth;
                    slider.Position = ((double)e.NewValue * slider.length) / slider.MaxValue;
                }
                )));
        }


        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        double _position;
        public double Position
        {
            get
            {
                //return (double)GetValue(PositionProperty);
                return _position;
            }
            set
            {
                //SetValue(PositionProperty, value);
                _position = value;
                SetValue(ValueProperty, (double)value * MaxValue / length);
                NotifyPropertyChanged("Position");
            }
        }

        double length = 0;

        void CustomSlider_Loaded(object sender, RoutedEventArgs e)
        {
            track.DataContext = this;
        }

        public double MaxValue { get; set; }

        public event EventHandler ValueChangedManually;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            length = this.Width;
            if (Double.IsNaN(this.Width))
                length = this.ActualWidth;
            Point p = Mouse.GetPosition(this);
            double pos = (p.X * MaxValue) / length;
            this.BeginAnimation(CustomSlider.ValueProperty, null);
            Position = p.X;
            if (ValueChangedManually != null)
            {
                ValueChangedManually(this, new ValueChanged(this.Position));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }

    public class ValueChanged : EventArgs
    {
        public ValueChanged(double position)
        {
            Position = position;
        }
        public double Position { get; set; }
    }
    
    public class PositionChangedEventArgs : EventArgs
    {
        public double OldPosition { get; set; }
        public double NewPosition { get; set; }
    }

}
