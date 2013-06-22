using LangInformGUI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LangInformModel;
using LangInformVM;

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
        Logic logic = new Logic();

        MainEntities entities = new MainEntities();

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            treeLessons.ItemsSource = entities.Languages.ToList();
        }

        private void treeLessons_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeLessons.SelectedItem is Language)
            {
                logic.SelectedLanguage = treeLessons.SelectedItem as Language;
                lblMap.Content = logic.SelectedLanguage.Name;
            }
            else if (treeLessons.SelectedItem is Lesson)
            {
                logic.SelelectedLesson = treeLessons.SelectedItem as Lesson;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.ShowDialog();
            logic.LessonChanged += logic_LessonChanged;
        }

        void logic_LessonChanged(object sender, EventArgs e)
        {
            //creating scene tab
            foreach (var child in grdScene.Children)
            {
                TabControl control = (TabControl)child;
                foreach (TabItem tabItem in control.Items)
                {
                    Image uGrid = (Image)tabItem.Content;
                    
                    uGrid = null;
                    //tabItem = null;
                }
                control = null;
            }
            grdScene.Children.Clear();
            TabControl myScenesTab = new TabControl();
            foreach (Scene scene in logic.SelelectedLesson.Scenes)
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = scene.Name;

                Image image = converter.ByteToWPFImage(scene.Picture);
                tabItem.Content = image;
                myScenesTab.Items.Add(tabItem);
            }
            grdScene.Children.Add(myScenesTab);
            //building vocabulary part

            foreach (var child in grdVocabulary.Children)
            {
                TabControl control = (TabControl)child;
                foreach (TabItem tabItem in control.Items)
                {
                    UniformGrid uGrid = (UniformGrid)tabItem.Content;
                    foreach (var gc in uGrid.Children)
                    {
                        MyItemControl mc = (MyItemControl)gc;
                    }
                    uGrid = null;
                    //tabItem = null;
                }
                control = null;
            }
            grdVocabulary.Children.Clear();
            GC.Collect();
            TabControl myVocabsTab = new TabControl();
            foreach (Vocabulary vocab in logic.SelelectedLesson.Vocabularies)
            {
                TabItem tabItem = new TabItem();
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
                vocabGrid.Rows = rowsCount;
                vocabGrid.Columns = columnsCount;
                tabItem.Content = vocabGrid;
                foreach (Word word in vocab.Words)
                {
                    MyItemControl item = new MyItemControl(word);
                    item.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
                    item.AllowPlay = true;
                    vocabGrid.Children.Add(item);
                }
                myVocabsTab.Items.Add(tabItem);
            }
            grdVocabulary.Children.Add(myVocabsTab);

        }

        Converter converter = new Converter();

        void myItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is MyItemControl)
            {
                MyItemControl myword = (MyItemControl)sender;
                if (myword.AllowPlay)
                {
                    //myword.Sound.Stop();
                    //myword.Sound.Position = TimeSpan.FromMilliseconds(0);
                    //double aa = myword.Sound.Volume;
                    myword.Play();
                }
                //stop all highlighting animations
                foreach (var child in ((UniformGrid)myword.Parent).Children)
                {
                    if (child is MyItemControl)
                    {
                        ((MyItemControl)child).StopHighlighting();
                    }
                }
                myword.StartHighlighting(2000);
            }
        }

        BitmapSource ConvertBitmapToImageSource(System.Drawing.Bitmap bmp)
        {
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EnterData enterData = new EnterData();
            enterData.InsertLanguage();

            return;
            
            Converter converter = new Converter();
            FileInfo[] soundFiles = new DirectoryInfo("C:\\LangLearnData\\voices\\").GetFiles();
            FileInfo[] imageFiles = new DirectoryInfo("C:\\LangLearnData\\pictures\\").GetFiles();
            List<MyItem> items = new List<MyItem>();
            foreach (FileInfo soundFile in soundFiles)
            {
                FileInfo imageFile = imageFiles.FirstOrDefault(f => f.Name.StartsWith(soundFile.Name.Substring(0, soundFile.Name.Length - 4)));
                if (imageFile != null)
                {
                    byte[] binImage = converter.BitmapToByte(new System.Drawing.Bitmap(imageFile.FullName));
                    byte[] binSound = converter.SoundToByte(soundFile.FullName);
                    MyItem item = new MyItem() { Id = int.Parse(Guid.NewGuid().ToString()), Name = soundFile.Name.Substring(0, soundFile.Name.Length - 4) };
                    item.Picture = binImage;
                    item.Sound = binSound;
                    item.SoundVol = 100;
                    items.Add(item);
                }
            }
            DataProvider entities = new DataProvider();
            entities.AddItems(items);
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
