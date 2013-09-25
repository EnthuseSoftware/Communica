using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using System.Collections.ObjectModel;

namespace LangInformModel
{
    public class MainEntities : SQLiteConnection
    {

        public MainEntities(string dbPath)
            : base(dbPath)
        {
            FileInfo f = new FileInfo(dbPath);
            if (!f.Exists)
            {
                this.CreateTable<Language>();
                this.CreateTable<LanguageToLevel>();
                this.CreateTable<Level>();
                this.CreateTable<LevelToUnit>();
                this.CreateTable<Unit>();
                this.CreateTable<UnitToLesson>();
                this.CreateTable<Lesson>();
                this.CreateTable<LessonToActivity>();
                this.CreateTable<Scene>();
                this.CreateTable<ScenePicture>();
                this.CreateTable<SceneItem>();
                this.CreateTable<Phrase>();
                this.CreateTable<Vocabulary>();
                this.CreateTable<VocabularyToWord>();
                this.CreateTable<Word>();
                this.CreateTable<WordToMeaning>();
                this.CreateTable<Meaning>();
                this.CreateTable<SentenceBuilding>();
                this.CreateTable<SentenceBuildingItemPicture>();
                this.CreateTable<SentenceBuildingItem>();
                this.CreateTable<SentenceToWord>();
            }
            ModelManager.Db = this;
        }
    }

    public class ModelManager
    {
        static MainEntities _db;
        public static MainEntities Db
        {
            get
            {
                if (_db == null)
                    throw new Exception("Instance of MainEntities not created yet.");
                return _db;
            }
            set
            {
                _db = value;
            }
        }
    }

    public class Language
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int InsertLevel(Level level)
        {
            int result = ModelManager.Db.Insert(level, typeof(Level));
            ModelManager.Db.Insert(new LanguageToLevel() { LanguageId = this.Id, LevelId = level.ID }, typeof(LanguageToLevel));
            IsLevelsDirty = true;
            return result;
        }
        [Ignore]
        public bool IsLevelsDirty { get; set; }

        private ObservableCollection<Level> _levels;
        [Ignore]
        public ObservableCollection<Level> Levels
        {
            get
            {
                if (IsLevelsDirty || _levels == null)
                {
                    try
                    {
                        var tempLevels = ModelManager.Db.Query<Level>
                            ("select lev.Id, lev.Name, lev.Description from languagetolevel as ll inner join level as lev on ll.levelid=lev.id where ll.languageid='" + this.Id.ToString() + "'");
                        tempLevels.ForEach((a) => { a.SetLanguage(this); });
                        _levels = new ObservableCollection<Level>(tempLevels);
                        IsLevelsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                return _levels;
            }
        }
    }

    public class LanguageToLevel
    {
        public Guid LanguageId { get; set; }
        public Guid LevelId { get; set; }
    }

    public class Level
    {
        [PrimaryKey]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int InsertUnit(Unit unit)
        {
            int result = ModelManager.Db.Insert(unit, typeof(Unit));
            ModelManager.Db.Insert(new LevelToUnit() { LevelId = this.ID, UnitId = unit.Id }, typeof(LevelToUnit));
            IsUnitsDirty = true;
            return result;
        }

        [Ignore]
        public bool IsUnitsDirty { get; set; }

        ObservableCollection<Unit> _units = null;
        [Ignore]
        public ObservableCollection<Unit> Units
        {
            get
            {
                if (IsUnitsDirty || _units == null)
                {
                    var tempUnits = ModelManager.Db.Query<Unit>("select u.Id, u.Name, u.Description from LevelToUnit as lu inner join unit as u on lu.unitid=u.id where lu.Levelid='" + this.ID + "'");
                    tempUnits.ForEach((u) =>
                    {
                        u.SetLevel(this);
                    });
                    _units = new ObservableCollection<Unit>(tempUnits);
                }
                return _units;
            }
        }

        public void SetLanguage(Language language)
        {
            _language = language;
        }
        Language _language;
        [Ignore]
        public Language Language
        {
            get { return _language; }
        }
    }

    public class LevelToUnit
    {
        public Guid LevelId { get; set; }
        public Guid UnitId { get; set; }
    }

    public class Unit
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string LevelId { get; set; }

        public int InsertLesson(Lesson lesson)
        {
            int result = ModelManager.Db.Insert(lesson, typeof(Lesson));
            ModelManager.Db.Insert(new UnitToLesson() { UnitId = this.Id, LessonId = lesson.Id }, typeof(UnitToLesson));
            IsLessonsDirty = true;
            return result;
        }
        [Ignore]
        public bool IsLessonsDirty { get; set; }

        ObservableCollection<Lesson> _lessons = null;
        [Ignore]
        public ObservableCollection<Lesson> Lessons
        {
            get
            {
                if (IsLessonsDirty || _lessons == null)
                {
                    var tempLessons = ModelManager.Db.Query<Lesson>("select l.Id, l.Name, l.Description from unittolesson as ul inner join lesson as l on ul.LessonId = l.id where ul.UnitId = '" + this.Id + "'");
                    tempLessons.ForEach((l) =>
                    {
                        l.SetUnit(this);
                    });
                    _lessons = new ObservableCollection<Lesson>(tempLessons);
                }
                return _lessons;
            }
        }
        public void SetLevel(Level level)
        {
            _level = level;
        }

        Level _level;
        [Ignore]
        public Level Level { get { return _level; } }
    }

