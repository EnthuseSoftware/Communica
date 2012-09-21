using System;
using Gtk;
using LangLearnGui;
using LangLearnData;

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

		MwTree ();
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

	public void MwTree ()
	{
		//Create "Courses" colume title.
		treeMain.AppendColumn ("Courses", new Gtk.CellRendererText (), "text", 0);

		//Create "coursesList" model and assign it to treeMain
		Gtk.TreeStore coursesList = new Gtk.TreeStore (typeof (string));
		treeMain.Model = coursesList;

//		//Create multi-node tree
//		Gtk.TreeIter iterCourse = coursesList.AppendValues ("English");
//
//		//Create Level node for Course node.
//		TreeIter iterLevel1 = coursesList.AppendNode (iterCourse);
//		coursesList.SetValue (iterLevel1, 0, "Level 1");
//
//		//Create Unit node for Level node.
//		iterLevel1 = coursesList.AppendNode (iterLevel1);
//		coursesList.SetValue (iterLevel1, 0, "Unit 1");
//
//		//Add Lesson to Unit node.
//		coursesList.AppendValues (iterLevel1, "Lesson 1");
//		
//		//Create Level node for Course node.
//		TreeIter iter2 = coursesList.AppendNode (iterCourse);
//		coursesList.SetValue (iter2, 0, "Level 2");

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

}
