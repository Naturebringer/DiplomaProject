using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSynthLite
{
    public class PianoArray
    {
        public string[] NoteNames = new string[88];
        public double[] Frequency = new double[88];
        public string[] ThreadsNames = new string[88];

        public PianoArray()
        {
            NoteNames[0] = "A0";
            Frequency[0] = 27.500;
        }
    }
}
