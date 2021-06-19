using System;
using Xamarin.Forms.Maps;

namespace JustDelivered.Controls
{
    public class CustomPin: Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Number { get; set; }
        public string Color { get; set; }
    }
}
