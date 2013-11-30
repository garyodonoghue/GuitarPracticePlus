using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using sample1.Resources;
using System.Windows.Media.Imaging;

namespace sample1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
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
                 new Uri("/Resources/achord.jpg", UriKind.Relative));
            }
            else if (value.Equals("B"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Resources/bChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("C"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Resources/cChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("D"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Resources/dChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("E"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Resources/eChord.jpg", UriKind.Relative));
            }
            else if (value.Equals("F"))
            {
                imgSource = new BitmapImage(
                 new Uri("/Resources/fChord.jpg", UriKind.Relative));
            }

            image.Source = imgSource;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}