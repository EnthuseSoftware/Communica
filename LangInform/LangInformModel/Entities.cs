using System;
using System.Collections.Generic;
using System.Data.Entity;
// The namespace below doesn't exist in Mono- don't use it
//using System.Data.Entity.ModelConfiguration.Conventions;
using SQLite;
using System.Linq;
using System.Text;

namespace LangInformModel
{
/*
// Not compatible with Mono
    public class mainEntities1 : DbContext
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

        public virtual Vocabulary Vocabuluary { get; set; }
    }
}
