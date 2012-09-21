using System;
using System.IO;
using System.Data;
using System.Drawing;
using Gtk;
using StoringImages.Model;
using TestsGtk;

public partial class MainWindow: Gtk.Window
{	
	private dBHelper dbHelp;


	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnFixDataTestsRealized (object sender, EventArgs e)
	{
		entFileName.Text = @"..\..\..\LangLearnData\images\bird.bmp";
		entFileName.Show();
	}

	protected void OnBtnStoreClicked (object sender, EventArgs e)
	{		
		dbHelp = new dBHelper(dBFunctions.ConnectionStringVocab);
		ImageHelper imgHelp = new ImageHelper();
//		entFileName.Text = @"D:\Users\Brian\Dropbox\LangLearn\LangLearn New\LangLearnData\images\bird.bmp";
//		entFileName.Show ();
        int result = imgHelp.InsertImage(entFileName.Text);
		lblResult.Text = result.ToString();
		lblStatus.Text = imgHelp.GetSuccess().ToString();
	}	


	protected void OnBtnGetImageClicked (object sender, EventArgs e)
	{
		UpdatePictureBoxById(Int32.Parse(entID.Text));
	}

	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		UpdatePictureBoxByWord(entWord.Text);
	}


	// TODO BB 7/25/12 This code's functionality should go in LangLearnData
	 private void GetData()
    {
        // Determin the ConnectionString
        string connectionString = dBFunctions.ConnectionStringVocab;

        // Determin the DataAdapter = CommandText + Connection
        string commandText = @"SELECT * FROM Picture ORDER BY PicId";

        // Make a new object
        dbHelp = new dBHelper(connectionString);

        // Load the data
        if (dbHelp.Load(commandText, "") == true)
        {
            // Show the data in the datagridview
           // dataGridViewImageList.DataSource = dbHelp.DataSet.Tables[0];
        }
    }


	// TODO BB 7/25/12 This code's functionality should go in LangLearnData
    private void UpdatePictureBoxById(Int32 i)
    {
		GetData ();
        if (i > 0)
        {
            DataRow dataRow = dbHelp.DataSet.Tables[0].Rows[i - 1];
            byte[] imageBytes = (byte[])dataRow[2];
            MemoryStream ms = new MemoryStream(imageBytes);
            Gdk.Pixbuf pb = new Gdk.Pixbuf(ms);
            imgPicture.Pixbuf = pb;
            //labelFileName.Text = labelFileName.Text + " " + dataRow["imageFileName"].ToString();
        }
        else
        {
            imgPicture.Pixbuf = null;
			ErrorDialog ed = new ErrorDialog();
			ed.Run();
        }
    }	

    private void UpdatePictureBoxByWord(string word)
    {
		imgPicture.Pixbuf = null; 
        if (word.Length > 0)
        {
	        // Determine the ConnectionString
	        string connectionString = dBFunctions.ConnectionStringVocab;

	        // Determine the DataAdapter = CommandText + Connection
			string commandText = @"SELECT *";
			commandText +=       @" FROM Picture INNER JOIN Word";
			commandText +=       @" ON Picture.PicId = Word.PicId";
			commandText +=       @" WHERE Word.Word = '" + word +"'";

	        dbHelp = new dBHelper(connectionString);
			if (dbHelp.Load(commandText, ""))
			{
	            DataRow dataRow = dbHelp.DataSet.Tables[0].Rows[0];
	            byte[] imageBytes = (byte[])dataRow[2];
	            MemoryStream ms = new MemoryStream(imageBytes);
	            Gdk.Pixbuf pb = new Gdk.Pixbuf(ms);
	            imgPicture.Pixbuf = pb;
			}
        }
		// if our proc succeeded, imgPicture.Pixbuf will not be null
    }	

	protected void OnBtnSoundTestsClicked (object sender, EventArgs e)
	{
			SoundTestWindow soundTestsWin = new SoundTestWindow();
			soundTestsWin.Show();
	}	


	protected void OnBtnDialogTestClicked (object sender, EventArgs e)
	{
		ErrorDialog dlgError = new ErrorDialog();
		dlgError.Run();

		ErrorDialog testdlg = new ErrorDialog();
		testdlg.Run();
	}
	




}
