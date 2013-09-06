using LangInformModel;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LangInformGUI.Controls
{
    /// <summary>
    /// Interaction logic for MyItem.xaml
    /// </summary>
    public partial class MyItemControl : UserControl, INotifyPropertyChanged
    {

        public MyItemControl(Word word)
        {
            InitializeComponent();
            _word = word;
            Picture = null;// con.ByteToWPFImage(word.Picture);
            waveReader = new WaveFileReader(con.byteArrayToStream(_word.Sound));
            SoundLength = waveReader.TotalTime;
            wc = new WaveChannel32(waveReader);
            audioOutput = new DirectSoundOut();
            audioOutput.Init(wc);
            track.ValueChangedManually += track_PositionChangedManually;
            myTrack.ValueChangedManually += track_PositionChangedManually;
            myTrack.MouseLeftButtonDown += track_MouseLeftButtonDown;
            myTrack.Loaded += track_Loaded;
        }
        Word _word;

        public CustomSlider myTrack = new CustomSlider() { Margin = new Thickness(5, 0, 5, 0) };

        public TimeSpan SoundLength { get; set; }

        string _includeToExamToolTip;
        public string IncludeToExamToolTip { get { return _includeToExamToolTip; } set { _includeToExamToolTip = value; NotifyPropertyChanged("IncludeToExamToolTip"); } }


        void track_PositionChangedManually(object sender, EventArgs e)
        {
            Play((int)track.Value);
            Play((int)myTrack.Value);
        }

        bool _showTools = true;
        public bool ShowTools { get { return _showTools; } set { _showTools = value; } }

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

        public void StartHighlighting(int stopAfter = 0)
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
            if (stopAfter == 0)
                stopAfter = (int)this.SoundLength.TotalMilliseconds;
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

        public void Play(int playFrom)
        {
            if (audioOutput.PlaybackState == PlaybackState.Playing)
            {
                audioOutput.Pause();
            }
            waveReader.CurrentTime = TimeSpan.FromMilliseconds(playFrom);
            //audioOutput.Volume 
            audioOutput.Play();
            DoubleAnimation anim = new DoubleAnimation();
            double from = playFrom;
            double to = waveReader.TotalTime.TotalMilliseconds;
            anim.From = from;
            anim.To = to;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(to - from));
            track.BeginAnimation(CustomSlider.ValueProperty, anim);
            myTrack.BeginAnimation(CustomSlider.ValueProperty, anim);
        }

        public void StopPlaying()
        {
            if (audioOutput.PlaybackState != PlaybackState.Stopped)
            {
                audioOutput.Pause();
                track.BeginAnimation(CustomSlider.ValueProperty, null);
                track.Position = 0;

                myTrack.BeginAnimation(CustomSlider.ValueProperty, null);
                myTrack.Position = 0;
            }
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
            btnExclude.DataContext = this;
            //ccChangeIncludeToExam((_word.IncludetoExam == 1 ? true : false));
        }

        void ChangeIncludeToExam(bool included, bool change = false)
        {
            if (included)
            {
                IncludeToExamToolTip = "Exclude from exam";
                IncludeToExam = true;
                dimmer.Opacity = 0;
                btnExclude.Background = new SolidColorBrush(Colors.White);
            //    if (change)
            //        _word.IncludetoExam = 1;
            }
            else
            {
                IncludeToExam = false;
                IncludeToExamToolTip = "Include to exam";
                dimmer.Opacity = .1;
                btnExclude.Background = new SolidColorBrush(Colors.Blue);
                //if (change)
                //    _word.IncludetoExam = 0;
            }
        }

        private void UserControl_MouseEnter_1(object sender, MouseEventArgs e)
        {
            if (_showTools)
            {
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = grdControls.Opacity;
                anim.To = 1;
                anim.Duration = TimeSpan.FromMilliseconds(300);
                grdControls.BeginAnimation(Grid.OpacityProperty, anim);
            }
        }

        private void UserControl_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (_showTools)
            {
                DoubleAnimation anim = new DoubleAnimation();
                anim.From = grdControls.Opacity;
                anim.To = 0;
                anim.Duration = TimeSpan.FromMilliseconds(300);
                grdControls.BeginAnimation(Grid.OpacityProperty, anim);
            }
        }

        private void track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void track_Loaded(object sender, RoutedEventArgs e)
        {
            track.MaxValue = SoundLength.TotalMilliseconds;
            myTrack.MaxValue = SoundLength.TotalMilliseconds;
        }

        bool IncludeToExam = true;

        public event EventHandler IncludeToExamChanged;

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ChangeIncludeToExam(IncludeToExam ? false : true, true);
            if (IncludeToExamChanged != null)
            {
                IncludeToExamChanged(this, new EventArgs());
            }
            
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

    }
}
