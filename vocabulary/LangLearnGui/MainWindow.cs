using System;
using Gtk;
using LangLearnGui;
using LangLearnData;
using System.Threading;
using System.Collections.Generic;
using System.IO;


public partial class MainWindow: Gtk.Window
{	
	private string user;
	public string User
	{
		set { user = value;}
		get { return user;}
	}
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		//Create "Courses" colume title.
		treeMain.AppendColumn ("Courses", new Gtk.CellRendererText (), "text", 0);;
		TreeThread ();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected void OnSigninActionActivated (object sender, System.EventArgs e)
	{
		//Signin dialog box.
		SignBox sign = new SignBox(this);
		sign.Show();

		//MwTree ();

	}

	protected void OnQuitActionActivated (object sender, System.EventArgs e)
	{
		//Quit
		Application.Quit();
	}


	private void BuildTree ()
	{
		//Create "coursesList" model and assign it to treeMain
		Gtk.TreeStore coursesList = new Gtk.TreeStore (typeof (string));
		treeMain.Model = coursesList;

		LevelCs userCourse = new LevelCs();

		TreeIter iterCourse;
		TreeIter iterLevel;
		//Populates tree down to 4 nodes.
		foreach (string name in userCourse.GetCourseNames())
		{	
			iterCourse = coursesList.AppendValues (name);
			
			foreach (string level in userCourse.GetLevelNamesByCourse(name))
			{
				iterLevel = coursesList.AppendNode (iterCourse);
				coursesList.SetValue (iterLevel, 0, level);

				foreach (string unit in userCourse.GetLevelNamesByUnit(name, level))
				{
					iterLevel = coursesList.AppendNode (iterLevel);
					coursesList.SetValue (iterLevel, 0, unit);
						
						foreach (string lesson in userCourse.GetLessonNamesForUnitAndLevel(name, level, unit))
						{
							coursesList.AppendValues (iterLevel, lesson);
						}
				}
			}
		}


	}

	protected void OnAboutActionActivated (object sender, EventArgs e)
	{
		//throw new System.NotImplementedException ();
		AboutDB about = new AboutDB();
		about.Show ();
	}

	protected void TreeClear()
	{
		//Gtk.TreeStore.Clear();
	}

	public void TreeThread ()
	{
		//Thread populates MainWindow tree.
		Thread t = new Thread (new ThreadStart (BuildTree));
		t.Start ();
	}
//	protected void TreeUpdate ()
//	{
//		MwTree();
//		//Runtime.DispatchService.GuiDispatch (new StatefulMessageHandler (UpdateGui), n);
//	}
//	protected void OnTreeMainRowActivated (object o, RowActivatedArgs args)
//	{
//		//throw new System.NotImplementedException ();
//		string level = "1";
//		string unit = "1";
//		string lesson = "1";
// 
//		DataManager picture = new DataManager();
//		List<System.Drawing.Bitmap> img = picture.LessonImages (level, unit, lesson);
//
//		System.IO.MemoryStream ms = new System.IO.MemoryStream();
//
//		int i=0;
//		foreach (Gtk.Image image in table3)
//		{
//			img[i++].Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);
//			byte[] data = ms.GetBuffer();
//
//			Gdk.Pixbuf pb = new Gdk.Pixbuf(data);
//
//			image.Pixbuf = pb;
//
//		}

	protected void OnTreeMainSelectCursorRow (object o, SelectCursorRowArgs args)
	{
		//throw new System.NotImplementedException ();
		string level = "1";
		string unit = "1";
		string lesson = "1";
 
		DataManager picture = new DataManager();
		List<System.Drawing.Bitmap> img = picture.LessonImages (level, unit, lesson);

		System.IO.MemoryStream ms = new System.IO.MemoryStream();

		int i=0;
		foreach (Gtk.Image image in table3)
		{
			img[i++].Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] data = ms.GetBuffer();

			Gdk.Pixbuf pb = new Gdk.Pixbuf(data);

			image.Pixbuf = pb;
		}
	}




}
