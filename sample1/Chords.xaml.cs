using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GuitarGuideLite.Resources;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace sample1
{
    public partial class MainPage : PhoneApplicationPage
    {        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Image image = chordImage;
            image.Source = null;
            BitmapImage imgSource = null;

            Button _myButton = (Button)sender;
            string value = _myButton.CommandParameter.ToString();

            if (value.Equals("A"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/aChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("B"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/bChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("C"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/cChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("D"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/dChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("E"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/eChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("F"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Images/fChord.jpg", UriKind.Relative));
            }

            image.Source = imgSource;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Record.xaml", UriKind.Relative));
        }
    }
}