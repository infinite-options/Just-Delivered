using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DeliveryApp.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using Xamarin;

namespace DeliveryApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // This is how we declare and instanciate a stack.
            MainPage = new NavigationPage(new DeliveryApp.MainPage());
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
