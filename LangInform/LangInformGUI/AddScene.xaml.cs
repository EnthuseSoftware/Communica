using LangInformGUI.Controls;
using LangInformGUI.Windows;
using LangInformModel;
using LangInformVM;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            MyPoints = new ObservableCollection<SceneItem>();
        }

        Lesson _lesson;
        public ObservableCollection<SceneItem> MyPoints { get; set; }
        ViewModel vm = MainWindow.vm;
        Border newCreatedDot;
        Border _selectedDot;

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

        public bool pictureFromdatabase = false;

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
            Pictures pictures = new Pictures(this);
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
            DeSelectAll();
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
            var point = MyPoints.FirstOrDefault(p => p.ToString() == border.Tag.ToString());
            MyPoints.Remove(point);
            grdPoints.Children.Remove(border);
            EnumerateTheDots();
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
                    border.Background = new SolidColorBrush(Colors.Green);
                }
            }
        }

        /// <summary>
        /// Moving the dots when picture resized
        /// </summary>
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
            if (newCreatedDot == null) return;
            System.Windows.Point p = Mouse.GetPosition(grdLayer);
            var width = sceneImage.ActualWidth;
            var height = sceneImage.ActualHeight;

            SceneItem point = new SceneItem()
            {
                XPos = (p.X * 100) / width,
                YPos = (p.Y * 100) / height,
                Id = (Guid)newCreatedDot.Tag,
                Size = Convert.ToInt32(newCreatedDot.Width),
                IsRound = (newCreatedDot.CornerRadius.BottomLeft > 0 ? true : false)
            };

            newCreatedDot.Margin = new Thickness((point.XPos * width / 100) - newCreatedDot.Width / 2, (point.YPos * height / 100) - newCreatedDot.Height / 2, 0, 0);
            
            newCreatedDot.Tag = point;
            grdPoints.Children.Add(newCreatedDot);
            SelectBorder(newCreatedDot);
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
                newCreatedDot.Background = new SolidColorBrush(Colors.Yellow);
            }

            MyPoints.Add(point);
            EnumerateTheDots();
            CreateNewBorder(newCreatedDot);//!!!This method needs to be reviewed
        }

        private void EnumerateTheDots()
        {
            int counter=0;
            foreach (var point in grdPoints.Children)
            {
                counter++;
               var dot = point as Border;
              
               dot.Child = new TextBlock() { Text = counter.ToString(), FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listPoints.DataContext = this;
            listPoints.SetBinding(ListBox.ItemsSourceProperty, new Binding("MyPoints"));
        }




        void DeSelectAll()
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
            border.Background = new SolidColorBrush(Colors.Green);
            border.Opacity = .4;
            border.VerticalAlignment = VerticalAlignment.Top;
            border.Tag = Guid.NewGuid();
            border.MouseLeftButtonDown += border_MouseLeftButtonDown;
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
            newCreatedDot = border;
        }

        void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border selectedDot = (Border)sender;
            SelectBorder(selectedDot);
        }

        void SelectBorder(Border selectedDot)
        {
            _selectedDot = selectedDot;
            foreach (Border dot in grdPoints.Children)
            {
                if (dot == selectedDot) continue;
                dot.BorderThickness = new Thickness(0);
            }
            if (selectedDot != null)
            {
                selectedDot.BorderThickness = new Thickness(2);
                selectedDot.BorderBrush = new SolidColorBrush(Colors.Blue);
            }
        }


        private void Button_Save(object sender, RoutedEventArgs e)
        {
            if (_lesson == null || _lesson.Id == null)
            {
                MetroMessage.Show(this, "No Lesson selected", "Lesson is not selected.");
                return;
            }
            //check all points
            string sceneName = "";
            while (true)
            {
                sceneName = MetroInputBox.Show(this, "add scene", "Please enter scene name");
                if (string.IsNullOrEmpty(sceneName))
                {
                    var res = MetroMessage.Show(this, "add acene", "Scene name cannot be empty. Do you want to enter?", MessageButtons.YesNo, MessageIcon.Exclamation);
                    if(res == MessageResult.No) return;
                }
                if (sceneName != "")
                    break;
            }
            Scene scene = new Scene() { Id = Guid.NewGuid(), Name = sceneName };
            foreach (SceneItem point in MyPoints)
            {
                if (point.PhraseId == null)
                {
                    MetroMessage.Show(this, "Not completed points", "Some created point(s) doesn't have the sound file attached yet. They are marked yellow. Please attach sound files before you save it.");
                    return;
                }
            }
            //Saving the Scene Image
            Guid pictureId;
            if (pictureFromdatabase)
            {
                pictureId = (sceneImage.Tag as ScenePicture).Id;
            }
            else
            {
                var pic = Assistant.BitmapImageToByte(sceneImage.Source as BitmapImage);
                ScenePicture scenePicture = new ScenePicture() { Id = Guid.NewGuid(), Picture = pic };
                var hash1 = pic.GetHashCode();
                var hash2 = sceneImage.Source.GetHashCode();
                pictureId = scenePicture.Id;
                vm.InsertData(scenePicture, typeof(ScenePicture));
            }
            //Saving the Scene
            scene.PictureId = pictureId;
            vm.InsertData(scene, typeof(Scene));

            //Insert into LessonToActivity
            var lessonToActivity = new LessonToActivity() { LessonId = _lesson.Id, SceneId = scene.Id };
            vm.InsertData(lessonToActivity, typeof(LessonToActivity));

            //Saving the SceneItems and Phrase
            foreach (SceneItem sceneItem in MyPoints)
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_selectedDot != null)
            {
                bool changed = false;
                if (e.Key == Key.Down)
                {
                    var oldThickness = _selectedDot.Margin;
                    _selectedDot.Margin = new Thickness(oldThickness.Left, oldThickness.Top + 1, 0, 0);
                    changed = true;
                }
                if (e.Key == Key.Up)
                {
                    var oldThickness = _selectedDot.Margin;
                    _selectedDot.Margin = new Thickness(oldThickness.Left, oldThickness.Top - 1, 0, 0);
                    changed = true;
                }
                if (e.Key == Key.Right)
                {
                    var oldThickness = _selectedDot.Margin;
                    _selectedDot.Margin = new Thickness(oldThickness.Left+1, oldThickness.Top, 0, 0);
                    changed = true;
                }
                if (e.Key == Key.Left)
                {
                    var oldThickness = _selectedDot.Margin;
                    _selectedDot.Margin = new Thickness(oldThickness.Left-1, oldThickness.Top, 0, 0);
                    changed = true;
                }
                if (changed)
                {
                    var width = sceneImage.ActualWidth;
                    var height = sceneImage.ActualHeight;
                    SceneItem point = _selectedDot.Tag as SceneItem;
                    point.XPos = ((_selectedDot.Margin.Left + (_selectedDot.Width / 2))*100)/width;
                    point.YPos = ((_selectedDot.Margin.Top + (_selectedDot.Height / 2)) * 100) / height;
                    listPoints.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
                        
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var scenes = _lesson.Scenes;
        }


    }
}
