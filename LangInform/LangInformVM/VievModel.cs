using LangInformModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangInformVM
{
    public class ViewModel
    {
        public ViewModel(string dataPath)
        {
            db = new MainEntities(dataPath);
        }
        MainEntities db;

        private List<Language> _languages = null;
        public List<Language> Languages
        {
            get
            {
                if (_languages == null)
                {
                    _languages = db.Query<Language>("select * from language");
                }
                return _languages;
            }
        }

    }
}
