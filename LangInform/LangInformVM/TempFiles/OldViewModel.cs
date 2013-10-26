using LangInformModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#pragma warning disable
namespace LangInformVM
{


    //public class OldViewModel
    //{
    //    public Random rnd = new Random();

    //    public OldVocabLogic VocabLogic { get; set; }

    //    //MainEntities entities = null;//new MainEntities();

    //    SelectedLesson _currentLesson;
    //    public SelectedLesson CurrentLesson
    //    {
    //        get { return _currentLesson; }
    //        set
    //        {
    //            _currentLesson = value;
    //            if (CurrentLessonChanged != null)
    //            {
    //                CurrentLessonChanged(this, null);
    //            }
    //        }
    //    }


    //    public OldViewModel()
    //    {
    //        //Languages = entities.Languages.ToList();
    //        VocabLogic = new OldVocabLogic(this);
    //    }

    //    public void SaveChanges()
    //    {
    //        //var ch = entities.ChangeTracker;
    //        //entities.SaveChanges();
    //    }

    //    public IEnumerable<Language> Languages { get; set; }

    //    public Language CurrentLanguage { get; set; }

    //    public Level CurrentLevel { get; set; }

    //    public Unit CurrentUnit { get; set; }

    //    public event EventHandler CurrentLessonChanged;

    //}

    //public class OldVocabLogic : INotifyPropertyChanged
    //{

    //    int _total;
    //    public int Total { get { return _total; } set { _total = value; NotifyPropertyChanged("Total"); } }

    //    int _rightAnswers;
    //    public int RightAnswers { get { return _rightAnswers; } set { _rightAnswers = value; NotifyPropertyChanged("RightAnswers"); } }

    //    int _wrongAnswers;
    //    public int WrongAnswers { get { return _wrongAnswers; } set { _wrongAnswers = value; NotifyPropertyChanged("WrongAnswers"); } }

    //    VocabularyActivity _currentActivity = VocabularyActivity.Learn;
    //    public VocabularyActivity Currentactivity { get { return _currentActivity; } set { _currentActivity = value; alreadyAsked.Clear(); } }

    //    Vocabulary _currentVocabulary;
    //    public Vocabulary CurrentVocabulary { get { return _currentVocabulary; } set { _currentVocabulary = value; Total = _currentVocabulary.Words.Count; } }

    //    OldViewModel _viewModel = null;

    //    public OldVocabLogic(OldViewModel viewModel)
    //    {
    //        _rnd = viewModel.rnd;
    //        _viewModel = viewModel;
    //    }

    //    Random _rnd;

    //    int currentPlaying = -1;

    //    int wrongAnswers = 0;

    //    //public int GetRandomItemForPractice(int playThis = -1)
    //    //{
    //    //    int count = CurrentVocabulary.Words.Where(w => w.IncludetoExam == 1).Count();
    //    //    int picked = _rnd.Next(0, count);
    //    //    if (playThis != -1)
    //    //    {
    //    //        picked = playThis;
    //    //    }
    //    //    currentPlaying = picked;
    //    //    return currentPlaying;
    //    //}

    //    List<int> alreadyAsked = new List<int>();

    //    //public int GetRandomItemForQuiz(out bool done)
    //    //{
    //    //    int count = CurrentVocabulary.Words.Where(w => w.IncludetoExam == 1).Count();
    //    //    int picked = _rnd.Next(0, count);
    //    //    if (count == alreadyAsked.Count)
    //    //    {
    //    //        done = true;
    //    //        return -1;
    //    //    }
    //    //    while (true)
    //    //    {
    //    //        if (!alreadyAsked.Contains(picked))
    //    //        {
    //    //            break;
    //    //        }
    //    //        picked = _rnd.Next(0, count);
    //    //    }
    //    //    alreadyAsked.Add(picked);
    //    //    currentPlaying = picked;
    //    //    done = false;
    //    //    return currentPlaying;
    //    //}

    //    //public Result CheckAnswer(int selectedItemNo)
    //    //{
    //    //    Result result = new Result();
    //    //    result.Highlight = false;
    //    //    if (selectedItemNo == currentPlaying)
    //    //    {
    //    //        if (Currentactivity == VocabularyActivity.Quiz)
    //    //        {
    //    //            RightAnswers++;
    //    //        }
    //    //        wrongAnswers = 0;
    //    //        result.Correct = true;
    //    //        return result;
    //    //    }
    //    //    else
    //    //    {
    //    //        if (Currentactivity == VocabularyActivity.Quiz)
    //    //        {
    //    //            WrongAnswers++;
    //    //        }
    //    //        wrongAnswers++;
    //    //        result.Correct = false;
    //    //        if (wrongAnswers == 3)
    //    //        {
    //    //            result.Highlight = true;
    //    //            wrongAnswers = 0;
    //    //        }
    //    //        return result;
    //    //    }
    //    //}

    //    #region INotifyPropertyChanged Members
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void NotifyPropertyChanged(String info)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(info));
    //        }
    //    }
    //    #endregion
    //}

    //public class Result
    //{
    //    public bool Correct { get; set; }

    //    public bool Highlight { get; set; }
    //}

    //public enum VocabularyActivity
    //{
    //    Learn = 0,
    //    Practice = 1,
    //    Review = 2,
    //    Quiz = 3
    //}
}
#pragma warning restore