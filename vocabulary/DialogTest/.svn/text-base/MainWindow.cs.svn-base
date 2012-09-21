using System;
using Gtk;
using DialogTest;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton65Clicked (object sender, EventArgs e)
	{
		//throw new System.NotImplementedException ();
		TestDialog dialogbox = new TestDialog ();
		dialogbox.Run();
	}
}
