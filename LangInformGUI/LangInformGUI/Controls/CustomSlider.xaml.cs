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

        double _position;
        /// <summary>
        /// Postion of slider. Postion can not be greater than MaxValue.
        /// </summary>
        public double Position
        {
            get
            {
                return _position;
            }
            set
            {
                double oldValue = _position;
                _position = value;
                if (_position >= MaxValue)
                {
                    _position = MaxValue;
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
            lblProgress.Width = (_position * this.Width) / MaxValue;
        }

        public event EventHandler PositionChanged;

        public event EventHandler PositionChangedManually;

        public double MaxValue { get; set; }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this);
            double oldValue = _position;
            Position = (p.X * MaxValue) / this.Width;
            if (PositionChangedManually != null)
                PositionChangedManually(this, new PositionChangedEventArgs() { OldPosition = oldValue, NewPosition = _position });
        }

    }
    public class PositionChangedEventArgs : EventArgs
    {
        public double OldPosition { get; set; }
        public double NewPosition { get; set; }
    }

}
