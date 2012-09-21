using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LangLearnData;
using LangLearnLogic;
using System.Collections;

namespace LangLearn
{
    public partial class frmBoard : Form
    {
        DataManager dmn = new DataManager();
        LevelCs lvl = new LevelCs();
        Board brd = new Board();
        InfClass inf = new InfClass();

        string voiceName;
        int imNum = 0;
        string _activityName;

        public frmBoard()
        {
            InitializeComponent();
        }

        string le;
        public void LoadPicturesToBoard()
        {
            string[] lv;
            lv = inf.GetSelectedLevel();
            string u = lv[0], l = lv[1];
            le = lv[2];
            List<Bitmap> img = new List<Bitmap>();
            img = dmn.LessonImages(u, l, le);
            P1.Image = img[0];
            P2.Image = img[1];
            P3.Image = img[2];
            P4.Image = img[3];
            P5.Image = img[4];
            P6.Image = img[5];
            P7.Image = img[6];
            P8.Image = img[7];
            P9.Image = img[8];
            P10.Image = img[9];
            P11.Image = img[10];
            P12.Image = img[11];

        }

        public void ResetHighlighting()
        {
            B1.BackColor = bgr;
            B2.BackColor = bgr;
            B3.BackColor = bgr;
            B4.BackColor = bgr;
            B5.BackColor = bgr;
            B6.BackColor = bgr;
            B7.BackColor = bgr;
            B8.BackColor = bgr;
            B9.BackColor = bgr;
            B10.BackColor = bgr;
            B11.BackColor = bgr;
            B12.BackColor = bgr;
            timer1.Enabled = false;
            hn = 0;
        }

        Color bgr;
        List<string> tempIN = new List<string>();

        private void frmBoard_Load(object sender, EventArgs e)
        {
            Start();
        }

        int imNum1;

        public void Start()
        {

            LoadPicturesToBoard();
            bgr = B1.BackColor;
            tempIN = brd.tempin = dmn.ItemsNameREADY;
            _activityName = inf.GetActivityName();
            if (_activityName == "Learn") { button2.Visible = false; }
            if (_activityName == "Practice")
            {
                imNum1 = brd.PracticeQuestionMaker();
            }
            if (_activityName == "Review")
            {
                ActivityReview();
            }
            if (_activityName == "Quiz")
            {
                label1.Visible = true; label1.Text = "Right answers: 0";
                label2.Visible = true; label2.Text = "Wrong answers: 0";
                brd.CheckQuestions.Clear();
                brd.RightAnsCount = 0;
                brd.WroungAnsCount = 0;
                imNum1 = brd.MakeQuizQuestions();
            }
        }

        int ResetIndicator;

        public void ActivityReview()
        {
            btnYes.Visible = true;
            btnNo.Visible = true;
            ResetIndicator = brd.GenerateAsking();
            ResetHighlighting();
            imNum = brd.CurrentItemNum + 1;
            timer1.Enabled = true;
        }

