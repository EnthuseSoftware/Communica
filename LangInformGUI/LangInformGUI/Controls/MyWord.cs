using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LangInformGUI.Controls
{
    public class MyWord : UserControl
    {
        public MyWord(Word word)
        {
            Sound = word.Sound;
            Picture = new Image();
            Picture.Source = ConvertBitmapToImageSource(word.Picture);
            CreateContent(Picture);
            this.Content = grd;
        }
        Grid grd;
        StackPanel bgrd;
        void CreateContent(Image image)
        {
            grd = new Grid();
            bgrd = new StackPanel();
            bgrd.Background = new SolidColorBrush(Colors.White);
            grd.Children.Add(bgrd);
            image.Margin = new Thickness(4);
            grd.Children.Add(image);
        }

        public void StopHighlighting()
        {
            bgrd.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
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

        public System.Media.SoundPlayer Sound { get; set; }
    }
}
