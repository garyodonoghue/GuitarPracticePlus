using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace sample1
{
    public partial class Metronome : PhoneApplicationPage
    {
        Task playSoundTask = null;
        //start the metronome at 40 bpm
        int frequency = 40;

        public Metronome()
        {
            InitializeComponent();
            createPlaySoundTask();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            changeFrequency(false);
        }
        
        private void playSound()
        {
                Stream stream = TitleContainer.OpenStream("Sound/Click1.wav");
                SoundEffect effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                
                while (frequency >= 40)
                {
                    effect.Play();
                    Thread.Sleep((1000*60)/ (frequency));
                }
            }

        private void changeFrequency(bool incrementValue)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if (incrementValue && (value) < 120)
            {
                freqBtn.Content = value + 1;
            }
            if (!incrementValue && (value) > 40)
            {
                freqBtn.Content = value - 1;
            }

            frequency = Convert.ToInt32(freqBtn.Content);

            createPlaySoundTask();
        }

        private void createPlaySoundTask()
        {
            if (playSoundTask == null)
            {
                playSoundTask = Task.Factory.StartNew(() => playSound());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            changeFrequency(true);
        }
    }
}