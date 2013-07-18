using System;
using System.Collections.Generic;
// The namespaces below doesn't exist in Mono- don't use them
//using System.Data.Entity;
//using System.Data.Entity.ModelConfiguration.Conventions;
using SQLite;
using System.Linq;
using System.Text;
using System.IO;

namespace LangInformModel
{


 /*
  * //     Not compatible with Mono    
    public class mainEntities1  : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Scene> Scenes { get; set; }
        public DbSet<SceneItem> SceneItems { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<MyItem> MyItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Chinook Database does not pluralize table names   
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
    */    

    public class MainEntities : SQLiteConnection 
    {
        public MainEntities()
            : base(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "LangInform.db3"))
        {
            // Nothing more to do here yet, but if I do, be sure to do it in a private Initialize method
        }

        public MainEntities(string dbPath)
            : base(dbPath)
        {
        }

        // Simulating DbSets. But should these be read-only?
        public TableQuery<Language> Languages { get; set; }
        public TableQuery<Level> Levels { get; set; }
        public TableQuery<Unit> Units { get; set; }
        public TableQuery<Lesson> Lessons { get; set; }
        public TableQuery<Scene> Scenes { get; set; }
        public TableQuery<SceneItem> SceneItems { get; set; }
        public TableQuery<Vocabulary> Vocabularies { get; set; }
        public TableQuery<Word> Words { get; set; }
        public TableQuery<MyItem> MyItems { get; set; }
    }


    public class Language
    {
        public Language()
        {
            this.Levels = new List<Level>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Level> Levels { get; set; }
    }

    public class Lesson
    {

        public Lesson()
        {
            Scenes = new List<Scene>();
            Vocabularies = new List<Vocabulary>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitId { get; set; }

        public virtual ICollection<Scene> Scenes { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
        public virtual Unit Unit { get; set; }
    }

    public class Level
    {
        public Level()
        { 
            Units = new List<Unit>();
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageId { get; set; }

        public virtual ICollection<Unit> Units { get; set; }
        public virtual Language Language { get; set; }
    }


    public class MyItem
    {
		[PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Sound { get; set; }
        public Nullable<long> SoundVol { get; set; }
    }

    public class Scene
    {
        public Scene()
        {
            this.SceneItems = new List<SceneItem>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LessonId { get; set; }
        public byte[] Picture { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<SceneItem> SceneItems { get; set; }
    }

    public class SceneItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Sound { get; set; }
        public Nullable<long> SoundVol { get; set; }
        public string SceneId { get; set; }

        public virtual Scene Scene { get; set; }
    }

    public class Unit
    {
        public Unit()
        {
            Lessons = new List<Lesson>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LevelId { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual Level Level { get; set; }
    }

    public class Vocabulary
    {
        public Vocabulary()
        {
            this.Words = new List<Word>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LessonId { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<Word> Words { get; set; }
    }

    public class Word
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Sound { get; set; }
        public string VocabularyId { get; set; }
        public Nullable<long> SoundVol { get; set; }
        public Nullable<long> IncludetoExam { get; set; }

        public virtual Vocabulary Vocabuluary { get; set; }
    }
}
