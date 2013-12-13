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
        CancellationToken ct;
        CancellationTokenSource tokenSource2;

        public Metronome()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if (tokenSource2 != null)
            {
                ct = tokenSource2.Token;
                tokenSource2.Cancel();
            }

            if ((value) > 0)
            {
                value = value - 1;
                freqBtn.Content = value;

                if (!ct.IsCancellationRequested)
                {
                    tokenSource2 = new CancellationTokenSource();
                    playSoundTask = Task.Factory.StartNew(() => { playSound(value); }, tokenSource2.Token);
                }
                else
                {
                    ct.ThrowIfCancellationRequested();
                }
            }
        }

        //This needs to be in its own thread so it can also listen out for button 
        private void playSound(int frequency)
        {
            while (true)
            {
                Stream stream = TitleContainer.OpenStream("Sound/Click1.wav");
                SoundEffect effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                effect.Play();
                Thread.Sleep(1000/frequency);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if (tokenSource2 != null)
            {
                ct = tokenSource2.Token;
                tokenSource2.Cancel();
                playSoundTask = null;
            }

            if ((value) < 10)
            {
                value = value + 1;
                freqBtn.Content = value;

                if (!ct.IsCancellationRequested)
                {
                    tokenSource2 = new CancellationTokenSource();
                    playSoundTask = Task.Factory.StartNew(() => { playSound(value); }, tokenSource2.Token);
                } else{
                    ct.ThrowIfCancellationRequested();
                }
            };
        }
    }
}