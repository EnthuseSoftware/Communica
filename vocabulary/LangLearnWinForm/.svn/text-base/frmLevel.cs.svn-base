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
    public partial class frmLevel : Form
    {
        LevelCs lvl = new LevelCs();
        InfClass inf = new InfClass();
        public frmLevel()
        {
            InitializeComponent();
            for (int i = 1; i <= Convert.ToInt16(lvl.GetAvailableUnits()); i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.Text = "Please select unit";
            comboBox2.Text = "Unit not selected";
            comboBox3.Text = "Level not selected";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == true && checkBox3.Checked == true)
            {
                bool success = inf.RecLevel(comboBox1.Text, comboBox2.Text, comboBox3.Text);
                if (success)
                {
                    bool lbtnEnab = true;
                    if (comboBox3.Text == "All lessons") lbtnEnab = false;
                    frmActivity frmA = new frmActivity(lbtnEnab);
                    Hide();
                    frmA.ShowDialog();
                }
                else
                {
                    MessageBox.Show("ERROR IO001: Appliaction couldn't save file! Please restert the program! ");
                    return;
                }
            }
            
        }

        private void frmLevel_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 1; i <= Convert.ToInt16(lvl.GetAvailableLevelsInCurrentUnit(Convert.ToInt16(comboBox1.Text) )); i++)
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.Text = "Please select Level";
            checkBox1.Checked=true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 1; i <= Convert.ToInt16(lvl.GetAvailableLessonsInCurrentLevelUnit(Convert.ToInt16(comboBox1.Text),
                Convert.ToInt16(comboBox2.Text))); i++)
            {
                comboBox3.Items.Add(i);
            }
            comboBox3.Items.Add("All lessons");
            comboBox3.Text = "Please select Lesson";
            checkBox2.Checked = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) { }
        

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignIn sgn=new SignIn();
            frmSignIn frms = new frmSignIn(sgn.UsersName());
            Hide();
            frms.Show();
        }
    }
}
