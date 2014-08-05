using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for CButton.xaml
    /// </summary>
    public partial class CButton : UserControl, INotifyPropertyChanged
    {

        new public static readonly DependencyProperty ContentProperty;
        static CButton()
        {
            ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(CButton),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(ChangeContent)));
        }

        private static void ChangeContent(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as CButton).Content = e.NewValue.ToString();

        }

        public CButton()
        {
            InitializeComponent();
        }

        new public string Content { get { return GetValue(ContentProperty).ToString(); } set { SetValue(ContentProperty, value); main.Content = value; } }

        public Brush BorderColor { get { return border.BorderBrush; } set { border.BorderBrush = value; } }

        new public Thickness BorderThickness { get { return border.BorderThickness; } set { border.BorderThickness = value; } }

        new public SolidColorBrush Background { get { return (SolidColorBrush)bgrd.Background; } set { bgrd.Background = value; } }

        public CornerRadius CornerRadius { get { return border.CornerRadius; } set { border.CornerRadius = value; } }

        new public FontFamily FontFamily { get { return main.FontFamily; } set { main.FontFamily = value; } }

        new public double FontSize { get { return main.FontSize; } set { main.FontSize = value; } }

        new public FontStyle FontStyle { get { return main.FontStyle; } set { main.FontStyle = value; } }

        new public FontWeight FontWeight { get { return main.FontWeight; } set { main.FontWeight = value; } }

        new public Brush Foreground { get { return main.Foreground; } set { main.Foreground = value; } }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        bool _disableButton;

        public bool DisableButton { get { return _disableButton; } set { _disableButton = value; ControlStatus(); } }

        void ControlStatus()
        {
            if (_disableButton)
            {
                this.IsEnabled = false;
                grd_dis.Background = new SolidColorBrush(Colors.Gray);
                grd_dis.Opacity = .3;
                grd_dis.Visibility = Visibility.Visible;
            }
            else
            {
                this.IsEnabled = true;
                grd_dis.Background = new SolidColorBrush(Colors.Transparent);
                grd_dis.Opacity = 10;
                grd_dis.Visibility = Visibility.Hidden;

            }

        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        public event MouseEventHandler OnClick;

        bool IsBtnDown = false;


        SolidColorBrush tempBackground;
        SolidColorBrush tempBackground1;
        SolidColorBrush tempForeground;


        private void main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsBtnDown = true;
            tempBackground = Background;
            bgrd.Opacity = 10;
            tempForeground = (SolidColorBrush)main.Foreground;
            bgrd.Background = new SolidColorBrush(Colors.White);
            main.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void main_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Background = tempBackground;
            bgrd.Opacity = 0.5;
            main.Foreground = tempForeground;
            if (IsBtnDown)
            {
                if (OnClick != null)
                    OnClick(this, e);
            }
            IsBtnDown = false;

        }

        private void main_MouseEnter(object sender, MouseEventArgs e)
        {
            tempBackground1 = Background;
            Background = new SolidColorBrush(Colors.Black);
            bgrd.Opacity = .5;
        }

        private void main_MouseLeave(object sender, MouseEventArgs e)
        {
            IsBtnDown = false;
            Background = tempBackground1;
            bgrd.Opacity = 10;
        }

        private void buttonDownBehavior()
        {
            tempBackground = Background;
            tempForeground = (SolidColorBrush)main.Foreground;
            Background = new SolidColorBrush(Colors.White);
            main.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void buttonUpBehavior()
        {
            Background = tempBackground;
            main.Foreground = tempForeground;
        }

        private void MouseInBehavior()
        {
            Background = new SolidColorBrush(Colors.Black);
            main.Opacity = .1;
        }

        private void MouseOutBehavior()
        {
            Background = tempBackground;
            main.Opacity = 10;
        }






        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Font
    {
        public FontFamily FontFamily { get; set; }
        public double FontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }

    }
}
