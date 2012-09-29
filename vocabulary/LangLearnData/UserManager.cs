using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace LangLearnLogic
{
    public class UserManager
    {
        public string CheckUserExists(string Username)
        {

            StreamReader s = File.OpenText("../../../LangLearnLogic/usernames.txt");
            string read = null;
            while ((read = s.ReadLine()) != null)
            {
                if (read == Username)
                {
                    s.Close();
                    return "STOP";
                }
            }
            s.Close();
            return "OK";
        }

        public string AddUser(string Username)
        {
            string a = CheckUserExists(Username);
            if (a == "STOP")
            {
                return "User exists with name " + Username + ". Please enter other name!";
            }
            //Add new user to users list
            StreamWriter f = new StreamWriter("../../../LangLearnLogic/usernames.txt", true);
            f.WriteLine(Username);
            f.Close();


            //Create new userfile
            /*FileStream fc = new FileStream("...\\FILES\\users\\" + Username + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fc);
            //sw.WriteLine(language + "#1-1-1");
            sw.Close();
            fc.Close();*/

            return "Account already created!";
        }

        public string DeleteUser(string Username)
        {
            //Deleting file with name username
            try
            {
                File.Delete("...\\FILES\\users\\" + Username + ".txt");
            }
            catch
            {
                return "Deleting failed! File with name " + Username + " using by other application.";
            }


            //Deleting username from the list of usernames
            ArrayList tempUN = new ArrayList();
            try
            {
                StreamReader s = File.OpenText("...\\FILES\\users\\usernames.txt");
                string read = null;
                while ((read = s.ReadLine()) != null)
                {
                    if (read != Username)
                    {
                        tempUN.Add(read);
                    }
                }
                s.Close();

                File.Delete("...\\FILES\\users\\usernames.txt");

                FileStream fc = new FileStream("...\\FILES\\users\\usernames.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fc);
                foreach (object obj in tempUN)
                {
                    sw.WriteLine(obj);
                }
                sw.Close();
                fc.Close();
            }
            catch
            {
                return "Couldn't delete username form usernames list!";
            }

            File.Delete("...\\FILES\\users\\usernames.txt");

            FileStream fc1 = new FileStream("...\\FILES\\users\\usernames.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(fc1);
            foreach (object obj in tempUN)
            {
                sw1.WriteLine(obj);
            }
            sw1.Close();
            fc1.Close();
            return "User with name " + Username + " already deleted!";
        }
    }
}
