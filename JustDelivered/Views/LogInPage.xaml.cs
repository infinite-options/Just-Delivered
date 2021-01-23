using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.LogIn.Classes;
using Newtonsoft.Json;
using Xamarin.Auth;
using Xamarin.Forms;

namespace JustDelivered.Views
{
    public partial class LogInPage : ContentPage
    {

        public event EventHandler SignIn;
        public bool createAccount = false;
        private string deviceId;
        public LogInPage()
        {
            InitializeComponent();
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
                Application.Current.MainPage = new DeliveriesPage(e.Account.Properties["access_token"], e.Account.Properties["refresh_token"], e);
            }
            else
            {
                Application.Current.MainPage = new LogInPage();
                await DisplayAlert("Error", "Google was not able to autheticate your account", "OK");
            }
        }

        public async void GoogleUserProfileAsync(string accessToken, string refreshToken, AuthenticatorCompletedEventArgs e)
        {
            try
            {
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
                socialLogInPost.delivery_date = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd 00:00:00");
                socialLogInPost.signup_platform = "GOOGLE";

                var socialLogInPostSerialized = JsonConvert.SerializeObject(socialLogInPost);
                var postContent = new StringContent(socialLogInPostSerialized, Encoding.UTF8, "application/json");

                Debug.WriteLine(socialLogInPostSerialized);

                var RDSResponse = await client.PostAsync(Constant.LogInUrl, postContent);
                var responseContent = await RDSResponse.Content.ReadAsStringAsync();

                Debug.WriteLine(responseContent);

                Debug.WriteLine(RDSResponse.IsSuccessStatusCode);
                //Application.Current.MainPage = new DeliveriesPage();
                if (RDSResponse.IsSuccessStatusCode)
                {
                    if (responseContent != null)
                    {
                        if (responseContent.Contains(Constant.EmailNotFound))
                        {
                            var signUp = await DisplayAlert("Message", "It looks like you don't have a Serving Fresh account. Please sign up!", "OK", "Cancel");
                            if (signUp)
                            {
                                //Application.Current.MainPage = new SocialSignUp(googleData.id, googleData.given_name, googleData.family_name, googleData.email, accessToken, refreshToken, "GOOGLE");
                            }
                        }
                        if (responseContent.Contains(Constant.AutheticatedSuccesful))
                        {
                            //Application.Current.MainPage = new DeliveriesPage();
                            //try
                            //{
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
            catch (Exception first)
            {
                Debug.WriteLine(first.Message);
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

                    if (loginAttempt != null && loginAttempt.message != "Request failed, wrong password.")
                    {
                        var client = new HttpClient();
                        var request = new RequestUserInfo();
                        request.uid = loginAttempt.result[0].customer_uid;

                        var requestSelializedObject = JsonConvert.SerializeObject(request);
                        var requestContent = new StringContent(requestSelializedObject, Encoding.UTF8, "application/json");

                        var clientRequest = await client.PostAsync(Constant.GetUserInfoUrl, requestContent);

                        if (clientRequest.IsSuccessStatusCode)
                        {
                            try
                            {
                                var SFUser = await clientRequest.Content.ReadAsStringAsync();
                                Debug.WriteLine("DATA FROM LOGIN ENDPOINT (DIRECT): " + SFUser);
                                //var userData = JsonConvert.DeserializeObject<UserInfo>(SFUser);

                                //DateTime today = DateTime.Now;
                                //DateTime expDate = today.AddDays(14);

                                //Application.Current.Properties["user_id"] = userData.result[0].customer_uid;
                                //Application.Current.Properties["time_stamp"] = expDate;
                                //Application.Current.Properties["platform"] = "DIRECT";
                                //Application.Current.Properties["user_email"] = userData.result[0].customer_email;
                                //Application.Current.Properties["user_first_name"] = userData.result[0].customer_first_name;
                                //Application.Current.Properties["user_last_name"] = userData.result[0].customer_last_name;
                                //Application.Current.Properties["user_phone_num"] = userData.result[0].customer_phone_num;
                                //Application.Current.Properties["user_address"] = userData.result[0].customer_address;
                                //Application.Current.Properties["user_unit"] = userData.result[0].customer_unit;
                                //Application.Current.Properties["user_city"] = userData.result[0].customer_city;
                                //Application.Current.Properties["user_state"] = userData.result[0].customer_state;
                                //Application.Current.Properties["user_zip_code"] = userData.result[0].customer_zip;
                                //Application.Current.Properties["user_latitude"] = userData.result[0].customer_lat;
                                //Application.Current.Properties["user_longitude"] = userData.result[0].customer_long;
                                //Application.Current.Properties["user_delivery_instructions"] = "";

                                //_ = Application.Current.SavePropertiesAsync();

                                //if (Device.RuntimePlatform == Device.iOS)
                                //{
                                //    deviceId = Preferences.Get("guid", null);
                                //    if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
                                //}
                                //else
                                //{
                                //    deviceId = Preferences.Get("guid", null);
                                //    if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
                                //}

                                //if (deviceId != null)
                                //{
                                //    NotificationPost notificationPost = new NotificationPost();

                                //    notificationPost.uid = (string)Application.Current.Properties["user_id"];
                                //    notificationPost.guid = deviceId.Substring(5);
                                //    Application.Current.Properties["guid"] = deviceId.Substring(5);
                                //    notificationPost.notification = "TRUE";

                                //    var notificationSerializedObject = JsonConvert.SerializeObject(notificationPost);
                                //    Debug.WriteLine("Notification JSON Object to send: " + notificationSerializedObject);

                                //    var notificationContent = new StringContent(notificationSerializedObject, Encoding.UTF8, "application/json");

                                //    var clientResponse = await client.PostAsync(Constant.NotificationsUrl, notificationContent);

                                //    Debug.WriteLine("Status code: " + clientResponse.IsSuccessStatusCode);

                                //    if (clientResponse.IsSuccessStatusCode)
                                //    {
                                //        System.Diagnostics.Debug.WriteLine("We have post the guid to the database");
                                //    }
                                //    else
                                //    {
                                //        await DisplayAlert("Ooops!", "Something went wrong. We are not able to send you notification at this moment", "OK");
                                //    }
                                //}



                                //Application.Current.MainPage = new SelectionPage();
                            }
                            catch (Exception ex)
                            {

                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert!", "Our internal system was not able to retrieve your user information. We are working to solve this issue.", "OK");
                        }
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

        public async Task<LogInResponse> LogInUser(string userEmail, string userPassword, AccountSalt accountSalt)
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
                    var loginResponse = JsonConvert.DeserializeObject<LogInResponse>(responseContent);
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

    }
}
