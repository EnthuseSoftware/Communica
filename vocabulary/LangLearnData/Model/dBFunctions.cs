using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace StoringImages.Model
{
    public class dBFunctions
    {
        public static string ConnectionStringVocab
        {
            get
            {
                string database =
                    AppDomain.CurrentDomain.BaseDirectory + @"../../../LangLearnData/Database/LLVocab.s3db";
                    //AppDomain.CurrentDomain.BaseDirectory + "\\Database\\ImageLib.s3db";

				string fullPath = Path.GetFullPath(database);
				if (File.Exists(fullPath))
					return @"Data Source=" + fullPath;
				else
	            	return "";            
			}
        }

		public static string ConnectionStringUserInfo
		{
			get
			{
				string database =
					AppDomain.CurrentDomain.BaseDirectory + @"../../../LangLearnData/Database/LLUserInfo.s3db";
				//AppDomain.CurrentDomain.BaseDirectory + "\\Database\\ImageLib.s3db";
				
				string fullPath = Path.GetFullPath(database);
				if (File.Exists(fullPath))
					return @"Data Source=" + fullPath;
				else
					return "";            
			}
		}
	}
}

/* The code is based on code written by Kribo and released under the CPOL (Code Project Open License)
 * CPOL: http://www.codeproject.com/info/cpol10.aspx
 * This code accompanied the article at: http://www.codeproject.com/Articles/196618/C-SQLite-Storing-Images
 */