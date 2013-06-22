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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangInformGUI.Controls
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        public new static readonly DependencyProperty FontSizeProperty;
        public static readonly DependencyProperty TextProperty;

        static CustomButton()
        {
            FontSizeProperty = DependencyProperty.Register("FontSize", typeof(int), typeof(CustomButton), new FrameworkPropertyMetadata());
            FontSizeProperty = DependencyProperty.Register("Text", typeof(string), typeof(CustomButton), new FrameworkPropertyMetadata());
        }

        public new int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public event RoutedEventHandler Click;

        bool isMouseDown = false;

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                if (Click != null)
                {
                    Click(this, new RoutedEventArgs());
                }
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
                isMouseDown = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

    }
}
