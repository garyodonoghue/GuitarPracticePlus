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
    public partial class Tuning : PhoneApplicationPage
    {
        //default the string to be E, this will be used to track what string the 
        //user is tuning to 
        string currentString = "E"; 

        public Tuning()
        {
            InitializeComponent();
        }

        //Change the string the user is trying to tune to
        private void string_Click(object sender, RoutedEventArgs e)
        {
            Button _Button = (Button)sender;
            string guitar_string = _Button.CommandParameter.ToString();
            noteBtn.Content = guitar_string;
            currentString = guitar_string;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Metronome.xaml", UriKind.Relative));
        }

    }
}