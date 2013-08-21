using LangInformModel;
using LangInformVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangInformGUI.Windows
{
    /// <summary>
    /// Interaction logic for Pictures.xaml
    /// </summary>
    public partial class Pictures : Window, INotifyPropertyChanged
    {
        public Pictures()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        ViewModel vm = MainWindow.vm;

        List<ScenePicture> pictures;

        double _imageWidth = 150;
        public double ImageWidth { get { return _imageWidth; } set { _imageWidth = value; NotifyPropertyChanged("ImageWidth"); } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pictures = vm.GetData<ScenePicture>("select * from scenepicture");
            foreach (ScenePicture picture in pictures)
            {
                Image image = new Image();
                image.DataContext = this;
                image.SetBinding(Image.WidthProperty, new Binding("ImageWidth"));
                image.Source = Assistant.ByteToBitmapSource(picture.Picture);
                listPictures.Items.Add(image);
            }
        }
        
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
