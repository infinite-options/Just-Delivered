using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;

namespace JustDelivered.Views
{
    public partial class ConfirmationPage : ContentPage
    {
        double y = 0;
        public bool state = false;
        int CurrentIndex = 0;
        public static DeliveryInfo LocalDelivery = new DeliveryInfo();
        public ConfirmationPage(DeliveryInfo Delivery, int Index)
        {
            InitializeComponent();
            Debug.WriteLine("CONFIRMATION PAGE");
            Debug.WriteLine("CUSTOMER NAME: " + Delivery.name);
            Debug.WriteLine("DELIVERY STATUS: " + Delivery.status);

            CustomerName.Text = Delivery.name;
            CustomerAddress.Text = Delivery.house_address;
            Debug.WriteLine("PHONE (PARSHED) FROM LINE 24: " + Delivery.parsedPhone);
            Debug.WriteLine("PHONE  FROM LINE 25: " + Delivery.phone);
            CurrentIndex = Index;
            LocalDelivery = Delivery;
        }

        public void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            // Handle the pan
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't y + e.TotalY pan beyond the wrapped user interface element bounds.
                    var translateY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs((Height * .25) - Height));
                    BottomSheet.TranslateTo(BottomSheet.X, translateY, 20);
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    y = BottomSheet.TranslationY;

                    // At the end of the event - snap to the closest location
                    var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetClosestLockState(e.TotalY + y)));

                    // Depending on Swipe Up or Down - change the snapping animation
                    if (IsSwipeUp(e))
                    {
                        BottomSheet.TranslateTo(BottomSheet.X, finalTranslation, 250, Easing.SpringIn);
                    }
                    else
                    {
                        BottomSheet.TranslateTo(BottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    }

                    // Dismiss the keyboard after a transition
                    y = BottomSheet.TranslationY;

                    break;
            }
        }

        public bool IsSwipeUp(PanUpdatedEventArgs e)
        {
            if (e.TotalY < 0)
            {
                return true;
            }
            return false;
        }

        public double GetClosestLockState(double TranslationY)
        {
            // Play with these values to adjust the locking motions - this will change depending on the amount of content ona  apge
            var lockStates = new double[] { 0, .5, .85 };

            // Get the current proportion of the sheet in relation to the screen
            var distance = Math.Abs(TranslationY);
            var currentProportion = distance / Height;

            // Calculate which lockstate it's the closest to
            var smallestDistance = 10000.0;
            var closestIndex = 0;
            for (var i = 0; i < lockStates.Length; i++)
            {
                var state = lockStates[i];
                var absoluteDistance = Math.Abs(state - currentProportion);

                if (absoluteDistance < smallestDistance)
                {
                    smallestDistance = absoluteDistance;
                    closestIndex = i;
                }
            }

            var selectedLockState = lockStates[closestIndex];
            var TranslateToLockState = GetProportionCoordinate(selectedLockState);

            return TranslateToLockState;
        }

        public double GetProportionCoordinate(double proportion)
        {
            return proportion * Height;
        }

        public void ReturnButton()
        {

            //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);
        }


        public string GetName()
        {
            if(LocalDelivery.name != null && LocalDelivery.name != "")
                return LocalDelivery.name;
            return "";
        }

        // This functions gets the current customer's phone number
        public string GetPhone()
        {
            if (state)
            {
                if (LocalDelivery.parsedPhone != null && LocalDelivery.parsedPhone.Length == 10)
                {
                    return LocalDelivery.parsedPhone;
                }
            }
            else
            {
                if (LocalDelivery.parsedPhone != null && LocalDelivery.parsedPhone.Length == 10)
                {
                    return LocalDelivery.parsedPhone;
                }
            }
            return "";
        }

        // This functions gets the current customer's email address
        public string GetEmail()
        {
            if (LocalDelivery.email != null && LocalDelivery.email != "")
            {
                return LocalDelivery.email;
            }
            return "";
        }

        public async void DisplayException(string message)
        {
            await DisplayAlert("Alert", message, "OK");
        }

        public void ReturnHome()
        {
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void CameraUnableToLoad(string message)
        {
            await DisplayAlert("Alert", "The camera is not able to take pictures at the moment. Our developer is currently working on fixing this issue.", "Ok");
        }

        public void UpdateMessage(string message)
        {
            //swipeFrameNavigationMessage.Text = message;
        }

        public void UpdateSubMessage(string message)
        {
            //swipeFrameNavigationSubMessage.Text = message;
        }

        public async void WarningMessage()
        {
            await DisplayAlert("Alert!", "You must take a picture before sending an email confirmations!", "OK");
        }
    }
}