    public class UnitToLesson
    {
        public Guid UnitId { get; set; }
        public Guid LessonId { get; set; }
    }

    public class Lesson
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public void SetUnit(Unit unit)
        {
            _unit = unit;
        }

        Unit _unit;
        [Ignore]
        public Unit Unit { get { return _unit; } }

        List<Scene> _scenes = null;
        [Ignore]
        public IList<Scene> Scenes
        {
            get
            {
                if (_scenes == null)
                {
                    var tempScenes = ModelManager.Db.Query<Scene>("select s.id, s.Name, s.Description, s.PictureId from LessonToActivity as ls " +
                        "inner join Scene as s on ls.SceneId = s.Id where ls.LessonId = '" + this.Id + "' and ls.SceneId is not null");
                    tempScenes.ForEach((l) =>
                    {
                        l.SetLesson(this);
                    });
                    _scenes = new List<Scene>(tempScenes);
                }
                return _scenes;
            }
        }

        List<Vocabulary> _vocabularies = null;
        [Ignore]
        public IList<Vocabulary> Vocabularies
        {
            get
            {
                if (_vocabularies == null)
                {
                    var tempVocabularies = ModelManager.Db.Query<Vocabulary>("select v.id, v.Name, v.Description from LessonToActivity as ls " +
                        "inner join Vocabulary as v on ls.VocabularyId = v.Id where ls.LessonId = '" + this.Id + "' and ls.VocabularyId is not null;");
                    tempVocabularies.ForEach((l) =>
                    {
                        l.SetLesson(this);
                    });
                    _vocabularies = new List<Vocabulary>(tempVocabularies);
                }
                return _vocabularies;
            }
        }

