using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Threading;

namespace AUDIOTEST
{

    class Program
    {
        static int bpm = 800;
        static int wait =60000/bpm;
        static Random rnd = new Random();
        static MixingSampleProvider mixer;
        static WasapiOut waveOut;

        static float PickFreq()
        {
            int temp = rnd.Next(1, 7);
            if (temp == 1)
                return 659.25f;
            else if (temp == 2)
                return 880;
            else if (temp == 3)
                return 1396.91f;
            else if (temp == 4)
                return 1318.51f;
            else if (temp == 5)
                return 987.77f;
            else
                return 1046.50f;
        }
        static AdsrSampleProvider CreateNote(float freq, SignalGeneratorType signal)
        {
            var signalGenerator1 = new SignalGenerator(44100, 1);
            signalGenerator1.Gain = 0.05;
            signalGenerator1.Type = signal;
            signalGenerator1.Frequency = freq/2;

            var signalGenerator2 = new SignalGenerator(44100, 1);
            signalGenerator2.Gain = 0.2;
            signalGenerator2.Type = SignalGeneratorType.SawTooth;
            signalGenerator2.Frequency = freq / 8;

            var mixerr = new MixingSampleProvider(new[] { signalGenerator1,signalGenerator2 });

            var adsr = new AdsrSampleProvider(mixerr);
            adsr.AttackSeconds = 0.05f;
            adsr.ReleaseSeconds = 0.7f;
            return adsr;
        }

        static void PlayNote(SignalGeneratorType signal)
        {
            while (true)
            {
                var adsr = CreateNote(PickFreq(), signal);
                Thread.Sleep(wait);
                if (rnd.Next(1, 3) == 1) 
                mixer.AddMixerInput(adsr); 
                Thread.Sleep(wait);
               
                mixer.RemoveMixerInput(adsr);
            }
        }
        static void PlayPerc()
        {//TO DO ADD SAMPLE INSTEAD OF GENERATIN A SOUND YOU FUCKING NERD
            while (true)
            {
                var signalGenerator1 = new SignalGenerator(44100, 1);
                signalGenerator1.Gain = 0.3;
                signalGenerator1.Frequency =50; // start frequency of the sweep
                signalGenerator1.FrequencyEnd = PickFreq()/2;
                signalGenerator1.Type = SignalGeneratorType.Sweep; 
                signalGenerator1.SweepLengthSecs =0.1; 
              
                var mixerr = new MixingSampleProvider(new[] { signalGenerator1 });

                var adsr = new AdsrSampleProvider(mixerr);
                adsr.AttackSeconds = 0.3f;
                adsr.ReleaseSeconds =0.5f;
                Thread.Sleep(2*wait/3);
              
                    mixer.AddMixerInput(adsr);
                Thread.Sleep(2 * wait /3);

                mixer.RemoveMixerInput(adsr);
            }
        }

        static void Main(string[] args)
        {
            waveOut = new WasapiOut(AudioClientShareMode.Shared, 50);

            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));



            mixer.ReadFully = true;
            waveOut.Init(mixer);

            waveOut.Play();
            new Thread(() => PlayNote(SignalGeneratorType.SawTooth)).Start();
            new Thread(() => PlayNote(SignalGeneratorType.Square)).Start(); 
            new Thread(() => PlayNote(SignalGeneratorType.Triangle)).Start();
            new Thread(() => PlayNote(SignalGeneratorType.Sin)).Start(); 
    
           // new Thread(() => PlayPerc()).Start();
          
            Console.ReadKey();
        }
    }
}
