using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StoringImages.Model
{
    public class Image
    {
        private string fileName = string.Empty;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private byte[] imageData;
        public byte[] ImageData
        {
            get { return imageData; }
            set { imageData = value; }
        }
        private long fileSize = 0;
        public long FileSize
        {
            get { return fileSize; }
            set {fileSize = value; }
        }
        public Image()
        { }
        public Image(string a, byte[] b, long c)
        {
            this.FileName = a;
            this.ImageData = b;
            this.FileSize = c;
        }
    }
}

/* The code is based on code written by Kribo and released under the CPOL (Code Project Open License)
 * CPOL: http://www.codeproject.com/info/cpol10.aspx
 * This code accompanied the article at: http://www.codeproject.com/Articles/196618/C-SQLite-Storing-Images
 */
