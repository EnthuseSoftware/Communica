using System;
using LangLearnLogic;

namespace LangLearnGui
{
	public partial class NewUser : Gtk.Dialog
	{
		public NewUser ()
		{
			this.Build ();
			
		}

		/*protected void OnButtonOkButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			//throw new System.NotImplementedException ();
			string user;
			user = entNewUser.Text;
			
			UserManager newUser = new UserManager();
			newUser.AddUser(user);

		}*/
		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Hide();
		}
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string user;
			user = entNewUser.Text;
			
			UserManager newUser = new UserManager();
			newUser.AddUser(user);
			this.Hide();
		}


	}
}

