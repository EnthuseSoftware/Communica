using System;
using LangLearnLogic;

namespace LangLearnGui
{
	public partial class SignBox : Gtk.Dialog
	{
		private MainWindow mwref;

		public SignBox ()
		{

			Initializer();			
		}
		public SignBox(MainWindow mw)
		{
			mwref = mw;
			Initializer();
		}

		private void Initializer ()
		{
			this.Build ();
			SignIn users = new SignIn();
			foreach (string name in users.UsersName())
				cmbeUser.AppendText(name);
		}
		protected void OnBtnNewUserClicked (object sender, EventArgs e)
		{
			NewUser newDialog = new NewUser();
			newDialog.Show();
			this.Hide();
		}
		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Hide();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			mwref.User = cmbeUser.ActiveText;
			mwref.MwTree ();
			this.Hide();

		}



	}
}

