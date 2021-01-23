using System;
using JustDelivered.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JustDelivered
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new MainPage();
            MainPage = new LogInPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
