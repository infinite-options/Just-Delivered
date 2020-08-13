using System;
using System.Collections.Generic;
using Plugin.Geolocator;
using Xamarin.Essentials;
using DeliveryApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Internals.GIFBitmap;
using Newtonsoft.Json;
using ZXing.Net.Mobile.Forms;
using ZXing.Net.Mobile;
using Leadtools;
using Leadtools.Camera.Xamarin;
using Leadtools.Barcode;
using System.Threading.Tasks;

namespace DeliveryApp
{
    public partial class FourthPage : ContentPage
    {
        ServingNowList deliveryData = new ServingNowList();
        static ServingNowList deliveryDataCopy = new ServingNowList();

        List<CheckBox> checkboxList = new List<CheckBox>();

        int num = 0;

        static int totalNumDeliveries = 0;
        static bool state = false;

        double middleY = 0;
        double y = 0;

        // Note that some icons come from icons8.com, we give credit for their work.
        public FourthPage(ServingNowList data, ServingNowList copy, int deliveryNum, int totalDeliveries)
        {
            InitializeComponent();
            SetCurrentLocationOnMap();

            deliveryData = data;
            num = deliveryNum;

            if (!state)
            {
                totalNumDeliveries = totalDeliveries;
                deliveryDataCopy = copy;
                state = true;
            }
            
            map.HeightRequest = Application.Current.MainPage.Height;
            map.WidthRequest = Application.Current.MainPage.Width;

            double x = Application.Current.MainPage.Width;
            double middleX = x / 2;
            double getHelpButtonWidth = helpButton.Width;
            double percent = 0.9;

            middleY = Application.Current.MainPage.Height / 2;
            deliveriesList.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);
            deliveriesList.WidthRequest = Application.Current.MainPage.Width;
            scrollFrame.HeightRequest = (Application.Current.MainPage.Height / 2) - (Application.Current.MainPage.Height - (Application.Current.MainPage.Height * percent));
            helpButton.Margin = new Thickness(x - 50, 60, 0, 0);

