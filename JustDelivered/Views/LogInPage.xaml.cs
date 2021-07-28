using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using JustDelivered.Config;
using JustDelivered.LogIn.Apple;
using JustDelivered.LogIn.Classes;
using JustDelivered.Models;
using JustDelivered.Notifications;
using Newtonsoft.Json;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;

using static JustDelivered.Views.SignUpPage;

namespace JustDelivered.Views
{
    public partial class LogInPage : ContentPage
    {
        public event EventHandler SignIn;
        public bool createAccount = false;
        INotifications appleNotification = DependencyService.Get<INotifications>();
        private string deviceId;

        public LogInPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                System.Diagnostics.Debug.WriteLine("Running on Android: Line 32");
                Console.WriteLine("guid: " + Preferences.Get("guid", null));
                appleLogInButton.IsEnabled = false;
            }
            else
            {
                InitializedAppleLogin();
                //appleNotification.IsNotifications();
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                deviceId = Preferences.Get("guid", null);
                if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
            }
            else
            {
                deviceId = Preferences.Get("guid", null);
                if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
            }
        }

        public void InitializedAppleLogin()
        {
            var vm = new AppleLogIn();
            vm.AppleError += AppleError;
            BindingContext = vm;
        }

        public void AppleLogInClick(System.Object sender, System.EventArgs e)
        {
            //SignIn?.Invoke(sender, e);
            //var c = (ImageButton)sender;
            //c.Command?.Execute(c.CommandParameter);
            OnAppleSignInRequest();
        }

        public async void OnAppleSignInRequest()
        {
            try
            {
                //IAppleSignInService appleSignInService = DependencyService.Get<IAppleSignInService>();
                //var account = await appleSignInService.SignInAsync();
                //if (account != null)
                //{
                //    Preferences.Set(App.LoggedInKey, true);
                //    await SecureStorage.SetAsync(App.AppleUserIdKey, account.UserId);

                //    if (account.Token == null) { account.Token = ""; }
                //    if (account.Email != null)
                //    {
                //        if (Application.Current.Properties.ContainsKey(account.UserId.ToString()))
                //        {
                //            //Application.Current.Properties[account.UserId.ToString()] = account.Email;
                //            Debug.WriteLine((string)Application.Current.Properties[account.UserId.ToString()]);
                //        }
                //        else
                //        {
                //            Application.Current.Properties[account.UserId.ToString()] = account.Email;
                //        }
                //    }
                //    if (account.Email == null) { account.Email = ""; }
                //    if (account.Name == null) { account.Name = ""; }

                //    if (Application.Current.Properties.ContainsKey(account.UserId.ToString()))
                //    {
                //        account.Email = (string)Application.Current.Properties[account.UserId.ToString()];
                //        //Application.Current.MainPage = new SelectionPage("", "", null, account, "APPLE");
                //        //var root = (LogInPage)Application.Current.MainPage;
                //        //root.AppleLogIn("", "", null, account, "APPLE");

                //        //var client = new SignIn();
                //        //UserDialogs.Instance.ShowLoading("Retrieving your SF account...");
                //        //var status = await client.VerifyUserCredentials("", "", null, account, "APPLE");
                //        //RedirectUserBasedOnVerification(status, direction);
                //        //AppleUserProfileAsync(account.UserId, account.Token, (string)Application.Current.Properties[account.UserId.ToString()], account.Name);
                //        Application.Current.MainPage = new DeliveriesPage();
                //    }
                //    else
                //    {
                //        var client = new HttpClient();
                //        var getAppleEmail = new AppleEmail();
                //        getAppleEmail.social_id = account.UserId;

                //        var socialLogInPostSerialized = JsonConvert.SerializeObject(getAppleEmail);

                //        System.Diagnostics.Debug.WriteLine(socialLogInPostSerialized);

                //        var postContent = new StringContent(socialLogInPostSerialized, Encoding.UTF8, "application/json");
                //        var RDSResponse = await client.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/AppleEmail", postContent);
                //        var responseContent = await RDSResponse.Content.ReadAsStringAsync();

                //        System.Diagnostics.Debug.WriteLine(responseContent);
                //        if (RDSResponse.IsSuccessStatusCode)
                //        {
                //            var data = JsonConvert.DeserializeObject<AppleUser>(responseContent);
                //            Application.Current.Properties[account.UserId.ToString()] = data.result[0].customer_email;
                //            account.Email = (string)Application.Current.Properties[account.UserId.ToString()];
                //            //var root = (LogInPage)Application.Current.MainPage;
                //            //root.AppleLogIn("", "", null, account, "APPLE");
                //            //Application.Current.MainPage = new SelectionPage("", "", null, account, "APPLE");
                //            //AppleUserProfileAsync(account.UserId, account.Token, (string)Application.Current.Properties[account.UserId.ToString()], account.Name);
                //            Application.Current.MainPage = new DeliveriesPage();
                //            //var client1 = new SignIn();
                //            //UserDialogs.Instance.ShowLoading("Retrieving your SF account...");
                //            //var status = await client1.VerifyUserCredentials("", "", null, account, "APPLE");
                //            //RedirectUserBasedOnVerification(status, direction);
                //        }
                //        else
                //        {
                //            await Application.Current.MainPage.DisplayAlert("Ooops", "Our system is not working. We can't process your request at this moment", "OK");
                //        }
                //    }
                //}
                //else
                //{
                //    //AppleError?.Invoke(this, default(EventArgs));

                //}

                IAppleSignInService appleSignInService = DependencyService.Get<IAppleSignInService>();
                var account = await appleSignInService.SignInAsync();

                if (account != null)
                {
                    Preferences.Set(App.LoggedInKey, true);
                    await SecureStorage.SetAsync(App.AppleUserIdKey, account.UserId);
                    string email = "";
                    if (account.Email != null)
                    {
                        await SecureStorage.SetAsync(account.UserId, account.Email);
                        Application.Current.Properties[account.UserId.ToString()] = account.Email;
                    }
                    else
                    {
                        email = await SecureStorage.GetAsync(account.UserId);

                        if (email == null)
                        {
                            if (Application.Current.Properties.ContainsKey(account.UserId.ToString()))
                            {
                                email = (string)Application.Current.Properties[account.UserId.ToString()];
                            }
                            else
                            {
                                email = "";
                            }
                        }
                        //Debug.WriteLine("EMAIL THAT WAS SAVED: " + email);

                        account.Email = email;

                    }

                    //string url = AppConstants.BaseUrl + AppConstants.addGuid;
                    //Debug.WriteLine("WRITE GUID: " + url);

                    //if (Device.RuntimePlatform == Device.iOS)
                    //{
                    //    deviceId = Preferences.Get("guid", "");
                    //    if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
                    //}
                    //else
                    //{
                    //    deviceId = Preferences.Get("guid", "");
                    //    if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
                    //}
                    //if (deviceId != "")
                    //{
                    //    Application.Current.Properties["guid"] = deviceId.Substring(5);
                    //}
                    //else
                    //{
                    //    Application.Current.Properties["guid"] = "";
                    //}

                    var client = new SignIn();
                    //UserDialogs.Instance.ShowLoading("We are processing your request...");

                    var authenticationStatus = await client.VerifyUserAccount("", "", null, account, "APPLE");

                    Debug.WriteLine("authenticationStatus: " + authenticationStatus);

                    ProcessRequest(authenticationStatus);

                }
            }
            catch (Exception errorAppleSignInRequest)
            {
                //var client = new Diagnostic();
                //client.parseException(errorAppleSignInRequest.ToString(), user);
            }
        }

        public void InvokeSignInEvent(object sender, EventArgs e)
            => SignIn?.Invoke(sender, e);

        private async void AppleError(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "We weren't able to set an account for you", "OK");
        }

        public void InitializeAppProperties()
        {
            Application.Current.Properties["user_email"] = "";
            Application.Current.Properties["user_first_name"] = "";
            Application.Current.Properties["user_last_name"] = "";
            Application.Current.Properties["user_phone_num"] = "";
            Application.Current.Properties["user_address"] = "";
            Application.Current.Properties["user_unit"] = "";
            Application.Current.Properties["user_city"] = "";
            Application.Current.Properties["user_state"] = "";
            Application.Current.Properties["user_zip_code"] = "";
            Application.Current.Properties["user_latitude"] = "";
            Application.Current.Properties["user_longitude"] = "";
            Application.Current.Properties["user_delivery_instructions"] = "";
        }

        void GoogleClick(System.Object sender, System.EventArgs e)
        {
            string clientId = string.Empty;
            string redirectUri = string.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constant.GoogleiOSClientID;
                    redirectUri = Constant.GoogleRedirectUrliOS;
                    break;

                case Device.Android:
                    clientId = Constant.GoogleAndroidClientID;
                    redirectUri = Constant.GoogleRedirectUrlAndroid;
                    break;
            }

            //var authenticator = new OAuth2Authenticator(clientId, string.Empty, Constant.GoogleScope, new Uri(Constant.GoogleAuthorizeUrl), new Uri(redirectUri), new Uri(Constant.GoogleAccessTokenUrl), null, true);
            var authenticator = new OAuth2Authenticator(clientId, string.Empty, Constant.GoogleScope, new Uri(Constant.GoogleAuthorizeUrl), new Uri(redirectUri), new Uri(Constant.GoogleAccessTokenUrl), null, true);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            authenticator.Completed += GoogleAuthenticatorCompleted;
            authenticator.Error += GoogleAuthenticatorError;

            AuthenticationState.Authenticator = authenticator;
            presenter.Login(authenticator);
        }

        public async void GoogleAuthenticatorError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= GoogleAuthenticatorCompleted;
                authenticator.Error -= GoogleAuthenticatorError;
            }

            await DisplayAlert("Authentication error: ", e.Message, "OK");
        }

        public async void GoogleAuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= GoogleAuthenticatorCompleted;
                authenticator.Error -= GoogleAuthenticatorError;
            }

            if (e.IsAuthenticated)
            {
                var client = new SignIn();
                var result = await client.VerifyUserAccount(e.Account.Properties["access_token"], e.Account.Properties["refresh_token"], e, null, "GOOGLE"); 
                if(result != null)
                {
                    ProcessRequest(result);
                }
            }
            else
            {
                Application.Current.MainPage = new LogInPage();
                await DisplayAlert("Error", "Google was not able to autheticate your account", "OK");
            }
        }

        public async void ProcessRequest(string code)
        {
            Debug.WriteLine("LOGIN CODE: " + code);
            if (code == "AUTHENTICATED")
            {
                if (user != null)
                {
                    Application.Current.MainPage = new DeliveriesPage();
                }
            }
            else if(code == "NEED TO SIGN UP")
            {
                //await DisplayAlert("Oops","You don't have an account with Just Delivered. Please sign up!","OK");
                Application.Current.MainPage = new NavigationPage(new SignUpPage());
            }
            else if (code == "WRONG PLATFORM")
            {
                await DisplayAlert("Oops", "It looks like you have an account with Just Delivered, but you tried to log in with the wrong social media account.", "OK");
            }
            else if (code == "LOG IN USING DIRECT LOG IN")
            {
                await DisplayAlert("Oops", "It looks like you have an account with Just Delivered; try logging in using the direct login.", "OK");
            }
            else
            {
                await DisplayAlert("Oops", "We weren't able to process your request. Please try again later.", "OK");
            }
        }

        async void DirectLogInClick(System.Object sender, System.EventArgs e)
        {
            LogInButton.IsEnabled = false;
            if (String.IsNullOrEmpty(userEmailAddress.Text) || String.IsNullOrEmpty(userPassword.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                LogInButton.IsEnabled = true;
            }
            else
            {
                var accountSalt = await RetrieveAccountSalt(userEmailAddress.Text.ToLower().Trim());

                if (accountSalt != null)
                {
                    var loginAttempt = await LogInUser(userEmailAddress.Text.ToLower().Trim(), userPassword.Text, accountSalt);

                    if (loginAttempt != null)
                    {


                        //var client = new HttpClient();
                        //var request = new RequestUserInfo();
                        //request.uid = loginAttempt.result[0].customer_uid;

                        //var requestSelializedObject = JsonConvert.SerializeObject(request);
                        //var requestContent = new StringContent(requestSelializedObject, Encoding.UTF8, "application/json");

                        //var clientRequest = await client.PostAsync(Constant.GetUserInfoUrl, requestContent);

                        //if (clientRequest.IsSuccessStatusCode)
                        //{
                        //    try
                        //    {
                        //        var SFUser = await clientRequest.Content.ReadAsStringAsync();
                        //        Debug.WriteLine("DATA FROM LOGIN ENDPOINT (DIRECT): " + SFUser);



                        //        // needs to implement direct log in...



                        //        //Application.Current.MainPage = new DeliveriesPage("", "", null,null, "");
                        //        //var userData = JsonConvert.DeserializeObject<UserInfo>(SFUser);

                        //        //DateTime today = DateTime.Now;
                        //        //DateTime expDate = today.AddDays(14);

                        //        //Application.Current.Properties["user_id"] = userData.result[0].customer_uid;
                        //        //Application.Current.Properties["time_stamp"] = expDate;
                        //        //Application.Current.Properties["platform"] = "DIRECT";
                        //        //Application.Current.Properties["user_email"] = userData.result[0].customer_email;
                        //        //Application.Current.Properties["user_first_name"] = userData.result[0].customer_first_name;
                        //        //Application.Current.Properties["user_last_name"] = userData.result[0].customer_last_name;
                        //        //Application.Current.Properties["user_phone_num"] = userData.result[0].customer_phone_num;
                        //        //Application.Current.Properties["user_address"] = userData.result[0].customer_address;
                        //        //Application.Current.Properties["user_unit"] = userData.result[0].customer_unit;
                        //        //Application.Current.Properties["user_city"] = userData.result[0].customer_city;
                        //        //Application.Current.Properties["user_state"] = userData.result[0].customer_state;
                        //        //Application.Current.Properties["user_zip_code"] = userData.result[0].customer_zip;
                        //        //Application.Current.Properties["user_latitude"] = userData.result[0].customer_lat;
                        //        //Application.Current.Properties["user_longitude"] = userData.result[0].customer_long;
                        //        //Application.Current.Properties["user_delivery_instructions"] = "";

                        //        //_ = Application.Current.SavePropertiesAsync();

                        //        //if (Device.RuntimePlatform == Device.iOS)
                        //        //{
                        //        //    deviceId = Preferences.Get("guid", null);
                        //        //    if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
                        //        //}
                        //        //else
                        //        //{
                        //        //    deviceId = Preferences.Get("guid", null);
                        //        //    if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
                        //        //}

                        //        //if (deviceId != null)
                        //        //{
                        //        //    NotificationPost notificationPost = new NotificationPost();

                        //        //    notificationPost.uid = (string)Application.Current.Properties["user_id"];
                        //        //    notificationPost.guid = deviceId.Substring(5);
                        //        //    Application.Current.Properties["guid"] = deviceId.Substring(5);
                        //        //    notificationPost.notification = "TRUE";

                        //        //    var notificationSerializedObject = JsonConvert.SerializeObject(notificationPost);
                        //        //    Debug.WriteLine("Notification JSON Object to send: " + notificationSerializedObject);

                        //        //    var notificationContent = new StringContent(notificationSerializedObject, Encoding.UTF8, "application/json");

                        //        //    var clientResponse = await client.PostAsync(Constant.NotificationsUrl, notificationContent);

                        //        //    Debug.WriteLine("Status code: " + clientResponse.IsSuccessStatusCode);

                        //        //    if (clientResponse.IsSuccessStatusCode)
                        //        //    {
                        //        //        System.Diagnostics.Debug.WriteLine("We have post the guid to the database");
                        //        //    }
                        //        //    else
                        //        //    {
                        //        //        await DisplayAlert("Ooops!", "Something went wrong. We are not able to send you notification at this moment", "OK");
                        //        //    }
                        //        //}



                        //        //Application.Current.MainPage = new SelectionPage();
                        //    }
                        //    catch (Exception ex)
                        //    {

                        //        System.Diagnostics.Debug.WriteLine(ex.Message);
                        //    }
                        //}
                        //else
                        //{
                        //    await DisplayAlert("Alert!", "Our internal system was not able to retrieve your user information. We are working to solve this issue.", "OK");
                        //}
                        var result = "";

                        if (loginAttempt.code.ToString() == Constant.EmailNotFound)
                        {
                            // need to sign up
                            userToSignUp = new SignUpAccount();

                            userToSignUp.platform = "DIRECT";
                            

                            result = "NEED TO SIGN UP";
                        }
                        if (loginAttempt.code.ToString() == Constant.AutheticatedSuccesful)
                        {

                            // authenticated
                            result = "AUTHENTICATED";
                            user = new Models.User();
                            user.id = loginAttempt.result[0].driver_uid;
                            user.email = "";
                            user.socialId = "";
                            user.platform = "";
                            user.route_id = "";
                            SaveUser(user);

                        }
                        if (loginAttempt.code.ToString() == Constant.ErrorPlatform)
                        {
                            result = "WRONG PLATFORM";
                            //var RDSCode = JsonConvert.DeserializeObject<RDSLogInMessage>(responseContent);
                            //await DisplayAlert("Message", RDSCode.message, "OK");
                        }

                        if (loginAttempt.code.ToString() == Constant.ErrorUserDirectLogIn)
                        {
                            result = "LOG IN USING DIRECT LOG IN";
                            //await DisplayAlert("Oops!", "You have an existing Serving Fresh account. Please use direct login", "OK");
                        }
                        ProcessRequest(result);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Wrong password was entered", "OK");
                        LogInButton.IsEnabled = true;
                    }
                }
                LogInButton.IsEnabled = true;
            }
        }

        private async Task<AccountSalt> RetrieveAccountSalt(string userEmail)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(userEmail);

                SaltPost saltPost = new SaltPost();
                saltPost.email = userEmail;

                var saltPostSerilizedObject = JsonConvert.SerializeObject(saltPost);
                var saltPostContent = new StringContent(saltPostSerilizedObject, Encoding.UTF8, "application/json");

                System.Diagnostics.Debug.WriteLine(saltPostSerilizedObject);

                var client = new HttpClient();
                var DRSResponse = await client.PostAsync(Constant.AccountSaltUrl, saltPostContent);
                var DRSMessage = await DRSResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(DRSMessage);

                AccountSalt userInformation = null;

                if (DRSResponse.IsSuccessStatusCode)
                {
                    var result = await DRSResponse.Content.ReadAsStringAsync();

                    AcountSaltCredentials data = new AcountSaltCredentials();
                    data = JsonConvert.DeserializeObject<AcountSaltCredentials>(result);

                    if (DRSMessage.Contains(Constant.UseSocialMediaLogin))
                    {
                        createAccount = true;
                        System.Diagnostics.Debug.WriteLine(DRSMessage);
                        await DisplayAlert("Oops!", data.message, "OK");
                    }
                    else if (DRSMessage.Contains(Constant.EmailNotFound))
                    {
                        await DisplayAlert("Oops!", "Our records show that you don't have an accout. Please sign up!", "OK");
                        userToSignUp = new SignUpAccount();
                        userToSignUp.platform = "DIRECT";
                        Application.Current.MainPage = new NavigationPage(new SignUpPage());
                    }
                    else
                    {
                        userInformation = new AccountSalt
                        {
                            password_algorithm = data.result[0].password_algorithm,
                            password_salt = data.result[0].password_salt
                        };
                    }
                }

                return userInformation;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<RDSAuthentication> LogInUser(string userEmail, string userPassword, AccountSalt accountSalt)
        {
            try
            {
                SHA512 sHA512 = new SHA512Managed();
                var client = new HttpClient();
                byte[] data = sHA512.ComputeHash(Encoding.UTF8.GetBytes(userPassword + accountSalt.password_salt));
                string hashedPassword = BitConverter.ToString(data).Replace("-", string.Empty).ToLower();

                LogInPost loginPostContent = new LogInPost();
                loginPostContent.email = userEmail;
                loginPostContent.password = hashedPassword;

                loginPostContent.social_id = "";
                loginPostContent.signup_platform = "";

                string loginPostContentJson = JsonConvert.SerializeObject(loginPostContent);

                var httpContent = new StringContent(loginPostContentJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Constant.LogInUrl, httpContent);
                var message = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(message);

                if (message.Contains(Constant.AutheticatedSuccesful))
                {

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<RDSAuthentication>(responseContent);
                    return loginResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception message: " + e.Message);
                return null;
            }
        }

        void FacebookLogInClick(System.Object sender, System.EventArgs e)
        {
            string clientID = string.Empty;
            string redirectURL = string.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientID = Constant.FacebookiOSClientID;
                    redirectURL = Constant.FacebookiOSRedirectUrl;
                    break;
                case Device.Android:
                    clientID = Constant.FacebookAndroidClientID;
                    redirectURL = Constant.FacebookAndroidRedirectUrl;
                    break;
            }

            var authenticator = new OAuth2Authenticator(clientID, Constant.FacebookScope, new Uri(Constant.FacebookAuthorizeUrl), new Uri(redirectURL), null, false);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            authenticator.Completed += FacebookAuthenticatorCompletedAsync;
            authenticator.Error += FacebookAutheticatorError;

            presenter.Login(authenticator);
        }

        void SaveUser(Models.User user)
        {
            string account = JsonConvert.SerializeObject(user);

            if (Application.Current.Properties.Keys.Contains(Constant.Autheticator))
            {
                Application.Current.Properties[Constant.Autheticator] = account;
            }
            else
            {
                Application.Current.Properties.Add(Constant.Autheticator, account);
            }

            Application.Current.SavePropertiesAsync();
        }

        public async void FacebookAuthenticatorCompletedAsync(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= FacebookAuthenticatorCompletedAsync;
                authenticator.Error -= FacebookAutheticatorError;
            }

            if (e.IsAuthenticated)
            {
                //FacebookUserProfileAsync(e.Account.Properties["access_token"]);
                //Application.Current.MainPage = new DeliveriesPage(e.Account.Properties["access_token"], "", null, null, "FACEBOOK");

                var client = new SignIn();
                var result = await client.VerifyUserAccount(e.Account.Properties["access_token"], "", null, null, "FACEBOOK");
                if (result != null)
                {
                    ProcessRequest(result);
                }
            }
            else
            {
                Application.Current.MainPage = new LogInPage();
                await DisplayAlert("Error", "Facebook was not able to autheticate your account", "OK");
            }
        }

        private async void FacebookAutheticatorError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= FacebookAuthenticatorCompletedAsync;
                authenticator.Error -= FacebookAutheticatorError;
            }

            await DisplayAlert("Authentication error: ", e.Message, "OK");
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SignUpPage());
        }
    }
}
