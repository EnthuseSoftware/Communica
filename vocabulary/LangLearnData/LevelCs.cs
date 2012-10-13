using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using StoringImages.Model;
using System.Data;


namespace LangLearnData
{
    public class LevelCs
    {
        // ? ULL - Unit, Level, Lesson
		// Course structure: Level, Unit, Lesson
		// (Within a level there will be units, within a unit there will be lessons)

        //Fields===========================================
        List<string> _itemsName = new List<string>();
        List<string> _itemsFromSelecedULL = new List<string>();
		dBHelper dbHelp;
        //===================================================

        //Properties========================================
        public List<string> ItemsName		// Vocab words
        {
            get { return _itemsName; }
            set { _itemsName = value; }
        }

        public List<string> ItemsFromSelectedULL
        {
            get { return _itemsFromSelecedULL; }
            set { _itemsFromSelecedULL = value; }
        }

        //===================================================


        //Returns all vocab words =============================
        public List<string> AllItemsName()
        {
            List<string> items = new List<string>();
            StreamReader f = File.OpenText("../itemsName.txt");
            string read = null;
            while ((read = f.ReadLine()) != null)
            {
                items.Add(read);
            }
            f.Close();
            return items;
        }
        //===================================================


        //DS: Get unit, level, lesson which user choose=========
		// Returns the last unit number listed in itemsName.txt
        public int GetAvailableUnits()
        {
            string un;
            string aun;
            int lun = 0;
            foreach (object itn in AllItemsName())
            {
                un = itn.ToString();

                if (un.Substring(0, 1) == "#")
                {
                    aun = un.Substring(1, 1);
                    lun = Convert.ToInt16(aun);
                }
            }
            return lun;
        }

        public int GetAvailableLevelsInCurrentUnit(int unit)
        {
            string un;
            string aun, aln;
            int lun = 0, lln = 0;

            foreach (object itn in AllItemsName())
            {
                un = itn.ToString();

                if (un.Substring(0, 1) == "#")
                {
                    aun = un.Substring(1, 1);
                    lun = Convert.ToInt16(aun);
                    if (lun == unit)
                    {
                        aln = un.Substring(3, 1);
                        lln = Convert.ToInt16(aln);
                    }
                }
            }
            return lln;
        }

        public int GetAvailableLessonsInCurrentLevelUnit(int unit, int level)
        {
            string un;
            string aun, aln;
            string ln;
            int lun = 0, lln = 0, lni = 0;

            foreach (object itn in AllItemsName())
            {
                un = itn.ToString();

                if (un.Substring(0, 1) == "#")
                {
                    aun = un.Substring(1, 1);
                    lun = Convert.ToInt16(aun);
                    if (lun == unit)
                    {
                        aln = un.Substring(3, 1);
                        lln = Convert.ToInt16(aln);
                        if (lln == level)
                        {
                            ln = un.Substring(5, 1);
                            lni = Convert.ToInt16(ln);
                        }
                    }
                }
            }
            return lni;
        }
        //===================================================

        //Get item names (unit, level, lesson) which user selected
        //This operation returns information to ItemsFormSelectedULL
        public void LoadItemsNameOneLesson(string unit, string level, string lesson)
        {
            string signal;
            int p = 0;
            string[] sp;
            ItemsFromSelectedULL.Clear();
            foreach (string itemName in AllItemsName())
            {
                signal = itemName.Substring(0, 1);
                if (signal == "#")
                {
                    sp = (itemName.Substring(1, itemName.Length - 1)).Split('-');
                    if (sp[0] == unit.ToString() && sp[1] == level.ToString() && sp[2] == lesson.ToString()) p = 1; else p = 0;
                }
                if (p == 1)
                {
                    if (signal != "#") ItemsFromSelectedULL.Add(itemName);
                }
            }
        }
        //===================================================

        //Get all items from unit, level which user selected
        //This operation returns information to ItemsFormSelectedULL
        public void LoadItemsNameAllLesson(string unit, string level)
        {
            Random rnd = new Random();
            int ln = GetAvailableLessonsInCurrentLevelUnit(Convert.ToInt16(unit), Convert.ToInt16(level));
            int itn = 0;
            List<string> tempIn = new List<string>();
            string tin;
            while (itn < 12)
            {
                for (int i = 1; i <= ln; i++)
                {
                    LoadItemsNameOneLesson(unit, level, i.ToString());
                st: tin = ItemsFromSelectedULL[rnd.Next(0, ItemsFromSelectedULL.Count() - 1)];
                    foreach (string t in tempIn)
                    {
                        if (t == tin) { goto st; }
                    }
                    tempIn.Add(tin);
                    itn++; if (itn == 12) { ItemsFromSelectedULL = tempIn; return; }
                }
            }

          /*
            Dictionary<string, int> people = new Dictionary<string, int>();
            people.Add("Dilshod", 23);
            people.Add("Glenn", 38);

            int age2 = people["Glenn"];
            foreach (int age in people.Values)
            {
                int x = age;
            }
            foreach (string name in people.Keys)
                name

            */

        }
        //===================================================

        //This function for mixing items randomly
        public List<string> MixItems(List<string> _itemsN)
        {
            Random rnd = new Random();
            int s = _itemsN.Count();
            ArrayList inum = new ArrayList();
            List<string> tempin = new List<string>();
            int num;
            for (int i = 0; i <= s - 1; i++)
            {
            st: num = rnd.Next(s);
                foreach (int n in inum)
                {
                    if (n == num) { goto st; }
                }
                inum.Add(num);
                tempin.Add(_itemsN[num]);
            }
            return tempin;
        }
        //===================================================


		// ****** Brian's new mehtods **********
		// 9/5/12: Just stubs so far

		public List<string> GetCourseNames()

		{
			List<string> courses = new List<string>();	
			courses.Add("English");
			courses.Add ("Tajik");
			courses.Add ("Russian");

			/*
			dbHelp = new dBHelper(dBFunctions.ConnectionStringUserInfo);
			string commandText = @"SELECT * FROM Course ORDER BY CourseId";

			if (dbHelp.Load(commandText, ""))
				foreach(DataRow row in dbHelp.DataSet.Tables[0].Rows)
					courses.Add((string)row[1]);
			*/
			return courses;
		}
			
		public List<string> GetLevelNamesByCourse(string courseName)
		{
			List<string> levels = new List<string>();
			levels.Add ("Beginning");
			levels.Add ("Intermediate");
			levels.Add ("Advanced");

			return levels;
		}

		public List<string> GetLevelNamesByUnit(string courseName, string unitName)
		{
			List<string> units = new List<string>();
			units.Add ("Vocabulary");
			units.Add ("Grammar");
			units.Add ("Reading");
			return units;
		}

		public List<string> GetLessonNamesForUnitAndLevel(string courseName, string unitName, string lessonName)
		{
			List<string> lessons = new List<string>();
			lessons.Add ("Greetings");
			lessons.Add ("Animals");
			lessons.Add ("Food");
			return lessons;
		}

    }

}