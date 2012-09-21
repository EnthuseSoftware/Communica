using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;



namespace StoringImages.Model
{
    public class ImageHelper
    {
        const string IMAGE_TABLE = "Picture";
        const string IMAGE_ID = "PicId";
        const string IMAGE_BLOB = "Picture";
        private dBHelper helper = null;
        private string fileLocation = string.Empty;
        private bool isSucces = false;
        private int maxImageSize = 2097152;

        //2MB   - 2097152
        //5MB   - 5242880
        //10MB  - 10485760

        /*  Conversion
         *  1 Byte = 8 Bit
         *  1 Kilobyte = 1024 Bytes
         *  1 Megabyte = 1048576 Bytes
         *  1 Gigabyte = 1073741824 Bytes
         * */


        private string FileLocation
        {
            get { return fileLocation; }
            set
            {
                fileLocation = value;
            }
        }


        public Boolean GetSuccess()
        {
            return isSucces;
        }


		// Load an image from a file into an Image object
        private Image LoadImage(string imageName)
        {
            //Create an instance of the Image Class/Object
            //so that we can store the information about the picture and send it back for
            //processing into the database.
            Image image = null;

            this.FileLocation = imageName;

            if (fileLocation == null || fileLocation == string.Empty)
			{
                return image;
			}
			else            
			{

                //Get file information and calculate the filesize
                FileInfo info = new FileInfo(FileLocation);
                long fileSize = info.Length;

                //reasign the filesize to calculated filesize
                maxImageSize = (Int32)fileSize;

                if (File.Exists(FileLocation))
                {
                    //Retreave image from file and binary it to Object image
                    using (FileStream stream = File.Open(FileLocation, FileMode.Open))
                    {
                        BinaryReader br = new BinaryReader(stream);
                        byte[] data = br.ReadBytes(maxImageSize);
                        image = new Image(fileLocation, data, fileSize);
                    }
                }
            }
            return image;
        }


		// Insert an image from a file into the database
        public Int32 InsertImage (string imageName)
		{
			DataRow dataRow = null;
			isSucces = false;

			Image image = LoadImage (imageName);

			//if no file was selected and no image was created return 0
			if (image == null) 
			{
				return 0;
			}
            else
            {
                // Determin the ConnectionString
                string connectionString = dBFunctions.ConnectionStringVocab;

                // Determin the DataAdapter = CommandText + Connection
                // TODO BB 7/25/12: Can we avoid embedding table info here?
                string commandText = "SELECT * FROM " + IMAGE_TABLE + " WHERE 1=0";

                // Make a new object
                helper = new dBHelper(connectionString);
                {
                    // Load Data
                    if (helper.Load(commandText, IMAGE_ID) == true)
                    {
                        // Add a row and determin the row
                        helper.DataSet.Tables[0].Rows.Add(helper.DataSet.Tables[0].NewRow());
                        dataRow = helper.DataSet.Tables[0].Rows[0];

                        // Enter the given values
                        // BB 7/25/12 Not used // dataRow["imageFileName"] = image.FileName;
                        dataRow[IMAGE_BLOB] = image.ImageData;
                        // BB 7/25/12 Not used // dataRow["imageFileSizeBytes"] = image.FileSize;

                        try
                        {
                            // Save -> determin succes
                            if (helper.Save() == true)
                            {
                                isSucces = true;

                            }
                            else
                            {
                                isSucces = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }//END IF
                }
            }
           //return the new image_id
           return Convert.ToInt32(dataRow[0].ToString());
        }


        public void DeleteImage(Int32 imageID)
        {
            //Set variables
            isSucces = false;

            // Determin the ConnectionString
            string connectionString = dBFunctions.ConnectionStringVocab;

            // Determin the DataAdapter = CommandText + Connection
            string commandText = "SELECT * FROM " + IMAGE_TABLE + " WHERE " + IMAGE_ID + " =" + imageID;

            // Make a new object
            helper = new dBHelper(connectionString);
            {
                // Load Data
                if (helper.Load(commandText, IMAGE_ID) == true)
                {
                    // Determin if the row was found
                    if (helper.DataSet.Tables[0].Rows.Count == 1)
                    {
                        // Found, delete row
                        helper.DataSet.Tables[0].Rows[0].Delete();
                        try
                        {
                            // Save -> determin succes
                            if (helper.Save() == true)
                            {
                                isSucces = true;
                            }
                            else
                            {
                                isSucces = false;
                            }
                        }
                        catch (Exception ex)
                        {
							throw ex;
                            // Show the Exception --> Dubbel ContactId/Name ?
                        }
                    }

                }
            }

        }
        /*
        public void SaveAsImage(Int32 imageID)
        {
            //set variables
            DataRow dataRow = null;
            Image image = null;
            isSucces = false;

            // Displays a SaveFileDialog so the user can save the Image
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = @"C:\\";
            dlg.Title = "Save Image File";
            //1
            dlg.Filter = "Tag Image File Format (*.tiff)|*.tiff";   
            //2
            dlg.Filter += "|Graphics Interchange Format (*.gif)|*.gif";
            //3
            dlg.Filter += "|Portable Network Graphic Format (*.png)|*.png";
            //4
            dlg.Filter += "|Joint Photographic Experts Group Format (*.jpg)|*.jpg";
            //5
            dlg.Filter += "|Joint Photographic Experts Group Format (*.jpeg)|*.jpeg";
            //6
            dlg.Filter += "|Bitmap Image File Format (*.bmp)|*.bmp";
            //7
            dlg.Filter += "|Nikon Electronic Format (*.nef)|*.nef";
            dlg.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (dlg.FileName != "")
            {
                //making shore only one of the 7 is being used if not added the default extention to the filename
                string defaultExt = ".png";
                int pos = -1;
                string[] ext = new string[7] {".tiff", ".gif", ".png", ".jpg", ".jpeg", ".bmp", ".nef"};
                string extFound = string.Empty;
                string filename = dlg.FileName.Trim();
                for (int i = 0; i < ext.Length; i++)
                {
                    pos = filename.IndexOf(ext[i], pos + 1);
                    if (pos > -1)
                    {
                        extFound = ext[i];
                        break;
                    }
                }
                if (extFound == string.Empty) filename = filename + defaultExt;
               


                // Determin the ConnectionString
                string connectionString = dBFunctions.ConnectionStringSQLite;

                // Determin the DataAdapter = CommandText + Connection
                string commandText = "SELECT * FROM" + IMAGE_TABLE + "WHERE " + IMAGE_ID + " =" + imageID;

                // Make a new object
                helper = new dBHelper(connectionString);

                // Load the data
                if (helper.Load(commandText, "") == true)
                {
                    // Show the data in the datagridview
                    dataRow = helper.DataSet.Tables[0].Rows[0];
                    image = new Image(
                                      (string)dataRow["imageFileName"],
                                      (byte[])dataRow["imageBlob"],
                                      (long)dataRow["imageFileSizeBytes"]
                                      );

                    // Saves the Image via a FileStream created by the OpenFile method.
                    using (FileStream stream = new FileStream(filename, FileMode.Create))
                    {
                        BinaryWriter bw = new BinaryWriter(stream);
                        bw.Write(image.ImageData);
                        isSucces = true;
                    }
                }
            }

            if (isSucces)
            {
            }
            else
            {
            }
        }

*/

    }
}

/* The code is based on code written by Kribo and released under the CPOL (Code Project Open License)
 * CPOL: http://www.codeproject.com/info/cpol10.aspx
 * This code accompanied the article at: http://www.codeproject.com/Articles/196618/C-SQLite-Storing-Images
 */