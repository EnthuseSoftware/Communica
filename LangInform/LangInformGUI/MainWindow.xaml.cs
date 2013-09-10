using LangInformModel;
using LangInformVM;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Media;
using LangInformGUI.Controls;
using System.IO;
using System.Windows.Data;

namespace LangInformGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            random = new Random(DateTime.Now.Millisecond);
        }

        Random random;

        public static ViewModel vm;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("..\\..\\..\\");
            FileInfo databaseFile = dir.GetFiles().FirstOrDefault(f => f.Name == "LangData.3db");
            vm = new ViewModel(databaseFile.FullName);
            treeLessons.ItemsSource = vm.Languages;
            //temporary
            var lesson = vm.GetData<Lesson>("SELECT * FROM Lesson WHERE Name='Lesson 2'").FirstOrDefault();
            AddScene addScene = new AddScene(lesson);
            addScene.ShowDialog();
            //end temporary
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            var parent = ((ContextMenu)item.Parent).PlacementTarget as TextBlock;
            if (item.Header.ToString() == "Add Language")
            {
                string newLanguageName = MetroInputBox.Show(this, "Please enter new Language name", "");
                if (!string.IsNullOrEmpty(newLanguageName))
                {
                    string description = MetroInputBox.Show(this, "Description of the new language", "");
                    vm.InsertData(new Language() { Name = newLanguageName, Description = description }, typeof(Language));
                    vm.IsLanguagesDirty = true;
                }
            }
            else if (item.Header.ToString() == "Delete this Language")
            {
                MessageResult result = MetroMessage.Show(this, "Deleting language", "Are you sure you want to delete language \"" + parent.Text + "\"?", MessageButtons.YesNo, MessageIcon.Question);
                if (result == MessageResult.Yes)
                {
                    var obj = parent.DataContext;
                    vm.DeleteData(obj);
                }
            }
            else if (item.Header.ToString() == "Add Level")
            {
                string newLevelName = MetroInputBox.Show(this, "Please enter new Level name", "");
                if (!string.IsNullOrEmpty(newLevelName))
                {
                    Language language = parent.DataContext as Language;
                    if (language != null)
                    {
                        string description = MetroInputBox.Show(this, "Description of the new level", "");
                        var level = new Level() { ID = Guid.NewGuid(), Name = newLevelName, Description = description };
                        vm.InsertData(level, typeof(Level));
                        vm.InsertData(new LanguageToLevel() { LanguageId = language.Id, LevelId = level.ID }, typeof(LanguageToLevel));
                    }
                }
            }
            else if (item.Header.ToString() == "Add Unit")
            {
                string newUnitName = MetroInputBox.Show(this, "Please enter new Unit name", "");
                if (!string.IsNullOrEmpty(newUnitName))
                {
                    Level level = parent.DataContext as Level;
                    if (level != null)
                    {
                        string description = MetroInputBox.Show(this, "Description of the new unit", "");
                        var unit = new Unit() { Id = Guid.NewGuid(), Name = newUnitName, Description = description };
                        vm.InsertData(unit, typeof(Unit));
                        vm.InsertData(new LevelToUnit() { LevelId = level.ID, UnitId = unit.Id }, typeof(LevelToUnit));
                    }
                }
            }
            else if (item.Header.ToString() == "Add Lesson")
            {
                string newUnitName = MetroInputBox.Show(this, "Please enter new Lesson name", "");
                if (!string.IsNullOrEmpty(newUnitName))
                {
                    Unit unit = parent.DataContext as Unit;
                    if (unit != null)
                    {
                        string description = MetroInputBox.Show(this, "Description of the new lesson", "");
                        var lesson = new Lesson() { Id = Guid.NewGuid(), Name = newUnitName, Description = description };
                        vm.InsertData(lesson, typeof(Lesson));
                        vm.InsertData(new UnitToLesson() { UnitId = unit.Id, LessonId = lesson.Id }, typeof(UnitToLesson));
                    }
                }
            }
            else if (item.Header.ToString() == "Add Scene")
            {

                AddScene addScene = new AddScene(parent.DataContext as Lesson);
                addScene.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please be patient! This feature is not implemented yet. Thanks!");
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.CloseSession();
        }

        private void treeLessons_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Lesson)
            {
                Lesson selectedLesson = e.NewValue as Lesson;
                CreateSceneTabs(selectedLesson);
            }
        }

        private void CreateSceneTabs(Lesson lesson)
        {
            TabControl scenesTab = new TabControl();
            grdScene.Children.Add(scenesTab);
            scenesTab.SelectionChanged += scenesTab_SelectionChanged;
            foreach (Scene scene in lesson.Scenes)
            {
                TabItem sceneTab = new TabItem() { Header = scene.Name };
                sceneTab.DataContext = scene;
                sceneTab.Tag = scene;
                //grid that will contain the image and the grid that contains points.
                Grid sceneBack = new Grid();

                Image sceneImage = new Image() { Source = Assistant.ByteToBitmapSource(scene.ScenePicture.Picture), DataContext = scene };
                sceneImage.Loaded += new RoutedEventHandler((s, args) =>
                {
                    Image image = s as Image;
                    var width = image.ActualWidth;
                    var height = image.ActualHeight;

                    var grid = image.Tag as Grid;
                    //add the points
                    foreach (SceneItem sceneItem in scene.SceneItems)
                    {
                        Border dot = new Border() { CornerRadius = new CornerRadius((sceneItem.IsRound ? 90 : 0)), VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left };
                        dot.Tag = sceneItem;
                        dot.MouseLeftButtonDown += dot_MouseLeftButtonDown;
                        dot.Width = sceneItem.Size * height / 100;
                        dot.Height = sceneItem.Size * height / 100;
                        double x = ((sceneItem.XPos * width) / 100) - (dot.Width / 2);
                        double y = ((sceneItem.YPos * height) / 100) - (dot.Height / 2);
                        dot.Margin = new Thickness((sceneItem.XPos * width / 100) - dot.Width / 2, (sceneItem.YPos * height / 100) - dot.Height / 2, 0, 0);
                        dot.Background = new SolidColorBrush(Colors.Green);
                        dot.Opacity = .5;
                        grid.Children.Add(dot);
                    }
                });
                //grid that will contain the actual points
                Grid sceneFront = new Grid();
                sceneFront.Tag = sceneImage;
                sceneFront.SizeChanged += new SizeChangedEventHandler((s, args) =>
                {
                    var grd = s as Grid;
                    MoveTheDots(grd, grd.Tag as Image);
                });
                sceneImage.Tag = sceneFront;
                //binding the front grid height and width with the image height and width
                sceneFront.DataContext = sceneImage;
                sceneFront.SetBinding(Grid.HeightProperty, new Binding("ActualHeight") { Mode = BindingMode.OneWay });
                sceneFront.SetBinding(Grid.WidthProperty, new Binding("ActualWidth") { Mode = BindingMode.OneWay });
                sceneBack.Children.Add(sceneImage);
                sceneBack.Children.Add(sceneFront);
                sceneTab.Content = sceneBack;
                scenesTab.Items.Add(sceneTab);
            }
        }

        TabItem activeSceneTab;

        void scenesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeSceneTab = e.AddedItems as TabItem;
        }

        void dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SceneItem sceneItem = (sender as Border).Tag as SceneItem;
            sceneItem.Phrase.Play();
        }

        /// <summary>
        /// Moving the dots when picture resized
        /// </summary>
        private void MoveTheDots(Grid grd, Image img)
        {
            var width = img.ActualWidth;
            var height = img.ActualHeight;
            foreach (var child in grd.Children)
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

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        SceneActivity sceneActivity = SceneActivity.Learn;

        void SceneActivityChange()
        {
            if (sceneActivity == SceneActivity.PlayAll)
            {
                Scene scene = activeSceneTab.Tag as Scene;
                isPlayAll = true;
                PlayAll(scene.SceneItems.ToList());
            }
        }

        bool isPlayAll = false;

        void PlayAll(List<SceneItem> items)
        {
            var sortedItems = items;
            if (playRandomly.IsChecked == true)
            {
                sortedItems = MixItems<SceneItem>(items);
            }

        }

        private List<T> MixItems<T>(List<T> items)
        {
            List<T> _items = new List<T>();
            List<int> randomNumbers = new List<int>();
            for (int i = 1; i <= items.Count; i++)
            {
                int rnd;
                while (true)
                {
                    rnd = random.Next(0, items.Count);
                    if (!randomNumbers.Contains(rnd))
                    {
                        randomNumbers.Add(rnd);
                        break;
                    }
                }
                _items.Add(items[rnd]);
            }
            return _items;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sceneLearn.IsChecked == true)
            {
                sceneActivity = SceneActivity.Learn;
            }
            else if (scenePractice.IsChecked == true)
            {
                sceneActivity = SceneActivity.Practice;
            }
            else if (scenePlayAll.IsChecked == true)
            {
                sceneActivity = SceneActivity.PlayAll;
            }
        }
    }

    public enum SceneActivity
    {
        Learn = 0,
        Practice = 1,
        PlayAll = 2
    }
}
