using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NAudio.Wave;

namespace LangInformModel
{
    public class MainEntities : SQLiteConnection
    {
        //public MainEntities()
        //    : base(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "LangInform.db3"))
        //{
        //    // Nothing more to do here yet, but if I do, be sure to do it in a private Initialize method
        //}

        public object GetData<T>(string query)
        {
            return null;// db.Query<T>(query);
        }

        public void GetLanguages()
        {
            List<Language> languages = this.Query<Language>("SELECT * FROM Language");
        }

        // Simulating DbSets. But should these be read-only?
        //public TableQuery<Language> Languages { get; set; }
        //public TableQuery<Level> Levels { get; set; }
        //public TableQuery<Unit> Units { get; set; }
        //public TableQuery<Lesson> Lessons { get; set; }
        //public TableQuery<Scene> Scenes { get; set; }
        //public TableQuery<SceneItem> SceneItems { get; set; }
        //public TableQuery<Vocabulary> Vocabularies { get; set; }
        //public TableQuery<Word> Words { get; set; }
        //public TableQuery<Word> Meanings { get; set; }
        //public TableQuery<Word> SentenceBuildings { get; set; }
        //public TableQuery<Word> SentenceBuildingItems { get; set; }
        //public TableQuery<Word> SentenceBuildingItemPictures { get; set; }

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

        private List<Level> _levels;
        [Ignore]
        public IList<Level> Levels
        {
            get
            {
                if (_levels == null)
                {
                    try
                    {
                        _levels = ModelManager.Db.Query<Level>
                            ("select lev.Id, lev.Name, lev.Description from languagetolevel as ll inner join level as lev on ll.levelid=lev.id where ll.languageid='" + this.Id.ToString() + "'");
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

        List<Unit> _units = null;
        [Ignore]
        public IList<Unit> Units
        {
            get
            {
                if (_units == null)
                {
                    _units = ModelManager.Db.Query<Unit>("select u.Id, u.Name, u.Description from LevelToUnit as lu inner join unit as u on lu.unitid=u.id where lu.Levelid='" + this.ID + "'");
                }
                return _units;
            }
        }
        //public Language Language { get; set; }
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

        List<Lesson> _lessons = null;
        [Ignore]
        public IList<Lesson> Lessons
        {
            get
            {
                if (_lessons == null)
                {
                    _lessons = ModelManager.Db.Query<Lesson>("select l.Id, l.Name, l.Description from unittolesson as ul inner join lesson as l on ul.LessonId = l.id where ul.UnitId = '" + this.Id + "'");
                }
                return _lessons;
            }
        }
        //public Level Level { get; set; }
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
        //public string UnitId { get; set; }

        List<Scene> _scenes = null;
        [Ignore]
        public IList<Scene> Scenes
        {
            get
            {
                if (_scenes == null)
                {
                    _scenes = ModelManager.Db.Query<Scene>("select s.id, s.Name, s.Description, s.PictureId from LessonToActivity as ls " +
                        "inner join Scene as s on ls.SceneId = s.Id where ls.LessonId = '" + this.Id + "' and ls.SceneId is not null");
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
                    _vocabularies = ModelManager.Db.Query<Vocabulary>("select v.id, v.Name, v.Description from LessonToActivity as ls " +
                        "inner join Vocabulary as v on ls.VocabularyId = v.Id where ls.LessonId = '" + this.Id + "' and ls.VocabularyId is not null;");
                }
                return _vocabularies;
            }
        }

        List<SentenceBuilding> _sentenceBuildings = null;
        [Ignore]
        public IList<SentenceBuilding> SentenceBuildings
        {
            get
            {
                if (_sentenceBuildings == null)
                {
                    _sentenceBuildings = ModelManager.Db.Query<SentenceBuilding>("select s.id, s.Name, s.Description from LessonToActivity as ls " +
                        "inner join SentenceBuilding as s on ls.SentBuildingId = s.Id where ls.LessonId = '" + this.Id + "' and ls.SentBuildingId is not null;");
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

        List<SceneItem> _sceneItems;
        [Ignore]
        public IList<SceneItem> SceneItems
        {
            get
            {
                if (_sceneItems == null)
                {
                    _sceneItems = ModelManager.Db.Query<SceneItem>("select * from SceneItem where SceneId = '" + Id.ToString() + "';");
                }
                return _sceneItems;
            }
        }

    }

    public class ScenePicture
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public byte[] Picture { get; set; }
    }

    public class SceneItem: INotifyPropertyChanged
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        double _xPos;
        double _yPos;
        public double XPos { get { return _xPos; } set { _xPos = value; NotifyPropertyChanged(); } }
        public double YPos { get { return _yPos; } set { _yPos = value; NotifyPropertyChanged(); } }
        public double Size { get; set; }
        public bool IsRound { get; set; }
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

        public void Play(int playFrom=0)
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

        public IList<Meaning> Meanings = new List<Meaning>();
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
