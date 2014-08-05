using LangInformModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LangInformVM
{
    //public class BaseViewModel : INotifyPropertyChanged
    //{

    //    public BaseViewModel(MainEntities model)
    //    {
    //        Model = model;
    //    }
    //    public MainEntities Model { get; private set; }

    //    Language language;
    //    public Language SelectedLanguage { get { return language; } set { language = value; NotifyPropertyChanged();} }

    //    Level level;
    //    public Level SelectedLevel { get { return level; } set { level = value; NotifyPropertyChanged(); } }

    //    Unit unit;
    //    public Unit Unit { get { return unit; } set { unit = value; NotifyPropertyChanged(); } }

    //    Lesson lesson;
    //    public Lesson Lesson { get { return lesson; } set { lesson = value; NotifyPropertyChanged(); } }



    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    //    {
    //        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    //    }

    //    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, e);
    //        }
    //    }
    //}
}
