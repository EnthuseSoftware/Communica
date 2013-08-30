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

        }

        public static ViewModel vm;

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

            foreach (Scene scene in lesson.Scenes)
            {
                TabItem sceneTab = new TabItem() { Header = scene.Name };
                sceneTab.DataContext = scene;
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
                        Border dot = new Border() { CornerRadius = new CornerRadius((sceneItem.IsRound ? 90 : 0)) };
                        dot.Width = sceneItem.Size;
                        dot.Height = sceneItem.Size;
                        double x = ((sceneItem.XPos * width) / 100);// -(dot.Width / 2);
                        double y = ((sceneItem.YPos * height) / 100);// -(dot.Height / 2);
                        dot.Margin = new Thickness(x,y , 0, 0);
                        dot.Background = new SolidColorBrush(Colors.Red);
                        dot.Opacity = 1;
                        grid.Children.Add(dot);
                    }
                });
                //grid that will contain the actual points
                Grid sceneFront = new Grid();
                sceneImage.Tag = sceneFront;
                //binding the front grid height and width with the image height and width
                sceneFront.DataContext = sceneImage;
                sceneFront.SetBinding(Grid.HeightProperty, new Binding("ActualHeight") { Mode = BindingMode.OneWay });
                sceneFront.SetBinding(Grid.WidthProperty, new Binding("ActualWidth") { Mode = BindingMode.OneWay });
                sceneFront.Background = new SolidColorBrush(Colors.Blue);
                sceneFront.Opacity = .2;

                sceneBack.Children.Add(sceneImage);
                sceneBack.Children.Add(sceneFront);
                sceneTab.Content = sceneBack;
                scenesTab.Items.Add(sceneTab);


            }
        }







        //ViewModel vm;// = new ViewModel();
        //MainEntities entities;// = new MainEntities();

        //VocabularyActivity activity = VocabularyActivity.Learn;
        //Vocabulary activeVocab;
        //UniformGrid activeVocabContainer;

        //SoundPlayer rightSound;
        //SoundPlayer wrongSound;
        //Converter converter = new Converter();

        //private void Window_Loaded_1(object sender, RoutedEventArgs e)
        //{
        //    vm.CurrentLessonChanged += logic_LessonChanged;
        //    rightSound = new SoundPlayer(Properties.Resources.right);
        //    wrongSound = new SoundPlayer(Properties.Resources.wrong5);
        //    this.DataContext = vm;
        //    treeLessons.ItemsSource = vm.Languages;

        //    //btnLoad.Visibility = System.Windows.Visibility.Hidden;
        //    //btnTest.Visibility = System.Windows.Visibility.Hidden;
        //    //Window1 w1 = new Window1(vm);
        //    //w1.ShowDialog();
        //}

        //private void btnLoad_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void treeLessons_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    if (treeLessons.SelectedItem is Language)
        //    {
        //        vm.CurrentLanguage = treeLessons.SelectedItem as Language;
        //        lblMap.Content = vm.CurrentLanguage.Name;
        //    }
        //    else if (treeLessons.SelectedItem is Level)
        //    {
        //        vm.CurrentLevel = treeLessons.SelectedItem as Level;
        //    }
        //    else if (treeLessons.SelectedItem is Unit)
        //    {
        //        vm.CurrentUnit = treeLessons.SelectedItem as Unit;
        //    }
        //    else if (treeLessons.SelectedItem is Lesson)
        //    {
        //        vm.CurrentLesson = treeLessons.SelectedItem as Lesson;
        //    }
        //}

        //private void CreateSceneTab(Lesson lesson)
        //{
        //    //grdScene.Children.Clear();
        //    //TabControl myScenesTab = new TabControl();
        //    //foreach (Scene scene in lesson.Scenes)
        //    //{
        //    //    TabItem tabItem = new TabItem();
        //    //    tabItem.Header = scene.Name;

        //    //    Image image = converter.ByteToWPFImage(scene.Picture);
        //    //    tabItem.Content = image;
        //    //    myScenesTab.Items.Add(tabItem);
        //    //}
        //    //grdScene.Children.Add(myScenesTab);
        //}

        //private void CreateVocabularyTab(Lesson lesson, bool learn = false)
        //{
        //    //grdVocabulary.Children.Clear();
        //    //TabControl myVocabsTab = new TabControl();
        //    //myVocabsTab.SelectionChanged += myVocabsTab_SelectionChanged;
        //    //foreach (Vocabulary vocab in lesson.Vocabularies)
        //    //{
        //    //    TabItem tabItem = new TabItem();
        //    //    tabItem.Tag = vocab;
        //    //    tabItem.Header = vocab.Name;
        //    //    int rowsCount = 0;
        //    //    int columnsCount = 0;
        //    //    int wc = vocab.Words.Count;

        //    //    double s = Math.Sqrt(wc);
        //    //    double sa = Math.Floor(s);
        //    //    if (sa == s)
        //    //    {
        //    //        rowsCount = Convert.ToInt32(sa);
        //    //        columnsCount = Convert.ToInt32(sa);
        //    //    }
        //    //    else
        //    //    {
        //    //        rowsCount = Convert.ToInt32(sa);
        //    //        columnsCount = Convert.ToInt32(sa) + 1;
        //    //    }

        //    //    UniformGrid vocabGrid = new UniformGrid();
        //    //    vocabGrid.Tag = vocab;

        //    //    vocabGrid.Rows = rowsCount;
        //    //    vocabGrid.Columns = columnsCount;
        //    //    tabItem.Content = vocabGrid;
        //    //    int count = 0;
        //    //    foreach (Word word in vocab.Words)
        //    //    {
        //    //        //if (!learn && word.IncludetoExam != 1)
        //    //        //    continue;
        //    //        count++;
        //    //        MyItemControl item = new MyItemControl(word);
        //    //        item.BorderThickness = new Thickness(1);
        //    //        item.BorderBrush = new SolidColorBrush(Colors.Blue);
        //    //        item.Margin = new Thickness(2, 2, 2, 2);
        //    //        if (!learn)
        //    //            item.grdControls.Visibility = System.Windows.Visibility.Hidden;
        //    //        item.IncludeToExamChanged += item_IncludeToExamChanged;
        //    //        item.Tag = count;
        //    //        item.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
        //    //        item.AllowPlay = true;
        //    //        vocabGrid.Children.Add(item);
        //    //    }
        //    //    if (vocabGrid.Children.Count != 0)
        //    //        myVocabsTab.Items.Add(tabItem);
        //    //}
        //    //if (!learn)
        //    //{
        //    //    myVocabsTab.Items.Add(CreateMixedExam(vm.CurrentLesson.Unit.Level));
        //    //}
        //    //grdVocabulary.Children.Add(myVocabsTab);

        //}

        //public TabItem CreateMixedExam(Level level, int max = 30)
        //{
        //    //TabItem tabItem = new TabItem();
        //    //Vocabulary tempVocabularySet = new Vocabulary() { Name = "Mixed" };

        //    //List<Word> allWords = new List<Word>();
        //    //foreach (Unit unit in level.Units)
        //    //    foreach (Lesson lesson in unit.Lessons)
        //    //        foreach (Vocabulary vocab in lesson.Vocabularies)
        //    //        {
        //    //            //allWords.AddRange(vocab.Words.Where(w => w.IncludetoExam == 1).ToList());
        //    //        }

        //    //List<int> checkList = new List<int>();
        //    //if (allWords.Count < max)
        //    //    max = allWords.Count;
        //    //for (int i = 1; i <= max; i++)
        //    //{
        //    //    int picked = 0;
        //    //    while (true)
        //    //    {
        //    //        picked = vm.rnd.Next(0, allWords.Count);
        //    //        if (!checkList.Contains(picked))
        //    //            break;
        //    //    }
        //    //    checkList.Add(picked);
        //    //    tempVocabularySet.Words.Add(allWords[picked]);
        //    //}

        //    //tabItem.Tag = tempVocabularySet;
        //    //tabItem.Header = tempVocabularySet.Name;
        //    //int rowsCount = 0;
        //    //int columnsCount = 0;
        //    //int wc = tempVocabularySet.Words.Count;

        //    //double s = Math.Sqrt(wc);
        //    //double sa = Math.Floor(s);
        //    //if (sa == s)
        //    //{
        //    //    rowsCount = Convert.ToInt32(sa);
        //    //    columnsCount = Convert.ToInt32(sa);
        //    //}
        //    //else
        //    //{
        //    //    rowsCount = Convert.ToInt32(sa);
        //    //    columnsCount = Convert.ToInt32(sa) + 1;
        //    //}

        //    //UniformGrid vocabGrid = new UniformGrid();
        //    //vocabGrid.Tag = tempVocabularySet;

        //    //vocabGrid.Rows = rowsCount;
        //    //vocabGrid.Columns = columnsCount;
        //    //tabItem.Content = vocabGrid;
        //    //int count = 0;
        //    //foreach (Word word in tempVocabularySet.Words)
        //    //{
        //    //    count++;
        //    //    MyItemControl item = new MyItemControl(word);
        //    //    item.IncludeToExamChanged += item_IncludeToExamChanged;
        //    //    item.Tag = count;
        //    //    item.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
        //    //    item.AllowPlay = true;
        //    //    vocabGrid.Children.Add(item);
        //    //}
        //    //return tabItem;
        //    return null;
        //}

        //void logic_LessonChanged(object sender, EventArgs e)
        //{
        //    //CreateSceneTab(vm.CurrentLesson);
        //    //CreateVocabularyTab(vm.CurrentLesson, true);
        //}

        //void item_IncludeToExamChanged(object sender, EventArgs e)
        //{
        //    //vm.SaveChanges();
        //}

        //void myVocabsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    //if (e.RemovedItems.Count != 0)
        //    //{
        //    //    TabItem oldtabItem = e.RemovedItems[0] as TabItem;
        //    //    if (oldtabItem != null)
        //    //    {
        //    //        StopPlayingVocabulary(oldtabItem.Content as UniformGrid, true);
        //    //    }
        //    //}
        //    //TabItem tabItem = e.AddedItems[0] as TabItem;
        //    //if (tabItem != null && tabItem.Tag is Vocabulary)
        //    //{
        //    //    activeVocab = tabItem.Tag as Vocabulary;
        //    //    activeVocabContainer = tabItem.Content as UniformGrid;
        //    //    vm.VocabLogic.CurrentVocabulary = activeVocab;
        //    //}

        //    ////turning controlls on/off
        //    //if (activity != VocabularyActivity.Learn)
        //    //{
        //    //    foreach (var item in activeVocabContainer.Children)
        //    //    {
        //    //        ((MyItemControl)item).ShowTools = false;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    foreach (var item in activeVocabContainer.Children)
        //    //    {
        //    //        ((MyItemControl)item).ShowTools = true;
        //    //    }
        //    //}


        //    //if (activity == VocabularyActivity.Practice)
        //    //{
        //    //    PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
        //    //}
        //}

        //void myItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    //if (sender is MyItemControl)
        //    //{
        //    //    MyItemControl myword = (MyItemControl)sender;
        //    //    if (activity == VocabularyActivity.Learn)
        //    //    {
        //    //        mainTrackPlaceholder.Children.Clear();
        //    //        mainTrackPlaceholder.Children.Add(myword.myTrack);
        //    //        //stop all highlighting animations
        //    //        StopPlayingVocabulary(activeVocabContainer, true);
        //    //        if (myword.AllowPlay)
        //    //        {
        //    //            myword.Play(0);
        //    //        }
        //    //        myword.StartHighlighting();
        //    //    }
        //    //    else if (activity == VocabularyActivity.Practice)
        //    //    {
        //    //        Result result = vm.VocabLogic.CheckAnswer((int)myword.Tag - 1);
        //    //        StopPlayingVocabulary(activeVocabContainer, true);
        //    //        myword.StartHighlighting(700);
        //    //        DispatcherTimer timer = new DispatcherTimer();
        //    //        timer.Interval = TimeSpan.FromMilliseconds(700);
        //    //        if (result.Correct)
        //    //        {
        //    //            rightSound.Play();
        //    //            timer.Tick += new EventHandler((s, args) =>
        //    //            {
        //    //                PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
        //    //                timer.Stop();
        //    //            });
        //    //            timer.Start();
        //    //        }
        //    //        else
        //    //        {
        //    //            wrongSound.Play();
        //    //            timer.Tick += new EventHandler((s, args) =>
        //    //            {
        //    //                if (result.Highlight)
        //    //                    (activeVocabContainer.Children[currentPlaying] as MyItemControl).StartHighlighting(2000);
        //    //                PlaySelected(vm.VocabLogic.GetRandomItemForPractice(currentPlaying));
        //    //                timer.Stop();
        //    //            });
        //    //            timer.Start();

        //    //        }

        //    //    }
        //    //    else if (activity == VocabularyActivity.Quiz)
        //    //    {
        //    //        //bool done = false;
        //    //        //Result result = vm.VocabLogic.CheckAnswer((int)myword.Tag - 1);
        //    //        //StopPlayingVocabulary(activeVocabContainer, true);
        //    //        //myword.StartHighlighting(700);
        //    //        //DispatcherTimer timer = new DispatcherTimer();
        //    //        //timer.Interval = TimeSpan.FromMilliseconds(700);

        //    //        //if (result.Correct)
        //    //        //{
        //    //        //    rightSound.Play();
        //    //        //}
        //    //        //else
        //    //        //{
        //    //        //    wrongSound.Play();
        //    //        //}

        //    //        //timer.Tick += new EventHandler((s, args) =>
        //    //        //{
        //    //        //    timer.Stop();
        //    //        //    int picked = vm.VocabLogic.GetRandomItemForQuiz(out done);
        //    //        //    if (picked == -1 && done)
        //    //        //    {
        //    //        //        MessageBox.Show("Do whatever you want. Done!");
        //    //        //        return;
        //    //        //    }
        //    //        //    else if (picked == -1 && !done)
        //    //        //    {
        //    //        //        throw new Exception("For some reason returned -1");
        //    //        //    }
        //    //        //    PlaySelected(picked);

        //    //        //});
        //    //        //timer.Start();
        //    //    }

        //    //}
        //}

        //void StopPlayingVocabulary(UniformGrid vocabContainer, bool stopHighlighting)
        //{
        //    //foreach (var child in activeVocabContainer.Children)
        //    //{
        //    //    if (child is MyItemControl)
        //    //    {
        //    //        if (stopHighlighting)
        //    //            ((MyItemControl)child).StopHighlighting();
        //    //        ((MyItemControl)child).StopPlaying();
        //    //    }
        //    //}
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    //EnterData ed = new EnterData();
        //    //ed.InsertLanguage();

        //    //Converter converter = new Converter();
        //    //FileInfo[] soundFiles = new DirectoryInfo("C:\\LangLearnData\\voices\\").GetFiles();
        //    //FileInfo[] imageFiles = new DirectoryInfo("C:\\LangLearnData\\pictures\\").GetFiles();
        //    //List<MyItem> items = new List<MyItem>();
        //    //foreach (FileInfo soundFile in soundFiles)
        //    //{
        //    //    FileInfo imageFile = imageFiles.FirstOrDefault(f => f.Name.StartsWith(soundFile.Name.Substring(0, soundFile.Name.Length - 4)));
        //    //    if (imageFile != null)
        //    //    {
        //    //        byte[] binImage = converter.BitmapToByte(new System.Drawing.Bitmap(imageFile.FullName));
        //    //        byte[] binSound = converter.SoundToByte(soundFile.FullName);
        //    //        MyItem item = new MyItem() { Id = Guid.NewGuid().ToString(), Name = soundFile.Name.Substring(0, soundFile.Name.Length - 4) };
        //    //        item.Picture = binImage;
        //    //        item.Sound = binSound;
        //    //        item.SoundVol = 100;
        //    //        items.Add(item);
        //    //    }
        //    //}
        //    //DateProvider entities = new DateProvider();
        //    //entities.AddItems(items);
        //}

        //private void menuClose_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

        //private void VocabularyActivityChanged(object sender, RoutedEventArgs e)
        //{
        //    //RadioButton radio = (RadioButton)sender;
        //    //vocabSettings.IsEnabled = true;
        //    //switch (radio.Content.ToString())
        //    //{
        //    //    case "Learn":
        //    //        CreateVocabularyTab(vm.CurrentLesson, true);
        //    //        activity = VocabularyActivity.Learn;
        //    //        vocabSettings.IsEnabled = false;
        //    //        result.Visibility = System.Windows.Visibility.Hidden;
        //    //        lblTotal.Visibility = System.Windows.Visibility.Hidden;
        //    //        break;
        //    //    case "Practice":
        //    //        CreateVocabularyTab(vm.CurrentLesson);
        //    //        activity = VocabularyActivity.Practice;
        //    //        result.Visibility = System.Windows.Visibility.Hidden;
        //    //        lblTotal.Visibility = System.Windows.Visibility.Hidden;
        //    //        //PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
        //    //        break;
        //    //    case "Review":
        //    //        activity = VocabularyActivity.Review;
        //    //        result.Visibility = System.Windows.Visibility.Hidden;
        //    //        lblTotal.Visibility = System.Windows.Visibility.Hidden;
        //    //        break;
        //    //    case "Quiz":
        //    //        activity = VocabularyActivity.Quiz;
        //    //        lblTotal.SetBinding(Label.ContentProperty, "VocabLogic.Total");
        //    //        lblRight.SetBinding(Label.ContentProperty, "VocabLogic.RightAnswers");
        //    //        lblWrong.SetBinding(Label.ContentProperty, "VocabLogic.WrongAnswers");
        //    //        result.Visibility = System.Windows.Visibility.Visible;
        //    //        lblTotal.Visibility = System.Windows.Visibility.Visible;
        //    //        vm.VocabLogic.Currentactivity = activity;
        //    //        bool done;
        //    //        PlaySelected(vm.VocabLogic.GetRandomItemForQuiz(out done));
        //    //        break;
        //    //}

        //}

        //int currentPlaying = -1;

        //void PlaySelected(int itemNo)
        //{
        //    //currentPlaying = itemNo;
        //    //((MyItemControl)activeVocabContainer.Children[itemNo]).Play(0);
        //}

        //private void vocabSettings_Click(object sender, RoutedEventArgs e)
        //{
        //    //VocabularySettings vs = new VocabularySettings(vm);
        //    //vs.Show();
        //}

    }
}
