using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Acr.UserDialogs;
using JustDelivered.Config;
using JustDelivered.LogIn.Classes;
using Newtonsoft.Json;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace JustDelivered.Views
{
    public partial class DeliveriesPage : ContentPage
    {
        double y = 0;

        private static int CurrentIndex = 1;
        private static int BackupIndex = -1;
        private static List<DeliveryInfo> LocalDeliveriesList = new List<DeliveryInfo>();
        private static List<DeliveryInfo> LocalBackupDeliveriesList = new List<DeliveryInfo>();
        ObservableCollection<DeliveryInfo> BackupDeliveries = new ObservableCollection<DeliveryInfo>();


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
            public string delivery_city { get; set; }
            public string delivery_state { get; set; }
            public string delivery_zip { get; set; }
            public string delivery_instructions { get; set; }
            public string delivery_items { get; set; }
        }


        public class Deliveries
        {
            public string message { get; set; }
            public int code { get; set; }
            public IList<Delivery> result { get; set; }
            public string sql { get; set; }
        }

        public class DeliveryInfo
        {
            public int route_id { get; set; }
            public string name { get; set; }
            public string house_address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }

            public string status { get; set; }
            public string delivery_instructions { get; set; }
            public int ID { get; set; }
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

        public DeliveriesPage(string accessToken, string refreshToken, AuthenticatorCompletedEventArgs e)
        {
            InitializeComponent();
            UserDialogs.Instance.ShowLoading("Please wait while we are processing your request...");
            SetHeightWidthOnMap();
            SetWidthOnHelpButtonRow();
            SetDefaultLocationOnMap();
            BackupDisplay.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);
            VerifyUserAccount(accessToken, refreshToken, e);
        }

        public DeliveriesPage(int Index)
        {
            if (Index == -1)
            {
                InitializeComponent();
                BackupDeliveries.Clear();
                Debug.WriteLine("CONSTRUCTOR CALLED FROM CONFIRMATION PAGE -1");
                Debug.WriteLine("FLAG 1: " + Index);
                Debug.WriteLine("BACKUP INDEX: " + BackupIndex);
                Debug.WriteLine("STATUS BEFORE: " + LocalBackupDeliveriesList[BackupIndex].status);
                LocalBackupDeliveriesList[BackupIndex].status = "Status: Completed";
                Debug.WriteLine("STATUS AFTER: " + LocalBackupDeliveriesList[BackupIndex].status);
                SetHeightWidthOnMap();
                SetWidthOnHelpButtonRow();
                SetDefaultLocationOnMap();
                GetNextDelivery();


                SetStartToFirstLocation();

                var Path = new Polyline();
                Path.StrokeColor = Color.Black;
                Path.StrokeWidth = 4;

                for (int i = 1; i < LocalDeliveriesList.Count; i++)
                {
                    var Pin = new Pin();
                    Pin.Label = "Delivery " + i + " For: " + LocalDeliveriesList[i].firstNameAndFirstLetterLastName;
                    Pin.Address = LocalDeliveriesList[i].house_address;
                    Pin.Type = PinType.Generic;
                    Pin.Position = new Position(LocalDeliveriesList[i].latitude, LocalDeliveriesList[i].longitude);

                    Path.Geopath.Add(Pin.Position);
                    DeliveriesMap.Pins.Add(Pin);
                    Debug.WriteLine(LocalDeliveriesList[i].parsedPhone);
                    LocalDeliveriesList[i].ID = i;
                    BackupDeliveries.Add(LocalDeliveriesList[i]);
                }

                BackupDeliveriesList.ItemsSource = BackupDeliveries;
                DeliveriesMap.MapElements.Add(Path);

                for (int i = 1; i < LocalBackupDeliveriesList.Count; i++)
                {
                    BackupDeliveries.Add(LocalBackupDeliveriesList[i]);
                }
                BackupDeliveriesList.ItemsSource = BackupDeliveries;
                BackupDisplay.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);
            }
            else
            {
                InitializeComponent();
                BackupDeliveries.Clear();
                CurrentIndex = Index;

                for (int i = 1; i < LocalBackupDeliveriesList.Count; i++)
                {
                    if(CurrentIndex == i)
                    {
                        LocalBackupDeliveriesList[i].status = "Status: Completed";
                    }
                    BackupDeliveries.Add(LocalBackupDeliveriesList[i]);
                }

                SetStartToFirstLocation();

                var Path = new Polyline();
                Path.StrokeColor = Color.Black;
                Path.StrokeWidth = 4;

                for (int i = 1; i < LocalDeliveriesList.Count; i++)
                {
                    var Pin = new Pin();
                    Pin.Label = "Delivery " + i + " For: " + LocalDeliveriesList[i].firstNameAndFirstLetterLastName;
                    Pin.Address = LocalDeliveriesList[i].house_address;
                    Pin.Type = PinType.Generic;
                    Pin.Position = new Position(LocalDeliveriesList[i].latitude, LocalDeliveriesList[i].longitude);

                    Path.Geopath.Add(Pin.Position);
                    DeliveriesMap.Pins.Add(Pin);
                    Debug.WriteLine(LocalDeliveriesList[i].parsedPhone);
                    LocalDeliveriesList[i].ID = i;
                    BackupDeliveries.Add(LocalDeliveriesList[i]);
                }

                BackupDeliveriesList.ItemsSource = BackupDeliveries;
                DeliveriesMap.MapElements.Add(Path);

                CurrentIndex++;
                Debug.WriteLine("CONSTRUCTOR CALLED FROM CONFIRMATION PAGE");
                Debug.WriteLine(CurrentIndex);

                SetHeightWidthOnMap();
                SetWidthOnHelpButtonRow();
                SetDefaultLocationOnMap();
                GetNextDelivery();

                BackupDeliveriesList.ItemsSource = BackupDeliveries;
                BackupDisplay.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);
            }
        }

        public void GetNextDelivery()
        {
            CustomerName.Text = LocalDeliveriesList[CurrentIndex].name;
            CustomerAddress.Text = LocalDeliveriesList[CurrentIndex].house_address;
            DeliveryInstructions.Text = LocalDeliveriesList[CurrentIndex].delivery_instructions;
            CurrentDeliveryNumber.Text = CurrentIndex.ToString();
            TotalDeliveriesNumber.Text = (LocalDeliveriesList.Count - 1).ToString();
        }

        public void SetStartLocation()
        {
            //var Map = new CustomMap();
            var P = new Pin();

            P.Address = LocalDeliveriesList[0].house_address;
            P.Type = PinType.Place;
            P.Position = new Position(LocalDeliveriesList[0].latitude, LocalDeliveriesList[0].longitude);

            DeliveriesMap.Pins.Add(P);
        }

        public async void VerifyUserAccount(string accessToken, string refreshToken, AuthenticatorCompletedEventArgs e)
        {
            try
            {
                //Application.Current.MainPage = null;
                //GoogleUserProfileAsync(e.Account.Properties["access_token"], e.Account.Properties["refresh_token"], e);
                //Application.Current.MainPage = new DeliveriesPage();
                //Application.Current.MainPage = new DeliveriesPage();

                //UserDialogs.Instance.ShowLoading("Please wait while we are processing your request...");
                //UserDialogs.Instance.ShowLoading("Please wait while we are processing your request...");
                Debug.WriteLine("IN SIDE GOOGLE PROFILE");
                var client = new HttpClient();
                var socialLogInPost = new SocialLogInPost();

                var request = new OAuth2Request("GET", new Uri(Constant.GoogleUserInfoUrl), null, e.Account);
                var GoogleResponse = await request.GetResponseAsync();
                var userData = GoogleResponse.GetResponseText();

                System.Diagnostics.Debug.WriteLine(userData);
                GoogleResponse googleData = JsonConvert.DeserializeObject<GoogleResponse>(userData);

                socialLogInPost.email = googleData.email;
                socialLogInPost.password = "";
                socialLogInPost.social_id = googleData.id;
                //socialLogInPost.delivery_date = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                socialLogInPost.delivery_date = "2021-01-24 00:00:00";
                socialLogInPost.signup_platform = "GOOGLE";

                var socialLogInPostSerialized = JsonConvert.SerializeObject(socialLogInPost);
                var postContent = new StringContent(socialLogInPostSerialized, Encoding.UTF8, "application/json");

                Debug.WriteLine(socialLogInPostSerialized);

                var RDSResponse = await client.PostAsync(Constant.LogInUrl, postContent);
                var responseContent = await RDSResponse.Content.ReadAsStringAsync();

                Debug.WriteLine(responseContent);

                Debug.WriteLine(RDSResponse.IsSuccessStatusCode);

                if (RDSResponse.IsSuccessStatusCode)
                {
                    if (responseContent != null)
                    {
                        //if (responseContent.Contains(Constant.EmailNotFound))
                        //{
                        //    var signUp = await DisplayAlert("Message", "It looks like you don't have a Serving Fresh account. Please sign up!", "OK", "Cancel");
                        //    if (signUp)
                        //    {
                        //        //Application.Current.MainPage = new SocialSignUp(googleData.id, googleData.given_name, googleData.family_name, googleData.email, accessToken, refreshToken, "GOOGLE");
                        //        Application.Current.MainPage = new MainPage();
                        //        //UserDialogs.Instance.HideLoading();
                        //    }
                        //}
                        if (responseContent.Contains(Constant.AutheticatedSuccesful))
                        {
                            //Application.Current.MainPage = new DeliveriesPage();
                            //Application.Current.MainPage.IsVisible = true;
                            //UserDialogs.Instance.HideLoading();
                            //UserDialogs.Instance.ShowLoading("Please wait while we are processing your request...");

                            var List = JsonConvert.DeserializeObject<Deliveries>(responseContent);
                            //Debug.WriteLine("LATITUDE OF ELEMENT 0: "+ data.result[0].delivery_coordinates.latitude);
                            //Debug.WriteLine("LATITUDE OF ELEMENT 1: " + data.result[0].delivery_coordinates.longitude);


                            //UserDialogs.Instance.ShowLoading("Please wait while we are processing your request...");
                            
                            foreach (Delivery a in List.result)
                            {

                                //var customer = JsonConvert.DeserializeObject<Item>(str2);
                                //element.route_id = 1;
                                //element.name = customer.customer;
                                //element.house_address = customer.delivery_street.ToUpper();
                                //element.city = "";
                                //element.state = "";
                                //element.zipcode = "";
                                //element.email = customer.email;
                                //element.phone = customer.phone;
                                //element.latitude = customer.coordinates.latitude;
                                //element.longitude = customer.coordinates.longitude;
                                //element.status = "Status: Pending...";
                                //element.ID = 0;
                                var element = new DeliveryInfo();
                                element.name = a.delivery_first_name +" "+ a.delivery_last_name;
                                element.house_address = a.delivery_street +" " + a.delivery_city + " "+ a.delivery_state+ " " + a.delivery_zip;
                                //element.city = "";
                                //element.state = "";
                                //element.zipcode = "";

                                element.email = a.delivery_email;
                                element.phone = a.delivery_phone;

                                var Coordinates = JsonConvert.DeserializeObject<Coordinates>(a.delivery_coordinates);

                                element.latitude = Coordinates.latitude;
                                element.longitude = Coordinates.longitude;
                                element.status = "Status: Pending...";
                                element.ID = 0;
                                if (a.delivery_instructions != null && a.delivery_instructions != "")
                                {
                                    element.delivery_instructions = a.delivery_instructions;
                                }
                                else
                                {
                                    element.delivery_instructions = "No delivery instructions for this order";
                                }
                                //var element = new DeliveryInfo();

                                //Debug.WriteLine("ROUTE_ID: " + a.route_id);
                                //Debug.WriteLine("ROUTE_OPTION: " + a.route_option);
                                //Debug.WriteLine("ROUTE_BUSINESS_ID: " + a.route_business_id);
                                //Debug.WriteLine("ROUTE_DRIVER_ID: " + a.route_driver_id);
                                //Debug.WriteLine("ROUTE_DELIVERY_INFO: " + a.route_delivery_info);

                                //string str2 = "";
                                //var array = a.route_delivery_info.ToCharArray();
                                //for (int i = 1; i < array.Length - 1; i++)
                                //{
                                //    str2 += array[i];
                                //}

                                //Debug.WriteLine(str2);
                                //var customer = JsonConvert.DeserializeObject<Item>(str2);
                                //element.route_id = 1;
                                //element.name = customer.customer;
                                //element.house_address = customer.delivery_street.ToUpper();
                                //element.city = "";
                                //element.state = "";
                                //element.zipcode = "";
                                //element.email = customer.email;
                                //element.phone = customer.phone;
                                //element.latitude = customer.coordinates.latitude;
                                //element.longitude = customer.coordinates.longitude;
                                //element.status = "Status: Pending...";
                                //element.ID = 0;
                                //if (customer.delivery_instructions != null && customer.delivery_instructions != "")
                                //{
                                //    element.delivery_instructions = customer.delivery_instructions;
                                //}
                                //else
                                //{
                                //    element.delivery_instructions = "No delivery instructions for this order";
                                //}
                                //Debug.WriteLine(customer.customer);
                                //Debug.WriteLine(customer.delivery_street);
                                //Debug.WriteLine(customer.coordinates.latitude);
                                //Debug.WriteLine(customer.coordinates.longitude);
                                //Debug.WriteLine(customer.email);
                                //Debug.WriteLine(customer.phone);

                                //Debug.WriteLine("NUM_DELIVERYES: " + a.num_deliveries);
                                //Debug.WriteLine("ROUTE_DISTANCE: " + a.route_distance);
                                //Debug.WriteLine("ROUTE_TIME: " + a.route_time);
                                //Debug.WriteLine("SHIPMENT_DATE: " + a.shipment_date);

                                LocalDeliveriesList.Add(element);
                                LocalBackupDeliveriesList.Add(element);
                            }
                            //UserDialogs.Instance.HideLoading();


                            CustomerName.Text = LocalDeliveriesList[CurrentIndex].name;
                            CustomerAddress.Text = LocalDeliveriesList[CurrentIndex].house_address;
                            DeliveryInstructions.Text = LocalDeliveriesList[CurrentIndex].delivery_instructions;
                            CurrentDeliveryNumber.Text = CurrentIndex.ToString();
                            TotalDeliveriesNumber.Text = (LocalDeliveriesList.Count - 1).ToString();

                            SetStartToFirstLocation();

                            var Path = new Polyline();
                            Path.StrokeColor = Color.Black;
                            Path.StrokeWidth = 4;

                            for (int i = 1; i < LocalDeliveriesList.Count; i++)
                            {
                                var Pin = new Pin();
                                Pin.Label = "Delivery " + i + " For: " + LocalDeliveriesList[i].firstNameAndFirstLetterLastName;
                                Pin.Address = LocalDeliveriesList[i].house_address;
                                Pin.Type = PinType.Generic;
                                Pin.Position = new Position(LocalDeliveriesList[i].latitude, LocalDeliveriesList[i].longitude);

                                Path.Geopath.Add(Pin.Position);
                                DeliveriesMap.Pins.Add(Pin);
                                Debug.WriteLine(LocalDeliveriesList[i].parsedPhone);
                                LocalDeliveriesList[i].ID = i;
                                BackupDeliveries.Add(LocalDeliveriesList[i]);
                            }

                            BackupDeliveriesList.ItemsSource = BackupDeliveries;
                            DeliveriesMap.MapElements.Add(Path);
                            BackupDisplay.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);

















                            UserDialogs.Instance.HideLoading();

                            Debug.WriteLine("GET DELIVERIES");
                            //try
                            //{
                            //    Application.Current.MainPage = new DeliveriesPage();

                            //    var data = JsonConvert.DeserializeObject<UserInfo>(responseContent);
                            //    //Application.Current.Properties["user_id"] = data.result[0].customer_uid;

                            //    UpdateTokensPost updateTokesPost = new UpdateTokensPost();
                            //    updateTokesPost.uid = data.result[0].customer_uid;
                            //    updateTokesPost.mobile_access_token = accessToken;
                            //    updateTokesPost.mobile_refresh_token = refreshToken;

                            //    var updateTokesPostSerializedObject = JsonConvert.SerializeObject(updateTokesPost);
                            //    var updateTokesContent = new StringContent(updateTokesPostSerializedObject, Encoding.UTF8, "application/json");
                            //    var updateTokesResponse = await client.PostAsync(Constant.UpdateTokensUrl, updateTokesContent);
                            //    var updateTokenResponseContent = await updateTokesResponse.Content.ReadAsStringAsync();
                            //    System.Diagnostics.Debug.WriteLine(updateTokenResponseContent);

                            //    if (updateTokesResponse.IsSuccessStatusCode)
                            //    {
                            //        var GoogleRequest = new RequestUserInfo();
                            //        GoogleRequest.uid = data.result[0].customer_uid;

                            //        var requestSelializedObject = JsonConvert.SerializeObject(GoogleRequest);
                            //        var requestContent = new StringContent(requestSelializedObject, Encoding.UTF8, "application/json");

                            //        var clientRequest = await client.PostAsync(Constant.GetUserInfoUrl, requestContent);

                            //        if (clientRequest.IsSuccessStatusCode)
                            //        {
                            //            var SFUser = await clientRequest.Content.ReadAsStringAsync();
                            //            var GoogleUserData = JsonConvert.DeserializeObject<UserInfo>(SFUser);

                            //            DateTime today = DateTime.Now;
                            //            DateTime expDate = today.AddDays(14);

                            //            Debug.WriteLine("I AM SIGN IN");


                            //            //Application.Current.Properties["user_id"] = data.result[0].customer_uid;
                            //            //Application.Current.Properties["time_stamp"] = expDate;
                            //            //Application.Current.Properties["platform"] = "GOOGLE";
                            //            //Application.Current.Properties["user_email"] = GoogleUserData.result[0].customer_email;
                            //            //Application.Current.Properties["user_first_name"] = GoogleUserData.result[0].customer_first_name;
                            //            //Application.Current.Properties["user_last_name"] = GoogleUserData.result[0].customer_last_name;
                            //            //Application.Current.Properties["user_phone_num"] = GoogleUserData.result[0].customer_phone_num;
                            //            //Application.Current.Properties["user_address"] = GoogleUserData.result[0].customer_address;
                            //            //Application.Current.Properties["user_unit"] = GoogleUserData.result[0].customer_unit;
                            //            //Application.Current.Properties["user_city"] = GoogleUserData.result[0].customer_city;
                            //            //Application.Current.Properties["user_state"] = GoogleUserData.result[0].customer_state;
                            //            //Application.Current.Properties["user_zip_code"] = GoogleUserData.result[0].customer_zip;
                            //            //Application.Current.Properties["user_latitude"] = GoogleUserData.result[0].customer_lat;
                            //            //Application.Current.Properties["user_longitude"] = GoogleUserData.result[0].customer_long;

                            //            //_ = Application.Current.SavePropertiesAsync();

                            //            //if (Device.RuntimePlatform == Device.iOS)
                            //            //{
                            //            //    deviceId = Preferences.Get("guid", null);
                            //            //    if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
                            //            //}
                            //            //else
                            //            //{
                            //            //    deviceId = Preferences.Get("guid", null);
                            //            //    if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
                            //            //}

                            //            //if (deviceId != null)
                            //            //{
                            //            //    NotificationPost notificationPost = new NotificationPost();

                            //            //    notificationPost.uid = (string)Application.Current.Properties["user_id"];
                            //            //    notificationPost.guid = deviceId.Substring(5);
                            //            //    Application.Current.Properties["guid"] = deviceId.Substring(5);
                            //            //    notificationPost.notification = "TRUE";

                            //            //    var notificationSerializedObject = JsonConvert.SerializeObject(notificationPost);
                            //            //    Debug.WriteLine("Notification JSON Object to send: " + notificationSerializedObject);

                            //            //    var notificationContent = new StringContent(notificationSerializedObject, Encoding.UTF8, "application/json");

                            //            //    var clientResponse = await client.PostAsync(Constant.NotificationsUrl, notificationContent);

                            //            //    Debug.WriteLine("Status code: " + clientResponse.IsSuccessStatusCode);

                            //            //    if (clientResponse.IsSuccessStatusCode)
                            //            //    {
                            //            //        System.Diagnostics.Debug.WriteLine("We have post the guid to the database");
                            //            //    }
                            //            //    else
                            //            //    {
                            //            //        await DisplayAlert("Ooops!", "Something went wrong. We are not able to send you notification at this moment", "OK");
                            //            //    }
                            //            //}

                            //            //Application.Current.MainPage = new SelectionPage();
                            //        }
                            //        else
                            //        {
                            //            await DisplayAlert("Alert!", "Our internal system was not able to retrieve your user information. We are working to solve this issue.", "OK");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        await DisplayAlert("Oops", "We are facing some problems with our internal system. We weren't able to update your credentials", "OK");
                            //    }
                            //}
                            //catch (Exception second)
                            //{
                            //    Debug.WriteLine(second.Message);
                            //}
                        }
                        if (responseContent.Contains(Constant.ErrorPlatform))
                        {
                            //var RDSCode = JsonConvert.DeserializeObject<RDSLogInMessage>(responseContent);
                            //await DisplayAlert("Message", RDSCode.message, "OK");
                        }

                        if (responseContent.Contains(Constant.ErrorUserDirectLogIn))
                        {
                            await DisplayAlert("Oops!", "You have an existing Serving Fresh account. Please use direct login", "OK");
                        }
                    }
                }
            }
            catch (Exception g)
            {
                Debug.WriteLine("IN side " + g.Message);
                MyStack.IsEnabled = false;
                await DisplayAlert("Oops", "Our system shows that there are no deliveries available for you at this moment. Please check again later.", "OK");
            }
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

        void SetWidthOnHelpButtonRow()
        {
            HelpButtonRow.WidthRequest = Application.Current.MainPage.Width;
        }

        //async void GetDeliveries()
        //{
        //    var Client = new HttpClient();
        //    var DeliveriesEndpoint = await Client.GetAsync("https://uqu7qejuee.execute-api.us-west-1.amazonaws.com/dev/api/v2/getRoutes");
        //    if (DeliveriesEndpoint.IsSuccessStatusCode)
        //    {
        //        var data = await DeliveriesEndpoint.Content.ReadAsStringAsync();
        //        var DeliveryList = JsonConvert.DeserializeObject<Deliveries>(data);
        //        Debug.WriteLine("DELIVERY DATA FROM GETROUTES ENDPOINT: " + data);

        //        foreach (DeliveryItem a in DeliveryList.result.result)
        //        {
        //            var element = new DeliveryInfo();

        //            Debug.WriteLine("ROUTE_ID: " + a.route_id);
        //            Debug.WriteLine("ROUTE_OPTION: " + a.route_option);
        //            Debug.WriteLine("ROUTE_BUSINESS_ID: " + a.route_business_id);
        //            Debug.WriteLine("ROUTE_DRIVER_ID: " + a.route_driver_id);
        //            Debug.WriteLine("ROUTE_DELIVERY_INFO: " + a.route_delivery_info);

        //            string str2 = "";
        //            var array = a.route_delivery_info.ToCharArray();
        //            for (int i = 1; i < array.Length - 1; i++)
        //            {
        //                str2 += array[i];
        //            }

        //            Debug.WriteLine(str2);
        //            var customer = JsonConvert.DeserializeObject<Item>(str2);
        //            element.route_id = 1;
        //            element.name = customer.customer;
        //            element.house_address = customer.delivery_street.ToUpper();
        //            element.city = "";
        //            element.state = "";
        //            element.zipcode = "";
        //            element.email = customer.email;
        //            element.phone = customer.phone;
        //            element.latitude = customer.coordinates.latitude;
        //            element.longitude = customer.coordinates.longitude;
        //            element.status = "Status: Pending...";
        //            element.ID = 0;
        //            if (customer.delivery_instructions != null && customer.delivery_instructions != "")
        //            {
        //                element.delivery_instructions = customer.delivery_instructions;
        //            }
        //            else
        //            {
        //                element.delivery_instructions = "No delivery instructions for this order";
        //            }
        //            Debug.WriteLine(customer.customer);
        //            Debug.WriteLine(customer.delivery_street);
        //            Debug.WriteLine(customer.coordinates.latitude);
        //            Debug.WriteLine(customer.coordinates.longitude);
        //            Debug.WriteLine(customer.email);
        //            Debug.WriteLine(customer.phone);

        //            Debug.WriteLine("NUM_DELIVERYES: " + a.num_deliveries);
        //            Debug.WriteLine("ROUTE_DISTANCE: " + a.route_distance);
        //            Debug.WriteLine("ROUTE_TIME: " + a.route_time);
        //            Debug.WriteLine("SHIPMENT_DATE: " + a.shipment_date);

        //            LocalDeliveriesList.Add(element);
        //            LocalBackupDeliveriesList.Add(element);
        //        }

        //        CustomerName.Text = LocalDeliveriesList[CurrentIndex].name;
        //        CustomerAddress.Text = LocalDeliveriesList[CurrentIndex].house_address;
        //        DeliveryInstructions.Text = LocalDeliveriesList[CurrentIndex].delivery_instructions;
        //        CurrentDeliveryNumber.Text = CurrentIndex.ToString();
        //        TotalDeliveriesNumber.Text = (LocalDeliveriesList.Count - 1).ToString();

        //        SetStartToFirstLocation();

        //        var Path = new Polyline();
        //        Path.StrokeColor = Color.Black;
        //        Path.StrokeWidth = 4;

        //        for (int i = 1; i < LocalDeliveriesList.Count; i++)
        //        {
        //            var Pin = new Pin();
        //            Pin.Label = "Delivery " + i + " For: " + LocalDeliveriesList[i].firstNameAndFirstLetterLastName;
        //            Pin.Address = LocalDeliveriesList[i].house_address;
        //            Pin.Type = PinType.Generic;
        //            Pin.Position = new Position(LocalDeliveriesList[i].latitude, LocalDeliveriesList[i].longitude);

        //            Path.Geopath.Add(Pin.Position);
        //            DeliveriesMap.Pins.Add(Pin);
        //            Debug.WriteLine(LocalDeliveriesList[i].parsedPhone);
        //            LocalDeliveriesList[i].ID = i;
        //            BackupDeliveries.Add(LocalDeliveriesList[i]);
        //        }

        //        BackupDeliveriesList.ItemsSource = BackupDeliveries;
        //        DeliveriesMap.MapElements.Add(Path);
        //        BackupDisplay.Margin = new Thickness(0, Application.Current.MainPage.Height, 0, 0);
        //    }
        //}

        public async void SetStartToFirstLocation()
        {
            if (LocalDeliveriesList.Count >= 2)
            {
                var Start = new Polyline();
                Start.StrokeColor = Color.Red;
                Start.StrokeWidth = 4;

                var Pin = new Pin();
                Pin.Label = "Start Location";
                Pin.Address = LocalDeliveriesList[0].house_address;
                Pin.Type = PinType.Place;
                Pin.Position = new Position(LocalDeliveriesList[0].latitude, LocalDeliveriesList[0].longitude);

                var P1 = new Position(LocalDeliveriesList[0].latitude, LocalDeliveriesList[0].longitude);
                var P2 = new Position(LocalDeliveriesList[1].latitude, LocalDeliveriesList[1].longitude);

                Start.Geopath.Add(P1);
                Start.Geopath.Add(P2);

                DeliveriesMap.MapElements.Add(Start);
                DeliveriesMap.Pins.Add(Pin);

                var Span = new MapSpan(Pin.Position, 0.1, 0.1);
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

        async void GetDirections(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("GET DIRECTIONS");

            var location = new Location(LocalDeliveriesList[CurrentIndex].latitude, LocalDeliveriesList[CurrentIndex].longitude);
            var options = new MapLaunchOptions { Name = LocalDeliveriesList[CurrentIndex].house_address, NavigationMode = NavigationMode.Driving };

            await Xamarin.Essentials.Map.OpenAsync(location, options);
            Application.Current.MainPage = new ConfirmationPage(LocalDeliveriesList[CurrentIndex], CurrentIndex);
        }

        void SkipDirections(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("SKIP DIRECTIONS");
            Application.Current.MainPage = new ConfirmationPage(LocalDeliveriesList[CurrentIndex], CurrentIndex);
        }

        async void SkipDelivery(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("SKIP DELIVERY");
            var Result = await DisplayAlert("Are you sure you want to skip this delivery?", "Press 'YES' to skip this delivery. Otherwise, press 'NO' to continue processing this delivery", "YES", "NO");
            if (Result)
            {
                string ReasonToSkipDelivery = await DisplayPromptAsync("", "What's the reason you are not able to complete this delivery?");
                CurrentIndex++;
                GetNextDelivery();
            }
        }

        async void CallCustomer(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("CALL CUSTOMER");
            try
            {
                if (LocalDeliveriesList[CurrentIndex].parsedPhone != null && LocalDeliveriesList[CurrentIndex].parsedPhone.Length == 10)
                {
                    PhoneDialer.Open(LocalDeliveriesList[CurrentIndex].parsedPhone);
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
            Debug.WriteLine("TEXT CUSTOMER");
            try
            {
                if (LocalDeliveriesList[CurrentIndex].parsedPhone != null && LocalDeliveriesList[CurrentIndex].parsedPhone.Length == 10)
                {
                    var message = new SmsMessage(null, new[] { LocalDeliveriesList[CurrentIndex].parsedPhone });
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
            Debug.WriteLine("BACKUP");
            var y = Application.Current.MainPage.Height / 2;
            var Width = Application.Current.MainPage.Width;
            Debug.WriteLine(y);
            BackupDisplay.Margin = new Thickness(0, y, 0, 0);
            BackupDisplay.WidthRequest = Width;
            BackupDeliveriesList.HeightRequest = y - 80;
        }

        void HideBackupDeliveriesFrame(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("HIDE");
            var y = Application.Current.MainPage.Height;
            Debug.WriteLine(y);
            BackupDisplay.Margin = new Thickness(0, y, 0, 0);
        }

        async void GetDirectionFromBackup(System.Object sender, System.EventArgs e)
        {
            var element = (Label)sender;
            Debug.WriteLine("INDEX TO REFERENCE: " + element.ClassId);
            var index = Int16.Parse(element.ClassId);
            if (index < LocalBackupDeliveriesList.Count)
            {
                BackupIndex = index;
                var location = new Location(LocalBackupDeliveriesList[index].latitude, LocalBackupDeliveriesList[index].longitude);
                var options = new MapLaunchOptions { Name = LocalBackupDeliveriesList[index].house_address, NavigationMode = NavigationMode.Driving };

                await Xamarin.Essentials.Map.OpenAsync(location, options);
                Application.Current.MainPage = new ConfirmationPage(LocalBackupDeliveriesList[index], -1);
            }
        }
    }
}
