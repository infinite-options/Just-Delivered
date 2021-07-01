using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.LogIn.Apple;
using JustDelivered.LogIn.Classes;
using JustDelivered.Views;
using Newtonsoft.Json;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;
using static JustDelivered.Views.SignUpPage;

namespace JustDelivered.Models
{
    public class SignIn
    {
        private object deviceId;

        public SignIn()
        {
        }

        //public async Task<string> VerifyUserCredentials(string accessToken = "", string refreshToken = "", AuthenticatorCompletedEventArgs googleAccount = null, AppleAccount appleCredentials = null, string platform = "")
        //{
        //    var isUserVerified = "";
        //    try
        //    {
        //        //var progress = UserDialogs.Instance.Loading("Loading...");
        //        var client = new HttpClient();
        //        var socialLogInPost = new SocialLogInPost();

        //        var googleData = new GoogleResponse();
        //        var facebookData = new FacebookResponse();

        //        if (platform == "GOOGLE")
        //        {
        //            var request = new OAuth2Request("GET", new Uri(Constant.GoogleUserInfoUrl), null, googleAccount.Account);
        //            var GoogleResponse = await request.GetResponseAsync();
        //            var googelUserData = GoogleResponse.GetResponseText();

        //            googleData = JsonConvert.DeserializeObject<GoogleResponse>(googelUserData);

        //            socialLogInPost.email = googleData.email;
        //            socialLogInPost.social_id = googleData.id;
        //            Debug.WriteLine("IMAGE: " + googleData.picture);
        //            //user.setUserImage(googleData.picture);
        //        }
        //        else if (platform == "FACEBOOK")
        //        {
        //            var facebookResponse = client.GetStringAsync(Constant.FacebookUserInfoUrl + accessToken);
        //            var facebookUserData = facebookResponse.Result;

        //            Debug.WriteLine("FACEBOOK DATA: " + facebookUserData);
        //            facebookData = JsonConvert.DeserializeObject<FacebookResponse>(facebookUserData);

        //            socialLogInPost.email = facebookData.email;
        //            socialLogInPost.social_id = facebookData.id;
        //        }
        //        else if (platform == "APPLE")
        //        {
        //            socialLogInPost.email = appleCredentials.Email;
        //            socialLogInPost.social_id = appleCredentials.UserId;
        //        }

        //        socialLogInPost.password = "";
        //        socialLogInPost.signup_platform = platform;

        //        var socialLogInPostSerialized = JsonConvert.SerializeObject(socialLogInPost);
        //        var postContent = new StringContent(socialLogInPostSerialized, Encoding.UTF8, "application/json");

        //        //var test = UserDialogs.Instance.Loading("Loading...");
        //        var RDSResponse = await client.PostAsync(Constant.LogInUrl, postContent);
        //        var responseContent = await RDSResponse.Content.ReadAsStringAsync();
        //        var authetication = JsonConvert.DeserializeObject<SuccessfulSocialLogIn>(responseContent);
        //        if (RDSResponse.IsSuccessStatusCode)
        //        {
        //            if (responseContent != null)
        //            {
        //                if (authetication.code.ToString() == Constant.EmailNotFound)
        //                {
        //                    //test.Hide();
        //                    isUserVerified = "USER NEEDS TO SIGN UP";
        //                    //if (platform == "GOOGLE")
        //                    //{
        //                    //    Application.Current.MainPage = new SocialSignUp(googleData.id, googleData.given_name, googleData.family_name, googleData.email, accessToken, refreshToken, "GOOGLE");
        //                    //}
        //                    //else if (platform == "FACEBOOK")
        //                    //{
        //                    //    Application.Current.MainPage = new SocialSignUp(facebookData.id, facebookData.name, "", facebookData.email, accessToken, accessToken, "FACEBOOK");
        //                    //}
        //                    //else if (platform == "APPLE")
        //                    //{
        //                    //    Application.Current.MainPage = new SocialSignUp(appleCredentials.UserId, appleCredentials.Name, "", appleCredentials.Email, appleCredentials.Token, appleCredentials.Token, "APPLE");
        //                    //}
        //                }
        //                if (authetication.code.ToString() == Constant.AutheticatedSuccesful)
        //                {
        //                    try
        //                    {
        //                        var data = JsonConvert.DeserializeObject<SuccessfulSocialLogIn>(responseContent);
        //                        //user.setUserID(data.result[0].customer_uid);

