using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LangLearn
{
    class InfClass
    {
        public string GetActivityName()
        {
            StreamReader s = File.OpenText("...\\activity.txt");
            string read = null;
            read = s.ReadLine();
            s.Close();
            return read;
        }

        public string[] GetSelectedLevel()
        {
            string[] ull;
            StreamReader s = File.OpenText("...\\FILES\\tempLevel.txt");
            string read = null;
            read = s.ReadLine();
            ull = read.Split('-');
            s.Close();
            return ull;
        }

        public bool RecLevel(string unit, string level, string lesson)
        {
            try
            {
                FileStream fc = new FileStream("...\\FILES\\tempLevel.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fc);
                sw.WriteLine(unit + "-" + level + "-" + lesson);
                sw.Close();
                fc.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ActivityName(string activityName)
        {
            FileStream fc = new FileStream("...\\activity.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fc);
            sw.WriteLine(activityName);
            sw.Close();
            fc.Close();
        }
    }
}
