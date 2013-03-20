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

namespace LangInformGUI.Controls
{
    /// <summary>
    /// Interaction logic for MyItem.xaml
    /// </summary>
    public partial class MyItem : UserControl
    {
        public MyItem(Word word)
        {
            InitializeComponent();
            Sound = new MediaPlayer();
            Sound.Open(new Uri(word.Sound.SoundLocation));
            Picture = new Image();
            Picture.Source = ConvertBitmapToImageSource(word.Picture);
            track.PositionChangedManually += track_PositionChangedManually;
        }

        void track_PositionChangedManually(object sender, EventArgs e)
        {
            //if (track.Position >= 0)
            //    Sound.Position = TimeSpan.FromMilliseconds(track.Position);
            //Sound.
        }

        public void StopHighlighting()
        {
            try
            {
                bgrd.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
            }
            catch
            {

            }
        }

        public void StartHighlighting(int stopAfter)
        {

            ColorAnimation anim = new ColorAnimation();
            anim.From = Colors.Red;
            anim.To = Colors.Blue;
            anim.DecelerationRatio = 0.9;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            bgrd.Background.BeginAnimation(SolidColorBrush.ColorProperty, anim);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(stopAfter);
            timer.Tick += new EventHandler((s, e) =>
            {
                bgrd.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                timer.Stop();
            });
            timer.Start();
        }

        public bool AllowPlay { get; set; }

        public System.Windows.Controls.Image Picture { get; set; }

        public System.Windows.Media.Imaging.BitmapSource ConvertBitmapToImageSource(System.Drawing.Bitmap bmp)
        {
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                                                                            IntPtr.Zero,
                                                                            Int32Rect.Empty,
                                                                            BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }

        public MediaPlayer Sound { get; set; }

        private void image_Loaded(object sender, RoutedEventArgs e)
        {
            image.Source = Picture.Source;
        }
        StackPanel bgrd;
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (bgrd == null)
            {
                bgrd = new StackPanel();
                bgrd.Background = new SolidColorBrush(Colors.White);
                grdMain.Children.Insert(0, bgrd);
            }
        }

        private void UserControl_MouseEnter_1(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = grdControls.Opacity;
            anim.To = 1;
            anim.Duration = TimeSpan.FromMilliseconds(300);
            grdControls.BeginAnimation(Grid.OpacityProperty, anim);
        }

        private void UserControl_MouseLeave_1(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = grdControls.Opacity;
            anim.To = 0;
            anim.Duration = TimeSpan.FromMilliseconds(300);
            grdControls.BeginAnimation(Grid.OpacityProperty, anim);
        }

        private void track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
