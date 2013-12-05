using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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

            if ((value)>0){
                value = value - 1;
                freqBtn.Content = value;
            };

            playSound();
        }

        private void playSound()
        {
            //throw new NotImplementedException();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(freqBtn.Content);

            if ((value) < 10)
            {
                value = value + 1;
                freqBtn.Content = value;
            };
        }
    }
}