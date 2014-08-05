using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace LangInformModel
{
    public class DataProvider
    {
        string _pathToDatabase;

        //List<MyItem> _items = new List<MyItem>();
        //public List<MyItem> Items { get { return _items; } }

        public DataProvider()
        {
            /*
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _pathToDatabase = Path.Combine(documents, "db_sqlite-net.db");
            */
            _pathToDatabase = "../../../Assets/Lang.db";
            using (var conn = new SQLite.SQLiteConnection(_pathToDatabase))
            {
                //conn.CreateTable<MyItem>();
            }
        }

        //public void AddItems(List<MyItem> items)
        //{
        //    using (var db = new SQLite.SQLiteConnection(_pathToDatabase))
        //    {
        //        foreach (MyItem item in items)
        //        {
        //            db.Insert(item);
        //        }
        //    }
        //}

        public void GetItems()
        {
            using (var db = new SQLite.SQLiteConnection(_pathToDatabase))
            {
                //_items = db.Query<MyItem>("select * from MyItem", null);
            }
            Word w = new Word();
        }

        /// <summary>
        /// Global way to grab a connection to the database, make sure to wrap in a using
        /// </summary>
        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(_pathToDatabase, true);
            //TODO: Get the initialized check to work
            /*
            if (!initialized)
            {
                CreateDatabase(connection, cancellationToken).Wait();
            }
            */
            return connection;
        }

    }
}