        ObservableCollection<SentenceBuilding> _sentenceBuildings = null;
        [Ignore]
        public ObservableCollection<SentenceBuilding> SentenceBuildings
        {
            get
            {
                if (_sentenceBuildings == null)
                {
                    var tempSents = ModelManager.Db.Query<SentenceBuilding>("select s.id, s.Name, s.Description from LessonToActivity as ls " +
                        "inner join SentenceBuilding as s on ls.SentBuildingId = s.Id where ls.LessonId = '" + this.Id + "' and ls.SentBuildingId is not null;");
                    tempSents.ForEach((l) =>
                    {
                        l.SetLesson(this);
                    });
                    _sentenceBuildings = new ObservableCollection<SentenceBuilding>(tempSents);
                }
                return _sentenceBuildings;
            }
        }
    }

    public class LessonToActivity
    {
        public Guid LessonId { get; set; }
        public Guid VocabularyId { get; set; }
        public Guid SceneId { get; set; }
        public Guid SentBuildingId { get; set; }
    }

    public class Scene
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PictureId { get; set; }

        ScenePicture _scenePicture;
        [Ignore]
        public ScenePicture ScenePicture
        {
            get
            {
                if (_scenePicture == null)
                {
                    _scenePicture = ModelManager.Db.Query<ScenePicture>("select * from ScenePicture where id = '" + PictureId.ToString() + "';").FirstOrDefault();
                }
                return _scenePicture;
            }
            set
            {
                _scenePicture = value;
            }
        }

        ObservableCollection<SceneItem> _sceneItems;
        [Ignore]
        public ObservableCollection<SceneItem> SceneItems
        {
            get
            {
                if (_sceneItems == null)
                {
                    var tempSceneItems = ModelManager.Db.Query<SceneItem>("select * from SceneItem where SceneId = '" + Id.ToString() + "';");
                    tempSceneItems.ForEach((s) =>
                    {
                        s.SetScene(this);
                    });
                    _sceneItems = new ObservableCollection<SceneItem>(tempSceneItems);
                }
                return _sceneItems;
            }
        }


        public void SetLesson(Lesson lesson)
        {
            _lesson = lesson;
        }
        Lesson _lesson;
        [Ignore]
        public Lesson Lesson { get { return _lesson; } }

    }

    public class ScenePicture
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public byte[] Picture { get; set; }
    }

    public class SceneItem : INotifyPropertyChanged
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        double _xPos;
        double _yPos;
        public double XPos { get { return _xPos; } set { _xPos = value; NotifyPropertyChanged(); } }
        public double YPos { get { return _yPos; } set { _yPos = value; NotifyPropertyChanged(); } }
        public double Size { get; set; }
        public bool IsRound { get; set; }
        public int Order { get; set; }
        public Guid SceneId { get; set; }
        public Guid PhraseId { get; set; }
        Phrase _phrase;
        [Ignore]
        public Phrase Phrase
        {
            get
            {
                if (_phrase == null)
                {
                    _phrase = ModelManager.Db.Query<Phrase>("select * from Phrase where Id = '" + PhraseId.ToString() + "';").FirstOrDefault();
                }
                return _phrase;
            }
            set
            {
                _phrase = value;
            }
        }

        public void SetScene(Scene scene)
        {
            _scene = scene;
        }

        Scene _scene;

        [Ignore]
        public Scene Scene { get { return _scene; } }


        public override string ToString()
        {
            return Convert.ToInt32(XPos) + " - " + Convert.ToInt32(YPos);
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

    public class Phrase
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Text { get; set; }

        byte[] _sound;
        public byte[] Sound { get { return _sound; } set { _sound = value; if (_sound != null) { PrepareAudio(); } } }

        #region NAudio stuff

        [Ignore]
        public TimeSpan SoundLength { get; set; }

        public void Play(int playFrom = 0)
        {
            if (_sound == null)
            {
                throw new InvalidOperationException("Phrase.Sound is not initialized yet.");
            }
            if (audioOutput.PlaybackState == PlaybackState.Playing)
            {
                audioOutput.Pause();
            }
            waveReader.CurrentTime = TimeSpan.FromMilliseconds(playFrom);
            audioOutput.Play();
        }

        public void StopPlaying()
        {
            if (_sound == null)
                return;

            if (audioOutput.PlaybackState != PlaybackState.Stopped)
            {
                audioOutput.Pause();
            }
        }

        private DirectSoundOut audioOutput;
        private WaveFileReader waveReader;
        void PrepareAudio()
        {
            waveReader = new WaveFileReader(Helper.byteArrayToStream(this.Sound));
            SoundLength = waveReader.TotalTime;
            var wc = new WaveChannel32(waveReader);
            audioOutput = new DirectSoundOut();
            audioOutput.Init(wc);
        }
        #endregion

    }



    public class Vocabulary
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<Word> Words = new List<Word>();

        public void SetLesson(Lesson lesson)
        {
            _lesson = lesson;
        }
        Lesson _lesson;
        [Ignore]
        public Lesson Lesson
        {
            get
            {
                return _lesson;
            }
        }
    }

    public class VocabularyToWord
    {
        public Guid VocabularyId { get; set; }
        public Guid WordId { get; set; }
        public bool DoNotIncludeToExam { get; set; }
    }

    public class Word
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Sound { get; set; }
        public int SoundVol { get; set; }

        public void SetVocabulary(Vocabulary vocabulary)
        {
            _vocabulary = vocabulary;
        }

        Vocabulary _vocabulary;
        [Ignore]
        public Vocabulary Vocabulary { get { return _vocabulary; } }


        ObservableCollection<Meaning> _meanings = null;
        [Ignore]
        public ObservableCollection<Meaning> Meanings
        {
            get
            {
                return _meanings;
            }
        }
    }

    public class WordToMeaning
    {
        public Guid WordId { get; set; }
        public Guid MeaningId { get; set; }
    }

    public class Meaning
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public IList<Word> Words = new List<Word>();
    }



    public class SentenceBuilding
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<SentenceBuildingItem> SentenceBuildingItems = new List<SentenceBuildingItem>();

        public void SetLesson(Lesson lesson)
        {
            _lesson = lesson;
        }
        Lesson _lesson;
        [Ignore]
        public Lesson Lesson { get { return _lesson; } }
    }

    public class SentenceBuildingItemPicture
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }

    public class SentenceBuildingItem
    {
        [PrimaryKey]
        public int Guid { get; set; }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int SentenceBuildingItemPictureId
        {
            get;
            set;
        }

        public int PhraseId
        {
            get;
            set;
        }
        [Ignore]
        public SentenceBuildingItemPicture Picture { get; set; }
        [Ignore]
        public Phrase Phrase { get; set; }
        public IList<Word> Words = new List<Word>();

    }

    public class SentenceToWord
    {
        public Guid SentenceBuildingItemId { get; set; }
        public Guid WordId { get; set; }
    }

    public class Helper
    {
        public static Stream byteArrayToStream(Byte[] bytes)
        {
            Stream str = new MemoryStream(bytes);
            return str;
        }
    }
}
