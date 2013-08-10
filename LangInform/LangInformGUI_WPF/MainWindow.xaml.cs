using LangInformGUI;
using LangInformGUI.Controls;
using LangInformModel;
using LangInformVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LangInformGUI_WPF
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

        ViewModel vm = new ViewModel();
        MainEntities entities = new MainEntities();

        VocabularyActivity activity = VocabularyActivity.Learn;
        Vocabulary activeVocab;
        UniformGrid activeVocabContainer;

        SoundPlayer rightSound;
        SoundPlayer wrongSound;
        Converter converter = new Converter();

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            vm.CurrentLessonChanged += logic_LessonChanged;
            rightSound = new SoundPlayer(Properties.Resources.right);
            wrongSound = new SoundPlayer(Properties.Resources.wrong5);
            this.DataContext = vm;
            treeLessons.ItemsSource = vm.Languages;

            //btnLoad.Visibility = System.Windows.Visibility.Hidden;
            //btnTest.Visibility = System.Windows.Visibility.Hidden;
            //Window1 w1 = new Window1(vm);
            //w1.ShowDialog();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void treeLessons_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeLessons.SelectedItem is Language)
            {
                vm.CurrentLanguage = treeLessons.SelectedItem as Language;
                lblMap.Content = vm.CurrentLanguage.Name;
            }
            else if (treeLessons.SelectedItem is Level)
            {
                vm.CurrentLevel = treeLessons.SelectedItem as Level;
            }
            else if (treeLessons.SelectedItem is Unit)
            {
                vm.CurrentUnit = treeLessons.SelectedItem as Unit;
            }
            else if (treeLessons.SelectedItem is Lesson)
            {
                vm.CurrentLesson = treeLessons.SelectedItem as Lesson;
            }
        }

        private void CreateSceneTab(Lesson lesson)
        {
            grdScene.Children.Clear();
            TabControl myScenesTab = new TabControl();
            foreach (Scene scene in lesson.Scenes)
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = scene.Name;

                Image image = converter.ByteToWPFImage(scene.Picture);
                tabItem.Content = image;
                myScenesTab.Items.Add(tabItem);
            }
            grdScene.Children.Add(myScenesTab);
        }

        private void CreateVocabularyTab(Lesson lesson, bool learn = false)
        {
            grdVocabulary.Children.Clear();
            TabControl myVocabsTab = new TabControl();
            myVocabsTab.SelectionChanged += myVocabsTab_SelectionChanged;
            foreach (Vocabulary vocab in lesson.Vocabularies)
            {
                TabItem tabItem = new TabItem();
                tabItem.Tag = vocab;
                tabItem.Header = vocab.Name;
                int rowsCount = 0;
                int columnsCount = 0;
                int wc = vocab.Words.Count;

                double s = Math.Sqrt(wc);
                double sa = Math.Floor(s);
                if (sa == s)
                {
                    rowsCount = Convert.ToInt32(sa);
                    columnsCount = Convert.ToInt32(sa);
                }
                else
                {
                    rowsCount = Convert.ToInt32(sa);
                    columnsCount = Convert.ToInt32(sa) + 1;
                }

                UniformGrid vocabGrid = new UniformGrid();
                vocabGrid.Tag = vocab;

                vocabGrid.Rows = rowsCount;
                vocabGrid.Columns = columnsCount;
                tabItem.Content = vocabGrid;
                int count = 0;
                foreach (Word word in vocab.Words)
                {
                    if (!learn && word.IncludetoExam != 1)
                        continue;
                    count++;
                    MyItemControl item = new MyItemControl(word);
                    item.BorderThickness = new Thickness(1);
                    item.BorderBrush = new SolidColorBrush(Colors.Blue);
                    item.Margin = new Thickness(2, 2, 2, 2);
                    if (!learn)
                        item.grdControls.Visibility = System.Windows.Visibility.Hidden;
                    item.IncludeToExamChanged += item_IncludeToExamChanged;
                    item.Tag = count;
                    item.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
                    item.AllowPlay = true;
                    vocabGrid.Children.Add(item);
                }
                if (vocabGrid.Children.Count != 0)
                    myVocabsTab.Items.Add(tabItem);
            }
            if (!learn)
            {
                myVocabsTab.Items.Add(CreateMixedExam(vm.CurrentLesson.Unit.Level));
            }
            grdVocabulary.Children.Add(myVocabsTab);

        }

        public TabItem CreateMixedExam(Level level, int max = 30)
        {
            TabItem tabItem = new TabItem();
            Vocabulary tempVocabularySet = new Vocabulary() { Name = "Mixed" };

            List<Word> allWords = new List<Word>();
            foreach (Unit unit in level.Units)
                foreach (Lesson lesson in unit.Lessons)
                    foreach (Vocabulary vocab in lesson.Vocabularies)
                    {
                        allWords.AddRange(vocab.Words.Where(w => w.IncludetoExam == 1).ToList());
                    }

            List<int> checkList = new List<int>();
            if (allWords.Count < max)
                max = allWords.Count;
            for (int i = 1; i <= max; i++)
            {
                int picked = 0;
                while (true)
                {
                    picked = vm.rnd.Next(0, allWords.Count);
                    if (!checkList.Contains(picked))
                        break;
                }
                checkList.Add(picked);
                tempVocabularySet.Words.Add(allWords[picked]);
            }

            tabItem.Tag = tempVocabularySet;
            tabItem.Header = tempVocabularySet.Name;
            int rowsCount = 0;
            int columnsCount = 0;
            int wc = tempVocabularySet.Words.Count;

            double s = Math.Sqrt(wc);
            double sa = Math.Floor(s);
            if (sa == s)
            {
                rowsCount = Convert.ToInt32(sa);
                columnsCount = Convert.ToInt32(sa);
            }
            else
            {
                rowsCount = Convert.ToInt32(sa);
                columnsCount = Convert.ToInt32(sa) + 1;
            }

            UniformGrid vocabGrid = new UniformGrid();
            vocabGrid.Tag = tempVocabularySet;

            vocabGrid.Rows = rowsCount;
            vocabGrid.Columns = columnsCount;
            tabItem.Content = vocabGrid;
            int count = 0;
            foreach (Word word in tempVocabularySet.Words)
            {
                count++;
                MyItemControl item = new MyItemControl(word);
                item.IncludeToExamChanged += item_IncludeToExamChanged;
                item.Tag = count;
                item.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
                item.AllowPlay = true;
                vocabGrid.Children.Add(item);
            }
            return tabItem;
        }

        void logic_LessonChanged(object sender, EventArgs e)
        {
            CreateSceneTab(vm.CurrentLesson);
            CreateVocabularyTab(vm.CurrentLesson, true);
        }

        void item_IncludeToExamChanged(object sender, EventArgs e)
        {
            vm.SaveChanges();
        }

        void myVocabsTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.RemovedItems.Count != 0)
            {
                TabItem oldtabItem = e.RemovedItems[0] as TabItem;
                if (oldtabItem != null)
                {
                    StopPlayingVocabulary(oldtabItem.Content as UniformGrid, true);
                }
            }
            TabItem tabItem = e.AddedItems[0] as TabItem;
            if (tabItem != null && tabItem.Tag is Vocabulary)
            {
                activeVocab = tabItem.Tag as Vocabulary;
                activeVocabContainer = tabItem.Content as UniformGrid;
                vm.VocabLogic.CurrentVocabulary = activeVocab;
            }

            //turning controlls on/off
            if (activity != VocabularyActivity.Learn)
            {
                foreach (var item in activeVocabContainer.Children)
                {
                    ((MyItemControl)item).ShowTools = false;
                }
            }
            else
            {
                foreach (var item in activeVocabContainer.Children)
                {
                    ((MyItemControl)item).ShowTools = true;
                }
            }


            if (activity == VocabularyActivity.Practice)
            {
                PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
            }
        }

        void myItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is MyItemControl)
            {
                MyItemControl myword = (MyItemControl)sender;
                if (activity == VocabularyActivity.Learn)
                {
                    mainTrackPlaceholder.Children.Clear();
                    mainTrackPlaceholder.Children.Add(myword.myTrack);
                    //stop all highlighting animations
                    StopPlayingVocabulary(activeVocabContainer, true);
                    if (myword.AllowPlay)
                    {
                        myword.Play(0);
                    }
                    myword.StartHighlighting();
                }
                else if (activity == VocabularyActivity.Practice)
                {
                    Result result = vm.VocabLogic.CheckAnswer((int)myword.Tag - 1);
                    StopPlayingVocabulary(activeVocabContainer, true);
                    myword.StartHighlighting(700);
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(700);
                    if (result.Correct)
                    {
                        rightSound.Play();
                        timer.Tick += new EventHandler((s, args) =>
                        {
                            PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
                            timer.Stop();
                        });
                        timer.Start();
                    }
                    else
                    {
                        wrongSound.Play();
                        timer.Tick += new EventHandler((s, args) =>
                        {
                            if (result.Highlight)
                                (activeVocabContainer.Children[currentPlaying] as MyItemControl).StartHighlighting(2000);
                            PlaySelected(vm.VocabLogic.GetRandomItemForPractice(currentPlaying));
                            timer.Stop();
                        });
                        timer.Start();

                    }

                }
                else if (activity == VocabularyActivity.Quiz)
                {
                    bool done = false;
                    Result result = vm.VocabLogic.CheckAnswer((int)myword.Tag - 1);
                    StopPlayingVocabulary(activeVocabContainer, true);
                    myword.StartHighlighting(700);
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(700);

                    if (result.Correct)
                    {
                        rightSound.Play();
                    }
                    else
                    {
                        wrongSound.Play();
                    }

                    timer.Tick += new EventHandler((s, args) =>
                    {
                        timer.Stop();
                        int picked = vm.VocabLogic.GetRandomItemForQuiz(out done);
                        if (picked == -1 && done)
                        {
                            MessageBox.Show("Do whatever you want. Done!");
                            return;
                        }
                        else if (picked == -1 && !done)
                        {
                            throw new Exception("For some reason returned -1");
                        }
                        PlaySelected(picked);

                    });
                    timer.Start();
                }

            }
        }

        void StopPlayingVocabulary(UniformGrid vocabContainer, bool stopHighlighting)
        {
            foreach (var child in activeVocabContainer.Children)
            {
                if (child is MyItemControl)
                {
                    if (stopHighlighting)
                        ((MyItemControl)child).StopHighlighting();
                    ((MyItemControl)child).StopPlaying();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EnterData ed = new EnterData();
            ed.InsertLanguage();

            //Converter converter = new Converter();
            //FileInfo[] soundFiles = new DirectoryInfo("C:\\LangLearnData\\voices\\").GetFiles();
            //FileInfo[] imageFiles = new DirectoryInfo("C:\\LangLearnData\\pictures\\").GetFiles();
            //List<MyItem> items = new List<MyItem>();
            //foreach (FileInfo soundFile in soundFiles)
            //{
            //    FileInfo imageFile = imageFiles.FirstOrDefault(f => f.Name.StartsWith(soundFile.Name.Substring(0, soundFile.Name.Length - 4)));
            //    if (imageFile != null)
            //    {
            //        byte[] binImage = converter.BitmapToByte(new System.Drawing.Bitmap(imageFile.FullName));
            //        byte[] binSound = converter.SoundToByte(soundFile.FullName);
            //        MyItem item = new MyItem() { Id = Guid.NewGuid().ToString(), Name = soundFile.Name.Substring(0, soundFile.Name.Length - 4) };
            //        item.Picture = binImage;
            //        item.Sound = binSound;
            //        item.SoundVol = 100;
            //        items.Add(item);
            //    }
            //}
            //DateProvider entities = new DateProvider();
            //entities.AddItems(items);
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void VocabularyActivityChanged(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (RadioButton)sender;
            vocabSettings.IsEnabled = true;
            switch (radio.Content.ToString())
            {
                case "Learn":
                    CreateVocabularyTab(vm.CurrentLesson, true);
                    activity = VocabularyActivity.Learn;
                    vocabSettings.IsEnabled = false;
                    result.Visibility = System.Windows.Visibility.Hidden;
                    lblTotal.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "Practice":
                    CreateVocabularyTab(vm.CurrentLesson);
                    activity = VocabularyActivity.Practice;
                    result.Visibility = System.Windows.Visibility.Hidden;
                    lblTotal.Visibility = System.Windows.Visibility.Hidden;
                    //PlaySelected(vm.VocabLogic.GetRandomItemForPractice());
                    break;
                case "Review":
                    activity = VocabularyActivity.Review;
                    result.Visibility = System.Windows.Visibility.Hidden;
                    lblTotal.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "Quiz":
                    activity = VocabularyActivity.Quiz;
                    lblTotal.SetBinding(Label.ContentProperty, "VocabLogic.Total");
                    lblRight.SetBinding(Label.ContentProperty, "VocabLogic.RightAnswers");
                    lblWrong.SetBinding(Label.ContentProperty, "VocabLogic.WrongAnswers");
                    result.Visibility = System.Windows.Visibility.Visible;
                    lblTotal.Visibility = System.Windows.Visibility.Visible;
                    vm.VocabLogic.Currentactivity = activity;
                    bool done;
                    PlaySelected(vm.VocabLogic.GetRandomItemForQuiz(out done));
                    break;
            }

        }

        int currentPlaying = -1;

        void PlaySelected(int itemNo)
        {
            currentPlaying = itemNo;
            ((MyItemControl)activeVocabContainer.Children[itemNo]).Play(0);
        }

        private void vocabSettings_Click(object sender, RoutedEventArgs e)
        {
            VocabularySettings vs = new VocabularySettings(vm);
            vs.Show();
        }

    }

}
