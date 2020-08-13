using System;
using System.Collections.Generic;
using System.ComponentModel;
using DeliveryApp.Models;
using Newtonsoft.Json;
using Plugin.LatestVersion;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeliveryApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer    
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        string action;
        
        public MainPage()
        {
            InitializeComponent();
            SetLogInPageIcon();
        }

        // This function sets the Just Delivered Icon on the Log In page
        public void SetLogInPageIcon()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                iconLogin.Source = "JDIcon.png";
                iconLogin.Scale = 0.5;
            }
            else
            {
                iconLogin.Source = "Icon.png";
                iconLogin.Scale = 1;
            }
        }

        // This function presents the user with a organizations menu
        private async void OrganizationsMenu(System.Object sender, System.EventArgs e)
        {
            action = await DisplayActionSheet(null, "Cancel", null, "Serving Now", "Feed The Hungry", "Prep To Your Door");

            if (action.Equals("Cancel"))
            {
                action = "Select Organization";
            }

            organizations.Text = action;
            organizations.TextColor = Color.Black;
        }

        // This function checks that device's network connection, requests data, and parse request's response
        private async void LoadData(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (action.Equals("Feed The Hungry"))
                {
                    await DisplayAlert("Alert", "This organization is not available", "Ok");
                }
                else if (action.Equals("Serving Now"))
                {
                    // Stores user's password
                    string p = password.Text;
                    password.Text = "*****************";
                    
                    if (p.Equals("justdelivered1658") || p.Equals("infinite!") || p.Equals("1234"))
                    {
                        if (NetworkCheck.IsInternet())
                        {
                            // Http client request data from the end point
                            var client = new System.Net.Http.HttpClient();
                            var response = await client.GetAsync("https://wrguk721j7.execute-api.us-west-1.amazonaws.com/dev/api/v1/deliveryRoute");
                            string deliveries = response.Content.ReadAsStringAsync().Result;

                            ServingNowList deliveryList = new ServingNowList();
  
                            if (deliveries != "")
                            {
                                deliveryList = JsonConvert.DeserializeObject<ServingNowList>(deliveries);
                                IList<Elements> sortedRouteList = new List<Elements>();

                                // Double checks that the given data is sorted
                                for (int i = 1; i < deliveryList.result.Count; i++)
                                {
                                    for (int j = 0; j < deliveryList.result.Count; j++)
                                    {
                                        if (deliveryList.result[j].route_id == i)
                                        {
                                            if (!deliveryList.result[j].name.Equals("start") && !deliveryList.result[j].email.Equals("None"))
                                            {
                                                sortedRouteList.Add(deliveryList.result[j]);
                                            }
                                        }
                                    }
                                }

                                deliveryList.result = sortedRouteList;
                                await Application.Current.MainPage.Navigation.PushAsync(new TransitionPage());
                                await Application.Current.MainPage.Navigation.PushAsync(new FourthPage(deliveryList, deliveryList, 1, deliveryList.result.Count));
                            }
                        }
                        else
                        {
                            await DisplayAlert("JSONParsing", "No network is available.", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alert!", "The password you entered is invalid.", "Ok");
                    }
                }
                else if (action.Equals("Prep To Your Door"))
                {
                    await DisplayAlert("Alert", "This organization is not available", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert!", ex.Message, "Ok");
            }
        }

        // This function push a new user page on the application's navigation stack
        public async void NewUserButton(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewUserPage());
        }

        // This function checks that user is using latest version
        public async void CheckLatestVersionInUse()
        {
            var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();

            if (!isLatest)
            {
                await DisplayAlert("Update Required", "Please install the newest version of this app", "Ok");
                await CrossLatestVersion.Current.OpenAppInStore();
            }
        }
    }
}
