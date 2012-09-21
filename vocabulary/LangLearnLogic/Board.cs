using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LangLearnData;
using System.Collections;
using System.Windows.Forms;


namespace LangLearnLogic
{

    public class Board
    {
        DataManager dmn = new DataManager();
        LevelCs lvl = new LevelCs();
        //This property gets value from UserInterface(LangLearn)
        public List<string> tempin = new List<string>();




        //Q U I Z -----------------------------------------
        public int TestNumber;
        public List<string> CheckQuestions = new List<string>();
        public int WroungAnsCount;
        public int RightAnsCount;
        public string SelectedItem;

        public int MakeQuizQuestions()
        {
            Random rnd = new Random();
            int rndDigit=-1;
            while (true)
            { 
                rndDigit=rnd.Next(0,tempin.Count());
                if (!CheckQuestions.Contains(tempin[rndDigit]))
                {
                    SelectedItem = tempin[rndDigit];
                    CheckQuestions.Add(SelectedItem);
                    break;
                }
            }
            dmn.PlayVoice("C:\\voice\\" + SelectedItem + ".wav");
            return rndDigit;
        }

        //=================================================



        
        
        //Practice-----------------------------------------

        public List<string> PQuestionsCheck = new List<string>();
        public int PQuestionsNum;
        public string CurrentItem;
        
        public int PracticeQuestionMaker()
        {
            Random rnd = new Random();
            int rndDig = -1;
            while (true)
            {
                rndDig = rnd.Next(0, tempin.Count());
                if (!PQuestionsCheck.Contains(tempin[rndDig]))
                {
                    CurrentItem = tempin[rndDig];
                    PQuestionsCheck.Add(CurrentItem);
                    break;
                }
            }
            dmn.PlayVoice("C:\\voice\\" + CurrentItem + ".wav");
            return rndDig ;
        }

        //=================================================


        //REVIEW-----------------------------------------------------

        public string[] CurrentItemInf
        { get; set; }
        public int CurrentItemNum
        { get; set; }
        public string CurrentItenName
        { get; set; }
        
        public string[] RightItemInf
        { get; set; }
        public int RightItemNum
        { get; set; }
        public string RightItemName
        { get; set; }
        int quesNum = -1;
        public int QuestionNum
        {
            get { return quesNum; } 
            set{quesNum=value;}
        }

        List<string> questions = new List<string>();

        public int GenerateAsking()
        {
            Random rnd=new Random();
            if (QuestionNum == -1) { questions = QuestionMaker(rnd.Next(0, 3) + 1); QuestionNum = 0; }
            if (QuestionNum == questions.Count() ) { questions = QuestionMaker(rnd.Next(0, 3) + 1); QuestionNum = 0; }
            CurrentItemInf = questions[QuestionNum].Split('#'); CurrentItemNum = Convert.ToInt16(CurrentItemInf[1]); CurrentItenName = CurrentItemInf[0];
            RightItemInf = questions[questions.Count() - 1].Split('#'); RightItemNum = Convert.ToInt16(RightItemInf[1]);
            RightItemName = RightItemInf[0];
            dmn.PlayVoice("C:\\voice\\" + RightItemName + ".wav");
            QuestionNum++;
            if (checkList.Count() == tempin.Count() && QuestionNum == questions.Count()) { return 1; }
            return 0;
        }
        
        public int Checkanswer(int currItemNum, int rightItemNum, string YesNo_btn)
        {
            if (YesNo_btn == "Yes")
            {
                if (currItemNum == rightItemNum)
                {
                    QuestionNum = -1;
                }
                else
                {
                    QuestionNum--;
                    MessageBox.Show("It was wrong, you pressed YES!");
                }
            }
            if (YesNo_btn == "No")
            {
                if (currItemNum == rightItemNum)
                {
                    QuestionNum--;
                    MessageBox.Show("It was right, you pressed NO!");
                }
                else
                { 
                
                }
            }
            return 0;
        }

        public List<string> checkList = new List<string>();
        /// <summary>
        /// Randomly makes questions.
        /// </summary>
        /// <param name="count">Count of questions</param>
        /// <returns></returns>
        public List<string> QuestionMaker(int count)
        {
            Random rnd = new Random();
            List<string> temp = new List<string>();
            
            int counter = 0;
            string quest = "";
            while (true)
            {
                counter = 0;
                temp.Clear();
                while (true)
                {
                    int rn = rnd.Next(0, tempin.Count());
                    quest = tempin[rn];
                    if (!temp.Contains(quest + "#" + rn))
                    {
                        temp.Add(quest + "#" + rn);
                        counter++;
                        if (counter == count) { break; }
                    }
                }
                if (!checkList.Contains(temp[temp.Count() - 1])) { checkList.Add(temp[temp.Count() - 1]); break; }
            }


            return temp;
        }

        

        //===========================================================


        
    }
}
