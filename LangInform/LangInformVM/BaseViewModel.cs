using LangInformModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LangInformVM
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public BaseViewModel(MainEntities model)
        {
            Model = model;
        }
        protected MainEntities Model { get; private set; }

        public ObservableCollection<Language> Languages { get { return new ObservableCollection<Language>(Model.Query<Language>("SELECT * FROM LANGUAGE;")); } }


        Language selectedLanguage;
        public Language SelectedLanguage { get { return selectedLanguage; } set { selectedLanguage = value; NotifyPropertyChanged();} }

        Level selectedLevel;
        public Level SelectedLevel { get { return selectedLevel; } set { selectedLevel = value; NotifyPropertyChanged(); } }

        Unit selectedUnit;
        public Unit Unit { get { return selectedUnit; } set { selectedUnit = value; NotifyPropertyChanged(); } }

        Lesson selectedLesson;
        public Lesson SelectedLesson { get { return selectedLesson; } set { selectedLesson = value; NotifyPropertyChanged(); } }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
