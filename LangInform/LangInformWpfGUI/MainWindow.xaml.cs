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
using System.Windows.Media.Animation;

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

        TabItem activeActivityTab;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("..\\..\\..\\");
            FileInfo databaseFile = dir.GetFiles().FirstOrDefault(f => f.Name == "LangData.3db");
            vm = new ViewModel(databaseFile.FullName);
            treeLessons.ItemsSource = vm.Languages;
            //temporary
            var lesson = vm.GetData<Lesson>("SELECT * FROM Lesson WHERE Name='Lesson 2'").FirstOrDefault();
            //AddScene addScene = new AddScene(lesson);
            //addScene.ShowDialog();
            //end temporary
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            var parent = ((ContextMenu)item.Parent).PlacementTarget as TextBlock;
            StopEverythingAndSetToDefaults();
            if (item.Header.ToString() == "Add Language")
            {
                string newLanguageName = MetroInputBox.Show(this, "Please enter new Language name", "");
                if (!string.IsNullOrEmpty(newLanguageName))
                {
                    string description = MetroInputBox.Show(this, "Description of the new language", "");
                    vm.InsertLanguage(new Language() { Name = newLanguageName, Description = description });
                }
            }
            else if (item.Header.ToString() == "Delete this Language")
            {
                MessageResult result = MetroMessage.Show(this, "Deleting language", "Are you sure you want to delete language \"" + parent.Text + "\"?", MessageButtons.YesNo, MessageIcon.Question);
                if (result == MessageResult.Yes)
                {
                    var obj = parent.DataContext;
                    vm.DeleteData(obj);
                    vm.IsLanguagesDirty = true;
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
                        language.InsertLevel(level);
                    }
                }
            }
            else if (item.Header.ToString() == "Delete this Level")
            {
                MessageResult result = MetroMessage.Show(this, "Deleting level", "Are you sure you want to delete level \"" + parent.Text + "\"?", MessageButtons.YesNo, MessageIcon.Question);
                if (result == MessageResult.Yes)
                {
                    var obj = parent.DataContext as Level;
                    vm.DeleteData(obj);
                    obj.Language.IsLevelsDirty = true;
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
            treeLessons.ItemsSource = vm.Languages;
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

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == e.OriginalSource)
                e.Handled = true;
            TabItem tab = e.AddedItems[0] as TabItem;
            activeActivityTab = tab;
            if (tab.Header.ToString() == "Scene")
            {

            }
            else
            {
                //StopEverythingAndSetToDefaults();
                SceneDefaults();
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

        /// <summary>
        /// Sets active activity to default
        /// </summary>
        private void StopEverythingAndSetToDefaults()
        {
            if (activeActivityTab != null)
            {
                if (activeActivityTab.Header.ToString() == "Scene")
                {
                    StopPlayAll(playAllTimer, currentSceneDot);
                    sceneLearn.IsChecked = true;
                }
            }
        }

        public TReturn RunThis<TReturn>(Func<TReturn> function, int runAfter)
        {
            TReturn output = default(TReturn);
            if (runAfter < 0) runAfter = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(runAfter);
            timer.Tick += new EventHandler((s, e) => {
                timer.Stop();
                output = function.Invoke();
            });
            timer.Start();
            return output;
        }

        public void RunThis(Action action, int runAfter)
        { 
            if (runAfter < 0) runAfter = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(runAfter);
            timer.Tick += new EventHandler((s, e) => {
                timer.Stop();
                action.Invoke();
            });
            timer.Start();
        }


        #region Scene stuff

        TabItem activeSceneTab;
        DispatcherTimer playAllTimer;
        Border currentSceneDot;
        SceneActivity sceneActivity = SceneActivity.Learn;
        List<KeyValuePair<Border, PracticeResult<SceneItem>>> practiceItems;
        Grid SceneGrdFront;
        private void SceneDefaults()
        {
            StopPlayAll(playAllTimer, currentSceneDot);
            sceneLearn.IsChecked = true;
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
                var menuItem = new MenuItem() { Header = "Edit scene" };
                menuItem.Click += new RoutedEventHandler((s, e) =>
                {
                    sceneImage.Source = null;
                    EditScene(scene);
                });
                sceneTab.ContextMenu = new System.Windows.Controls.ContextMenu();
                sceneTab.ContextMenu.Items.Add(menuItem); 
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
                        dot.MouseLeftButtonDown += dot_Click;
                        dot.Width = sceneItem.Size * height / 100;
                        dot.Height = sceneItem.Size * height / 100;
                        double x = ((sceneItem.XPos * width) / 100) - (dot.Width / 2);
                        double y = ((sceneItem.YPos * height) / 100) - (dot.Height / 2);
                        dot.Margin = new Thickness((sceneItem.XPos * width / 100) - dot.Width / 2, (sceneItem.YPos * height / 100) - dot.Height / 2, 0, 0);
                        dot.Background = new SolidColorBrush(Colors.Green);
                        dot.Opacity = .4;
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
                sceneFront.Name = "grdFront";
                sceneFront.SetBinding(Grid.HeightProperty, new Binding("ActualHeight") { Mode = BindingMode.OneWay });
                sceneFront.SetBinding(Grid.WidthProperty, new Binding("ActualWidth") { Mode = BindingMode.OneWay });
                sceneBack.Children.Add(sceneImage);
                sceneBack.Children.Add(sceneFront);
                sceneTab.Content = sceneBack;
                scenesTab.Items.Add(sceneTab);
            }
        }

        void EditScene(Scene scene)
        {
            AddScene addScene = new AddScene(scene.Lesson, scene.Id);
            addScene.ShowDialog();
        }

        private void SceneActivityChange()
        {
            try
            {
                Scene scene = activeSceneTab.Tag as Scene;
                var grdBack = activeSceneTab.Content as Grid;
                SceneGrdFront = grdBack.Children[1] as Grid;

                StopPlayAll(playAllTimer, currentSceneDot);
                if (sceneActivity == SceneActivity.PlayAll)
                {

                    List<Border> borders = new List<Border>();
                    foreach (var item in SceneGrdFront.Children)
                    {
                        var border = item as Border;
                        if (border != null)
                            borders.Add(border);
                    }
                    PlayAll(borders);
                }
                else if (sceneActivity == SceneActivity.Practice)
                {
                    StartPractice();
                }
            }
            catch
            {
                //will fail before all the controls loaded
            }
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
            SceneActivityChange();
        }

        private void dot_Click(object sender, MouseButtonEventArgs e)
        {
            var dot = sender as Border;
            SceneItem sceneItem = dot.Tag as SceneItem;
            if (sceneActivity == SceneActivity.Learn)
            {
                SetSceneDotToDefault(currentSceneDot, -1, true);
                currentSceneDot = dot;
                HighlightTheSceneDot(dot, -1, null, true);
            }
            else if (sceneActivity == SceneActivity.Practice)
            {
                ContinuePractice(dot);
            }
        }

        private void ContinuePractice(Border selectedDot)
        {
            var currentItem = practiceItems.FirstOrDefault(i => i.Value.Status == PracticeStatus.Asking);
            currentItem.Value.Item.Phrase.StopPlaying();
            KeyValuePair<Border, PracticeResult<SceneItem>> nextPlayingItem;
            if (currentItem.Key != selectedDot)
            {
                HighlightTheSceneDot(selectedDot, 1000, new SolidColorBrush(Colors.Red));
                currentItem.Value.WrongAnswersCount++;
                if (currentItem.Value.WrongAnswersCount == 3)
                {
                    currentItem.Value.WrongAnswersCount = 0;
                    HighlightTheSceneDot(currentItem.Key, -1, new SolidColorBrush(Colors.Yellow), true, 1000);
                    return;
                }
                nextPlayingItem = currentItem;
            }
            else
            {
                HighlightTheSceneDot(selectedDot, 1000, new SolidColorBrush(Colors.Green));
                currentItem.Value.Status = PracticeStatus.Asked;
                nextPlayingItem = practiceItems.FirstOrDefault(i => i.Value.Status == PracticeStatus.NotAsked);
                if (nextPlayingItem.Value == null)
                {
                    Action action1 = new Action(() =>
                    {
                        MessageResult result = MetroMessage.Show(this, "practice finished", "Do you want to redo the practice?", MessageButtons.YesNo, MessageIcon.Question);
                        if (result == MessageResult.Yes)
                        {
                            StartPractice();
                        }
                        else
                        {
                            StopEverythingAndSetToDefaults();
                        }
                    });
                    RunThis(action1, (int)currentItem.Value.Item.Phrase.SoundLength.TotalMilliseconds);
                    return;
                }
            }
            Action action2 = new Action(() => {
                nextPlayingItem.Value.Status = PracticeStatus.Asking;
                nextPlayingItem.Value.Item.Phrase.Play();
            });
            RunThis(action2, 1000);
            
        }

        private void StartPractice()
        {
            Dictionary<Border, PracticeResult<SceneItem>> borders = new Dictionary<Border, PracticeResult<SceneItem>>();
            foreach (var item in SceneGrdFront.Children)
            {
                var border = item as Border;
                if (border != null)
                    borders.Add(border, new PracticeResult<SceneItem>(border.Tag as SceneItem));
            }
            practiceItems = MixItems<KeyValuePair<Border, PracticeResult<SceneItem>>>(borders.ToList());
            if (practiceItems.Count >= 1)
            {
                var firstItem = practiceItems[0];
                firstItem.Value.Status = PracticeStatus.Asking;
                firstItem.Value.Item.Phrase.Play();
            }
        }

        private void scenesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == e.OriginalSource)
                e.Handled = true;
            activeSceneTab = e.AddedItems[0] as TabItem;
        }



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
            var a = MetroInputBox.Show(this, "title", "[this is test]");
        }

        private void PlayAll(List<Border> items)
        {
            var sortedItems = items;
            int counter = 0;
            sortedItems = MixItems<Border>(items);
            playAllTimer = new DispatcherTimer();
            playAllTimer.Interval = TimeSpan.FromMilliseconds(500);
            playAllTimer.Tick += new EventHandler((s, e) =>
            {
                if (counter == sortedItems.Count - 1)
                {
                    if (!loop.IsChecked)
                    {
                        playAllTimer.Stop();
                        return;
                    }

                    if (playRandomly.IsChecked)
                        sortedItems = MixItems<Border>(items);
                    else
                        sortedItems = items;
                    counter = 0;
                }
                var sceneItem = sortedItems[counter].Tag as SceneItem;
                currentSceneDot = sortedItems[counter];
                sceneItem.Phrase.Play();
                if (showPlaying.IsChecked)
                {
                    HighlightTheSceneDot(sortedItems[counter]);
                }
                playAllTimer.Interval = sceneItem.Phrase.SoundLength + TimeSpan.FromMilliseconds(500);
                SetSceneDotToDefault(sortedItems[counter], Convert.ToInt32((sceneItem.Phrase.SoundLength + TimeSpan.FromMilliseconds(400)).TotalMilliseconds));
                counter++;

            });
            playAllTimer.Start();

        }

        private void SetSceneDotToDefault(Border dot, int after = -1, bool stopPlaying = false)
        {
            if (dot == null) return;
            if (after <= -1)
            {
                after = 1;
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(after);
            timer.Tick += new EventHandler((s, e) =>
            {
                timer.Stop();
                if (stopPlaying)
                {
                    SceneItem sceneItem = dot.Tag as SceneItem;
                    sceneItem.Phrase.StopPlaying();
                }
                dot.BeginAnimation(Border.OpacityProperty, null);
                dot.Opacity = 0.4;
                dot.Background = new SolidColorBrush(Colors.Green);
            });
            timer.Start();

        }

        private void StopPlayAll(DispatcherTimer playAllTimer, Border dot)
        {
            if (playAllTimer == null || dot == null) return;
            playAllTimer.Stop();
            var sceneItem = dot.Tag as SceneItem;
            SetSceneDotToDefault(dot, -1, true);
            //sceneItem.Phrase.StopPlaying();
            //dot.BeginAnimation(Border.OpacityProperty, null);
            //dot.Background = new SolidColorBrush(Colors.Green);
            //dot.Opacity = .4;
        }

        /// <summary>
        /// Highlights the dot for specific time
        /// </summary>
        /// <param name="dot">Dot</param>
        /// <param name="length">Length of the highlighting</param>
        private void HighlightTheSceneDot(Border dot, int length = -1, SolidColorBrush color = null, bool shouldPlay = false, int playAfter = -1)
        {
            if (dot == null || !(dot.Tag is SceneItem))
                return;
            if (color == null)
            {
                color = new SolidColorBrush(Colors.Red);
            }
            SceneItem item = dot.Tag as SceneItem;
            if (length == -1)
                length = (int)item.Phrase.SoundLength.TotalMilliseconds;
            DispatcherTimer timer = new DispatcherTimer();
            if (playAfter == -1) playAfter = 1;
            timer.Interval = TimeSpan.FromMilliseconds(playAfter);
            timer.Tick += new EventHandler((s, e) =>
            {
                timer.Stop();
                if (shouldPlay)
                {
                    item.Phrase.Play();
                }
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0;
                animation.To = 1;
                animation.Duration = TimeSpan.FromMilliseconds(700);
                animation.AutoReverse = true;
                animation.RepeatBehavior = RepeatBehavior.Forever;
                dot.Background = color;
                dot.BeginAnimation(Border.OpacityProperty, animation);
                SetSceneDotToDefault(dot, length);
            });
            timer.Start();
        }


        #endregion



    }

    public enum PracticeStatus
    {
        NotAsked = 0,
        Asking = 1,
        Asked = 2
    }

    public class PracticeResult<TItemType>
    {
        public PracticeResult(TItemType item)
        {
            Item = item;
            Status = PracticeStatus.NotAsked;
        }

        public TItemType Item { get; private set; }

        public PracticeStatus Status
        {
            get;
            set;
        }

        public int WrongAnswersCount { get; set; }

        public void Reset()
        {
            Status = PracticeStatus.NotAsked;
            WrongAnswersCount = 0;
        }
    }

    public enum SceneActivity
    {
        Learn = 0,
        Practice = 1,
        PlayAll = 2
    }
}