        int hn = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_activityName == "Quiz")
            {
                timer1.Interval = 200;
            }
            else
            {
                timer1.Interval = 500;
            }
            hn++;
            //if (hn == 1) { ResetHighlighting(); }
            switch (imNum)
            {
                case 1: B1.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B1.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 2: B2.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B2.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 3: B3.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B3.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 4: B4.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B4.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 5: B5.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B5.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 6: B6.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B6.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 7: B7.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B7.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 8: B8.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B8.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 9: B9.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B9.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 10: B10.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B10.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 11: B11.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B11.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;

                case 12: B12.BackColor = dmn.Highlighting(hn);
                    if (hn == 6) { B12.BackColor = bgr; timer1.Enabled = false; hn = 0; }
                    break;
            }
        }

        int PWrongAnsCount;
        public void ClickToItem(int cellNumber, string activity)
        {

            if (_activityName == "Learn")
            {
                timer3.Enabled = false;
                imNum = cellNumber;
                ResetHighlighting();
                timer1.Enabled = true;
                voiceName = tempIN[cellNumber - 1];
                dmn.PlayVoice("C:\\voice\\" + voiceName + ".wav");
                int soundLength=dmn.GetSoundLength("C:\\voice\\" + voiceName + ".wav");
                timer3.Interval = soundLength + 200;
                timer3.Enabled = true;
            }

            if (_activityName == "Practice")
            {
                imNum = cellNumber;
                ResetHighlighting();
                timer1.Enabled = true;

                if ((cellNumber - 1) != imNum1)
                {
                    PWrongAnsCount++;
                    if (PWrongAnsCount == 3)
                    {
                        PWrongAnsCount = 0;
                        MessageBox.Show("You pressed three times wrong answer! Now program will show right answer.");
                        ResetHighlighting();
                        imNum = imNum1 + 1;
                        timer1.Enabled = true;
                        dmn.PlayVoice("C:\\voice\\" + brd.CurrentItem + ".wav");
                        return;
                    }
                    MessageBox.Show("You pressed on wrong answer!");
                    dmn.PlayVoice("C:\\voice\\" + brd.CurrentItem + ".wav");
                    ResetHighlighting();
                    return;
                }
                else { PWrongAnsCount = 0; }
                if (brd.PQuestionsCheck.Count() == brd.tempin.Count())
                {
                    brd.PQuestionsCheck.Clear();
                    Start();
                    ResetHighlighting();
                }
                timer2.Enabled = true;
                //imNum1 = brd.PracticeQuestionMaker();
            }

            if (_activityName == "Quiz")
            {
                if (label3.Text == "1") { return; }
                ResetHighlighting();
                imNum = cellNumber;
                timer1.Enabled = true;
                if ((cellNumber - 1) == imNum1)
                {
                    brd.RightAnsCount++;
                    label1.Text = "Right answers: " + brd.RightAnsCount;
                }
                else
                {
                    brd.WroungAnsCount++;
                    label2.Text = "Wrong answers: " + brd.WroungAnsCount;
                    MessageBox.Show("It was WRONG!");
                }

                if (brd.CheckQuestions.Count() == brd.tempin.Count())
                {
                    DialogResult msg = new DialogResult();
                    msg = MessageBox.Show("Test finished! \n Count of right answers: " + brd.RightAnsCount + " \n Count of wrong answers: " + brd.WroungAnsCount
                        + "\n \n Do you want to repeat quizing?", "Quiz", MessageBoxButtons.YesNo);
                    if (msg == DialogResult.Yes)
                    {
                        Start();
                    }
                    else
                    {
                        label3.Text = "1";
                    }
                    return;
                }
                timer2.Enabled = true;
                //imNum1 = brd.MakeQuizQuestions();
            }

        }

        private void P1_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(1, _activityName);
        }

        private void P2_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(2, _activityName);
        }

        private void P3_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(3, _activityName);
        }

        private void P4_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(4, _activityName);
        }

        private void P5_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(5, _activityName);
        }

        private void P6_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(6, _activityName);
        }

        private void P7_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(7, _activityName);
        }

        private void P8_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(8, _activityName);
        }

        private void P9_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(9, _activityName);
        }

        private void P10_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(10, _activityName);
        }

        private void P11_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(11, _activityName);
        }

        private void P12_Click(object sender, EventArgs e)
        {
            ResetHighlighting();
            ClickToItem(12, _activityName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool lbt;
            if (le == "All lessons")
            {
                lbt = false;
            }
            else
            {
                lbt = true;
            }
            frmActivity frmA = new frmActivity(lbt);
            frmA.Show();
            Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            brd.Checkanswer(brd.CurrentItemNum, brd.RightItemNum, "Yes");
            if (ResetIndicator == 1)
            {
                brd.checkList.Clear();
                brd.QuestionNum = -1;
                Start();
                return;
            }
            ActivityReview();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            brd.Checkanswer(brd.CurrentItemNum, brd.RightItemNum, "No");
            if (ResetIndicator == 1)
            {
                brd.checkList.Clear();
                brd.QuestionNum = -1;
                Start();
                return;
            }
            ActivityReview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_activityName == "Quiz")
            {
                if (label3.Text != "1")
                {
                    dmn.PlayVoice("C:\\voice\\" + brd.SelectedItem + ".wav");
                }
            }
            if (_activityName == "Review")
            {
                dmn.PlayVoice("C:\\voice\\" + brd.RightItemName + ".wav");
                ResetHighlighting();
                imNum = brd.CurrentItemNum + 1;
                timer1.Enabled = true;
            }
            if (_activityName == "Practice")
            {
                dmn.PlayVoice("C:\\voice\\" + brd.CurrentItem + ".wav");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (_activityName == "Practice")
            {
                imNum1 = brd.PracticeQuestionMaker();
            }
            if (_activityName == "Quiz")
            {
                imNum1 = brd.MakeQuizQuestions();
            }
            timer2.Enabled = false;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            dmn.PlayVoice("C:\\voice\\" + voiceName + ".wav");
            timer3.Enabled = false;
        }

    }
}
