using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace LangInformGUI
{


    public abstract class BasicObj
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }

    public class Language : BasicObj
    {
        List<Level> _levels = new List<Level>();
        public List<Level> Levels
        {
            get { return _levels; }
            set { _levels = value; }
        }
    }

    public class Level : BasicObj
    {
        List<Unit> _units = new List<Unit>();
        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }
    }

    public class Unit : BasicObj
    {
        List<Lesson> _lessons = new List<Lesson>();
        public List<Lesson> Lessons
        {
            get { return _lessons; }
            set { _lessons = value; }
        }
    }

    public class Lesson : BasicObj
    {
        List<Word> _vocabulary = new List<Word>();
        public List<Word> Vocabulary
        {
            get { return _vocabulary; }
            set { _vocabulary = value; }
        }

        List<Scene> _scenes = new List<Scene>();
        public List<Scene> Scenes
        {
            get { return _scenes; }
            set { _scenes = value; }
        }

        List<SentenceStructure> _sentenceStructures = new List<SentenceStructure>();
        public List<SentenceStructure> SentenceStructure
        {
            get { return _sentenceStructures; }
            set { _sentenceStructures = value; }
        }
    }

    public class Scene : BasicObj
    {
        public Bitmap Picture { get; set; }
        public Dictionary<int, SoundPlayer> Sounds { get; set; }
    }

    public class Word : BasicObj
    {
        public Bitmap Picture { get; set; }
        public SoundPlayer Sound { get; set; }
    }

    public class SentenceStructure : BasicObj
    {

    }

}
