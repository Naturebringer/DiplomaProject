using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Threading;

namespace MSynthLite
{
    public class MainParam
    {
        public SignalGeneratorType WaveForm;
        public double Frequency;
        public double Time;

        public MainParam (SignalGeneratorType wave, double freq, double time)
        {
            WaveForm = wave;
            Frequency = freq;
            Time = time;
        }
    }
}
