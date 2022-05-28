using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Threading;

namespace AUDIOTEST
{
    class Program
    {
        static Random rnd = new Random();

        static int pause = 0;

        static int buffer = 0;

        static int trig = 1;

      


        static void pauseCalc(int tempo)
        {
            int pause = tempo * 100 / 60;
        }


        static double PickFreq()
        {
            int temp = rnd.Next(1, 6);
            if (temp == 1)
                return 440;
            else if (temp == 2)
                return 880;
            else if (temp == 3)
                return 1396.91;
            else if (temp == 4)
                return 1318.51;
            else
                return 1046.50;
        }

        static void playHigh(int channel)
        {

            while (true)
            {
                if (trig == channel)
                {
                    var sineSeconds = new SignalGenerator()
                    {
                        Gain = 0.2,
                        Frequency = PickFreq(),
                        Type = SignalGeneratorType.Sin
                    }
               .Take(TimeSpan.FromMilliseconds(125/2));

                    using (var wo = new WaveOutEvent())
                    {
                        wo.Init(sineSeconds);
                        wo.Play();
                        while (wo.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(250);
                        }
                    }
                }
            }
        }

        static void playLow(int channel)
        {

            while (true)
            {
                if (trig == channel && rnd.Next(1,3)==1)
                {
                    var sineSeconds = new SignalGenerator()
                    {
                        Gain = 0.1,
                        Frequency = PickFreq()/4,
                        Type = SignalGeneratorType.Sin
                    }
           .Take(TimeSpan.FromMilliseconds(125/2));

                    using (var wo = new WaveOutEvent())
                    {
                        wo.Init(sineSeconds);
                        wo.Play();
                        while (wo.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(125/2);
                        }
                    }
                }
            }
        }

       static void newTrig()
        {
            while (true)
            {
                Thread.Sleep(125);
                trig = rnd.Next(1, 4);
            }
        }
            static void Main(string[] args)
            {
                int tempo = 120;
                pauseCalc(tempo);

                new Thread(() => playHigh( 1)).Start();

                new Thread(() => playLow( 2)).Start();

            new Thread(() => playLow(3)).Start();
            new Thread(() => newTrig()).Start();

        }
        }
    }

