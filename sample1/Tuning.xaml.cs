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
        }

    }
}