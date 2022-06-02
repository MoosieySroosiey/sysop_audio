//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Windows;
//using NAudio.CoreAudioApi;
//using NAudio.Wave;
//using NAudio.Wave.SampleProviders;

//namespace AUDIOTEST
//{
//    class PlaybackEngine : IDisposable
//    {
//        private readonly Dictionary<string, string> noteFiles = new Dictionary<string, string>()
//        {
//            {"C", "P1D V105 C4.wav"},
//            {"D#", "P1D V105 Eb4.wav"},
//            {"F#", "P1D V105 Gb4.wav"},
//            {"A", "P1D V105 A4.wav"},
//        };

//        private readonly Dictionary<string, ISampleProvider> mixerInputs = new Dictionary<string, ISampleProvider>();

//        private readonly Dictionary<string, byte[]> sampleData = new Dictionary<string, byte[]>();

//        private readonly WasapiOut waveOut;
//        private readonly MixingSampleProvider mixer;

//        public PlaybackEngine()
//        {
//            waveOut = new WasapiOut(AudioClientShareMode.Shared, 50);
//            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
//            LoadSampleData();
//            mixer.ReadFully = true;
//            waveOut.Init(mixer);
//            waveOut.Play();
//        }

//        private void LoadSampleData()
//        {
//            foreach (var kvp in noteFiles)
//            {
//                using (var reader = new WaveFileReader("samples\\" + kvp.Value))
//                {
//                    var data = new byte[reader.Length];
//                    reader.Read(data, 0, (int)reader.Length);
//                    sampleData[kvp.Key] = data;
//                }
//            }
//        }

//        public void StartNote(string noteName)
//        {
//            byte[] data;
//            if (sampleData.TryGetValue(noteName, out data))
//            {
//                var sampleStream = new RawSourceWaveStream(new MemoryStream(data), new WaveFormat(44100, 16, 1));
//                var sampleProvider = sampleStream.ToSampleProvider();
//                mixerInputs[noteName] = sampleProvider;
//                mixer.AddMixerInput(sampleProvider);
//            }
//        }

//        public void StopNote(string noteName)
//        {
//            ISampleProvider mixerInput;
//            if (mixerInputs.TryGetValue(noteName, out mixerInput))
//            {
//                mixer.RemoveMixerInput(mixerInput);
//            }
//        }

//        public void Dispose()
//        {
//            waveOut.Dispose();
//        }
//    }
//}