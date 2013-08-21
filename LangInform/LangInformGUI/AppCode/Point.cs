using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LangInformGUI.AppCode
{
    public class PicturePoints
    {
        public string Name { get; set; }

        public string Path { get; set; }

        List<MyPoint> _points = new List<MyPoint>();
        public List<MyPoint> Points { get { return _points; } set { _points = value; } }

    }

    public class MyPoint
    {
        public Guid ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Size { get; set; }
        public bool IsRound { get; set; }
        public string PathToSound { get; set; }

        public override string ToString()
        {
            return Math.Round(X) + " - " + Math.Round(Y);
        }
    }
}
