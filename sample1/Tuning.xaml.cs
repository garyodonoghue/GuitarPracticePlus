using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SoundAnalysis;
using System.IO;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace sample1
{
    public partial class Tuning : PhoneApplicationPage
    {
        //default the string to be E, this will be used to track what string the 
        //user is tuning to 
        string currentString = "E"; 

        Microphone microphone = Microphone.Default;
        byte[] buffer;
        MemoryStream stream = new MemoryStream();
        List<SoundEffect> listSounds = new List<SoundEffect>();

        public Tuning()
        {
            InitializeComponent();

            setUpNoteMap();

            // Timer to simulate the XNA Game Studio game loop (Microphone is from XNA Game Studio)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(33);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();
            microphone.BufferReady += new EventHandler<EventArgs>(microphone_BufferReady);

            record();
        }

        private void setUpNoteMap() {
            noteMap.Add("A", 1);
            noteMap.Add("A#", 2);
            noteMap.Add("B/H", 3);
            noteMap.Add("C", 4);
            noteMap.Add("C#", 5);
            noteMap.Add("D", 6);
            noteMap.Add("D#", 7);
            noteMap.Add("E", 8);
            noteMap.Add("F", 9);
            noteMap.Add("F#", 10);
            noteMap.Add("G", 11);
            noteMap.Add("G#", 12);
        }
        //Change the string the user is trying to tune to
        private void string_Click(object sender, RoutedEventArgs e)
        {
            Button _Button = (Button)sender;
            string guitar_string = _Button.CommandParameter.ToString();
            noteBtn.Content = guitar_string;
            currentString = guitar_string;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Metronome.xaml", UriKind.Relative));
        }


        /////////////////////////////////////////////////////////////////////////////////
        ////////////////////// Microphone Stuff ////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////

        void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);
            stream.Write(buffer, 0, buffer.Length);
            
            microphone.Stop();
            
            double freq = FindFundamentalFrequency(convertToDouble(stream.ToArray()), microphone.SampleRate, 60, 1300);
            System.Diagnostics.Debug.WriteLine("fundamental freq: " + freq);

            double closestFrequency;
            string noteName;

            string closestNote = FindClosestNote(freq, out closestFrequency, out noteName);
            System.Diagnostics.Debug.WriteLine("closest note: " + closestNote);

            updateDisplay(closestNote);

            stream = new MemoryStream();

            //record another seconds worth of data
            record();
        }
        
        static string[] NoteNames = { "A", "A#", "B/H", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
        static double ToneStep = Math.Pow(2, 1.0 / 12);
        Dictionary<string, int> noteMap = new Dictionary<string, int>();

        private void updateDisplay(string closestNte) {
            int actualNote = noteMap[closestNte];
            int desiredNote = noteMap[currentString];

            //check if note is flat, sharp or in tune
            if(actualNote == desiredNote)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(noteBtn);
                IInvokeProvider invokeProv =
                  peer.GetPattern(PatternInterface.Invoke)
                  as IInvokeProvider;
                invokeProv.Invoke();
            }
            else if (actualNote < desiredNote) //highlight flat button
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(flatBtn);
                IInvokeProvider invokeProv =
                  peer.GetPattern(PatternInterface.Invoke)
                  as IInvokeProvider;
                invokeProv.Invoke();
            }
            else {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(sharpBtn);
                IInvokeProvider invokeProv =
                  peer.GetPattern(PatternInterface.Invoke)
                  as IInvokeProvider;
                invokeProv.Invoke();
            }

            noteBtn.Opacity = 1.0;
            flatBtn.Opacity = 1.0;
            sharpBtn.Opacity = 1.0;
        }
        
        private void record()
        {
            //1. record one second's data. 
            //2. stop.
            //3. analyse the data. 
            //4. repeat
            microphone.BufferDuration = TimeSpan.FromMilliseconds(1000);
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
            microphone.Start();
        }

        //Convert byte array from captured sound to double array for frequency analysis.
        private double[] convertToDouble(byte[] bytes){
            double[] doubleArr = new double[bytes.Length];
            int counter = 0;

            foreach(byte byt in bytes){
                doubleArr[counter] = Convert.ToDouble(byt);
                counter++;
            }
            return doubleArr;
        }


        /////////////////////////////////////////////////////////////////////////////////
        ////////////////////// FFT Stuff ////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Calculates FFT using Cooley-Tukey FFT algorithm.
        /// </summary>
        /// <param name="x">input data</param>
        /// <returns>spectrogram of the data</returns>
        /// <remarks>
        /// If amount of data items not equal a power of 2, then algorithm
        /// automatically pad with 0s to the lowest amount of power of 2.
        /// </remarks>
        public static double[] Calculate(double[] x)
        {
            int length;
            int bitsInLength;
            if (IsPowerOfTwo(x.Length))
            {
                length = x.Length;
                bitsInLength = Log2(length) - 1;
            }
            else
            {
                bitsInLength = Log2(x.Length);
                length = 1 << bitsInLength;
                // the items will be pad with zeros
            }

            // bit reversal
            ComplexNumber[] data = new ComplexNumber[length];
            for (int i = 0; i < x.Length; i++)
            {
                int j = ReverseBits(i, bitsInLength);
                data[j] = new ComplexNumber(x[i]);
            }

            // Cooley-Tukey 
            for (int i = 0; i < bitsInLength; i++)
            {
                int m = 1 << i;
                int n = m * 2;
                double alpha = -(2 * Math.PI / n);

                for (int k = 0; k < m; k++)
                {
                    // e^(-2*pi/N*k)
                    ComplexNumber oddPartMultiplier = new ComplexNumber(0, alpha * k).PoweredE();

                    for (int j = k; j < length; j += n)
                    {
                        ComplexNumber evenPart = data[j];
                        ComplexNumber oddPart = oddPartMultiplier * data[j + m];
                        data[j] = evenPart + oddPart;
                        data[j + m] = evenPart - oddPart;
                    }
                }
            }

            // calculate spectrogram
            double[] spectrogram = new double[length];
            for (int i = 0; i < spectrogram.Length; i++)
            {
                spectrogram[i] = data[i].AbsPower2();
            }
            return spectrogram;
        }

        /// <summary>
        /// Gets number of significat bytes.
        /// </summary>
        /// <param name="n">Number</param>
        /// <returns>Amount of minimal bits to store the number.</returns>
        private static int Log2(int n)
        {
            int i = 0;
            while (n > 0)
            {
                ++i; n >>= 1;
            }
            return i;
        }

        /// <summary>
        /// Reverses bits in the number.
        /// </summary>
        /// <param name="n">Number</param>
        /// <param name="bitsCount">Significant bits in the number.</param>
        /// <returns>Reversed binary number.</returns>
        private static int ReverseBits(int n, int bitsCount)
        {
            int reversed = 0;
            for (int i = 0; i < bitsCount; i++)
            {
                int nextBit = n & 1;
                n >>= 1;

                reversed <<= 1;
                reversed |= nextBit;
            }
            return reversed;
        }

        /// <summary>
        /// Checks if number is power of 2.
        /// </summary>
        /// <param name="n">number</param>
        /// <returns>true if n=2^k and k is positive integer</returns>
        private static bool IsPowerOfTwo(int n)
        {            
            return n > 1 && (n & (n - 1)) == 0;
        }



        /// <summary>
        /// Finds fundamental frequency: calculates spectrogram, finds peaks, analyzes
        /// and refines frequency by diff sample values.
        /// </summary>
        /// <param name="x">The sounds samples data</param>
        /// <param name="sampleRate">The sound sample rate</param>
        /// <param name="minFreq">The min useful frequency</param>
        /// <param name="maxFreq">The max useful frequency</param>
        /// <returns>Found frequency, 0 - otherwise</returns>
        internal static double FindFundamentalFrequency(double[] x, int sampleRate, double minFreq, double maxFreq)
        {
            double[] spectr = Calculate(x);

            int usefullMinSpectr = Math.Max(0,
                (int)(minFreq * spectr.Length / sampleRate));
            int usefullMaxSpectr = Math.Min(spectr.Length,
                (int)(maxFreq * spectr.Length / sampleRate) + 1);

            // find peaks in the FFT frequency bins 
            const int PeaksCount = 5;
            int[] peakIndices;
            peakIndices = FindPeaks(spectr, usefullMinSpectr, usefullMaxSpectr - usefullMinSpectr,
                PeaksCount);

            if (Array.IndexOf(peakIndices, usefullMinSpectr) >= 0)
            {
                // lowest usefull frequency bin shows active
                // looks like is no detectable sound, return 0
                return 0;
            }

            // select fragment to check peak values: data offset
            const int verifyFragmentOffset = 0;
            // ... and half length of data
            int verifyFragmentLength = (int)(sampleRate / minFreq);

            // trying all peaks to find one with smaller difference value
            double minPeakValue = Double.PositiveInfinity;
            int minPeakIndex = 0;
            int minOptimalInterval = 0;
            for (int i = 0; i < peakIndices.Length; i++)
            {
                int index = peakIndices[i];
                int binIntervalStart = spectr.Length / (index + 1), binIntervalEnd = spectr.Length / index;
                int interval;
                double peakValue;
                // scan bins frequencies/intervals
                ScanSignalIntervals(x, verifyFragmentOffset, verifyFragmentLength,
                    binIntervalStart, binIntervalEnd, out interval, out peakValue);

                if (peakValue < minPeakValue)
                {
                    minPeakValue = peakValue;
                    minPeakIndex = index;
                    minOptimalInterval = interval;
                }
            }

            return (double)sampleRate / minOptimalInterval;
        }

        private string FindClosestNote(double freq, out double closestFrequency, out string noteName)
        {
            const double AFrequency = 440.0;
            const int ToneIndexOffsetToPositives = 120;

            int toneIndex = (int)Math.Round(Math.Log(freq / AFrequency, ToneStep));
            noteName = NoteNames[(ToneIndexOffsetToPositives + toneIndex) % NoteNames.Length];
            closestFrequency = Math.Pow(ToneStep, toneIndex) * AFrequency;
            
            return noteName;
        }

        private static int[] FindPeaks(double[] values, int index, int length, int peaksCount)
        {
            double[] peakValues = new double[peaksCount];
            int[] peakIndices = new int[peaksCount];

            for (int i = 0; i < peaksCount; i++)
            {
                peakValues[i] = values[peakIndices[i] = i + index];
            }

            // find min peaked value
            double minStoredPeak = peakValues[0];
            int minIndex = 0;
            for (int i = 1; i < peaksCount; i++)
            {
                if (minStoredPeak > peakValues[i]) minStoredPeak = peakValues[minIndex = i];
            }

            for (int i = peaksCount; i < length; i++)
            {
                if (minStoredPeak < values[i + index])
                {
                    // replace the min peaked value with bigger one
                    peakValues[minIndex] = values[peakIndices[minIndex] = i + index];

                    // and find min peaked value again
                    minStoredPeak = peakValues[minIndex = 0];
                    for (int j = 1; j < peaksCount; j++)
                    {
                        if (minStoredPeak > peakValues[j]) minStoredPeak = peakValues[minIndex = j];
                    }
                }
            }

            return peakIndices;
        }

        private static void ScanSignalIntervals(double[] x, int index, int length,
           int intervalMin, int intervalMax, out int optimalInterval, out double optimalValue)
        {
            optimalValue = Double.PositiveInfinity;
            optimalInterval = 0;

            // distance between min and max range value can be big
            // limiting it to the fixed value
            const int MaxAmountOfSteps = 30;
            int steps = intervalMax - intervalMin;
            if (steps > MaxAmountOfSteps)
                steps = MaxAmountOfSteps;
            else if (steps <= 0)
                steps = 1;

            // trying all intervals in the range to find one with
            // smaller difference in signal waves
            for (int i = 0; i < steps; i++)
            {
                int interval = intervalMin + (intervalMax - intervalMin) * i / steps;

                double sum = 0;
                for (int j = 0; j < length; j++)
                {
                    double diff = x[index + j] - x[index + j + interval];
                    sum += diff * diff;
                }
                if (optimalValue > sum)
                {
                    optimalValue = sum;
                    optimalInterval = interval;
                }
            }
        }
    }
}