        //                        UpdateTokensPost updateTokesPost = new UpdateTokensPost();
        //                        updateTokesPost.uid = data.result[0].customer_uid;
        //                        if (platform == "GOOGLE")
        //                        {
        //                            updateTokesPost.mobile_access_token = accessToken;
        //                            updateTokesPost.mobile_refresh_token = refreshToken;
        //                        }
        //                        else if (platform == "FACEBOOK")
        //                        {
        //                            updateTokesPost.mobile_access_token = accessToken;
        //                            updateTokesPost.mobile_refresh_token = accessToken;
        //                        }
        //                        else if (platform == "APPLE")
        //                        {
        //                            updateTokesPost.mobile_access_token = appleCredentials.Token;
        //                            updateTokesPost.mobile_refresh_token = appleCredentials.Token;
        //                        }

        //                        var updateTokesPostSerializedObject = JsonConvert.SerializeObject(updateTokesPost);
        //                        var updateTokesContent = new StringContent(updateTokesPostSerializedObject, Encoding.UTF8, "application/json");
        //                        var updateTokesResponse = await client.PostAsync(Constant.UpdateTokensUrl, updateTokesContent);
        //                        var updateTokenResponseContent = await updateTokesResponse.Content.ReadAsStringAsync();

        //                        if (updateTokesResponse.IsSuccessStatusCode)
        //                        {
        //                            var user1 = new RequestUserInfo();
        //                            user1.uid = data.result[0].customer_uid;

        //                            var requestSelializedObject = JsonConvert.SerializeObject(user1);
        //                            var requestContent = new StringContent(requestSelializedObject, Encoding.UTF8, "application/json");

        //                            var clientRequest = await client.PostAsync(Constant.GetUserInfoUrl, requestContent);

        //                            if (clientRequest.IsSuccessStatusCode)
        //                            {
        //                                var userSfJSON = await clientRequest.Content.ReadAsStringAsync();
        //                                var userProfile = JsonConvert.DeserializeObject<UserInfo>(userSfJSON);

        //                                DateTime today = DateTime.Now;
        //                                //DateTime expDate = today.AddDays(Constant.days);
        //                                DateTime expDate = today.AddDays(16);

        //                                //user.setUserID(data.result[0].customer_uid);
        //                                //user.setUserSessionTime(expDate);
        //                                //user.setUserPlatform(platform);
        //                                //user.setUserType("CUSTOMER");
        //                                //user.setUserEmail(userProfile.result[0].customer_email);
        //                                //user.setUserFirstName(userProfile.result[0].customer_first_name);
        //                                //user.setUserLastName(userProfile.result[0].customer_last_name);
        //                                //user.setUserPhoneNumber(userProfile.result[0].customer_phone_num);
        //                                //user.setUserAddress(userProfile.result[0].customer_address);
        //                                //user.setUserUnit(userProfile.result[0].customer_unit);
        //                                //user.setUserCity(userProfile.result[0].customer_city);
        //                                //user.setUserState(userProfile.result[0].customer_state);
        //                                //user.setUserZipcode(userProfile.result[0].customer_zip);
        //                                //user.setUserLatitude(userProfile.result[0].customer_lat);
        //                                //user.setUserLongitude(userProfile.result[0].customer_long);

        //                                //SaveUser(user);

        //                                //if (data.result[0].role == "GUEST")
        //                                //{
        //                                //    var clientSignUp = new SignUp();
        //                                //    var content = clientSignUp.UpdateSocialUser(user, userProfile.result[0].mobile_access_token, userProfile.result[0].mobile_refresh_token, userProfile.result[0].social_id, platform);
        //                                //    var signUpStatus = await SignUp.SignUpNewUser(content);
        //                                //}

        //                                isUserVerified = "LOGIN USER";

        //                                //SetMenu(guestMenuSection, customerMenuSection, historyLabel, profileLabel);
        //                                //GetBusinesses();
        //                                //if (Device.RuntimePlatform == Device.iOS)
        //                                //{
        //                                //    deviceId = Preferences.Get("guid", null);
        //                                //    if (deviceId != null) { Debug.WriteLine("This is the iOS GUID from Log in: " + deviceId); }
        //                                //}
        //                                //else
        //                                //{
        //                                //    deviceId = Preferences.Get("guid", null);
        //                                //    if (deviceId != null) { Debug.WriteLine("This is the Android GUID from Log in " + deviceId); }
        //                                //}

        //                                //if (deviceId != null)
        //                                //{
        //                                //    NotificationPost notificationPost = new NotificationPost();

        //                                //    notificationPost.uid = user.getUserID();
        //                                //    notificationPost.guid = deviceId.Substring(5);
        //                                //    user.setUserDeviceID(deviceId.Substring(5));
        //                                //    notificationPost.notification = "TRUE";

