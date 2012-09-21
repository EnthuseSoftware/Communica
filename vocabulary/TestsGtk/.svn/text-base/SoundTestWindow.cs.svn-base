using System;
using System.Media;
using System.IO;
using StoringImages.Model;
using System.Data;

namespace TestsGtk
{

	public partial class SoundTestWindow : Gtk.Window
	{
		dBHelper helper;

		public SoundTestWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			fcbSoundFile.SetCurrentFolder(@"..\..\..\voice\");
		}


		/* BB 7/27/12 This didn't work. Is this the wrong signal to use?
		protected void OnFixSoundTestRealized (object sender, EventArgs e)
		{
			fcbSoundFile.SetCurrentFolder(@"..\..\..\voice\");
		}
*/


		protected void OnBtnPlayFileClicked (object sender, EventArgs e)
		{
			string soundFileName = fcbSoundFile.Filename;
			SoundPlayer sp = new SoundPlayer();

			// Load the file and play it
			sp.SoundLocation = soundFileName;
			sp.Play(); 

			// Convert the file to a stream and then play it
			/* byte[] soundArray = File.ReadAllBytes (soundFileName);
			MemoryStream soundStream = new MemoryStream(soundArray);
			sp.Stream = soundStream;
			sp.Play(); */
		}		


		protected void OnBtnLoadSoundClicked (object sender, EventArgs e)
		{
			string soundFileName = fcbSoundFile.Filename;
			byte[] soundArray = File.ReadAllBytes (soundFileName);
			//	MemoryStream soundStream = new MemoryStream (soundArray);

			// TODO BB 7/27/12 Move this code into LangLearnData
		//	if (soundStream == null) 
			if(soundArray.Length < 1)
			{
				lblSoundStatus.Text = "Null sound stream object";
			}
			else
            {
                // Load an empty DataSet
                if (GetSoundData(0) != true)
				{
					lblSoundStatus.Text = "Load command failed";
				}
				else
                {
                    // Add a row and determine the row
                    helper.DataSet.Tables[0].Rows.Add(helper.DataSet.Tables[0].NewRow());
                    System.Data.DataRow dataRow = helper.DataSet.Tables[0].Rows[0];

                    // Enter the given values
                    dataRow["Sound"] = soundArray;
					dataRow["LangId"] = 1; // 1 is the ID for English
					int beginName = soundFileName.LastIndexOf('\\') + 1;
					int lengthName = soundFileName.Length - beginName - 4;
					dataRow["Word"] = soundFileName.Substring(beginName, lengthName);
					dataRow["WordId"] = 0; // This will be changed by the column's auto increment
					if(helper.Save())
					{
						lblSoundStatus.Text = "Success";
						lblWordId.Text = dataRow[0].ToString();
					}
					else
					{
						lblSoundStatus.Text = "Failed";
					}
                }
            }

		}		


		protected void OnBtnPlayFromDbClicked (object sender, EventArgs e)
		{
			int wordId = Int32.Parse(entWordId.Text);
	        if (wordId < 1)
			{
				lblSoundStatus.Text = "Not a valid WordId";
			}
			else
	        {			
				if(!GetSoundData(wordId))
				{
					lblSoundStatus.Text = "DataSet load failed";
				}
				else
				{
		            DataRow dataRow = helper.DataSet.Tables[0].Rows[0];
		            byte[] soundArray = (byte[])dataRow[6];
					MemoryStream soundStream = new MemoryStream(soundArray);
					SoundPlayer sp = new SoundPlayer(soundStream);
					sp.Play();
				}
			}
		}


		// Get a dataset from the database. Dataset will be in helper
		// Pass in zero if you want an empty dataset
		private bool GetSoundData(int wordId)
		{
            // Determin the ConnectionString
            string connectionString = dBFunctions.ConnectionStringVocab;

			string commandText = "SELECT * FROM " + "Word";
			commandText += " WHERE " + wordId.ToString() + " = WordId";

            // Make a new object
            helper = new dBHelper(connectionString);
            // Load Data
			bool success = helper.Load(commandText, "WordId");
			if (helper.DataSet.Tables[0].Rows.Count == 0 && wordId > 0)
				success = false;
			return success;
		}		

	}	// namespace TestsGtk
}

