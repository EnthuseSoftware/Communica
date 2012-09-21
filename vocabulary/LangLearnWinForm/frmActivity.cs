using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LangLearn
{
    public partial class frmActivity : Form
    {
        InfClass inf = new InfClass();
        public frmActivity(bool lbtnEnab)
        {
            InitializeComponent();
            if (lbtnEnab ==false)
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmBoard brd = new frmBoard();
            inf.ActivityName("Learn");
            Hide();
            brd.Show();
        }

        

        private void frmActivity_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmLevel frmL = new frmLevel();
            frmL.Show();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBoard brd = new frmBoard();
            inf.ActivityName("Practice");
            Hide();
            brd.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmBoard brd = new frmBoard();
            inf.ActivityName("Review");
            Hide();
            brd.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmBoard brd = new frmBoard();
            inf.ActivityName("Quiz");
            Hide();
            brd.Show();
        }
    }
}
