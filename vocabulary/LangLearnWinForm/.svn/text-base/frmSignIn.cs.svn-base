using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using LangLearnLogic;
using LangLearnData;
using System.IO;

namespace LangLearn
{
    public partial class frmSignIn : Form
    {
        

        SignIn signIn = new SignIn();
        public frmSignIn(ArrayList usrName)
        {
            InitializeComponent();
            comboBox1.Items.Clear();

            foreach (object usn in usrName)
            {
                comboBox1.Items.Add(usn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "") { MessageBox.Show("You did not select username!"); return; }
            if (comboBox2.Text == "") { MessageBox.Show("You did not select language!"); return; }
            int u = 0, l = 0;
            string c1 = comboBox1.Text, c2 = comboBox2.Text;
            foreach (object un in signIn.UsersName())
            {
                if (c1 == un.ToString()) { u = 1; }
            }

            foreach (object ln in signIn.Languages())
            {
                if (c2==ln.ToString()) { l = 1; }
            }

            if (u == 0) {MessageBox.Show("Error in username! Please select it from list of usernames.");return; }
            if (l == 0) { MessageBox.Show("Error in language! Please select it from list of languages."); return; }
            frmLevel frm = new frmLevel();
            Hide();
            frm.Show();
        }

       

       
        private void frmSignIn_Load_1(object sender, EventArgs e)
        {
            //ArrayList a = new ArrayList();

            /*foreach (object obj in signIn.UsersName())
            {
                comboBox1.Items.Add(obj);
            }*/

            foreach (object obj in signIn.Languages())
            {
                comboBox2.Items.Add(obj);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*FileStream fc = new FileStream("...\\FILES\\users\\usernames.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fc);
            
            
                sw.WriteLine("Dilshod new created");
            
            sw.Close();
            fc.Close();
             */
            Close();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string userInfo, userName = comboBox1.Text;
            if (userName == "Settings (Add & delete users.)")
            {
                frmAddUser addUser = new frmAddUser();
                Hide();
                addUser.Show();
                
                
                return;
            }
            String[] UInf = new string[2];
            userInfo = signIn.UserInf(userName);
            UInf = userInfo.Split('#');
            comboBox2.Text = UInf[0].ToString();
        }
    }
}