        //                                //    var notificationSerializedObject = JsonConvert.SerializeObject(notificationPost);
        //                                //    Debug.WriteLine("Notification JSON Object to send: " + notificationSerializedObject);

        //                                //    var notificationContent = new StringContent(notificationSerializedObject, Encoding.UTF8, "application/json");

        //                                //    var clientResponse = await client.PostAsync(Constant.NotificationsUrl, notificationContent);

        //                                //    Debug.WriteLine("Status code: " + clientResponse.IsSuccessStatusCode);

        //                                //    if (clientResponse.IsSuccessStatusCode)
        //                                //    {
        //                                //        Debug.WriteLine("We have post the guid to the database");
        //                                //    }
        //                                //    else
        //                                //    {
        //                                //        //await DisplayAlert("Ooops!", "Something went wrong. We are not able to send you notification at this moment", "OK");
        //                                //    }
        //                                //}
        //                                //test.Hide();
        //                                //Application.Current.MainPage = new SelectionPage();
        //                            }
        //                            else
        //                            {
        //                                isUserVerified = "ERROR1";
        //                                //test.Hide();
        //                                //await DisplayAlert("Alert!", "Our internal system was not able to retrieve your user information. We are working to solve this issue.", "OK");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            isUserVerified = "ERROR2";
        //                            //test.Hide();
        //                            //await DisplayAlert("Oops", "We are facing some problems with our internal system. We weren't able to update your credentials", "OK");
        //                        }
        //                        //test.Hide();
        //                    }
        //                    catch (Exception second)
        //                    {
        //                        Debug.WriteLine(second.Message);
        //                    }
        //                }
        //                if (authetication.code.ToString() == Constant.ErrorPlatform)
        //                {
        //                    // need RDSLogIn
        //                    //var RDSCode = JsonConvert.DeserializeObject<RDSLogInMessage>(responseContent);
        //                    //if(RDSCode.message != null && RDSCode.message == "")
        //                    //{
        //                    //    Debug.WriteLine("DATA FROM LOGIN WHEN USING WRONG PLATFORM: " + responseContent);
        //                    //    isUserVerified = "PLEASE SIGN IN THROUGH";
        //                    //}
        //                    //else
        //                    //{
        //                    //    isUserVerified = "SIGN IN USING SOCIAL MEDIA";
        //                    //}

        //                    isUserVerified = "WRONG SOCIAL MEDIA TO SIGN IN";

        //                    //test.Hide();
        //                    //Application.Current.MainPage = new LogInPage();
        //                }

        //                if (authetication.code.ToString() == Constant.ErrorUserDirectLogIn)
        //                {
        //                    isUserVerified = "SIGN IN DIRECTLY";
        //                    //test.Hide();
        //                    //Application.Current.MainPage = new LogInPage();
        //                }
        //            }
        //        }
        //        //test.Hide();
        //        return isUserVerified;
        //    }
        //    catch (Exception errorVerifyUserCredentials)
        //    {

        //        //var client = new Diagnostic();
        //        //client.parseException(errorVerifyUserCredentials.ToString(), user);
        //        isUserVerified = "ERROR";
        //        return isUserVerified;
        //    }
        //}


