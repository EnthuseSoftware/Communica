using LangInformModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangInformVM
{
    public class VocabularyLogic : INotifyPropertyChanged
    {
        mainEntities1 entities = new mainEntities1();
        Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day);

        Lesson _selectedLesson;
        public Lesson SelectedLesson
        {
            get { return _selectedLesson; }
            set
            {
                _selectedLesson = value;
                NotifyPropertyChanged("SelectedLesson");
            }
        }

        List<Language> _languages;
        public List<Language> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                NotifyPropertyChanged("Languages");
            }

        }

        Activity _activity;
        public Activity Activity
        {
            get { return _activity; }
            set
            {
                var oldActivity = _activity;
                _activity = value;

                ActivityChanged(this, new ActivityChangedEventArgs() { OldValue = oldActivity, NewValue = _activity });
            }
        }

       Lesson _lessonToBoard;
        public Lesson LessonToBoard { get { return _lessonToBoard; } set { _lessonToBoard = value; } }


        public void GetItemsForThisActivity()
        {
            if (_activity == Activity.Learn)
            {
                LessonToBoard = SelectedLesson;
            }
            else if (_activity == Activity.Practice)
            {

            }
        }

        public void PrepareVocabularyItemsForPracticeAndQuiz()
        {
            foreach (var vocabulary in _selectedLesson.Vocabularies)
            {
                int wordsCount = vocabulary.Words.Count;
                List<Word> words = new List<Word>();
                List<int> checkList = new List<int>();
                int counter = 0;
                var arrWords = vocabulary.Words.ToArray();
                foreach (var word in vocabulary.Words)
                {
                    counter++;
                    Word newWord = new Word();
                    newWord.Picture = word.Picture;
                    while (true)
                    {
                        int rndDigit = random.Next(0, wordsCount);
                        if (!checkList.Contains(rndDigit))
                        {
                            checkList.Add(rndDigit);
                            newWord.Sound = arrWords[rndDigit].Sound;
                            newWord.SoundVol = arrWords[rndDigit].SoundVol;
                            break;
                        }
                    }
                }
                var newVocabulary = new Vocabulary() { Name = vocabulary.Name, Description = vocabulary.Description, Id = vocabulary.Id };
                newVocabulary.Words = words;
                _lessonToBoard.Vocabularies.Add(newVocabulary);
            }
        }

        public event EventHandler ActivityChanged;

        public void Items()
        {

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

    public class ActivityChangedEventArgs : EventArgs
    {
        public Activity OldValue { get; set; }
        public Activity NewValue { get; set; }
    }

    public enum Activity
    {
        Learn = 0,
        Practice = 1,
        Review = 2,
        Quiz = 3
    }
}
