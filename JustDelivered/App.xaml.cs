using System;
using System.Collections.Generic;
using System.Diagnostics;
using JustDelivered.Config;
using JustDelivered.LogIn.Apple;
using JustDelivered.Models;
using JustDelivered.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static JustDelivered.Views.DeliveriesPage;
using static JustDelivered.Views.ProductsPage;

namespace JustDelivered
{
    public partial class App : Application
    {
        public const string LoggedInKey = "LoggedIn";
        public const string AppleUserIdKey = "AppleUserIdKey";
        string userId;

        public App()
        {
            InitializeComponent();

            if (Application.Current.Properties.Keys.Contains(Constant.Autheticator))
            {
                var tempUser = JsonConvert.DeserializeObject<User>(Current.Properties[Constant.Autheticator].ToString());

                if(tempUser.id != "")
                {
                    DateTime today = DateTime.Now;
                    var expTime = tempUser.sessionTime;

                    if (today <= expTime)
                    {
                        SetUser(tempUser);
                        MainPage = new DeliveriesPage();
                    }
                    else
                    {
                        MainPage = new NavigationPage(new LogInPage());

                        //string socialPlatform = tempUser.getUserPlatform();

                        //if (socialPlatform.Equals(Constant.Facebook))
                        //{
                        //    Application.Current.MainPage.Navigation.PushModalAsync(new LogInPage(Constant.Facebook));
                        //}
                        //else if (socialPlatform.Equals(Constant.Google))
                        //{
                        //    Application.Current.MainPage.Navigation.PushModalAsync(new LogInPage(Constant.Google));
                        //}
                        //else if (socialPlatform.Equals(Constant.Apple))
                        //{
                        //    Application.Current.MainPage.Navigation.PushModalAsync(new LogInPage(Constant.Apple));
                        //}
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new LogInPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new LogInPage());
            }
        }

        protected override async void OnStart()
        {
            var appleSignInService = DependencyService.Get<IAppleSignInService>();

            if (appleSignInService != null)
            {
                userId = await SecureStorage.GetAsync(AppleUserIdKey);
                if (appleSignInService.IsAvailable && !string.IsNullOrEmpty(userId))
                {
                    var credentialState = await appleSignInService.GetCredentialStateAsync(userId);
                    switch (credentialState)
                    {
                        case AppleSignInCredentialState.Authorized:
                            break;
                        case AppleSignInCredentialState.NotFound:
                        case AppleSignInCredentialState.Revoked:
                            //Logout;
                            SecureStorage.Remove(AppleUserIdKey);
                            Preferences.Set(LoggedInKey, false);
                            MainPage = new LogInPage();
                            break;
                    }
                }
            }
        }

        void SetUser(User temp)
        {
            DateTime today = DateTime.Now;
            DateTime expDate = today.AddDays(Constant.days);

            user = new User();
            user.id = temp.id;
            user.sessionTime = expDate;
            user.email = "";
            user.socialId = "";
            user.platform = "";
            user.route_id = "";
        }

        protected override void OnSleep()
        {
            //Debug.WriteLine("WHEN APP IS CLOSE BUT RUNNING ON BACKGROUND");
            //try
            //{
            //    var client = new UpdateRoutes();
            //    if (list.Count > 0 && user != null)
            //    {
            //        Debug.WriteLine("user.route_id: " + user.route_id);
            //        if(user.route_id != "")
            //        {
            //            client.UpdateDeliveryStatus(user.route_id, list);
            //        }
            //    }
            //}
            //catch
            //{

            //}

            try
            {
                if (routeID != "")
                {
                    if (isProductionSave != null && isProductionSave != "TRUE")
                    {
                        UpdateSavedProductsWhenClosingApp();
                    }
                }
            }
            catch(Exception issueOnSleep)
            {
                Current.MainPage.DisplayAlert("Oops", issueOnSleep.Message, "OK");
            }
        }

        protected override void OnResume()
        {
            
        }
    }
}
