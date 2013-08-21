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

namespace Icons.Controls
{
    /// <summary>
    /// Interaction logic for ExclamationMark.xaml
    /// </summary>
    public partial class ExclamationMark : UserControl
    {
        public ExclamationMark()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 1;
            anim.To = .2;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            back.BeginAnimation(Label.OpacityProperty, anim);
        }
    }
}
