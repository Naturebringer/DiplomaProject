using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Threading;

namespace MSynthLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PianoArray pianokeys = new PianoArray();

        public MainWindow()
        {
            InitializeComponent();
        }
         
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Thread myThread = new Thread(new ThreadStart(PlayMusic));
            // myThread.Start();
        }


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            SignalGeneratorType WaveType = SignalGeneratorType.Sin;
            switch (WaveTypeSimple.SelectedIndex)
            {
                case 0:
                    WaveType = SignalGeneratorType.Sin;
                    break;
                case 1:
                    WaveType = SignalGeneratorType.Square;
                    break;
                case 2:
                    WaveType = SignalGeneratorType.Triangle;
                    break;
                case 3:
                    WaveType = SignalGeneratorType.SawTooth;
                    break;
                case 4:
                    WaveType = SignalGeneratorType.White;
                    break;
                case 5:
                    WaveType = SignalGeneratorType.Pink;
                    break;
                case 6:
                    WaveType = SignalGeneratorType.Sweep;
                    break;
            }

            Thread myThread = new Thread(new ThreadStart(()=> PlaySimpleMusic(SignalGeneratorType , Convert.ToInt32(SimpleFrequency.Text), Convert.ToInt32(SimpleTime.Text))));
            myThread.Start();
                //var sine20Seconds = new SignalGenerator()
                //{
                //    Gain = 0.01,
                //    Frequency = 440,
                //    Type = SignalGeneratorType.SawTooth
                //}
                //    .Take(TimeSpan.FromSeconds(5));
                //using (var wo = new WaveOutEvent())
                //{
                //    wo.Init(sine20Seconds);
                //    wo.Play();
                //    while (wo.PlaybackState == PlaybackState.Playing)
                //    {
                //        Thread.Sleep(500);
                //    }
                //}
        }

        public void PlaySimpleMusic(SignalGeneratorType type, double freq, double time)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var PianoKey1 = new SignalGenerator();
            PianoKey1.Gain = 0.2;
            PianoKey1.Frequency = pianokeys.Frequency[0];
            PianoKey1.Type = SignalGeneratorType.Sin;

            Thread myThread = new Thread(new ThreadStart(() => PlayPianoMusic(PianoKey1,0)));
            pianokeys.ThreadsNames[0] = myThread.Name;
            myThread.Start();
        }



        public void PlayPianoMusic(SignalGenerator generator, int num)
        {
            using (var wo = new WaveOutEvent())
            {
                wo.Init(generator);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(500);
                }
            }
        }

 
    }
}
