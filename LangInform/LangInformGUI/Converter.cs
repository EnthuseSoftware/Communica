using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangInformGUI
{
    public class StreamHelper
    {
        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

    }
    public class Converter
    {
        public Byte[] BitmapToByte(Bitmap bitmap)
        {
            System.Drawing.Image bmp = bitmap;
            //Modify bmp
            Byte[] blob;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            blob = StreamHelper.ReadToEnd(stream);
            //blob = new byte[stream.Length];
            stream.Read(blob, 0, (int)stream.Length);
            stream.Close();
            return blob;
        }

        public Byte[] SoundToByte(string soundPath)
        {
            FileStream fs = new FileStream(soundPath, FileMode.Open);
            System.IO.Stream stream;
            stream = fs;
            Byte[] blob;
            blob = StreamHelper.ReadToEnd(stream);
            return blob;
        }

        public Bitmap byteArrayToBitmap(Byte[] byteArrayIn)
        {

            MemoryStream ms = new MemoryStream(byteArrayIn, true);
            ms.Write(byteArrayIn, 0, byteArrayIn.Length);
            Bitmap bp = (Bitmap)Bitmap.FromStream(ms);
            //Image returnImage = Image.FromStream(ms);
            return bp;
        }

        public Stream byteArrayToStream(Byte[] bytes)
        {
            Stream str = new MemoryStream(bytes);
            return str;
        }

        public System.Windows.Controls.Image ByteToWPFImage(byte[] blob)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(blob, 0, blob.Length);
            stream.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            bi.BeginInit();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            System.Windows.Controls.Image image2 = new System.Windows.Controls.Image() { Source = bi };
            return image2;
        }
    }
}
