using LangInformModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LangInformVM
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel(string dataPath)
        {
            db = new MainEntities(dataPath);
            IsLanguagesDirty = true;
        }
        MainEntities db;

        public void CloseSession()
        {
            db.Close();
        }

        public int InsertData(object obj, Type type)
        {
            return db.Insert(obj, type);
        }

        public int InsertLanguage(object obj)
        {
            int result = db.Insert(obj, typeof(Language));
            IsLanguagesDirty = true;
            var languages = Languages;
            return result;
        }

        public int DeleteData(object obj)
        {
            return db.Delete(obj);
        }



        public List<T> GetData<T>(string query) where T: new()
        {
            return db.Query<T>(query);
        }

        public bool IsLanguagesDirty { get; set; }

        private ObservableCollection<Language> _languages = null;
        public ObservableCollection<Language> Languages
        {
            get
            {
                if (IsLanguagesDirty || _languages == null)
                {
                    _languages = new ObservableCollection<Language>(db.Query<Language>("select * from language"));
                }
                return _languages;
            }
        }

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
