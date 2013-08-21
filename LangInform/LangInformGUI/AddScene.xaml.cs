using LangInformGUI.AppCode;
using LangInformGUI.Controls;
using LangInformGUI.Windows;
using LangInformModel;
using LangInformVM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangInformGUI
{
    /// <summary>
    /// Interaction logic for AddScene.xaml
    /// </summary>
    public partial class AddScene : Window
    {
        public AddScene(Lesson lesson)
        {
            InitializeComponent();
            _lesson = lesson;
        }

        Lesson _lesson;

        ObservableCollection<MyPoint> myPoints = new ObservableCollection<MyPoint>();


        private void txtPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetFilePath();
        }

        void GetFilePath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image File (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();
            string fileName = fileDialog.FileName;
            txtPath.Text = fileName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetFilePath();
            FileInfo file = new FileInfo(txtPath.Text);
            if (file.Exists)
            {
                var image = Assistant.GetBitmapImageFrom(file.FullName);
                sceneImage.Source = image;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Pictures pictures = new Pictures();
            pictures.ShowDialog();
        }

       

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border selectedBorder = (Border)sender;
            UnSelectAll();
            selectedBorder.BorderThickness = new Thickness(2, 2, 2, 2);
            CreateNewBorder(selectedBorder);

        }




        void item2_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void item1_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        Border currentDot;

        private void grdLayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentDot == null) return;
            System.Windows.Point p = Mouse.GetPosition(grdLayer);
            currentDot.Margin = new Thickness(p.X - currentDot.Width / 2, p.Y - currentDot.Height / 2, 0, 0);

            MyPoint point = CreatePoint(currentDot, p.X, p.Y);
            grdPoints.Children.Add(currentDot);
            string fname = "";
            if (chkImmediate.IsChecked == true)
            {
                OpenFileDialog fopen = new OpenFileDialog();
                fopen.Filter = "WAV(*.WAV)|*.WAV";
                fopen.ShowDialog();
                fname = fopen.FileName;
            }
            if (fname != "")
            {
                point.PathToSound = fname;
            }
            else
            {
                currentDot.Background = new SolidColorBrush(Colors.Yellow);
            }

            myPoints.Add(point);
            CreateNewBorder(currentDot);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listPoints.ItemsSource = myPoints;
        }


        MyPoint CreatePoint(Border border, double x, double y)
        {
            MyPoint point = new MyPoint();
            point.X = x;
            point.Y = y;
            point.Size = Convert.ToInt32(border.Width);
            point.PathToSound = null;
            point.ID = (Guid)border.Tag;
            point.IsRound = false;
            if (border.CornerRadius.BottomLeft > 0)
                point.IsRound = true;
            return point;
        }

        void UnSelectAll()
        {
            foreach (var child in stcBorders.Children)
            {
                if (child is Border)
                {
                    ((Border)child).BorderThickness = new Thickness(0, 0, 0, 0);
                }
            }
        }

        void CreateNewBorder(Border sample)
        {
            Border border = new Border() { HorizontalAlignment = System.Windows.HorizontalAlignment.Left };
            border.Height = sample.Height;
            border.Width = sample.Width;
            border.CornerRadius = sample.CornerRadius;
            border.Background = new SolidColorBrush(Colors.Red);
            border.Opacity = .4;
            border.VerticalAlignment = VerticalAlignment.Top;
            border.Tag = Guid.NewGuid();
            System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();

            System.Windows.Controls.MenuItem item1 = new System.Windows.Controls.MenuItem();
            item1.Header = "Change sound file";
            item1.Tag = border;
            item1.Click += item1_Click;
            menu.Items.Add(item1);

            System.Windows.Controls.MenuItem item2 = new System.Windows.Controls.MenuItem();
            item2.Header = "Delete point";
            item2.Tag = border;
            item2.Click += item2_Click;
            menu.Items.Add(item2);
            border.ContextMenu = menu;
            currentDot = border;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //check all points
            foreach (MyPoint point in myPoints)
            {
                if (string.IsNullOrEmpty(point.PathToSound))
                {
                    MetroMessage.Show(this, "Not completed points", "Some created point(s) doesn't have the sound file attached yet. They are marked yellow. Please attach sound files before you save it.");
                    return;
                }
            }
        }




        

    }
}
