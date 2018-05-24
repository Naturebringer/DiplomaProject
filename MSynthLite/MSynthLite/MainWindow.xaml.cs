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
        

        public MainWindow()
        {
            InitializeComponent();
        }

        public SignalGenerator SimpleGenerator = new SignalGenerator();
        public SignalGenerator NotetableGeneretor = new SignalGenerator();
        public SignalGenerator GeneretorA = new SignalGenerator();
        public SignalGenerator GeneretorB = new SignalGenerator();
        public SignalGenerator LFO_A = new SignalGenerator();
        public SignalGenerator LFO_B = new SignalGenerator();


        public bool SimpleGeneratorState = false;
        public bool PButton1_State = false;


        private void Play_Click(object sender, RoutedEventArgs e)
        {
            switch (Generato_type.SelectedIndex)
            {
                case 0:
                    SignalGeneratorType WaveType = new SignalGeneratorType();
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
                    double Testtime = Convert.ToDouble(SimpleTime.Text.Trim());
                    double TestFrequency = Convert.ToDouble(SimpleFrequency.Text.Trim());
                    SimpleGenerator.Gain = SimpleGainSlider.Value;

                    MainParam InputParams = new MainParam(WaveType, TestFrequency, Testtime);
                    Thread myThread = new Thread(new ThreadStart(() => PlaySimpleMusic(InputParams)));
                    myThread.Start();
                    double a;
                    double d;
                    double s;
                    double r;
                    if (Main_Gain_ADSR_Box.IsChecked == true)
                    {
                        
                        Thread SimpleGainADSRThread = new Thread(new ThreadStart(() => SimpleGainADSR_Work(5,3,0.5,2)));
                        SimpleGainADSRThread.Start();
                    }
                    if (Main_Frequency_ADSR_Box.IsChecked == true)
                    {
                        Thread SimpleFrequencyADSRThread = new Thread(new ThreadStart(() => PlaySimpleMusic(InputParams)));
                        SimpleFrequencyADSRThread.Start();
                    }
                    break;

                case 1:
                    double[] notearr = new double[80];
                    int timequant = 5;
                    notearr[0] = 440;
                    notearr[1] = 320;
                    notearr[2] = 200;
                    notearr[3] = 150;
                    notearr[4] = 240;
                    notearr[5] = 370;
                    NotetableGeneretor.Type = SignalGeneratorType.Sin;
                    Thread myThread2 = new Thread(new ThreadStart(() => PlayTableMusic(notearr, timequant)));
                    myThread2.Start();
                    break;
                case 2:
                    break;
            
            }
        }

        public void SimpleGainADSR_Work(double attack, double decay, double sustain, double release)
        {
            //SimpleGenerator.Gain = 0;
            double MaxGain =0.8;
            double timeA = 100*attack/MaxGain;
            double timeD = 100*decay/sustain;
            double timeS = 5;
            double timeR;

            int i = 0;

            while (SimpleGeneratorState == true)
            {
                while ((SimpleGeneratorState == true) || (SimpleGenerator.Gain < 0.8))
                {
                    SimpleGenerator.Gain = SimpleGenerator.Gain + 0.02;
                }

                while ((SimpleGeneratorState == true) || (SimpleGenerator.Gain > 0.2))
                {
                    SimpleGenerator.Gain = SimpleGenerator.Gain - timeD;
                }



            }


        }

        public void SimpleFrequencyADSR_Work()
        {

        }

        public void PlayTableMusic(double[] notecollection, int time)
        {
            NotetableGeneretor.Take(TimeSpan.FromSeconds(time*5));
            NotetableGeneretor.Gain = 0.05;
            {
                using (var wo = new WaveOutEvent())
                {
                    wo.Init(NotetableGeneretor);
                    wo.Play();
                    for (int i = 0; i < 5; i++)
                    {
                        NotetableGeneretor.Frequency = notecollection[i];
                        Thread.Sleep(Convert.ToInt32(time) * 1000);
                    }
                    wo.Stop();
                }
            }
        }

        //Обычная генерация

        public void PlaySimpleMusic(MainParam param)
        {
            SimpleGeneratorState = true;
            SimpleGenerator.Frequency = param.Frequency;
            SimpleGenerator.Type = param.WaveForm;
            if (param.Time != 0)
            {
                SimpleGenerator.Take(TimeSpan.FromSeconds(param.Time));
                using (var wo = new WaveOutEvent())
                {
                    wo.Init(SimpleGenerator);
                    wo.Play();              
                    Thread.Sleep(Convert.ToInt32(param.Time)*1000);
                    wo.Stop();
                }
            }
            else
            {
                using (var wo = new WaveOutEvent())
                {
                    wo.Init(SimpleGenerator);
                    wo.Play();
                    while ((wo.PlaybackState == PlaybackState.Playing) || SimpleGeneratorState)
                    {
                        if (SimpleGeneratorState == false)
                        {
                            wo.Stop();
                            break;
                        }
                    }
                }
            }
        }

        private void WaveTypeSimple_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SignalGeneratorType WaveType = new SignalGeneratorType();
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
            SimpleGenerator.Type = WaveType;
        }
       
        private void SimpleFrequency_TextChanged(object sender, TextChangedEventArgs e)
        {
            SimpleGenerator.Frequency = Convert.ToDouble(SimpleFrequency.Text.Trim());
        }

        public PianoArray pianokeys = new PianoArray();

        //таблица



        //Piano
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }



        public void PlayPianoMusic(SignalGenerator generator, int num, bool state)
        {
            using (var wo = new WaveOutEvent())
            {
                wo.Init(generator);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    //Thread.Sleep(100);
                    if (PButton1_State == false)
                    {
                        wo.Stop();
                        break;
                    }
                }
                wo.Stop();
            }
        }

        private void PButton1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PButton1_State = false;
        }

        private void PButton1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PButton1_State = true;
            var PianoKey1 = new SignalGenerator();
            PianoKey1.Gain = 0.05;
            // PianoKey1.Frequency = pianokeys.Frequency[0];
            PianoKey1.Frequency = 440;
            PianoKey1.Type = SignalGeneratorType.Sin;

            Thread myThread = new Thread(new ThreadStart(() => PlayPianoMusic(PianoKey1, 0, PButton1_State)));
            pianokeys.ThreadsNames[0] = myThread.Name;
            myThread.Start();
        }

        private void PButton1_LostFocus(object sender, RoutedEventArgs e)
        {
            PButton1_State = false;
        }

        private void FilterType_A_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FilterType_B_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SimpleGain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SimpleGenerator.Gain = SimpleGainSlider.Value;
        }

        private void Stop_playing_Click(object sender, RoutedEventArgs e)
        {
            SimpleGeneratorState = false;
        }

        private void ApplicationVolum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
    }
}
