using LangInformModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangInformVM
{
    public class EnterData
    {

        //MainEntities entities = new MainEntities();
        //public void InsertLanguage()
        //{
        //    Converter con = new Converter();
        //    var selectedLanguage = entities.Languages.FirstOrDefault();
        //    var selectedLevel = selectedLanguage.Levels.FirstOrDefault(l => l.Name == "Level 1");
        //    if (selectedLevel != null)
        //    {
        //        var selectedUnit = selectedLevel.Units.FirstOrDefault();
        //        var dirs = new DirectoryInfo("C:\\lang").GetDirectories();
        //        foreach (DirectoryInfo l in dirs)
        //        {
        //            SelectedLesson _lesson = new SelectedLesson() { Name = l.Name, Id = Guid.NewGuid().ToString() };
        //            foreach (var dir in l.GetDirectories())
        //            {
        //                var vocab = new Vocabulary() { Id = Guid.NewGuid().ToString(), Name = dir.Name, LessonId = _lesson.Id };

        //                var soundFiles = dir.GetFiles("*.wav");
        //                var imageFiles = dir.GetFiles("*.jpeg");

        //                var list = from s in soundFiles
        //                           join i in imageFiles on s.Name.Replace(s.Extension, "") equals i.Name.Replace(i.Extension, "")
        //                           select new Word()
        //                           {
        //                               Id = Guid.NewGuid().ToString(),
        //                               Name = s.Name.Replace(s.Extension, ""),
        //                               Picture = con.BitmapToByte(new System.Drawing.Bitmap(i.FullName)),
        //                               Sound = con.SoundToByte(s.FullName),
        //                               SoundVol = 100,
        //                               IncludetoExam = 1,
        //                               VocabularyId = vocab.Id
        //                           };
        //                vocab.Words = list.ToList();
        //                _lesson.Vocabularies.Add(vocab);
        //            }
        //            //add Scene to the selectedLesson
        //            foreach (var file in l.GetFiles("*.jpeg"))
        //            {
        //                var scene = new Scene()
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    Name = file.Name.Replace(file.Extension, ""),
        //                    LessonId = _lesson.Id,
        //                    Picture = con.BitmapToByte(new System.Drawing.Bitmap(file.FullName))
        //                };
        //                _lesson.Scenes.Add(scene);
        //            }
        //            selectedUnit.Lessons.Add(_lesson);
        //        }



        //        selectedLevel.Units.Add(selectedUnit);
        //    }
        //    try
        //    {
        //        //SOLVE:entities.SaveChanges();
        //    }


        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }

        //}

        public void TestGetData()
        {
            DataProvider dp = new DataProvider();
            dp.GetItems();
        }
    }
}