            // Creating relativelayout to wrap each delivery
            RelativeLayout box;
            for (int i = 0; i < deliveryDataCopy.result.Count; i++)
            {
                if(Device.RuntimePlatform == Device.iOS)
                {
                    box = new RelativeLayout();

                    Button deliveryNumLabel = new Button
                    {
                        Text = (i + 1).ToString(),
                        TextColor = Color.Black,
                        Margin = new Thickness(10, 0, 0, 0),
                        HeightRequest = 20,
                        WidthRequest = 20,
                        CornerRadius = 10,
                        BorderColor = Color.Black,
                        BorderWidth = 1,
                        BackgroundColor = Color.White,
                    };

                    Label name = new Label
                    {
                        Text = deliveryDataCopy.result[i].firstNameAndFirstLetterLastName,
                        TextColor = Color.Black,
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(35, 0, 0, 0),
                    };

                    Label address = new Label
                    {
                        Text = deliveryDataCopy.result[i].house_address,
                        TextColor = Color.Black,
                        FontSize = 10,
                        Margin = new Thickness(35, 20, 0, 0),
                    };

                    Label csz = new Label
                    {
                        Text = deliveryDataCopy.result[i].city + ", " + deliveryDataCopy.result[i].state + ", " + deliveryDataCopy.result[i].zipcode,
                        TextColor = Color.Black,
                        FontSize = 10,
                        Margin = new Thickness(35, 30, 0, 0),
                    };

                    CheckBox checkbox = new CheckBox
                    {
                        Color = Color.Black,
                        IsChecked = false,
                        IsEnabled = false,
                        Margin = new Thickness(middleX - 15, 0, 0, 0),
                        Scale = 1
                    };

                    ImageButton call = new ImageButton
                    {
                        Source = "callIcon.png",
                        Scale = 0.6,
                        ClassId = i.ToString(),
                        HeightRequest = 45,
                        Margin = new Thickness(middleX + 15, 0, 0, 0),
                        BackgroundColor = Color.WhiteSmoke
                    };
                    call.Clicked += CallCustomerFromDeliveryList;

                    ImageButton text = new ImageButton
                    {
                        Source = "textMessageIcon.png",
                        Scale = 0.6,
                        ClassId = i.ToString(),
                        HeightRequest = 45,
                        Margin = new Thickness(middleX + 50, 0, 0, 0),
                        BackgroundColor = Color.WhiteSmoke
                    };
                    text.Clicked += TextCustomerFromDeliveryList;

                    Button directions = new Button
                    {
                        Text = "Get Directions",
                        TextColor = Color.Red,
                        FontSize = 10,
                        FontAttributes = FontAttributes.Bold,
                        CornerRadius = 15,
                        BackgroundColor = Color.WhiteSmoke,
                        ClassId = i.ToString(),
                        Margin = new Thickness(middleX + 100, 0, 0, 0)
                    };
                    directions.Clicked += GetDirectionsFromDeliveryList;

                    box.Children.Add(deliveryNumLabel, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(name, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(address, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(checkbox, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(csz, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(call, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(text, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(directions, Constraint.Constant(0), Constraint.Constant(0));
                    listOfDeliveries.Children.Add(box);
                    checkboxList.Add(checkbox);
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    box = new RelativeLayout();

                    Button deliveryNumLabel = new Button
                    {
                        Text = (i + 1).ToString(),
                        TextColor = Color.Black,
                        Margin = new Thickness(15, 5, 0, 0),
                        WidthRequest = 30,
                        HeightRequest = 30,
                        CornerRadius = 15,
                        BorderColor = Color.Black,
                        BorderWidth = 1,
                        BackgroundColor = Color.White,
                        Padding = new Thickness(0, -1, 0, 0)
                    };

                    Label name = new Label
                    {
                        Text = deliveryDataCopy.result[i].firstNameAndFirstLetterLastName,
                        TextColor = Color.Black,
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(50, 0, 0, 0),
                    };

                    Label address = new Label
                    {
                        Text = deliveryDataCopy.result[i].house_address,
                        TextColor = Color.Black,
                        FontSize = 10,
                        Margin = new Thickness(50, 20, 0, 0),
                    };

                    Label csz = new Label
                    {
                        Text = deliveryDataCopy.result[i].city + ", " + deliveryDataCopy.result[i].state + ", " + deliveryDataCopy.result[i].zipcode,
                        TextColor = Color.Black,
                        FontSize = 10,
                        Margin = new Thickness(50, 30, 0, 0),
                    };

                    CheckBox checkbox = new CheckBox
                    {
                        Color = Color.Black,
                        IsChecked = false,
                        IsEnabled = false,
                        Margin = new Thickness(middleX - 15, 5, 0, 0),
                        Scale = 1
                    };

                    ImageButton call = new ImageButton
                    {
                        Source = "callIcon.png",
                        Scale = 0.6,
                        ClassId = i.ToString(),
                        HeightRequest = 45,
                        Margin = new Thickness(middleX + 5, 0, 0, 0),
                        BackgroundColor = Color.WhiteSmoke
                    };
                    call.Clicked += CallCustomerFromDeliveryList;

                    ImageButton text = new ImageButton
                    {
                        Source = "textMessageIcon.png",
                        Scale = 0.6,
                        ClassId = i.ToString(),
                        HeightRequest = 45,
                        Margin = new Thickness(middleX + 40, 0, 0, 0),
                        BackgroundColor = Color.WhiteSmoke
                    };
                    text.Clicked += TextCustomerFromDeliveryList;

                    Button directions = new Button
                    {
                        Text = "Get Directions",
                        TextColor = Color.Red,
                        HeightRequest = 35,
                        FontSize = 10,
                        FontAttributes = FontAttributes.Bold,
                        CornerRadius = 15,
                        BackgroundColor = Color.WhiteSmoke,
                        ClassId = i.ToString(),
                        Margin = new Thickness(middleX + 80, 5, 0, 0),
                        Padding = new Thickness(0, -3, 0, 0),
                    };
                    directions.Clicked += GetDirectionsFromDeliveryList;

                    box.Children.Add(deliveryNumLabel, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(name, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(address, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(checkbox, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(csz, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(call, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(text, Constraint.Constant(0), Constraint.Constant(0));
                    box.Children.Add(directions, Constraint.Constant(0), Constraint.Constant(0));
                    listOfDeliveries.Children.Add(box);
                    checkboxList.Add(checkbox);
                }  
            }

            if (num <= totalNumDeliveries)
            {
                deliveryNumber.Text = num.ToString();

                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                var size = titleSwipeFrame.FontSize;
                double length = 0;

                if (Device.RuntimePlatform == Device.Android)
                {
                    totalDeliveriesNumber.Margin = new Thickness(x - 65, -10, 0, 0);
                    totalDeliveiesLabel.Margin = new Thickness(x - 60, 35, 0, 0);
                }

                if(Device.RuntimePlatform == Device.iOS)
                {
                    totalDeliveriesNumber.Margin = new Thickness(x - 50, -5, 0, 0);
                    totalDeliveiesLabel.Margin = new Thickness(x - 70, 35, 0, 0);
                }
                
                totalDeliveriesNumber.Text = totalNumDeliveries.ToString();
                customerSectionStack.Margin = new Thickness(30, 10, 0, 0);

                

                if (Device.RuntimePlatform == Device.iOS)
                {
                    length = titleSwipeFrame.Text.Length - 2;
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    length = titleSwipeFrame.Text.Length - 5;
                }
                
                var density = mainDisplayInfo.Density;
                var r = ((size * length) / 2) / density;
                var position = middleX - r;
 
                titleSwipeFrame.Margin = new Thickness(position, 0, 0, 0);

                double xPosition = 0;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    xPosition = middleX - (265 / 2);
                }

                if(Device.RuntimePlatform == Device.Android)
                {
                    xPosition = middleX - (215 / 2);
                }

                titleSwipeFrameUnderline.Margin = new Thickness(xPosition, 25, 0, 0);
                customerNameLabel.Text = deliveryData.result[0].firstNameAndFirstLetterLastName;

                addressLabel.Text = deliveryData.result[0].house_address;
                cityStateZipcodeLabel.Text = deliveryData.result[0].city + ", " + deliveryData.result[0].state + ", " + deliveryData.result[0].zipcode;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    getDirectionsButton.CornerRadius = 5;
                    skipDeliveryButton.CornerRadius = 5;
                    confirmDelivery.CornerRadius = 5;
                }

                deliveryInstructionsMessage.Text = "There are no delivery instructions for this delivery.";

                Polyline polyline = new Polyline
                {
                    StrokeColor = Color.Blue,
                    StrokeWidth = 3
                };

                for (int i = 0; i < deliveryData.result.Count; i++)
                {
                    var p = new Position(deliveryData.result[i].latitude, deliveryData.result[i].longitude);
                    Pin pin = new Pin
                    {
                        Label = deliveryData.result[i].firstNameAndFirstLetterLastName +" [Delivery " + (i + 1) + "]",
                        Address = deliveryData.result[i].house_address+" "+ deliveryData.result[i].city + ", " + deliveryData.result[i].state + ", " + deliveryData.result[i].zipcode,
                        Type = PinType.Place,
                        Position = new Position(deliveryData.result[i].latitude, deliveryData.result[i].longitude)
                    };
                    
                    map.Pins.Add(pin);
                    polyline.Geopath.Add(p);
                }
                map.MapElements.Add(polyline);
            }
            else
            {
                deliveryNumber.IsVisible = false;
                deliveryNumberLabel.IsVisible = false;

                totalDeliveriesNumber.IsVisible = false;
                totalDeliveiesLabel.IsVisible = false;

                titleSwipeFrame.Margin = new Thickness(middleX - 100, 25, 0, 0);
                titleSwipeFrame.IsVisible = false;
                titleSwipeFrameUnderline.IsVisible = false;

                customerSectionTitle.Text = "Thank you for driving for Just Delivered! There are no deliveries at this moment.";
                customerSectionTitle.Margin = new Thickness(30, 0, 30, 0);
                customerSectionStack.HorizontalOptions = LayoutOptions.CenterAndExpand;
                customerNameLabel.IsVisible = false;

                callButton.IsVisible = false;
                callButton.IsEnabled = false;

                textButton.IsVisible = false;
                textButton.IsEnabled = false;

                dividerLine1.IsVisible = false;

                addressSectionTitle.IsVisible = false;
                addressLabel.IsVisible = false;

                getDirectionsButton.IsVisible = false;
                getDirectionsButton.IsEnabled = false;

                confirmDelivery.IsVisible = false;
                confirmDelivery.IsEnabled = false;

                skipDeliveryButton.IsVisible = false;
                skipDeliveryButton.IsEnabled = false;

                dividerLine2.IsVisible = false;

                deliveryInstructionsSectionTitle.IsVisible = false;
                deliveryInstructionsMessage.IsVisible = false;

                dividerLine3.IsVisible = false;

                itemsSectionTitle.IsVisible = false;
                dividerLine4.IsVisible = false;
            }
        }

        // =====================================================================
        // The following functions correspond to the list of deliveries shown
        // after clicking on the total number of deliveries

        // This function gets directions for delivery
        public async void GetDirectionsFromDeliveryList(object sender, EventArgs e)
        {
            var location = new Location(deliveryDataCopy.result[GetIndexDirections(sender)].latitude, deliveryDataCopy.result[GetIndexDirections(sender)].longitude);
            var options = new MapLaunchOptions { Name = deliveryDataCopy.result[GetIndexDirections(sender)].house_address, NavigationMode = NavigationMode.Driving };

            await Xamarin.Essentials.Map.OpenAsync(location, options);

            checkboxList[GetIndexDirections(sender)].IsChecked = true;

            SixthPage p = new SixthPage(deliveryData, num, deliveryDataCopy, GetIndexDirections(sender));
            await Application.Current.MainPage.Navigation.PushAsync(p);
        }

        // This function allows the user to send a text message to customer
        public async void TextCustomerFromDeliveryList(object sender, EventArgs e)
        {
            try
            {
                var message = new SmsMessage(null, new[] { deliveryDataCopy.result[GetIndex(sender)].phone });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Alert!", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!", ex.Message, "OK");
            }
        }

        // This function allows the user to call the customer
        public async void CallCustomerFromDeliveryList(object sender, EventArgs e)
        {
            string num = deliveryDataCopy.result[GetIndex(sender)].parsedPhone;
            if (num.Length == 10)
            {
                PhoneDialer.Open(num);
            }
            else
            {
                await DisplayAlert("Alert", "Phone number not available", "OK");
            }
        }

        // This function gets the index of the call or text icon to determine customer's info
        public int GetIndex(object sender)
        {
            var b = (ImageButton)sender;
            var index = b.ClassId.ToString();
            return int.Parse(index);
        }

        // This function gets the index of the direction button to determine customer's geocoordinates
        public int GetIndexDirections(object sender)
        {
            var b = (Button)sender;
            var index = b.ClassId.ToString();
            return int.Parse(index);
        }

        // This function shows the list of deliveries
        public void ShowTotalDeliveries(System.Object sender, System.EventArgs e)
        {
            deliveriesList.Margin = new Thickness(0, middleY, 0, 0);
        }

        // This function hides the list of deliveries
        public void HideTotalDeliveries(System.Object sender, System.EventArgs e)
        {
            deliveriesList.Margin = new Thickness(0, middleY * 2, 0, 0);
        }
        // =====================================================================

        // =====================================================================
        // This function places your current location on the background map
        public async void SetCurrentLocationOnMap()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);

                Geocoder geoCoder = new Geocoder();

                Position current = new Position(position.Latitude, position.Longitude);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(current);

                string address = possibleAddresses.FirstOrDefault();

                Pin pin = new Pin
                {
                    Label = "Current Location",
                    Position = new Position(position.Latitude, position.Longitude),
                };

                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(10)));
            }
            catch(Exception ex)
            {
                await DisplayAlert("Permision", ex.Message, "OK");
            }
        }

        // This function gets the delivery geo coordinates and feeds them onto the native maps function
        public async void GetDirections(System.Object sender, System.EventArgs e)
        {
            var location = new Location(deliveryData.result[0].latitude, deliveryData.result[0].longitude);
            var options = new MapLaunchOptions { Name = deliveryData.result[0].house_address, NavigationMode = NavigationMode.Driving };

            await Xamarin.Essentials.Map.OpenAsync(location, options);

            SixthPage p = new SixthPage(deliveryData, num, deliveryDataCopy, -1);
            await Application.Current.MainPage.Navigation.PushAsync(p);
        }

        // This function gets skips the current delivery and sets the next delivery
        public async void SkipDelivery(System.Object sender, System.EventArgs e)
        {
            string action = await DisplayActionSheet("Are you sure you want to skip this delivery?", "Cancel", null, "Yes","No");

            if (action.Equals("Yes"))
            {
                if (deliveryData.result.Count != 0)
                {
                    try
                    {
                        await DisplayAlert("Alert!", "Please let " + deliveryData.result[0].firstNameAndFirstLetterLastName + " why you can't complete this delivery", "OK");
                        await Sms.ComposeAsync(new SmsMessage(null, new[] { deliveryData.result[0].phone, "4084760001", "4158329643" }));
                        UpdateDeliveryList();
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        await DisplayAlert("Alert!",ex.Message,"OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Alert!", ex.Message, "OK");
                    }
                }
            }
        }

        // This function calls the current delivery customer
        public async void CallCustomer(System.Object sender, System.EventArgs e)
        {
            string num = deliveryData.result[0].parsedPhone;
            if (num.Length == 10)
            {
                PhoneDialer.Open(num);
            }
            else
            {
                await DisplayAlert("Alert", "Phone number not available", "Ok");
            }
        }

        // This functions texts the current delivery customer
        public async void TextCustomer(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Sms.ComposeAsync(new SmsMessage(null, new[] { deliveryData.result[0].phone }));
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Alert!", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!", ex.Message, "OK");
            }
        }

        // This function updates the delivery data list when user skips current delivery
        public async void UpdateDeliveryList()
        {
            if(deliveryData.result.Count != 0)
            {
                if (deliveryData.result.Count != 1)
                {
                    for (int i = 1; i < deliveryData.result.Count; i++)
                    {
                        deliveryData.result[i - 1] = deliveryData.result[i];
                    }
                }
                deliveryData.result.RemoveAt(deliveryData.result.Count - 1);

                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new FourthPage(this.deliveryData, deliveryDataCopy, num + 1, totalNumDeliveries));
            }
        }

        // This function skips the get directions of current delivery and sends the user to the confirmation page
        public async void SkipGetDirections(System.Object sender, System.EventArgs e)
        {
            SixthPage p = new SixthPage(deliveryData, num, deliveryDataCopy, -1);
            await Application.Current.MainPage.Navigation.PushAsync(p);
        }

        // This fuction contacts customer service
        public async void ContactCustomerService(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("Customer Service", "We are connecting you to customer service.", "Ok");
            PhoneDialer.Open("4084760001");
        }
        // =====================================================================

        // =====================================================================
        // The following functions allows the bottom sheet to move up or down
        public void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
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

                    // At the end of the event - snap to the closest location
                    var finalTranslation = Math.Max(Math.Min(0, -1000), -Math.Abs(GetClosestLockState(e.TotalY + y)));

                    // Depending on Swipe Up or Down - change the snapping animation
                    if (IsSwipeUp(e))
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringIn);
                    }
                    else
                    {
                        bottomSheet.TranslateTo(bottomSheet.X, finalTranslation, 250, Easing.SpringOut);
                    }

                    // Dismiss the keyboard after a transition
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
        // =====================================================================

    }
}
