using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WPFCourse.Controls
{
    /// <summary>
    /// Interaction logic for CToggleSwitch.xaml
    /// </summary>
    public partial class CToggleSwitch : UserControl
    {
        public CToggleSwitch()
        {
            InitializeComponent();
            slider = new Slider();
            slider.Maximum = 80;
            slider.Value = 80;
            slider.ValueChanged += slider_ValueChanged;
        }
        public bool _isOn;
        public bool IsOn { get { return _isOn; } set { _isOn = value; ChangePostition(value); } }
        Slider slider;

        public string Text { get { return lbl_text.Content.ToString(); } set { lbl_text.Content = value; } }

        void ChangePostition(bool on)
        {
            if (on)
            {
                lbl_background.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF4618B2");
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = 80;
                anim.To = 0;
                anim.Duration = TimeSpan.FromMilliseconds(100);
                slider.BeginAnimation(Slider.ValueProperty, anim);
                _isOn = true;
                lbl_status.Content = "On";
            }
            else
            {
                lbl_background.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB6B6B6");
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = 0;
                anim.To = 80;
                anim.Duration = TimeSpan.FromMilliseconds(100);
                slider.BeginAnimation(Slider.ValueProperty, anim);
                _isOn = false;
                lbl_status.Content = "Off";
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }

        void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = e.NewValue;
            lbl_toggle.Margin = new Thickness((value + 10) * -1, 0, 0, 0);
        }


        private void lbl_handle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsOn)
            {
                lbl_background.Opacity = .9;
            }
        }

        private void lbl_handle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsOn)
            {
                lbl_background.Opacity = 1;
            }
        }

        System.Drawing.Point currentPos;
        private void lbl_handle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label obj = (Label)sender;
            if (obj.Name == "lbl_toggle")
            {
                currentPos = System.Windows.Forms.Cursor.Position;
            }
            if (IsOn)
            {
                lbl_background.Opacity = .7;
                e.Handled = true;
            }
        }

        private void lbl_handle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsOn)
                ChangePostition(false);
            else
                ChangePostition(true);
            lbl_background.Opacity = 1;
            e.Handled = true;
        }

        private void lbl_toggle_MouseMove(object sender, MouseEventArgs e)
        {
            //if (mouseDown)
            //{
            //    System.Drawing.Point newPoint = System.Windows.Forms.Cursor.Position;
            //    double diff = newPoint.X - currentPos.X;
            //    bool pos = diff > 0 ? true : false;
            //    diff = Math.Abs(diff);
            //    if (diff >= 81)
            //    {
            //        diff = 80;
            //    }
            //    double exactMove = (90 - diff)*-1;
            //    test.Content = exactMove;
            //    lbl_toggle.Margin = new Thickness(exactMove, 0, 0, 0);
            //}
        }

    }
}
