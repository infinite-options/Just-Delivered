using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryApp.Models;
using Plugin.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class SixthPage : ContentPage
    {
        // Note some icons come from icons8.com, we give credit for their work.
        ServingNowList deliveryData = new ServingNowList();
        ServingNowList deliveryDataCopy = new ServingNowList();

        int currentDelivery = 0;
        int index = 0;

        double y = 0;

        public bool state = false;

        string action = "";

        public SixthPage(ServingNowList deliveryData, int currentDelivery, ServingNowList deliveryDataCopy, int index)
        {
            this.deliveryData = deliveryData;
            this.deliveryDataCopy = deliveryDataCopy;
            this.currentDelivery = currentDelivery;
            this.index = index;

            InitializeComponent();
            SetUpCustomerInfo(this.index);
        }

        // This function sets current delivery customer's info
        public void SetUpCustomerInfo(int i)
        {
            swipeFrameNavigationMessage.Text = "Take picture of delivery";
            deliveryInstructionsMessage.Text = "There are no delivery instructions for this delivery.";

            if (i == -1)
            {
                customerNameLabel.Text = deliveryData.result[0].firstNameAndFirstLetterLastName;
                addressLabel.Text = deliveryData.result[0].house_address;
                cityStateZipcodeLabel.Text = deliveryData.result[0].city + ", " + deliveryData.result[0].state + ", " + deliveryData.result[0].zipcode;
            }
            else
            {
                state = true;
                customerNameLabel.Text = deliveryDataCopy.result[i].firstNameAndFirstLetterLastName;
                addressLabel.Text = deliveryDataCopy.result[i].house_address;
                cityStateZipcodeLabel.Text = deliveryDataCopy.result[i].city + ", " + deliveryDataCopy.result[i].state + ", " + deliveryDataCopy.result[i].zipcode;
            }
        }

        // This functions updates the list of deliveries 
        public void UpdateDeliveryList()
        {
            for (int i = 1; i < this.deliveryData.result.Count; i++)
            {
                deliveryData.result[i - 1] = deliveryData.result[i];
            }
            deliveryData.result.RemoveAt(deliveryData.result.Count - 1);
        }

        // This functions gets the current customer's name
        public string GetName()
        {
            if (state)
            {
                return deliveryData.result[index].firstNameAndFirstLetterLastName;
            }
            else
            {
                return deliveryData.result[0].firstNameAndFirstLetterLastName;
            }
        }

        // This functions gets the current customer's phone number
        public string GetPhone()
        {
            if (state)
            {
                return deliveryData.result[index].parsedPhone;
            }
            else
            {
                return deliveryData.result[0].parsedPhone;
            }
        }

        // This functions gets the current customer's email address
        public string GetEmail()
        {
            if (state)
            {
                return deliveryData.result[index].email;
            }
            else
            {
                return deliveryData.result[0].email;
            }
        }

        // This functions update the a message on the bottom sheet 
        public void UpdateMessage(string message)
        {
            swipeFrameNavigationMessage.Text = message;
        }

        // This functions update the a sub message on the bottom sheet
        public void UpdateSubMessage(string message)
        {
            swipeFrameNavigationSubMessage.Text = message;
        }

        // This functions gets the current customer's email address
        public async void CallCustomer(System.Object sender, System.EventArgs e)
        {

            if (GetPhone().Length == 10)
            {
                PhoneDialer.Open(GetPhone());
            }
            else
            {
                await DisplayAlert("Alert", "Phone number not available", "OK");
            }
        }

        // This function removes elements from the navigation
        // stack to update the delivery page with the next delivery
        public async void ReturnButton()
        {
            UpdateDeliveryList();
            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new FourthPage(deliveryData, deliveryDataCopy, currentDelivery + 1, deliveryData.result.Count + currentDelivery));
        }

        // This function removes elements from the navigation
        // stack to update the delivery page with the next delivery coming
        // the total list of deliveries.
        public async void ReturnHome()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        // =====================================================================
        // This function prompts the user with a warning to take a picture before sending a confirmation (iOS & ANDROID)
        public async void WarningMessage()
        {
            await DisplayAlert("Alert!", "You must take a picture before sending an email confirmations!", "OK");
        }

        // This function prompts the user with a warning that the camera was not able to load (iOS)
        public async void CameraUnableToLoad(string message)
        {
            await DisplayAlert("Alert", "The camera is not able to take pictures at the moment. Our developer is currently working on fixing this issue.", "Ok");
        }

        // This function prompts the user with a message to allow the app to access their current location in settings (ANDROID)
        public async void CameraAccess(string message)
        {
            await DisplayAlert("Permision", "We need access to your current location. Set your location on for this app on settings", "Ok");
        }

        // This function displays an alert with the given message from an exception
        public async void DisplayException(string message)
        {
            await DisplayAlert("Alert", message, "OK");
        }
        // =====================================================================

        // =====================================================================
        // The following functions are for ANDROID ONLY

        // This function pre sets a text message for user to send to seller, customer, or both
        // The user will be able to add a picture of delivery
        public async void TextMessageAndroid()
        {
            action = await DisplayActionSheet("Send Text Message To", "Cancel", null, "Seller", "Customer", "Seller And Customer");
            try
            {
                if (action.Equals("Seller"))
                {
                    _ = SendSMSMessage("Hello " + GetName() + Environment.NewLine + "You package was just delivered", new[] {"4084760001", "4158329643"});
                }
                if (action.Equals("Customer"))
                {
                    _ = SendSMSMessage("Hello " + deliveryData.result[0].firstNameAndFirstLetterLastName + Environment.NewLine + "You package was just delivered", new[] { GetPhone() });
                }
                if (action.Equals("Seller And Customer"))
                {
                    _ = SendSMSMessage("Hello " + deliveryData.result[0].firstNameAndFirstLetterLastName + Environment.NewLine + "You package was just delivered", new[] { GetPhone(), "4084760001", "4158329643" });
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Alert!", ex.Message, "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!", ex.Message, "Ok");
            }
        }

        // This function pre sets a email message with attachment for user to send to seller, customer, or both
        public async void EmailMessageAndroid(byte[] attachment)
        {
            action = await DisplayActionSheet("Send Email Confirmation To", "Cancel", null, "Seller", "Customer", "Seller And Customer");

            try
            {
                List<string> recipients = new List<string>();
                if (action.Equals("Seller"))
                {
                    recipients.Add("pmarathay@gmail.com");
                    recipients.Add("omarfacio2010@gmail.com");

                    _ = SendEmailMessageAttachmentIncluded(recipients, "Email Confirmation By Just Delivered", "Hello Prashant and Carlos," + Environment.NewLine + "You package was just delivered", attachment);
                }

                if (action.Equals("Customer"))
                {
                    recipients.Add(GetEmail());

                    _ = SendEmailMessageAttachmentIncluded(recipients, "Email Confirmation By Just Delivered", "Hello " + GetName() + Environment.NewLine + "You package was just delivered", attachment);
                }

                if (action.Equals("Seller And Customer"))
                {
                    recipients.Add(GetEmail());
                    recipients.Add("pmarathay@gmail.com");
                    recipients.Add("omarfacio2010@gmail.com");

                    _ = SendEmailMessageAttachmentIncluded(recipients, "Email Confirmation By Just Delivered", "Hello " + GetName() + " Prashant and Carlos, " + Environment.NewLine + "You package was just delivered", attachment);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!",ex.Message,"OK");
            }
        }

        // This function builds a email with attachment
        public async Task SendEmailMessageAttachmentIncluded(List<string> r, string s, string m, byte[] attachment)
        {
            var message = new EmailMessage();
            var fileName = "photo.png";
            var file = Path.Combine(FileSystem.CacheDirectory, fileName);

            File.WriteAllBytes(file, attachment);
            ExperimentalFeatures.Enable("EmailAttachments_Experimental");

            message.To = r;
            message.Subject = s;
            message.Body = m;
            message.Attachments.Add(new Xamarin.Essentials.EmailAttachment(file));

            await Email.ComposeAsync(message);
        }

        // This function builds a sms message with attachment
        public async Task SendSMSMessage(string message, IEnumerable<string> recipients)
        {
            await Sms.ComposeAsync(new SmsMessage(message, recipients));
        }
        // =====================================================================

        // =====================================================================
        // The following functions control the bottom sheet motion
        void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            // Handle the pan
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't y + e.TotalY pan beyond the wrapped user interface element bounds.
                    var translateY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs((Height * .25) - Height));
                    bottomSheet.TranslateTo(bottomSheet.X, translateY, 20);
                    break;
                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    y = bottomSheet.TranslationY;

                    //at the end of the event - snap to the closest location
                    var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetClosestLockState(e.TotalY + y)));

                    //depending on Swipe Up or Down - change the snapping animation
                    if (IsSwipeUp(e))
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringIn);
                    }
                    else
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    }

                    //dismiss the keyboard after a transition

                    y = bottomSheet.TranslationY;

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
            //Play with these values to adjust the locking motions - this will change depending on the amount of content ona  apge
            var lockStates = new double[] { 0, .5, .85 };

            //get the current proportion of the sheet in relation to the screen
            var distance = Math.Abs(TranslationY);
            var currentProportion = distance / Height;

            //calculate which lockstate it's the closest to
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
        // =====================================================================
    }
}
