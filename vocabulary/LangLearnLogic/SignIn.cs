using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace LangLearnLogic
{
    public class SignIn
    {
            //Function for a loading usernames
            public ArrayList UsersName()
            {
                ArrayList users = new ArrayList();
                //try
                //{
                    StreamReader s = File.OpenText("../../../LangLearnLogic/usernames.txt");
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        users.Add(read);
                    }
                    s.Close();
                //}
                //catch { }
                //users.Add("Settings (Add & delete users.)");
                return users;
            }

            //Function for a loading languages
            public ArrayList modules()
            {
                ArrayList mod = new ArrayList();
                try
                {
                    StreamReader s = File.OpenText("../../../LangLearnLogic/modules.txt");
                    string read = null;
                    while ((read = s.ReadLine()) != null)
                    {
                        mod.Add(read);
                    }
                    s.Close();
                }
                catch { }
                return mod;
            }

            //Funtion UserInf reurns users language and level. 
            public string UserInf(string userUame)
            {
                try
                {
                    StreamReader s = File.OpenText("...\\FILES\\users\\" + userUame + ".txt");
                    string read = null;
                    read = s.ReadLine();
                    read = read + "#" + s.ReadLine();
                    s.Close();
                    return read;
                }
                catch
                {
                    if (userUame != "Settings (Add & delete users.)")
                    {
                        return "File not found";
                    }
                    return "";
                }
            }
        }
    }

