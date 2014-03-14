using LangInformGUI.Controls;
using LangInformGUI.Windows;
using LangInformModel;
using LangInformVM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        Border sampleDot;
        Image img;
        Scene scene;
        Lesson _lesson;
        BaseViewModel vm = MainWindow.vm;
        Border newCreatedDot;
        Border _selectedDot;
        bool alreadyExist;
        public ObservableCollection<SceneItem> SceneItems { get; set; }
        
        public AddScene(Lesson lesson)
        {
            InitializeComponent();
            _lesson = lesson;
            SceneItems = new ObservableCollection<SceneItem>();
        }

        public AddScene(Scene existingScene, Image image)
        {
            InitializeComponent();
            _lesson = existingScene.Lesson;
            sceneImage.Source = image.Source;
            scene = existingScene;
            SceneItems = new ObservableCollection<SceneItem>(existingScene.SceneItems.OrderBy(s=>s.Order).ToList());
            sceneImage.Tag = existingScene.ScenePicture;
            alreadyExist = true;
        }
        

        /// <summary>
        /// Gets the file path and shows the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtPath.Text = GetFilePath();
            if (txtPath.Text == "")
                return;
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
            if (string.IsNullOrEmpty(fileDialog.FileName))
                return "";
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
            if (txtPath.Text == "")
                return;
            FileInfo file = new FileInfo(txtPath.Text);
            if (file.Exists)
            {
                var image = Assistant.GetBitmapImageFrom(file.FullName);
                sceneImage.Source = image;
                sceneImage.Tag = null;
                MetroMessage.Show(this, "add scene", "Please check the pictures from the Database for not to saving the same picture in the database twice.", MessageButtons.OK, MessageIcon.Information);
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
            sampleDot = (Border)sender;
            DeSelectAll();
            sampleDot.BorderThickness = new Thickness(2, 2, 2, 2);
            //CreateNewDot(selectedBorder.Height, selectedBorder.CornerRadius.BottomLeft);
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
            var point = SceneItems.FirstOrDefault(p => p.ToString() == border.Tag.ToString());
            SceneItems.Remove(point);
            ModelManager.Db.Delete(point);
            grdPoints.Children.Remove(border);
            EnumerateTheDots();
        }

        void PlayAudio_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            var border = menu.Tag as Border;
            var point = border.Tag as SceneItem;
            if (point.Phrase != null)
            {
                StopAllPlaying(SceneItems.ToList());
                point.Phrase.Play();
            }
        }

        void ResetTheDotNumber_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            var dot = menu.Tag as Border;
            var point = dot.Tag as SceneItem;
            int oldIndex = SceneItems.IndexOf(point);
            string number = MetroInputBox.Show(this, "resetting the dot numbers", "Please enter number you want to set");
            if (!string.IsNullOrEmpty(number))
            {
                int newIndex = Convert.ToInt32(number);
                if (newIndex >= 1 && newIndex <= SceneItems.Count)
                {
                    SceneItems.Move(oldIndex, newIndex - 1);
                    grdPoints.Children.Remove(dot);
                    grdPoints.Children.Insert(newIndex - 1, dot);
                    EnumerateTheDots();
                }
            }
            else
            {
                MetroMessage.Show(this, "resetting the dot numbers", "Please enter integer numbers between 0 and " + SceneItems.Count + ".", MessageButtons.OK, MessageIcon.Exclamation);
            }
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
                dot.Height = (point.Size * height) / 100;
                dot.Width = (point.Size * height) / 100;
                if (dot.Height < 10) dot.Height = 10;
                if (dot.Width < 10) dot.Width = 10;
                dot.Margin = new Thickness((point.XPos * width / 100) - dot.Width / 2, (point.YPos * height / 100) - dot.Height / 2, 0, 0);
            }
        }

        private void grdLayer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sampleDot == null)
            {
                MetroMessage.Show(this, "add scene", "Please select one of the samples below!");
                return;
            }
            System.Windows.Point p = Mouse.GetPosition(grdLayer);
            var sceneItem = CreateSceneItem(p.X, p.Y, sampleDot.Height, (sampleDot.CornerRadius.BottomLeft == 0 ? false : true));
            CreateNewDotAndPlace(sceneItem);
            SceneItems.Add(sceneItem);

            EnumerateTheDots();
            //CreateNewDot(newCreatedDot.Height, newCreatedDot.CornerRadius.BottomLeft);//!!!This method needs to be reviewed
        }

        SceneItem CreateSceneItem(double xPos, double yPos, double size, bool isRound)
        {
            var width = sceneImage.ActualWidth;
            var height = sceneImage.ActualHeight;
            SceneItem point = new SceneItem()
            {
                XPos = (xPos * 100) / width,
                YPos = (yPos * 100) / height,
                Id = Guid.NewGuid(),
                Size = (size*100)/height,
                IsRound = isRound
            };

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
            return point;
        }

        public void CreateNewDotAndPlace(SceneItem sceneItem)
        {
            var width = sceneImage.ActualWidth;
            var height = sceneImage.ActualHeight;

            Border border = new Border()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                Height = (sceneItem.Size * height) / 100,
                Width = (sceneItem.Size*height)/100,
                CornerRadius = new CornerRadius((sceneItem.IsRound ? 90 : 0)),
                Background = new SolidColorBrush(Colors.Green),
                Opacity = .4,
                VerticalAlignment = VerticalAlignment.Top,
                Tag = Guid.NewGuid()
            };

            border.MouseLeftButtonDown += border_MouseLeftButtonDown;
            System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();

            MenuItem item1 = new MenuItem() { Header = "Change sound file", Tag = border };
            item1.Click += ChangeSoundFile_Click;
            menu.Items.Add(item1);

            MenuItem item2 = new MenuItem() { Header = "Delete point", Tag = border };
            item2.Click += DeleteDot_Click;
            menu.Items.Add(item2);

            MenuItem item3 = new MenuItem() { Header = "Play", Tag = border };
            item3.Click += PlayAudio_Click;
            menu.Items.Add(item3);

            MenuItem item4 = new MenuItem() { Header = "Reset the order", Tag = border };
            item4.Click += ResetTheDotNumber_Click;
            menu.Items.Add(item4);
            border.ContextMenu = menu;

            border.Margin = new Thickness((sceneItem.XPos * width / 100) - border.Width / 2, (sceneItem.YPos * height / 100) - border.Height / 2, 0, 0);

            border.Tag = sceneItem;
            //add it to grid
            grdPoints.Children.Add(border);
            //Heighlight the border
            SelectBorder(border);
            if(sceneItem.Phrase==null)
            {
                border.Background = new SolidColorBrush(Colors.Yellow);
            }
        }


        private void EnumerateTheDots()
        {
            int counter = 0;
            foreach (var point in grdPoints.Children)
            {
                counter++;
                var dot = point as Border;
                var sceneItem = dot.Tag as SceneItem;
                sceneItem.Order = counter;
                var textBlock = new TextBlock() { FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                textBlock.DataContext = sceneItem;
                textBlock.SetBinding(TextBlock.TextProperty, new Binding("Order"));
                dot.Child = textBlock;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listPoints.DataContext = this;
            listPoints.SetBinding(ListBox.ItemsSourceProperty, new Binding("SceneItems"));
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

        void StopAllPlaying(List<SceneItem> items)
        {
            foreach (var item in items)
            {
                item.Phrase.StopPlaying();
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
            if (!alreadyExist)
            {
                while (true)
                {
                    sceneName = MetroInputBox.Show(this, "add scene", "Please enter scene name");
                    if (string.IsNullOrEmpty(sceneName))
                    {
                        var res = MetroMessage.Show(this, "add acene", "Scene name cannot be empty. Do you want to enter?", MessageButtons.YesNo, MessageIcon.Exclamation);
                        if (res == MessageResult.No) return;
                    }
                    if (sceneName != "")
                        break;
                }
            }
            foreach (SceneItem point in SceneItems)
            {
                if (point.Phrase == null)
                {
                    MetroMessage.Show(this, "Not completed points", "Some created point(s) doesn't have the sound file attached yet. They are marked yellow. Please attach sound files before you save it.");
                    return;
                }
            }
            //Saving the Scene Image
            Guid pictureId;
            ScenePicture scenePicture;
            if ((scenePicture = sceneImage.Tag as ScenePicture) != null)
            {
                pictureId = scenePicture.Id;
            }
            else
            {
                var pic = Assistant.BitmapImageToByte(sceneImage.Source as BitmapImage);
                scenePicture = new ScenePicture() { Id = Guid.NewGuid(), Picture = pic };
                pictureId = scenePicture.Id;
                vm.InsertData(scenePicture, typeof(ScenePicture));
            }
            //Saving the Scene
            if (scene == null)
            {
                scene = new Scene() { Id = Guid.NewGuid(), Name = sceneName };
                scene.PictureId = pictureId;
                vm.InsertData(scene, typeof(Scene));
            }
            

            //Insert into LessonToActivity
            if (!alreadyExist)
            {
                var lessonToActivity = new LessonToActivity() { LessonId = _lesson.Id, SceneId = scene.Id };
                vm.InsertData(lessonToActivity, typeof(LessonToActivity));
            }
            //Saving the SceneItems and Phrase
            foreach (SceneItem sceneItem in SceneItems)
            {
                if (!sceneItem.AlreadyInDb)
                {
                    Phrase phrase = sceneItem.Phrase;
                    vm.InsertData(phrase, typeof(Phrase));
                    sceneItem.PhraseId = phrase.Id;
                    sceneItem.SceneId = scene.Id;
                    vm.InsertData(sceneItem, typeof(SceneItem));
                }
                else
                {
                    if (sceneItem.HasChanges)
                    {
                        ModelManager.Db.Update(sceneItem);
                        if (sceneItem.Phrase.HasChanges)
                        {
                            ModelManager.Db.Update(sceneItem.Phrase);
                        }
                    }
                }
            }
            var result = MetroMessage.Show(this, "saving the scene", "Saving the scene sucessfully finished. Do you want to close this form?", MessageButtons.YesNo, MessageIcon.Question);
            if (result == MessageResult.Yes)
            {
                this.Close();
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
                    _selectedDot.Margin = new Thickness(oldThickness.Left + 1, oldThickness.Top, 0, 0);
                    changed = true;
                }
                if (e.Key == Key.Left)
                {
                    var oldThickness = _selectedDot.Margin;
                    _selectedDot.Margin = new Thickness(oldThickness.Left - 1, oldThickness.Top, 0, 0);
                    changed = true;
                }
                if (changed)
                {
                    var width = sceneImage.ActualWidth;
                    var height = sceneImage.ActualHeight;
                    SceneItem point = _selectedDot.Tag as SceneItem;
                    point.XPos = ((_selectedDot.Margin.Left + (_selectedDot.Width / 2)) * 100) / width;
                    point.YPos = ((_selectedDot.Margin.Top + (_selectedDot.Height / 2)) * 100) / height;
                    listPoints.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var scenes = _lesson.Scenes;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void sceneImage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (SceneItem item in SceneItems)
            {
                CreateNewDotAndPlace(item);
            }
            EnumerateTheDots();
        }
    }
}

