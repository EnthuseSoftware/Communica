using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace LangInformVM
{



    public abstract class EntityBase
    {
        string _name;
        public string Name { get { return _name; } set { _name = value; } }

        public List<EntityBase> Children { get; set; }

        public abstract bool? Selected { get; set; }

        Helper helper = new Helper();

        public EntityBase Parent { get; set; }

        public virtual void ChangeSelected(bool? selected)
        { 
        
        }
    }



    public class UnitLimited : EntityBase, INotifyPropertyChanged
    {
        public UnitLimited()
        {
            Children = new List<EntityBase>();
        }

        Helper helper = new Helper();

        bool? _selected = false;
        public override bool? Selected { get { return _selected; } set { _selected = value; NotifyPropertyChanged("Selected"); helper.ChangeParentState(null, this); } }

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



    public class LessonLimited : EntityBase, INotifyPropertyChanged
    {

        public void SelectAll(bool select)
        {
            this._selected = select;
            NotifyPropertyChanged("Selected");
            foreach (VocabularyLimited vocab in this.Children)
            {
                vocab.SelectAll(select);
            }
        }

        public LessonLimited()
        {
            Children = new List<EntityBase>();
        }

        Helper helper = new Helper();

        bool? _selected = false;
        public override bool? Selected { get { return _selected; } set { _selected = value; NotifyPropertyChanged("Selected"); helper.ChangeParentState(Parent, this); } }

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



    public class VocabularyLimited : EntityBase, INotifyPropertyChanged
    {
        public VocabularyLimited()
        {
            Children = new List<EntityBase>();
        }

        public void SelectAll(bool select)
        {
            this._selected = select;
            NotifyPropertyChanged("Selected");
            foreach (WordLimited word in this.Children)
            {
                word.Selected = select;
            }
        }

        Helper helper = new Helper();

        bool? _selected = false;
        public override bool? Selected { get { return _selected; } set { _selected = value; NotifyPropertyChanged("Selected"); helper.ChangeParentState(Parent, this); helper.ChangeChildren(this,_selected); } }

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



    public class WordLimited : EntityBase, INotifyPropertyChanged
    {
        Helper helper = new Helper();
        bool? _selected = false;
        public override bool? Selected { get { return _selected; } set { _selected = value; NotifyPropertyChanged("Selected"); helper.ChangeParentState(Parent, this); } }

        public new void ChangeSelected(bool? selected)
        {
            _selected = selected;
            NotifyPropertyChanged("Selected");
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



    public class Helper
    {
        public void ChangeParentState(EntityBase parent, EntityBase self)
        {
            if (self is UnitLimited)
            {
                UnitLimited unit = (UnitLimited)self;
                if (unit.Selected == true)
                {
                    foreach (LessonLimited lesson in unit.Children)
                    {
                        lesson.SelectAll(true);
                    }
                }
            }
            else if (self is LessonLimited)
            {
                bool selected = false;
                LessonLimited lesson = (LessonLimited)self;
                UnitLimited unit = (UnitLimited)parent;
                if (lesson.Selected == true)
                {
                    selected = true;
                }
                else if (lesson.Selected == null)
                {
                    bool allFalse = lesson.Children.All(v => v.Selected == false);
                    if (allFalse)
                    {
                        lesson.Selected = false;
                    }
                    else
                        unit.Selected = null;
                    return;
                }
                bool allSame = unit.Children.All(l => l.Selected == selected);
                if (allSame)
                {
                    unit.Selected = selected;
                }
                else
                {
                    unit.Selected = null;
                }
            }
            else if (self is VocabularyLimited)
            {
                bool selected = false;
                VocabularyLimited vocab = (VocabularyLimited)self;
                LessonLimited lesson = (LessonLimited)parent;
                if (vocab.Selected == true)
                {
                    selected = true;
                }
                else if (vocab.Selected == null)
                {
                    bool allFalse = vocab.Children.All(v => v.Selected == false);
                    if (allFalse)
                    {
                        vocab.Selected = false;
                    }
                    else
                        lesson.Selected = null;
                    return;
                }
                bool allSame = lesson.Children.All(l => l.Selected == selected);
                if (allSame)
                {
                    lesson.Selected = selected;
                }
                else
                {
                    lesson.Selected = null;
                }
            }
            else if (self is WordLimited)
            {
                bool selected = false;
                WordLimited word = (WordLimited)self;
                VocabularyLimited vocab = (VocabularyLimited)parent;
                if (word.Selected == true)
                {
                    selected = true;
                }

                bool allSame = vocab.Children.All(l => l.Selected == selected);
                if (allSame)
                {
                    vocab.Selected = selected;
                }
                else
                {
                    vocab.Selected = null;
                }
            }
        }

        public void ChangeChildren(EntityBase entity, bool? selected)
        {
            foreach (EntityBase child in entity.Children)
            {
                child.ChangeSelected(selected);
            }
        }
    }

}
