using LangInformModel;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class MyItemControl : UserControl
    {

        public MyItemControl(Word word)
        {
            InitializeComponent();
            _word = word;
            Picture = con.ByteToWPFImage(word.Picture);
            waveReader = new WaveFileReader(con.byteArrayToStream(_word.Sound));
            SoundLength = waveReader.TotalTime;
            //wc = new WaveChannel32(waveReader) { PadWithZeroes = false };
            wc = new WaveChannel32(waveReader);
            audioOutput = new DirectSoundOut();
            audioOutput.Init(wc);
            track.PositionChangedManually += track_PositionChangedManually;
        }
        Word _word;

        public TimeSpan SoundLength { get; set; }

        void track_PositionChangedManually(object sender, EventArgs e)
        {
            if (track.Position >= 0)
            {
                CurrentPosition = TimeSpan.FromMilliseconds(track.Position);
                if (audioOutput.PlaybackState == PlaybackState.Playing)
                {
                    audioOutput.Pause();
                }
            }
            audioOutput.Play();
            play = false;
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

        public TimeSpan CurrentPosition { get { return waveReader.CurrentTime; } set { waveReader.CurrentTime = value; } }

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
        Converter con = new Converter();
        
        WaveFileReader waveReader;
        DirectSoundOut audioOutput;
        WaveChannel32 wc;

        bool play = true;
        public void Play()
        {
            if (!play)
            {
                play = true;
                return;
            }
            if (audioOutput.PlaybackState == PlaybackState.Playing)
            {
                audioOutput.Pause();
            }
            waveReader.Position = 0;
            audioOutput.Play();
            DoubleAnimation anim = new DoubleAnimation();
            double from = waveReader.CurrentTime.TotalMilliseconds;
            double to = waveReader.TotalTime.TotalMilliseconds;
            anim.From = from;
            anim.To = to;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(to - from));
        }


        public System.Windows.Controls.Image Picture { get; set; }

        public System.Windows.Controls.Image ByteToWPFImage(byte[] blob)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(blob, 0, blob.Length);
            stream.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            bi.BeginInit();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            System.Windows.Controls.Image image2 = new System.Windows.Controls.Image() { Source = bi };
            return image2;
        }
        
        //public MediaPlayer Sound { get; set; }

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
            //e.Handled = true;
            double d=track.Position;
        }

        private void track_Loaded(object sender, RoutedEventArgs e)
        {
            track.MaxValue = SoundLength.TotalMilliseconds;
        }

    }
}
