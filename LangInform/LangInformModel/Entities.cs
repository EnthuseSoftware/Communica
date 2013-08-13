using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.IO;

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

        public void InsertData()
        {
            Language lang = new Language();
            lang.Name = "Uzbek";
            lang.Description = "Uzbek is a Turkish based language. Exist since 1500";
            this.Insert(lang);
            this.SaveTransactionPoint();
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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
                   _levels = ModelManager.Db.Query<Level>
                       ("select lev.Id, lev.Name, lev.Description from languagetolevel as ll inner join level as lev on ll.levelid=lev.id where ll.languageid=" + this.Id);
                }
                return _levels;
            }
        }
    }

    public class LanguageToLevel
    {
        public int LanguageId { get; set; }
        public int LevelId { get; set; }
    }

    public class Level
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        List<Unit> _units = null;
        [Ignore]
        public IList<Unit> Units { get {
            if (_units == null)
            {
                _units = ModelManager.Db.Query<Unit>("select u.Id, u.Name, u.Description from LevelToUnit as lu inner join unit as u on lu.unitid=u.id where lu.Levelid=" + this.ID);
            }
            return _units;
        } }
        //public Language Language { get; set; }
    }

    public class LevelToUnit
    {
        public int LevelId { get; set; }
        public int UnitId { get; set; }
    }

    public class Unit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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
                    _lessons = ModelManager.Db.Query<Lesson>("select l.Id, l.Name, l.Description from unittolesson as ul inner join lesson as l on ul.LessonId = l.id where ul.UnitId = " + this.Id);
                }
                return _lessons;
            }
        }
        //public Level Level { get; set; }
    }

    public class UnitToLesson
    {
        public int UnitId { get; set; }
        public int LessonId { get; set; }
    }

    public class Lesson
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string UnitId { get; set; }

        public IList<Scene> Scenes = new List<Scene>();

        public IList<Vocabulary> Vocabularies = new List<Vocabulary>();

        public IList<SentenceBuilding> SentenceBuildings = new List<SentenceBuilding>();
    }

    public class LessonToActivity
    {
        public int LessonId { get; set; }
        public int VocabularyId { get; set; }
        public int SceneId { get; set; }
        public int SentBuildingId { get; set; }
    }

    public class Scene
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int PictureId
        {
            get;
            set;
        }

        public ScenePicture ScenePicture { get; set; }

        public IList<SceneItem> SceneItems = new List<SceneItem>();
        //public string LessonId { get; set; }
        //public virtual Lesson Lesson { get; set; }
        //public virtual ICollection<SceneItem> SceneItems { get; set; }
    }

    public class ScenePicture
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] Picture { get; set; }
        public int SceneId { get; set; }
    }

    public class SceneItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double XPos { get; set; }
        public double YPos { get; set; }
        public int ScenePictureId { get; set; }
        public int PhraseId { get; set; }
        public Phrase Phrase { get; set; }
    }

    public class Phrase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[] Sound { get; set; }
    }



    public class Vocabulary
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<Word> Words = new List<Word>();
    }

    public class VocabularyToWord
    {
        public int VocabularyId { get; set; }
        public int WordId { get; set; }
        public bool DoNotIncludeToExam { get; set; }
    }

    public class Word
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Sound { get; set; }
        public int SoundVol { get; set; }

        public IList<Meaning> Meanings = new List<Meaning>();
    }

    public class WordToMeaning
    {
        public int WordId { get; set; }
        public int MeaningId { get; set; }
    }

    public class Meaning
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public IList<Word> Words = new List<Word>();
    }



    public class SentenceBuilding
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<SentenceBuildingItem> SentenceBuildingItems = new List<SentenceBuildingItem>();
    }

    public class SentenceBuildingItemPicture
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }

    public class SentenceBuildingItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

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

        public SentenceBuildingItemPicture Picture { get; set; }

        public Phrase Phrase { get; set; }
        public IList<Word> Words = new List<Word>();

    }

    public class SentenceToWord
    {
        public int SentenceBuildingItemId { get; set; }
        public int WordId { get; set; }
    }
}
