using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using JustDelivered.Config;
using JustDelivered.Controls;
using JustDelivered.Interfaces;
using JustDelivered.LogIn.Apple;
using JustDelivered.LogIn.Classes;
using JustDelivered.Models;
using Newtonsoft.Json;
using Plugin.LatestVersion;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace JustDelivered.Views
{
    public partial class DeliveriesPage : ContentPage
    {
        public static Models.User user = null;
        double y = 0;


        public static string routeID = "";
        public static int CurrentIndex = 0;
        public static readonly DeliveryInfo startLocation = new DeliveryInfo();
        public static List<string> list = new List<string>();
        // list of deliveries
        public static ObservableCollection<DeliveryInfo> deliveryList = new ObservableCollection<DeliveryInfo>();
        public static DeliveryInfo delivery = null;
        // list of items of first delivery in the queue
        ObservableCollection<DisplayItem> OrderList = new ObservableCollection<DisplayItem>();

        public class DisplayItem : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            public string img { get; set; }
            public string title { get; set; }
            public string itemName { get; set; }
            public string quantity { get; set; }
            public double opacityValue { get; set; }
            public int index { get; set; }
            public string itemUID { get; set; }

            public void updateOpacity(double value)
            {
                opacityValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs("opacityValue"));
            }
            public Color color { get; set; }
        }

        public class DeliveryItem
        {
            public string img { get; set; }
            public int qty { get; set; }
            public string name { get; set; }
            public string unit { get; set; }
            public double price { get; set; }
            public string item_uid { get; set; }
            public string description { get; set; }
            public double business_price { get; set; }
            public string itm_business_uid { get; set; }
        }

        public class Route
        {
            public string uid { get; set; }
            public string delivery_date { get; set; }
        }

        public class Order
        {
            public IList<DeliveryItem> items { get; set; }
        }

        public class Delivery
        {
            public string driver_uid { get; set; }
            public string driver_first_name { get; set; }
            public string driver_last_name { get; set; }
            public string driver_email { get; set; }
            public string route_id { get; set; }
            public string route_option { get; set; }
            public string route_business_id { get; set; }
            public int num_deliveries { get; set; }
            public double route_distance { get; set; }
            public string route_time { get; set; }
            public string shipment_date { get; set; }
            public string delivery_first_name { get; set; }
            public string delivery_last_name { get; set; }
            public string delivery_email { get; set; }
            public string delivery_phone { get; set; }
            public string delivery_coordinates { get; set; }
            public string delivery_street { get; set; }
            public string delivery_unit { get; set; }
            public string delivery_city { get; set; }
            public string delivery_state { get; set; }
            public string delivery_zip { get; set; }
            public string delivery_instructions { get; set; }
            public string delivery_status { get; set; }
            public string purchase_uid { get; set; }
            public string customer_uid { get; set; }
            public string delivery_items { get; set; }
            public string start_delivery_date { get; set; }
        }

        public class DriverInfo
        {
            public string driver_uid { get; set; }
            public string driver_first_name { get; set; }
            public string driver_last_name { get; set; }
            public object business_id { get; set; }
            public object driver_available_hours { get; set; }
            public object driver_scheduled_hours { get; set; }
            public object driver_street { get; set; }
            public object driver_city { get; set; }
            public object driver_state { get; set; }
            public object driver_zip { get; set; }
            public object driver_latitude { get; set; }
            public object driver_longitude { get; set; }
            public object driver_phone_num { get; set; }
            public string driver_email { get; set; }
            public object driver_phone_num2 { get; set; }
            public object driver_ssn { get; set; }
            public object driver_license { get; set; }
            public object driver_license_exp { get; set; }
            public object driver_insurance_carrier { get; set; }
            public object driver_insurance_num { get; set; }
            public object driver_insurance_exp_date { get; set; }
            public object driver_insurance_picture { get; set; }
            public object emergency_contact_name { get; set; }
            public object emergency_contact_phone { get; set; }
            public object emergency_contact_relationship { get; set; }
        }

        public class RDSAuthentication
        {
            public string message { get; set; }
            public int code { get; set; }
            public IList<DriverInfo> result { get; set; }
            public string sql { get; set; }
        }

        public class Deliveries
        {
            public string message { get; set; }
            public int code { get; set; }
            public IList<Delivery> result { get; set; }
            public string sql { get; set; }
        }

        public class DeliveryInfo : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };

            public string route_id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }

            public string name { get; set; }
            public string address { get; set; }
            public string unit { get; set; }
            public string house_address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string status { get; set; }
            public string skipAndUnskip { get; set; }
            public string delivery_instructions { get; set; }
            public int ID { get; set; }
            public int placement { get; set; }
            public string delivery_items { get; set; }
            public string purchase_uid { get; set; }
            public string customer_uid { get; set; }
            public string delivery_date { get; set; }

            public string updateStatus
            {
                set
                {
                    status = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("status"));
                }
            }

            public int updateID
            {
                set
                {
                    ID = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ID"));
                }
            }

            public string updateSkipText
            {
                set
                {
                    skipAndUnskip = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("skipAndUnskip"));
                }
            }

            public string parsedPhone
            {
                get
                {
                    foreach (char digit in phone.ToCharArray())
                    {
                        if (!char.IsDigit(digit)) { return "Phone # Not Available"; }
                    }
                    if (phone.Length == 10)
                    {
                        return phone;
                    }
                    if (phone.Length == 11)
                    {
                        var p = phone.Substring(1);
                        return p;
                    }
                    return "Phone # Not Available";
                }
            }

            public string firstNameAndFirstLetterLastName
            {
                get
                {
                    string formattedName = "";
                    string n = name;
                    int i = 0;
                    while ((int)n[i] != (int)' ')
                    {
                        formattedName += n[i];
                        i++;
                    }
                    int j = i + 1;
                    if (j < n.Length)
                    {
                        formattedName += ". ";
                        formattedName += n[j];
                    }
                    return formattedName;
                }
            }
        }

        public class Coordinates
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public class Item
        {
            public string email { get; set; }
            public string phone { get; set; }
            public string customer { get; set; }
            public Coordinates coordinates { get; set; }
            public string delivery_street { get; set; }
            public string delivery_instructions { get; set; }
        }

        public DeliveriesPage()
        {
            InitializeComponent();
            SetDefaultLocationOnMap();
            VerifyUserAccount();
            //CheckVersion();
        }

        public DeliveriesPage(string back)
        {
            InitializeComponent();
            ResetMap();
            SetDefaultLocationOnMap();
            deliveryListView.ItemsSource = deliveryList;
            FindNextDeliveryAvailable(deliveryList);

            SetDelivery();
            SetHeightWidthOnMap();
            SetWidthOnHelpButtonRow();
            SetStartToFirstLocation(Color.Black);
            SetCompleteRouteView("versionB");

        }

        public async Task CheckVersion()
        {
            try
            {
                var client = new AppVersion();
                string versionStr = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
                var result = await client.isRunningLatestVersion(versionStr);
                Debug.WriteLine("isRunningLatestVersion: " + result);

                if (result == "FALSE")
                {
                    await DisplayAlert("Just Delivered\nhas gotten even better!", "Please visit the App Store to get the latest version.", "OK");
                    await CrossLatestVersion.Current.OpenAppInStore();
                }
            }
            catch (Exception issueVersionChecking)
            {
                string str = issueVersionChecking.Message;
            }
        }

        async void FindNextDeliveryAvailable(ObservableCollection<DeliveryInfo> deliveryList)
        {
            bool found = false;

            //if (CurrentIndex < deliveryList.Count)
            //{

            //    Debug.WriteLine("Index: " + CurrentIndex);
            //    Debug.WriteLine("deliveryList[CurrentIndex].status: " + deliveryList[CurrentIndex].status);
            //    Debug.WriteLine("(deliveryList[CurrentIndex].purchase_uid: " + (deliveryList[CurrentIndex].purchase_uid));
            //    AddPurchaseIdToArray(deliveryList[CurrentIndex].purchase_uid);
            //}
            list.Clear();
            for (int i = 0; i <  deliveryList.Count; i++)
            {
                if(deliveryList[i].status == "Status: Pending...")
                {
                    SetStartToFirstLocation(Color.Black);
                    CurrentIndex = i;
                    found = true;
                    AddPurchaseIdToArray(deliveryList[CurrentIndex].purchase_uid);
                    break;
                }
            }

            if (!found)
            {
                await DisplayAlert("Great job!","It looks like you completed all your deliveries","OK");
            }
        }

        public async void CompletedAllDeliveries()
        {
            await DisplayAlert("Congratulations!", "You have completed all your deliveries!!!\n\nThank you for driving for Just Delivered.", "OK");
        }

        public async void VerifyUserAccount()
        {
            try
            {
                
                var client = new HttpClient();
                var routeClient = new Route();
                var currentDate = DateTime.Now;

                for (int i = 0; i < 7; i++)
                {
                    if (currentDate.DayOfWeek == DayOfWeek.Wednesday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        break;
                    }
                    currentDate = currentDate.AddDays(1);
                }

                //socialLogInPost.email = user.email;
                //socialLogInPost.social_id = user.socialId;

                Debug.WriteLine("Current Date: " + currentDate.ToString("yyyy-MM-dd 10:00:00"));

                //socialLogInPost.password = "";
                //socialLogInPost.delivery_date = "2021-06-06 10:00:00";
                //socialLogInPost.signup_platform = user.platform;


                //LIFE
                routeClient.uid = user.id;
                routeClient.delivery_date = currentDate.ToString("yyyy-MM-dd 10:00:00");

                //TEST
                //routeClient.uid = user.id;
                //routeClient.delivery_date = currentDate.ToString("2021-07-18 10:00:00");

                var socialLogInPostSerialized = JsonConvert.SerializeObject(routeClient);

                Debug.WriteLine("JSON: " + socialLogInPostSerialized);

                var postContent = new StringContent(socialLogInPostSerialized, Encoding.UTF8, "application/json");

                Debug.WriteLine(socialLogInPostSerialized);
                
                var RDSResponse = await client.PostAsync(Constant.DriverRouteUrl, postContent);
                var responseContent = await RDSResponse.Content.ReadAsStringAsync();

                Debug.WriteLine(responseContent);

                Debug.WriteLine(RDSResponse.IsSuccessStatusCode);

                if (RDSResponse.IsSuccessStatusCode)
                {
                    if (responseContent != null)
                    {
                        //var message = JsonConvert.DeserializeObject<RDSAuthentication>(responseContent);
                    

                        var List = JsonConvert.DeserializeObject<Deliveries>(responseContent);
                        deliveryList.Clear();
                        int id = 0;
                        foreach (Delivery a in List.result)
                        {
                            var element = new DeliveryInfo();
                            element.name = a.delivery_first_name + " " + a.delivery_last_name;
                            element.address = a.delivery_street;
                            element.unit = a.delivery_unit == null ? "" : a.delivery_unit;
                            element.house_address = a.delivery_street + " " + a.delivery_city + " " + a.delivery_state + " " + a.delivery_zip;
                            element.delivery_date = a.start_delivery_date;
                            element.email = a.delivery_email;
                            element.phone = a.delivery_phone;

                            element.firstName = a.delivery_first_name;
                            element.lastName = a.delivery_last_name;
                            element.city = a.delivery_city;
                            element.state = a.delivery_state;
                            element.zipcode = a.delivery_zip;

                            var Coordinates = JsonConvert.DeserializeObject<Coordinates>(a.delivery_coordinates);

                            element.latitude = Coordinates.latitude;
                            element.longitude = Coordinates.longitude;
                            if(a.delivery_status == "TRUE")
                            {
                                element.status = "Status: Delivered";
                                element.skipAndUnskip = "Skip";
                            }
                            else if (a.delivery_status == "FALSE")
                            {
                                element.status = "Status: Pending...";
                                element.skipAndUnskip = "Skip";
                            }
                            else if (a.delivery_status == "SKIP")
                            {
                                element.status = "Status: Skipped";
                                element.skipAndUnskip = "Unskip";
                            }

                            element.ID = id;
                            element.placement = id;
                            if (a.delivery_instructions != null && a.delivery_instructions != "")
                            {
                                element.delivery_instructions = a.delivery_instructions;
                            }
                            else
                            {
                                element.delivery_instructions = "No delivery instructions for this order";
                            }

                            element.delivery_items = a.delivery_items;
                            element.customer_uid = a.customer_uid;
                            element.purchase_uid = a.purchase_uid;

                            deliveryList.Add(element);
                            id++;
                            user.route_id = a.route_id;
                        }
                        if (deliveryList.Count != 0)
                        {
                            deliveryListView.ItemsSource = deliveryList;
                            SaveStartingPoint(deliveryList[0]);

                            if (IsOneDeliveryCompleted(deliveryList))
                            {
                                SetStartToFirstLocation(Color.Black);
                            }
                            else
                            {
                                SetStartToFirstLocation(Color.Red);
                            }

                            FindNextDeliveryAvailable(deliveryList);
                            SetDelivery();
                            SetHeightWidthOnMap();
                            SetWidthOnHelpButtonRow();
                            SetCompleteRouteView();
                        }
                        else
                        {
                            SetHeightWidthOnMap();
                            SetWidthOnHelpButtonRow();
                            SetCompleteRouteView();
                            await DisplayAlert("Oops", "Our system shows that there are no deliveries available for you at this moment. Please check again later.", "OK");
                            BottomSheet.IsEnabled = false;
                        }
                    }
                }
                
            }
            catch (Exception g)
            {
                Debug.WriteLine("IN side " + g.Message);
                //MyStack.IsEnabled = false;
                await DisplayAlert("Oops", "Our system shows that there are no deliveries available for you at this moment. Please check again later.", "OK");
                BottomSheet.IsEnabled = false;
            }
        }

        bool IsOneDeliveryCompleted(ObservableCollection<DeliveryInfo> deliveryList)
        {
            bool result = false;
            foreach(DeliveryInfo delivery in deliveryList)
            {
                if(delivery.status == "Status: Delivered" || delivery.status == "Status: Skipped")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        void SaveStartingPoint(DeliveryInfo location)
        {
            routeID = user.route_id;
            startLocation.email = location.email;
            startLocation.email = location.delivery_items;
            startLocation.phone = location.phone;
            startLocation.customer_uid = location.customer_uid;
            startLocation.zipcode = location.zipcode;
            startLocation.purchase_uid = location.purchase_uid;
            startLocation.city = location.city;
            startLocation.unit = location.unit;
            startLocation.state = location.state;
            startLocation.status = "";
            startLocation.address = location.address;
            startLocation.lastName = location.lastName;
            startLocation.firstName = location.firstName;
            startLocation.delivery_date = location.delivery_date;
            startLocation.delivery_instructions = location.delivery_instructions;
            startLocation.latitude = location.latitude;
            startLocation.longitude = location.longitude;

            deliveryList.Remove(location);
            UpdateDeliveryIDs(deliveryList);
        }

        void UpdateDeliveryIDs(ObservableCollection<DeliveryInfo> deliveryList)
        {
            int index = 0;
            foreach(DeliveryInfo a in deliveryList)
            {
                a.placement = index;
                a.ID = index + 1;
                a.updateID = index + 1; 
                index++;
            }
        }


        public void SetDelivery()
        {
            CustomerName.Text = deliveryList[CurrentIndex].name;
            CustomerAddress.Text = deliveryList[CurrentIndex].house_address;
            DeliveryInstructions.Text = deliveryList[CurrentIndex].delivery_instructions;
            CurrentDeliveryNumber.Text = deliveryList[CurrentIndex].ID.ToString();
            TotalDeliveriesNumber.Text = (deliveryList.Count).ToString();

            var items = JsonConvert.DeserializeObject<ObservableCollection<DeliveryItem>>(deliveryList[CurrentIndex].delivery_items);

            var listOfItems = new ObservableCollection<DisplayItem>();
            foreach (DeliveryItem item in items)
            {
                var el = new DisplayItem();
                el.img = item.img;
                el.title = item.name + " (" + item.unit + ") ";
                el.quantity = item.qty.ToString();
                listOfItems.Add(el);
            }

            OrderItemsList.ItemsSource = listOfItems;
        }

        public void SetCompleteRouteView()
        {
            //DeliveriesMap.CustomPins = new List<CustomPin>();
            //DeliveriesMap.MapElements.Clear();
            //DeliveriesMap.CustomPins.Clear();

            var Path = new Polyline();
            Path.StrokeColor = Color.Black;
            Path.StrokeWidth = 4;

            for (int i = 0; i < deliveryList.Count; i++)
            {
                //deliveryList[i].ID
                
                var pin = new CustomPin();
                pin.Label = "Delivery " + (i + 1) + " For: " + deliveryList[i].firstNameAndFirstLetterLastName;
                pin.Address = deliveryList[i].house_address;
                pin.Type = PinType.Generic;
                pin.Position = new Position(deliveryList[i].latitude, deliveryList[i].longitude);
                pin.Name = (i + 1) + "";
                pin.Url = "";
                pin.Number = (i + 1) + "";

                if (i == CurrentIndex)
                {
                    if (deliveryList[i].status == "Status: Pending...")
                    {
                        if (isDeliveryListFullyPending(deliveryList))
                        {
                            pin.Color = "Black";
                        }
                        else
                        {
                            pin.Color = "Red";
                        }
                    }
                    else
                    {
                        pin.Color = SetPinColor(deliveryList[i].status);
                    }
                }
                else
                {

                    Debug.WriteLine("deliveryList[i].status: " + deliveryList[i].status);
                    pin.Color = SetPinColor(deliveryList[i].status);
                }

                Path.Geopath.Add(pin.Position);
                DeliveriesMap.CustomPins.Add(pin);
                DeliveriesMap.Pins.Add(pin);
            }
            DeliveriesMap.MapElements.Add(Path);
        }

        public bool isDeliveryListFullyPending(ObservableCollection<DeliveryInfo> deliveryList)
        {
            bool result = true;

            foreach(DeliveryInfo delivery in deliveryList)
            {
                if(delivery.status != "Status: Pending...")
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public void SetCompleteRouteView(string versionB)
        {
            

            var Path = new Polyline();
            Path.StrokeColor = Color.Black;
            Path.StrokeWidth = 4;

            for (int i = 0; i < deliveryList.Count; i++)
            {
                //deliveryList[i].ID

                var pin = new CustomPin();
                pin.Label = "Delivery " + (i + 1) + " For: " + deliveryList[i].firstNameAndFirstLetterLastName;
                pin.Address = deliveryList[i].house_address;
                pin.Type = PinType.Generic;
                pin.Position = new Position(deliveryList[i].latitude, deliveryList[i].longitude);
                pin.Name = (i + 1)+ "";
                pin.Url = "";
                pin.Number = (i + 1) + "";

                if (i == CurrentIndex)
                {
                    if(deliveryList[i].status == "Status: Pending...")
                    {
                        if (isDeliveryListFullyPending(deliveryList))
                        {
                            pin.Color = "Black";
                        }
                        else
                        {
                            pin.Color = "Red";
                        }
                    }
                    else
                    {
                        pin.Color = SetPinColor(deliveryList[i].status);
                    }
                }
                else
                {

                    Debug.WriteLine("deliveryList[i].status: " + deliveryList[i].status);
                    pin.Color = SetPinColor(deliveryList[i].status);
                }
                
                
                Path.Geopath.Add(pin.Position);
                DeliveriesMap.CustomPins.Add(pin);
                DeliveriesMap.Pins.Add(pin);
            }
            DeliveriesMap.MapElements.Add(Path);

            if (deliveryList.Count > 0)
            {
                AdjustMapCenter(deliveryList[0].latitude, deliveryList[0].longitude);
            }
        }


        string SetPinColor(string status)
        {
            string result = "";
            Debug.WriteLine("COLOR: " + status);
            if (status == "Status: Delivered")
            {
                result = "Green";
            }
            else if (status == "Status: Pending...")
            {
                result = "Black";
            }
            else if (status == "Status: Skipped")
            {
                result = "Gray";
            }
            Debug.WriteLine("RESULT: " + result);
            return result;
        }

        void SetHeightWidthOnMap()
        {
            DeliveriesMap.HeightRequest = Application.Current.MainPage.Height;
            DeliveriesMap.WidthRequest = Application.Current.MainPage.Width;
        }

        void SetDefaultLocationOnMap()
        {
            var Point = new Position(37.334789, -121.888138);
            var Span = new MapSpan(Point, 0.25, 0.25);
            DeliveriesMap.MoveToRegion(Span);
        }

        void AdjustMapCenter(double lat, double lon)
        {
            var Point = new Position(lat, lon);
            var Span = new MapSpan(Point, 0.10, 0.10);
            DeliveriesMap.MoveToRegion(Span);
        }

        void SetWidthOnHelpButtonRow()
        {
            HelpButtonRow.WidthRequest = Application.Current.MainPage.Width;
            menuRow.WidthRequest = Application.Current.MainPage.Width;
        }

        public async void SetStartToFirstLocation(Color pathColor)
        {
            if (deliveryList.Count >= 1)
            {

                var Start = new Polyline();
                Start.StrokeColor = pathColor;
                Start.StrokeWidth = 4;

                
                CustomPin pin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(startLocation.latitude, startLocation.longitude),
                    Label = "Start Location",
                    Name ="JD",
                    Url = "jd",
                    Address = startLocation.house_address,
                    Number = "S",
                    
                };

                if(pathColor == Color.Red)
                {
                    pin.Color = "Red";
                }
                else
                {
                    pin.Color = "Green";
                }

                DeliveriesMap.CustomPins.Add(pin);
                DeliveriesMap.Pins.Add(pin);
              
                var P1 = new Position(startLocation.latitude, startLocation.longitude);
                var P2 = new Position(deliveryList[0].latitude, deliveryList[0].longitude);

                Start.Geopath.Add(P1);
                Start.Geopath.Add(P2);

                DeliveriesMap.MapElements.Add(Start);

                var Span = new MapSpan(pin.Position, 0.1, 0.1);
                DeliveriesMap.MoveToRegion(Span);
            }
            else
            {
                await DisplayAlert("Oops", "Our records show there are no deliveries at the moment", "OK");
            }
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

        // problem here
        async void GetDirections(System.Object sender, System.EventArgs e)
        {
            try
            {
                Debug.WriteLine("GET DIRECTIONS");
                DeliveryInfo currentDelivery = null;
                if (fullDeliveryListView.IsVisible == false)
                {
                    currentDelivery = deliveryList[CurrentIndex];
                }
                else
                {

                    var caller = (ImageButton)sender;
                    var selectedItem = (DeliveryInfo)caller.CommandParameter;
                    currentDelivery = selectedItem;
                }

                var location = new Location(currentDelivery.latitude, currentDelivery.longitude);
                var options = new MapLaunchOptions { Name = currentDelivery.house_address, NavigationMode = NavigationMode.Driving };
                delivery = currentDelivery;
                await Xamarin.Essentials.Map.OpenAsync(location, options);
                Application.Current.MainPage = new VerificationPage();
            }
            catch
            {

            }
            
        }

        // problem here
        public static void GetDirectionsFromIOSProject(string index)
        {
            if(index != null && index != "")
            {
                var i = Int16.Parse(index) - 1;
                CurrentIndex = i;
                delivery = deliveryList[CurrentIndex];

                var location = new Location(delivery.latitude, delivery.longitude);
                var options = new MapLaunchOptions { Name = delivery.house_address, NavigationMode = NavigationMode.Driving };
                Xamarin.Essentials.Map.OpenAsync(location, options);
                Application.Current.MainPage = new VerificationPage();
            }
        }
                

        void SkipDirections(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("SKIP DIRECTIONS");
            delivery = deliveryList[CurrentIndex];
            Application.Current.MainPage = new VerificationPage();
        }

        async void SkipDelivery(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("SKIP DELIVERY");
            DeliveryInfo currentDelivery = null;
            if (fullDeliveryListView.IsVisible == false)
            {
                currentDelivery = deliveryList[CurrentIndex];
            }
            else
            {
                //var caller = (ImageButton)sender;
                //var selectedItem = (DeliveryInfo)caller.CommandParameter;
                var caller = (Frame)sender;
                var gesture = (TapGestureRecognizer)caller.GestureRecognizers[0];
                var selectedItem = (DeliveryInfo)gesture.CommandParameter;
                currentDelivery = selectedItem;
            }
            Debug.WriteLine("STATUS AT SKIP: " + currentDelivery.status);
            Debug.WriteLine("Index: " + currentDelivery.placement);
            if (currentDelivery.status == "Status: Pending...")
            {
                var Result = await DisplayAlert("Are you sure you want to skip this delivery?", "Press 'YES' to skip this delivery. Otherwise, press 'NO' to continue processing this delivery", "YES", "NO");
                if (Result)
                {
                    string ReasonToSkipDelivery = await DisplayPromptAsync("", "What's the reason you are not able to complete this delivery?");
                    if (ReasonToSkipDelivery != null && ReasonToSkipDelivery != "")
                    {
                        _ = UpdateDeliveryStatus(currentDelivery.purchase_uid, ReasonToSkipDelivery, "Skip");
                    }

                    //deliveryList.Remove(currentDelivery);
                    
                    currentDelivery.status = "Status: Skipped";
                    currentDelivery.updateStatus = "Status: Skipped";
                    currentDelivery.updateSkipText = "Unskip";
                    //deliveryList.Add(currentDelivery);
                   
                    deliveryList[currentDelivery.placement].status = "Status: Skipped";
                    //AddPurchaseIdToArray(currentDelivery.purchase_uid);
                    ResetMap();
                    FindNextDeliveryAvailable(deliveryList);
                    SetDelivery();
                    SetStartToFirstLocation(Color.Black);
                    SetCompleteRouteView();
                }
                return;
            }
            else if(currentDelivery.status == "Status: Skipped")
            {
                deliveryList[currentDelivery.placement].status = "Status: Pending...";
                currentDelivery.updateStatus = "Status: Pending...";
                currentDelivery.updateSkipText = "Skip";

                _ = UpdateDeliveryStatus(currentDelivery.purchase_uid, "", "Undo");
                //RemovePurchaseIdToArray(currentDelivery.purchase_uid);
                ResetMap();
                SetStartToFirstLocation(Color.Black);
                SetCompleteRouteView();
            }
        }

        void ResetMap()
        {
            DeliveriesMap.MapElements.Clear();
            DeliveriesMap.CustomPins.Clear();
            DeliveriesMap.Pins.Clear();
        }

        void AddPurchaseIdToArray(string id)
        {
            if (!list.Contains(id))
            {
                list.Add(id);
            }
        }

        void RemovePurchaseIdToArray(string id)
        {
            if (list.Contains(id))
            {
                list.Remove(id);
            }
        }

        async Task<bool> UpdateDeliveryStatus(string purchaseId, string note, string command)
        {
            try
            {
                var client = new HttpClient();
                var delivery = new UpdateDelivery();

                delivery.purchase_uid = purchaseId;
                delivery.cmd = command;
                delivery.note = note;

                var deliveryJSON = JsonConvert.SerializeObject(delivery);
                Debug.WriteLine("DELIVERY JSON: " + deliveryJSON);
                var content = new StringContent(deliveryJSON, Encoding.UTF8, "application/json");

                var RDSResponse = await client.PostAsync("https://0ig1dbpx3k.execute-api.us-west-1.amazonaws.com/dev/api/v2/UpdateDeliveryStatus", content);
                Debug.WriteLine("UPDATE DELIVERY STATUS ENDPOINT " + RDSResponse.IsSuccessStatusCode);

            }
            catch (Exception ErrorUpdatingStatus)
            {
                Debug.WriteLine("Exception: " + ErrorUpdatingStatus.Message);
            }
            return true;
        }

        async void CallCustomer(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("CALL CUSTOMER");

            DeliveryInfo currentDelivery = null;
            if (fullDeliveryListView.IsVisible == false)
            {
                currentDelivery = deliveryList[CurrentIndex];
            }
            else
            {
                var caller = (ImageButton)sender;
                var selectedItem = (DeliveryInfo)caller.CommandParameter;
                currentDelivery = selectedItem;
            }

            try
            {
                if (currentDelivery.parsedPhone != null && currentDelivery.parsedPhone.Length == 10)
                {
                    PhoneDialer.Open(currentDelivery.parsedPhone);
                }
                else
                {
                    await DisplayAlert("Oops", "Unfortunaly, we can't process your request at this moment. There is no available phone number for this customer", "OK");
                }
            }
            catch (Exception PhoneDataError)
            {
                Debug.WriteLine(PhoneDataError.Message);
                //await DisplayAlert("Oops", "There was an error gathering this customer's phone number","OK");
            }
        }

        async void TextCustomer(System.Object sender, System.EventArgs e)
        {
            DeliveryInfo currentDelivery = null;
            if (fullDeliveryListView.IsVisible == false)
            {
                currentDelivery = deliveryList[CurrentIndex];
            }
            else
            {
                var caller = (ImageButton)sender;
                var selectedItem = (DeliveryInfo)caller.CommandParameter;
                currentDelivery = selectedItem;
            }
            try
            {
                if (currentDelivery.parsedPhone != null && currentDelivery.parsedPhone.Length == 10)
                {
                    var message = new SmsMessage(null, new[] { currentDelivery.parsedPhone });
                    await Sms.ComposeAsync(message);

                }
                else
                {
                    await DisplayAlert("Oops", "Unfortunaly, we can't process your request at this moment. There is no available phone number for this customer", "OK");
                }
            }
            catch (Exception PhoneDataError)
            {
                Debug.WriteLine(PhoneDataError.Message);
            }
        }

        void ShowBackupDeliveriesFrame(System.Object sender, System.EventArgs e)
        {
            if (fullDeliveryListView.IsVisible == false)
            {
                
                BottomSheet.TranslationY = -800;
                //var argument = new PanUpdatedEventArgs();
                PanGestureRecognizer_PanUpdated(sender, new PanUpdatedEventArgs(GestureStatus.Completed, 0));
                

                sigleDeliveryView.IsVisible = false;
                fullDeliveryListView.IsVisible = true;
            }
            else
            {
                fullDeliveryListView.IsVisible = false;
                sigleDeliveryView.IsVisible = true;
            }
        }

        void HideBackupDeliveriesFrame(System.Object sender, System.EventArgs e)
        {
            BottomSheet.TranslationY = 0;
            PanGestureRecognizer_PanUpdated(sender, new PanUpdatedEventArgs(GestureStatus.Completed, 0));
            fullDeliveryListView.IsVisible = false;
            sigleDeliveryView.IsVisible = true;


            var structToSave = new Dictionary<string, List<DeliveryItemToSave>>();

            var start = new DeliveryItemToSave();
            start.email = "";
            start.items = new List<ItemToSave>();
            start.phone = startLocation.phone;

            var c = new Models.Coordinates();
            c.latitude = startLocation.latitude;
            c.longitude = startLocation.longitude;

            start.coordinates = c;
            start.customer_uid = startLocation.customer_uid;
            start.delivery_zip = startLocation.zipcode;
            start.purchase_uid = startLocation.customer_uid;
            start.delivery_city = startLocation.city;
            start.delivery_unit = startLocation.unit;
            start.delivery_state = startLocation.state;
            start.delivery_street = startLocation.address;
            start.delivery_last_name = startLocation.lastName;
            start.delivery_first_name = startLocation.firstName;
            start.start_delivery_date = startLocation.delivery_date;
            start.delivery_instructions = startLocation.delivery_instructions;
            start.delivery_status = startLocation.status;


            var starList = new List<DeliveryItemToSave>();
            starList.Add(start);
            structToSave.Add("1", starList);
            int i = 2;
            foreach(DeliveryInfo delivery in deliveryList)
            {
                var list = new List<DeliveryItemToSave>();
                var deliveryToSave = new DeliveryItemToSave();

                deliveryToSave.email = delivery.email;
                deliveryToSave.items = JsonConvert.DeserializeObject<List<ItemToSave>>(delivery.delivery_items);
                deliveryToSave.phone = delivery.phone;

                var saveCoordinates = new Models.Coordinates();
                saveCoordinates.latitude = delivery.latitude;
                saveCoordinates.longitude = delivery.longitude;

                deliveryToSave.coordinates = saveCoordinates;
                deliveryToSave.customer_uid = delivery.customer_uid; 
                deliveryToSave.delivery_zip = delivery.zipcode; 
                deliveryToSave.purchase_uid = delivery.purchase_uid; 
                deliveryToSave.delivery_city = delivery.city;
                deliveryToSave.delivery_unit = delivery.unit;
                if(delivery.status == "Status: Delivered")
                {
                    deliveryToSave.delivery_status = "TRUE";
                }
                else if (delivery.status == "Status: Pending...")
                {
                    deliveryToSave.delivery_status = "FALSE";
                }
                else if (delivery.status == "Status: Skipped")
                {
                    deliveryToSave.delivery_status = "SKIP";
                }

                deliveryToSave.delivery_state = delivery.state; 
                deliveryToSave.delivery_street = delivery.address; 
                deliveryToSave.delivery_last_name = delivery.lastName; 
                deliveryToSave.delivery_first_name = delivery.firstName; 
                deliveryToSave.start_delivery_date = delivery.delivery_date; 
                deliveryToSave.delivery_instructions = delivery.delivery_instructions;
                list.Add(deliveryToSave);
                structToSave.Add(i.ToString(),list);
                i++;
            }

            //var contentString = JsonConvert.SerializeObject(structToSave);
            SaveChanges(structToSave);
        }

        public static async void SaveChanges(Dictionary<string, List<DeliveryItemToSave>> route)
        {
            var data = new UpdateDeliveryRoute();

            data.route = route;
            data.route_id = routeID;

            var contentString = JsonConvert.SerializeObject(data);
            var content = new StringContent(contentString, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var endpointCall = await client.PostAsync(Constant.UpdateRouteOrder, content);

            Debug.WriteLine("JSON TO SEND: " + contentString);
            Debug.WriteLine("CALL STATUS: " + endpointCall.IsSuccessStatusCode);
        }

        void ReverseListClick(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Reverse List");
            
            deliveryListView.ItemsSource = reverseDeliveryList(deliveryList);
           
            ResetMap();
            FindNextDeliveryAvailable(deliveryList);
            SetDelivery();
            SetStartToFirstLocation(Color.Black);
            SetCompleteRouteView();

        }

        ObservableCollection<DeliveryInfo> reverseDeliveryList(ObservableCollection<DeliveryInfo> source)
        {
            if(source == null)
            {
                return null;
            }
            else
            {
                ObservableCollection<DeliveryInfo> reverseList = new ObservableCollection<DeliveryInfo>();
                for (int i = source.Count - 1; i >= 0; i--)
                {
                    reverseList.Add(source[i]);
                }
                //BackupDeliveries = reverseList
                UpdateDeliveryIDs(reverseList);
                deliveryList = reverseList;
                return reverseList;
            }
        }

        void DragGestureRecognizer_DragStarting(System.Object sender, Xamarin.Forms.DragStartingEventArgs e)
        {
            var f = (sender as Element)?.Parent as Frame;
            
            e.Data.Properties.Add("Frame", f);
        }

        void DropGestureRecognizer_Drop(System.Object sender, Xamarin.Forms.DropEventArgs e)
        {
            //var button = (ImageButton)sender;
            //var itemModelObject = (SingleItem)button.CommandParameter;
            //var itemSelected = new ItemPurchased();

            var button = (Frame)e.Data.Properties["Frame"];
            var g = (TapGestureRecognizer)button.GestureRecognizers[1];
            var itemModelObject = (DeliveryInfo)g.CommandParameter;

            deliveryList.Remove(itemModelObject);
            deliveryList.Add(itemModelObject);
            deliveryListView.ItemsSource = deliveryList;

            ResetMap();
            UpdateDeliveryIDs(deliveryList);
            FindNextDeliveryAvailable(deliveryList);
            SetDelivery();
            SetCompleteRouteView();
        }

        void ShowMenu(System.Object sender, System.EventArgs e)
        {
            if(menuRow.IsVisible == false)
            {
                menuRow.IsVisible = true;
            }
            else
            {
                menuRow.IsVisible = false;
            }
        }

        void MenuDirections(System.Object sender, System.EventArgs e)
        {
            GetDirections(sender, e);
        }

        void MenuDetails(System.Object sender, System.EventArgs e)
        {
            BottomSheet.TranslationY = -800;
            sigleDeliveryView.IsVisible = true;
            fullDeliveryListView.IsVisible = false;
            PanGestureRecognizer_PanUpdated(sender, new PanUpdatedEventArgs(GestureStatus.Completed, 0));
        }

        void MenuOverview(System.Object sender, System.EventArgs e)
        {
            sigleDeliveryView.IsVisible = false;
            fullDeliveryListView.IsVisible = true;
            BottomSheet.TranslationY = -800;
            PanGestureRecognizer_PanUpdated(sender, new PanUpdatedEventArgs(GestureStatus.Completed, 0));
        }

        void LogOut(System.Object sender, System.EventArgs e)
        {
            user.PrintUser();
            if (Application.Current.Properties.Keys.Contains(Constant.Autheticator))
            {
                user.id = "";
                user.route_id = "";
                user.PrintUser();
                string account = JsonConvert.SerializeObject(user);
                //Debug.WriteLine("USER: " + account);

                Application.Current.Properties[Constant.Autheticator] = account;
                Application.Current.SavePropertiesAsync();
            }
            Application.Current.MainPage = new LogInPage();
        }

        void NavigateToProductsPage(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new ProductsPage();
        }
    }
}
