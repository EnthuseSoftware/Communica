using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Resources;
using System.Media;
using System.Runtime.InteropServices;

namespace LangLearnData
{
    public class DataManager
    {
        LevelCs lvl = new LevelCs();
        public List<string> ItemsNameREADY = new List<string>();
        //Working with pictures===========================
        //Function returns picture from Resources which required(by name)
        public Bitmap GetIMG(string imageName)
        {
            //Bitmap im = Properties.Resources.apple;
            ResourceManager rm = Properties.Resources.ResourceManager;
            Bitmap myImage = (Bitmap)rm.GetObject(imageName);

            return myImage;
        }

		// TODO BB 7/30/12 We need to change the lesson higherarchy to level/unit/lesson
		// But, the concepts of levels, lessons, and units probably belongs in the logic layer
		// TODO JR 10/13/12 Need to rewrite LessonImages to return a byte array list.

		//This operation returns images from ULL which user selected
        //for a getting pictures from one lesson or all lessons will be difined here!
        public List<Bitmap> LessonImages(string unit,string level,string lesson)
        {
            List<Bitmap > timg = new List<Bitmap>();
            if (lesson == "All lessons")
            {
                lvl.LoadItemsNameAllLesson(unit, level);
            }
            else
            {
                lvl.LoadItemsNameOneLesson(unit, level, lesson);
            }
            
            lvl.ItemsName=lvl.MixItems(lvl.ItemsFromSelectedULL);
            ItemsNameREADY = lvl.ItemsName;
            foreach (string en in lvl.ItemsName)
            {
                timg.Add(GetIMG(en));
            }
            return timg;
        }
        //=================================================



        

        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        /// <summary>
        /// Returns length of sound 
        /// </summary>
        /// <param name="fileName">path to file</param>
        /// <returns></returns>
        public int GetSoundLength(string fileName)
        {
            StringBuilder lengthBuf = new StringBuilder(32);

            mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            mciSendString("close wave", null, 0, IntPtr.Zero);

            int length = 0;
            int.TryParse(lengthBuf.ToString(), out length);

            return length;
        }

        


        //Working with voices==============================
        
        public void PlayVoice(string path)
        {
            SoundPlayer sp=new SoundPlayer( path );
            
                sp.Play();
           
            
        }
        //=================================================

        //Highlighting color will be set from thos function----------------
        //Because in future for upgrading project. You can change when you want(Programmer)
        Color col;
        public Color Highlighting(int HighNum)
        {
            if (HighNum % 2 == 0)
            {
                col = Color.Blue;
            }
            else
            {
                col = Color.Red;
            }
            return col;
        }
    }
}
