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

namespace sample1
{
    public partial class Metronome : PhoneApplicationPage
    {
        public Metronome()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if ((value) > 0)
            {
                value = value - 1;
                freqBtn.Content = value;

                playSound(value);
            }
        }

        private void playSound(int frequency)
        {
            Stream stream = TitleContainer.OpenStream("Sound/Click1.wav");
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();

            //System.Threading.Thread.Sleep(1000);
            //playSound(frequency);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if ((value) < 10)
            {
                value = value + 1;
                freqBtn.Content = value;
            };

            playSound(value);
        }
    }
}