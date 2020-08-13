using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Foundation;
using UIKit;

namespace DeliveryApp.iOS
{

    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.

            // I had to call the below function to make maps work. 
            Xamarin.FormsMaps.Init();
            
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
