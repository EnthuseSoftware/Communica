using LangInformModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangInformVM
{
    public class SceneViewModel : BaseViewModel
    {
        public SceneViewModel(MainEntities model)
            : base(model)
        { }

        Scene selectedScene;
        public Scene SelectedScene { get { return selectedScene; } set { selectedScene = value; NotifyPropertyChanged();} }


    }
}
