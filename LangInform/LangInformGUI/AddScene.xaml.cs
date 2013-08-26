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
        ObservableCollection<SceneItem> myPoints = new ObservableCollection<SceneItem>();
        ViewModel vm = MainWindow.vm;
        Border currentDot;

        /// <summary>
        /// Gets the file path and shows the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtPath.Text = GetFilePath();
            FileInfo file = new FileInfo(txtPath.Text);
            if (file.Exists)
            {
                var image = Assistant.GetBitmapImageFrom(file.FullName);
                sceneImage.Source = image;
            }
        }


        /// <summary>
        /// Opens a file open dialog and gets the file path
        /// </summary>
        /// <returns></returns>
        string GetFilePath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image File (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }


        /// <summary>
        /// Gets the file path and shows the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getFileAndShow(object sender, RoutedEventArgs e)
        {
            txtPath.Text = GetFilePath();
            FileInfo file = new FileInfo(txtPath.Text);
            if (file.Exists)
            {
                var image = Assistant.GetBitmapImageFrom(file.FullName);
                sceneImage.Source = image;
            }
        }


        /// <summary>
        /// Opens a new window. User will be able to choose pictures from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromDB_Click(object sender, RoutedEventArgs e)
        {
            Pictures pictures = new Pictures();
            pictures.ShowDialog();
        }


        /// <summary>
        /// Selecting the sample Dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dots_Click(object sender, MouseButtonEventArgs e)
        {
            Border selectedBorder = (Border)sender;
            UnSelectAll();
            selectedBorder.BorderThickness = new Thickness(2, 2, 2, 2);
            CreateNewBorder(selectedBorder);
        }



        /// <summary>
        /// Deletes the the created dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteDot_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            var border = menu.Tag as Border;
            var point = myPoints.FirstOrDefault(p => p.ToString() == border.Tag.ToString());
            myPoints.Remove(point);
            grdPoints.Children.Remove(border);
        }

        /// <summary>
        /// Setting or changing the sound file of dot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeSoundFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fopen = new OpenFileDialog();
            fopen.Filter = "WAV(*.WAV)|*.WAV";
            fopen.ShowDialog();
            if (!string.IsNullOrEmpty(fopen.FileName) && new FileInfo(fopen.FileName).Exists)
            {
                var border = ((MenuItem)sender).Tag as Border;
                var point = border.Tag as SceneItem;
                if (point != null)
                {
                    point.Phrase = new Phrase() { Id = Guid.NewGuid(), Sound = Assistant.SoundToByte(fopen.FileName) };
                }
            }
        }

        private void MoveTheDots()
        {
            //(p.X - currentDot.Width / 2, p.Y - currentDot.Height / 2, 0, 0)
            var width = sceneImage.ActualWidth;
            var height = sceneImage.ActualHeight;
            foreach (var child in grdPoints.Children)
            {
                var dot = child as Border;
                if (dot == null) continue;
                var point = dot.Tag as SceneItem;
                dot.Margin = new Thickness((point.XPos * width / 100) - dot.Width / 2, (point.YPos * height / 100) - dot.Height / 2, 0, 0);
            }
        }

        private void grdLayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (currentDot == null) return;
            System.Windows.Point p = Mouse.GetPosition(grdLayer);
            var width = sceneImage.ActualWidth;
            var height = sceneImage.ActualHeight;

            SceneItem point = new SceneItem()
            {
                XPos = (p.X * 100)/width,
                YPos = (p.Y*100)/height,
                Id = (Guid)currentDot.Tag,
                Size = Convert.ToInt32(currentDot.Width),
                IsRound = (currentDot.CornerRadius.BottomLeft > 0 ? true : false)
            };

            currentDot.Margin = new Thickness((point.XPos * width / 100) - currentDot.Width / 2, (point.YPos * height / 100) - currentDot.Height / 2, 0, 0);
            //new Thickness(p.X - currentDot.Width / 2, p.Y - currentDot.Height / 2, 0, 0);

            currentDot.Tag = point;
            grdPoints.Children.Add(currentDot);
            string fname = "";
            if (chkImmediate.IsChecked == true)
            {
                OpenFileDialog fopen = new OpenFileDialog();
                fopen.Filter = "WAV(*.WAV)|*.WAV";
                fopen.ShowDialog();
                fname = fopen.FileName;
            }
            if (!string.IsNullOrEmpty(fname) && new FileInfo(fname).Exists)
            {
                point.Phrase = new Phrase() { Id = Guid.NewGuid(), Sound = Assistant.SoundToByte(fname) };
                point.PhraseId = point.Phrase.Id;
            }
            else
            {
                currentDot.Background = new SolidColorBrush(Colors.Yellow);
            }

            myPoints.Add(point);
            CreateNewBorder(currentDot);//!!!This method needs to be reviewed
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listPoints.ItemsSource = myPoints;
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
            item1.Click += ChangeSoundFile_Click;
            menu.Items.Add(item1);

            System.Windows.Controls.MenuItem item2 = new System.Windows.Controls.MenuItem();
            item2.Header = "Delete point";
            item2.Tag = border;
            item2.Click += DeleteDot_Click;
            menu.Items.Add(item2);
            border.ContextMenu = menu;
            currentDot = border;
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            //check all points
            if (string.IsNullOrEmpty(txtSceneName.Text))
            {
                MetroMessage.Show(this, "Empty scene name", "Scene name cannot be empty. Please enter scene name!", MessageButtons.OK, MessageIcon.Exclamation);
                return;
            }
            Scene scene = new Scene() { Id = Guid.NewGuid(), Name = txtSceneName.Text };
            foreach (SceneItem point in myPoints)
            {
                if (point.PhraseId==null)
                {
                    MetroMessage.Show(this, "Not completed points", "Some created point(s) doesn't have the sound file attached yet. They are marked yellow. Please attach sound files before you save it.");
                    return;
                }
            }
            //Saving the Scene Image
            ScenePicture scenePicture = new ScenePicture() { Id = Guid.NewGuid(), Picture = Assistant.BitmapImageToByte(sceneImage.Source as BitmapImage) };
            vm.InsertData(scenePicture, typeof(ScenePicture));

            //Saving the Scene
            scene.PictureId = scenePicture.Id;
            vm.InsertData(scene, typeof(Scene));

            //Saving the SceneItems and Phrase
            foreach (SceneItem sceneItem in myPoints)
            {
                Phrase phrase = sceneItem.Phrase;
                vm.InsertData(phrase, typeof(Phrase));
                sceneItem.PhraseId = phrase.Id;
                vm.InsertData(sceneItem, typeof(SceneItem));
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveTheDots();
        }






    }
}
