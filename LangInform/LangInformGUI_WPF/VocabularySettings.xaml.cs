using LangInformGUI.Controls;
using LangInformModel;
using LangInformVM;
using System;
using System.Collections.Generic;
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

namespace LangInformGUI
{
    /// <summary>
    /// Interaction logic for VocabularySettings.xaml
    /// </summary>
    public partial class VocabularySettings : Window
    {
        public VocabularySettings(ViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            level = viewModel.CurrentLesson.Unit.Level;
        }

        Level level = null;
        ViewModel _viewModel;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myTree.tree.ItemsSource = CreateTree(level);
        }

        List<MyTreeViewItem> CreateTree(Level level)
        {
            List<MyTreeViewItem> tree = new List<MyTreeViewItem>();
            MyTreeViewItem levelBranch = new MyTreeViewItem(level.Name);
            levelBranch.IsInitiallySelected = true;
            tree.Add(levelBranch);
            foreach (Unit unit in level.Units)
            {
                var unitBranch = new MyTreeViewItem(unit.Name);
                foreach (Lesson lesson in unit.Lessons)
                {
                    var lessonBranch = new MyTreeViewItem(lesson.Name);
                    foreach (Vocabulary vocab in lesson.Vocabularies)
                    {
                        var vocabBranch = new MyTreeViewItem(vocab.Name);
                        
                        foreach (Word word in vocab.Words)
                        {
                            var wordBranch = new MyTreeViewItem(word.Name, word);
                            vocabBranch.Children.Add(wordBranch);
                        }
                        vocabBranch.IsChecked = vocabBranch.GetState();
                        lessonBranch.Children.Add(vocabBranch);
                    }
                    lessonBranch.IsChecked = lessonBranch.GetState();
                    unitBranch.Children.Add(lessonBranch);
                }
                unitBranch.IsChecked = unitBranch.GetState();
                levelBranch.Children.Add(unitBranch);
            }
            levelBranch.IsChecked = levelBranch.GetState();
            levelBranch.Initialize();
            return tree;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveChanges();
        }
    }
}
