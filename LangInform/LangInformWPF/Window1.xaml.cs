using LangInformGUI.Controls;
using LangInformModel;
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
using System.Windows.Shapes;

namespace LangInformGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        
        }

        MyItemControl control;
        Converter con = new Converter();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Word word = new Word();
            //word.Picture = con.BitmapToByte(Properties.Resources._1);
            word.Sound = con.SoundToByte("C:\\LangLearnData\\media3.wav");
            control = new MyItemControl(word);
            control.MouseLeftButtonDown += control_MouseLeftButtonDown;
            grd.Children.Add(control);
        }

        void control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MyItemControl cont = (MyItemControl)sender;
            cont.Play(0);
            cont.StartHighlighting(3000);
        }
    }
}
