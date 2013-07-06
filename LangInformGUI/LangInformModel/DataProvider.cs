using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangInformModel
{
    public class DataProvider
    {
        
        mainEntities1 entities = new mainEntities1();
        public void AddItems(List<MyItem> items)
        {
            foreach (MyItem item in items)
            {
                entities.MyItems.Add(item);
            }
            entities.SaveChanges();
        }

        List<MyItem> _items = new List<MyItem>();
        public List<MyItem> Items { get { return _items; } }

        public void GetItems()
        {
            _items = entities.MyItems.ToList();
            Word w = new Word();
            var ws = entities.Words.ToList();
        }

    }
}