        public async Task<string> VerifyUserAccount(string accessToken = "", string refreshToken = "", AuthenticatorCompletedEventArgs e = null, AppleAccount account = null, string platform = "")
        {
            string result = null;
            try
            {
                var client = new HttpClient();
                var socialLogInPost = new SocialLogInPost();
                string dataString = "";
                if (platform == "GOOGLE")
                {
                    Debug.WriteLine("IN SIDE GOOGLE PROFILE");

                    var request = new OAuth2Request("GET", new Uri(Constant.GoogleUserInfoUrl), null, e.Account);
                    var GoogleResponse = await request.GetResponseAsync();
                    var userData = await GoogleResponse.GetResponseTextAsync();
                    dataString = userData;
                    Debug.WriteLine(userData);

                    GoogleResponse googleData = JsonConvert.DeserializeObject<GoogleResponse>(userData);
                    //LIVE
                    socialLogInPost.email = googleData.email;
                    socialLogInPost.social_id = googleData.id;

                    //TEST
                    //socialLogInPost.email = "pmarathay@gmail.com";
                    //socialLogInPost.social_id = "117240672996349246664";

                }
                else if (platform == "FACEBOOK")
                {
                    var facebookResponse = client.GetStringAsync(Constant.FacebookUserInfoUrl + accessToken);
                    var userData = facebookResponse.Result;
                    dataString = userData;
                    Debug.WriteLine(userData);

                    FacebookResponse facebookData = JsonConvert.DeserializeObject<FacebookResponse>(userData);
                    socialLogInPost.email = facebookData.email;
                    socialLogInPost.social_id = facebookData.id;

                    //socialLogInPost.email = "stampshailey@gmail.com";
                    //socialLogInPost.social_id = "1642693409247274";
                }
                else if (platform == "APPLE")
                {
                    socialLogInPost.email = account.Email;
                    socialLogInPost.social_id = account.UserId;
                }

                //socialLogInPost.delivery_date = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                var currentDate = DateTime.Now;

                for (int i = 0; i < 7; i++)
                {
                    if (currentDate.DayOfWeek == DayOfWeek.Wednesday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        break;
                    }
                    currentDate = currentDate.AddDays(1);
                }

                Debug.WriteLine("Current Date: " + currentDate.ToString("yyyy-MM-dd 10:00:00"));

                socialLogInPost.password = "";
                //socialLogInPost.delivery_date = currentDate.ToString("yyyy-MM-dd 10:00:00");
                //socialLogInPost.delivery_date = "2021-06-06 10:00:00";

                socialLogInPost.signup_platform = platform;

                var socialLogInPostSerialized = JsonConvert.SerializeObject(socialLogInPost);

                Debug.WriteLine("JSON:" + socialLogInPostSerialized);

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
                        var message = JsonConvert.DeserializeObject<RDSAuthentication>(responseContent);
                        if (message.code.ToString() == Constant.EmailNotFound)
                        {
                            // need to sign up
                            userToSignUp = new SignUpAccount();
                           
                            if (platform == "GOOGLE")
                            {
                                GoogleResponse googleData = JsonConvert.DeserializeObject<GoogleResponse>(dataString);
                                userToSignUp.socialID = googleData.id;
                                userToSignUp.socialEmail = googleData.email;
                                userToSignUp.firstName = googleData.given_name;
                                userToSignUp.lastName = googleData.family_name;
                                userToSignUp.accessToken = accessToken;
                                userToSignUp.refreshToken = refreshToken;
                                userToSignUp.platform = platform;
                            }
                            else if (platform == "FACEBOOK")
                            {
                                FacebookResponse facebookData = JsonConvert.DeserializeObject<FacebookResponse>(dataString);
                                userToSignUp.socialID = facebookData.id;
                                userToSignUp.socialEmail = facebookData.email;
                                userToSignUp.firstName = facebookData.name;
                                userToSignUp.accessToken = accessToken;
                                userToSignUp.refreshToken = refreshToken;
                                userToSignUp.platform = platform;
                            }
                            else if (platform == "APPLE")
                            {
                                userToSignUp.socialID = account.UserId;
                                userToSignUp.socialEmail = account.Email;
                                userToSignUp.firstName = account.Name;
                                userToSignUp.accessToken = account.Token;
                                userToSignUp.refreshToken = account.Token;
                                userToSignUp.platform = platform;
                            }
                            else
                            {
                                userToSignUp.platform = "DIRECT";
                            }

                            result = "NEED TO SIGN UP";
                        }
                        if (message.code.ToString() == Constant.AutheticatedSuccesful)
                        {

                            // authenticated
                            result = "AUTHENTICATED";
                            user = new User();
                            user.id = message.result[0].driver_uid;
                            user.email = "";
                            user.socialId = "";
                            user.platform = "";
                            user.route_id = "";
                            SaveUser(user);

                        }
                        if (message.code.ToString() == Constant.ErrorPlatform)
                        {
                            result = "WRONG PLATFORM";
                            //var RDSCode = JsonConvert.DeserializeObject<RDSLogInMessage>(responseContent);
                            //await DisplayAlert("Message", RDSCode.message, "OK");
                        }

                        if (message.code.ToString() == Constant.ErrorUserDirectLogIn)
                        {
                            result = "LOG IN USING DIRECT LOG IN";
                            //await DisplayAlert("Oops!", "You have an existing Serving Fresh account. Please use direct login", "OK");
                        }
                    }
                }
                else
                {
                    result = "ENDPOINT CALL WAS NOT SUCCESSFUL";
                    //MyStack.IsEnabled = false;
                    //await DisplayAlert("Oops", "Our system shows that there are no deliveries available for you at this moment. Please check again later.", "OK");
                }
            }
            catch (Exception g)
            {
                result = "ERROR";
                //Debug.WriteLine("IN side " + g.Message);
                //MyStack.IsEnabled = false;
                //await DisplayAlert("Oops", "Our system shows that there are no deliveries available for you at this moment. Please check again later.", "OK");
            }
            return result;
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
    }
}
