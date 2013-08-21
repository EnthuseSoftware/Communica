using LangInformModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace LangInformVM
{
    public class ViewModel
    {
        public ViewModel(string dataPath)
        {
            db = new MainEntities(dataPath);
            IsLanguagesDirty = true;
        }
        MainEntities db;

        public string InsertData(object obj, Type type)
        {
            db.Insert(obj, type);
            return db.SaveTransactionPoint();
        }

        public int DeleteData(object obj)
        {
            return db.Delete(obj);
        }

        public bool IsLanguagesDirty { get; set; }

        public List<T> GetData<T>(string query) where T: new()
        {
            return db.Query<T>(query);
        }

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
    }
}
