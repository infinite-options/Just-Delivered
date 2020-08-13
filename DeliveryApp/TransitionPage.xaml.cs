using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class TransitionPage : ContentPage
    {
        public TransitionPage()
        {
            InitializeComponent();
            SetIconOnTransitionPage();
        }

        public void SetIconOnTransitionPage()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                iconTransition.Source = "JDIcon.png";
                iconTransition.Scale = 0.5;
            }
            else
            {
                iconTransition.Source = "Icon.png";
                iconTransition.Scale = 1;
            }
        }
    }
}
