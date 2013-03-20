using LangInformGUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            treeLessons.ItemsSource = logic.GenerateLanguages();
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
            logic.LessonChanged += logic_LessonChanged;
        }

        void logic_LessonChanged(object sender, EventArgs e)
        {
            //creating scene tab
            grdScene.Children.Clear();
            TabControl tab = new TabControl();
            foreach (Scene scene in logic.SelelectedLesson.Scenes)
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = scene.Name;
                var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(scene.Picture.GetHbitmap(),
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());
                //var brush = new ImageBrush(bitmapSource);
                Image image = new Image();
                image.Source = bitmapSource;
                tabItem.Content = image;
                tab.Items.Add(tabItem);
            }
            int col = 0;
            int row = 0;
            grdWords.Children.Clear();
            foreach (Word item in logic.SelelectedLesson.Vocabulary)
            {
                MyItem myItem = new MyItem(item);

                Grid.SetRow(myItem, row);
                Grid.SetColumn(myItem, col);
                if (col == 3)
                {
                    col = -1;
                    row++;
                }
                col++;
                myItem.Margin = new Thickness(2);
                myItem.MouseLeftButtonDown += myItem_MouseLeftButtonDown;
                myItem.AllowPlay = true;
                grdWords.Children.Add(myItem);
            }

            grdScene.Children.Add(tab);
        }

        void myItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is MyItem)
            {
                MyItem myword = (MyItem)sender;
                if (myword.AllowPlay)
                    myword.Sound.Play();
                //stop all highlighting animations
                foreach (var child in grdWords.Children)
                {
                    if (child is MyItem)
                    {
                        ((MyItem)child).StopHighlighting();
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

        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    
}
