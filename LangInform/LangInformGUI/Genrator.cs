using LangInformModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangInformGUI
{
    /// <summary>
    /// Generates items for testing
    /// </summary>
    public class Logic
    {
        public event EventHandler LessonChanged;

        public Language SelectedLanguage { get; set; }
        public Level SelelectedLevel { get; set; }
        public Unit SelelectedUnit { get; set; }
        Lesson _lesson;
        public Lesson SelelectedLesson
        {
            get { return _lesson; }
            set
            {
                _lesson = value;
                if (LessonChanged != null)
                {
                    LessonChanged(value, null);
                }
            }
        }

        DataProvider entities = new DataProvider();
    }
}
