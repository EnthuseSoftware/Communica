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


        //public List<Language> GenerateLanguages()
        //{
        //    Random rnd = new Random();
        //    List<Language> languages = new List<Language>();
        //    List<FileInfo> sounds = new List<FileInfo>();
        //    List<FileInfo> pictures = new List<FileInfo>();
        //    sounds = new DirectoryInfo("C:\\LangLearnData\\voices\\").GetFiles().ToList();
        //    pictures = new DirectoryInfo("C:\\LangLearnData\\pictures\\").GetFiles().ToList();
        //    entities.GetItems();
        //    for (int i = 1; i <= 2; i++)
        //    {
        //        Language lang = new Language() { Id = Guid.NewGuid(), Name = "Language " + i, Note = "Language note" };
        //        List<Level> levels = new List<Level>();
        //        for (int j = 1; j <= 1; j++)
        //        {
        //            Level level = new Level() { Id = Guid.NewGuid(), Name = "Level " + j, Note = "Level note" };
        //            List<Unit> units = new List<Unit>();
        //            for (int k = 1; k <= 2; k++)
        //            {
        //                Unit unit = new Unit() { Id = Guid.NewGuid(), Name = "Unit " + k, Note = "Unit note" };
        //                List<Lesson> lessons = new List<Lesson>();
        //                for (int m = 1; m <= 2; m++)
        //                {
        //                    Lesson lesson = new Lesson() { Id = Guid.NewGuid(), Name = "Lesson " + m, Note = "Lesson note" };
        //                    List<Vocabulary> vocabularies = new List<Vocabulary>();
        //                    List<Scene> scenes = new List<Scene>();
        //                    // List<SentenceStructure> sentenceStructures = new List<SentenceStructure>();
        //                    int vocabCount = rnd.Next(1, 3);

        //                    for (int v = 1; v <= vocabCount; v++)
        //                    {
        //                        List<MyItem> words = new List<MyItem>();
        //                        for (int a = 1; a <= 12; a++)
        //                        {
        //                            //Word word = new Word() { Id = Guid.NewGuid(), Name = "Word " + a, Note = "Word note", Picture = new System.Drawing.Bitmap(pictures[a - 1].FullName, true), Sound = new System.Media.SoundPlayer(sounds[ a - 1].FullName) };

        //                            words.Add(entities.Items[a - 1]);
        //                        }
        //                        vocabularies.Add(new Vocabulary() { Id = Guid.NewGuid(), Name = "Vocab " + v, Words = words });
        //                    }

        //                    int sceneCount = 0;// rnd.Next(1, 4);
        //                    for (int s = 1; s <= sceneCount; s++)
        //                    {

        //                        Scene scene = new Scene() { Id = Guid.NewGuid(), Name = "Scene " + s, Note = "Scene note", Sounds = null };
        //                        if (s == 1)
        //                            scene.Picture = Properties.Resources._1;
        //                        else if (s == 2)
        //                            scene.Picture = Properties.Resources._2;
        //                        else
        //                            scene.Picture = Properties.Resources._3;
        //                        scenes.Add(scene);
        //                    }
        //                    lesson.Vocabularies = vocabularies;
        //                    lesson.Scenes = scenes;
        //                    //lesson.SentenceStructure = sentenceStructures;
        //                    lessons.Add(lesson);
        //                }
        //                unit.Lessons = lessons;
        //                units.Add(unit);
        //            }
        //            level.Units = units;
        //            levels.Add(level);
        //        }
        //        lang.Levels = levels;
        //        languages.Add(lang);
        //    }
        //    return languages;
        //}
    }
}
