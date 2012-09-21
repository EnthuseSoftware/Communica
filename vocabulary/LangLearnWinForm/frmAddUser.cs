using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LangLearnLogic;
namespace LangLearn
{
    public partial class frmAddUser : Form
    {
        SignIn signIn = new SignIn();
        UserManager usrMan = new UserManager();
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string language = comboBox1.Text;
            string s;
            s = usrMan.AddUser(userName, language);
            listBox1.Items.Clear();
            foreach (object obj1 in signIn.UsersName())
            {
                if (obj1 != "Settings (Add & delete users.)")
                {
                    listBox1.Items.Add(obj1);
                }
            }
            MessageBox.Show(s);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i;
            i = listBox1.SelectedIndex;
            if (i < 0)
            {
                MessageBox.Show("Please select username from the list!");
                return;
            }
            string Username = listBox1.Text;
            string result = usrMan.DeleteUser(Username);
            listBox1.Items.Clear();
            foreach (object obj1 in signIn.UsersName())
            {
                if (obj1 != "Settings (Add & delete users.)")
                {
                    listBox1.Items.Add(obj1);
                }
            }
            MessageBox.Show(result);
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            foreach (object obj in signIn.Languages())
            {
                comboBox1.Items.Add(obj);
            }
            foreach (object obj1 in signIn.UsersName())
            {
                if (obj1 != "Settings (Add & delete users.)")
                {
                    listBox1.Items.Add(obj1);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            /*List<frmSignIn> userNames = new List<frmSignIn>();
            foreach (object obj in signIn.UsersName())
            {
                userNames.Add(new frmSignIn(userNames));
            }
             * */
            //new frmSignIn();
            frmSignIn f1 = new frmSignIn(signIn.UsersName());
            Close();
            f1.ShowDialog();
        }
    }
}
