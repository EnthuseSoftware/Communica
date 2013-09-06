using LangInformModel;
using LangInformVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelAndVMTest
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this is just temporary project 
            DirectoryInfo dir = new DirectoryInfo("..\\..\\..\\");
            FileInfo databaseFile = dir.GetFiles().FirstOrDefault(f => f.Name == "LangData.3db");
            ViewModel vm = new ViewModel(databaseFile.FullName);
            var langs = vm.Languages;
            var v = langs.FirstOrDefault().Levels.FirstOrDefault().Units.FirstOrDefault().Lessons.FirstOrDefault().Vocabularies;

        }
    }
}
