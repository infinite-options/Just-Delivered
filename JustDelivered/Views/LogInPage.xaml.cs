using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using JustDelivered.Config;
using JustDelivered.Interfaces;
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
        public string direction = "";

        public LogInPage()
        {
            InitializeComponent();
            SetAppVersion(versionAndBuild);
            InitializeAppProperties();
            SetAppleContinueButtonBaseOnPlatform();
        }

        public LogInPage(string platform)
        {
            if (platform == Constant.Facebook)
            {
                FacebookLogInClick(new System.Object(), new EventArgs());
            }
            else if (platform == Constant.Google)
            {
                GoogleLogInClick(new System.Object(), new EventArgs());
            }
            else if (platform == Constant.Apple)
            {
                AppleLogInClick(new System.Object(), new EventArgs());
            }
        }

        void SetAppleContinueButtonBaseOnPlatform()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                appleLogInButton.IsVisible = false;
            }
        }

        // MAIN LOGIN EVENT HANDLERS _________________________________________________

        // This function handles the direct login request. It first retrives the salt account, then it
        // verifies the credentails to find out if user gets access or not. This function calls functions
        // in the sign in class and uses their responses to find out what action to take at this level.

        public async void DirectLogInClick(System.Object sender, System.EventArgs e)
        {
            LogInButton.IsEnabled = false;

            try
            {
                if (String.IsNullOrEmpty(userEmailAddress.Text) || String.IsNullOrEmpty(userPassword.Text))
                {
                    await DisplayAlert("Error", "Please fill in all fields", "OK");
                }
                else
                {
                    var signInClient = new SignIn();

                    var accountSalt = await signInClient.RetrieveAccountSalt(userEmailAddress.Text.ToLower().Trim());

                    if (accountSalt != null)
                    {
                        if (accountSalt.password_algorithm == null && accountSalt.password_salt == null && accountSalt.message != "USER NEEDS TO SIGN UP")
                        {
                            await DisplayAlert("Oops", accountSalt.message, "OK");
                        }
                        else if (accountSalt.password_algorithm == null && accountSalt.password_salt == null && accountSalt.message == "USER NEEDS TO SIGN UP")
                        {
                            userToSignUp.password = userPassword.Text.Trim();
                            UserDialogs.Instance.ShowLoading("Retrieving your JD account...");
                            RedirectUserBasedOnVerification(accountSalt.message, direction);
                        }
                        else if (accountSalt.password_algorithm != null && accountSalt.password_salt != null && accountSalt.message == null)
                        {
                            var status = await signInClient.VerifyUserCredentials(userEmailAddress.Text.ToLower().Trim(), userPassword.Text, accountSalt);
                            UserDialogs.Instance.ShowLoading("Retrieving your JD account...");
                            RedirectUserBasedOnVerification(status, direction);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alert!", "Our internal system was not able to retrieve your user information. We are working to solve this issue.", "OK");
                    }
                }
            }
            catch (Exception errorDirectLogIn)
            {
                //var client = new Diagnostic();
                //client.parseException(errorDirectLogIn.ToString(), user);

                Debug.WriteLine("ERROR IN 'DirectLogInClick' :" + errorDirectLogIn.Message);
            }

            LogInButton.IsEnabled = true;
        }

        // This function handles the Facebook login request. It calls the FacebookAutheticationCompleted function
        // to find out if user login successfully or not. Note that if the LoginPage is push modally into the navigation
        // stack you have to pop modally again to present the Facebook login screen. 

        public void FacebookLogInClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();

            var client = new SignIn();
            var authenticator = client.GetFacebookAuthetication();
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            authenticator.Completed += FacebookAuthenticatorCompleted;
            authenticator.Error += FacebookAutheticatorError;
            presenter.Login(authenticator);
        }

        // This function handles the Google login request. It calls the GoogleAutheticationCompleted function
        // to find out if user login successfully or not. Note that if the LoginPage is push modally into the navigation
        // stack you have to pop modally again to present the Google login screen. Also, in comparison with the Facebook or Apple
        // login mechanism Google needs to set up a the state of the autheticator to know when to redirect the user back to the app.

        public void GoogleLogInClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();

            var client = new SignIn();
            var authenticator = client.GetGoogleAuthetication();
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            AuthenticationState.Authenticator = authenticator;

            authenticator.Completed += GoogleAuthenticatorCompleted;
            authenticator.Error += GoogleAuthenticatorError;
            presenter.Login(authenticator);
        }

        // This function handles the Apple login request. It calls the OnAppleSignInRequest function
        // to find out if user login successfully or not. 

        public void AppleLogInClick(System.Object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform != Device.Android)
            {
                OnAppleSignInRequest();
            }
        }

        // AUTHETICATION HANDLERS _________________________________________________

        // This function checks if the user was able to succesfully login to their Facebook account. Once the
        // user autheticates throught Facebook, we validate their credentials in our system.

        async void FacebookAuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= FacebookAuthenticatorCompleted;
                authenticator.Error -= FacebookAutheticatorError;
            }

            if (e.IsAuthenticated)
            {
                try
                {
                    var client = new SignIn();
                    UserDialogs.Instance.ShowLoading("Retrieving your JD account...");
                    var status = await client.VerifyUserCredentials(e.Account.Properties["access_token"], "", null, null, "FACEBOOK");
                    RedirectUserBasedOnVerification(status, direction);
                }
                catch (Exception errorFacebookAuthenticatorCompleted)
                {
                    //var client = new Diagnostic();
                    //client.parseException(errorFacebookAuthenticatorCompleted.ToString(), user);

                    Debug.WriteLine("ERROR IN 'FacebookAuthenticatorCompleted': " + errorFacebookAuthenticatorCompleted.Message);
                }
            }
        }

        // This function checks if the user was able to succesfully login to their Google account. Once the
        // user autheticates throught Google, we validate their credentials in our system.

        async void GoogleAuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {

            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= GoogleAuthenticatorCompleted;
                authenticator.Error -= GoogleAuthenticatorError;
            }

            if (e.IsAuthenticated)
            {
                try
                {
                    var client = new SignIn();
                    UserDialogs.Instance.ShowLoading("Retrieving your JD account...");
                    var status = await client.VerifyUserCredentials(e.Account.Properties["access_token"], e.Account.Properties["refresh_token"], e, null, "GOOGLE");
                    RedirectUserBasedOnVerification(status, direction);
                }
                catch (Exception errorGoogleAutheticatorCompleted)
                {
                    //var client = new Diagnostic();
                    //client.parseException(errorGoogleAutheticatorCompleted.ToString(), user);
                    Debug.WriteLine("ERROR IN 'GoogleAuthenticatorCompleted': " + errorGoogleAutheticatorCompleted.Message);
                }

            }
            else
            {
                //Application.Current.MainPage = new LogInPage();
                //await DisplayAlert("Error", "Google was not able to autheticate your account", "OK");
            }
        }

        // This function checks if the user was able to succesfully login to their Apple account. Once the
        // user autheticates throught Apple, we validate their credentials in our system.

        async void OnAppleSignInRequest()
        {
            try
            {
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

                        account.Email = email;
                    }

                    var client = new SignIn();
                    UserDialogs.Instance.ShowLoading("Retrieving your JD account...");
                    var status = await client.VerifyUserCredentials("", "", null, account, "APPLE");
                    RedirectUserBasedOnVerification(status, direction);
                }
            }
            catch (Exception errorAppleSignInRequest)
            {
                //var client = new Diagnostic();
                //client.parseException(errorAppleSignInRequest.ToString(), user);
                Debug.WriteLine("ERROR IN 'OnAppleSignInRequest': " + errorAppleSignInRequest.Message);
            }
        }

        // AUTHETICATION HANDLERS _________________________________________________

        // AUTHETICATION ERROR HANDLERS ___________________________________________

        // This function gets call if there was an error authenticating with Facebook.

        private async void FacebookAutheticatorError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= FacebookAuthenticatorCompleted;
                authenticator.Error -= FacebookAutheticatorError;
            }
            Application.Current.MainPage = new NavigationPage(new LogInPage());
            await DisplayAlert("Authentication error: ", e.Message, "OK");
        }

        // This function gets call if there was an error authenticating with Google.

        async void GoogleAuthenticatorError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= GoogleAuthenticatorCompleted;
                authenticator.Error -= GoogleAuthenticatorError;
            }
            Application.Current.MainPage = new NavigationPage(new LogInPage());
            await DisplayAlert("Authentication error: ", e.Message, "OK");
        }

        // AUTHETICATION ERROR HANDLERS ___________________________________________

        // This function handles the status of each authetication and redirects user to
        // appropiate page or gives them an alert message to find out what they should do next.

        async void RedirectUserBasedOnVerification(string status, string direction)
        {
            try
            {
                if (status.Contains("SUCCESSFUL:"))
                {
                    UserDialogs.Instance.HideLoading();

                    Debug.WriteLine("DIRECTION VALUE: " + direction);
                    if (direction == "")
                    {
                        Application.Current.MainPage = new DeliveriesPage();
                    }
                    else
                    {

                        var client = new SignUp();

                        if(userToSignUp != null)
                        {
                            var s = await client.FastSignUp(userToSignUp);

                        }
                        //Dictionary<string, Page> array = new Dictionary<string, Page>();

                        //array.Add("ServingFresh.Views.CheckoutPage", new CheckoutPage());
                        //array.Add("ServingFresh.Views.SelectionPage", new SelectionPage());
                        //array.Add("ServingFresh.Views.HistoryPage", new HistoryPage());
                        //array.Add("ServingFresh.Views.RefundPage", new RefundPage());
                        //array.Add("ServingFresh.Views.ProfilePage", new ProfilePage());
                        //array.Add("ServingFresh.Views.ConfirmationPage", new ConfirmationPage());
                        //array.Add("ServingFresh.Views.InfoPage", new InfoPage());

                        //var root = Application.Current.MainPage;
                        //Application.Current.MainPage = array[root.ToString()];
                    }

                }
                else if (status == "USER NEEDS TO SIGN UP")
                {
                    UserDialogs.Instance.HideLoading();


                    var client = new SignUp();

                    if (userToSignUp != null)
                    {
                        var s = await client.FastSignUp(userToSignUp);
                        if (s)
                        {
                            Application.Current.MainPage = new DeliveriesPage();

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Oops", "We were not able to create an account. Please try again.", "OK");
                        }

                    }


                    //if (messageList != null)
                    //{
                    //    if (messageList.ContainsKey("701-000037"))
                    //    {
                    //        await Application.Current.MainPage.DisplayAlert(messageList["701-000037"].title, messageList["701-000037"].message, messageList["701-000037"].responses);
                    //    }
                    //    else
                    //    {
                    //        await Application.Current.MainPage.DisplayAlert("Oops", "It looks like you don't have an account with Serving Fresh. Please sign up!", "OK");
                    //    }
                    //}
                    //else
                    //{
                    //    await Application.Current.MainPage.DisplayAlert("Oops", "It looks like you don't have an account with Serving Fresh. Please sign up!", "OK");
                    //}

                    await Application.Current.MainPage.DisplayAlert("Oops", "It looks like you don't have an account with Just Delivered. Please sign up!", "OK");
                    //await Navigation.PopModalAsync(false);
                    //await Application.Current.MainPage.Navigation.PushModalAsync(new AddressPage(), true);
                }
                else if (status == "WRONG DIRECT PASSWORD")
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Wrong password was entered", "OK");
                }
                else if (status == "WRONG SOCIAL MEDIA TO SIGN IN")
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Oops", "Our records show that you have attempted to log in with a different social media account. Please log in through the correct social media platform. Thanks!", "OK");
                }
                else if (status == "SIGN IN DIRECTLY")
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Oops", "Our records show that you have attempted to log in with a social media account. Please log in through our direct log in. Thanks!", "OK");
                }
                else if (status == "ERROR1")
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Oops", "There was an error getting your account. Please contact customer service", "OK");

                }
                else if (status == "ERROR2")
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Oops", "There was an error getting your account. Please contact customer service", "OK");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Oops", "There was an error getting your account. Please contact customer service", "OK");
                }
            }
            catch (Exception errorRedirectUserBaseOnVerification)
            {
                //var client = new Diagnostic();
                //client.parseException(errorRedirectUserBaseOnVerification.ToString(), user);

                Debug.Write("ERROR IN 'RedirectUserBasedOnVerification' FUNCTION: " + errorRedirectUserBaseOnVerification.Message);
            }
        }


        public void SetAppVersion(Label appVersionLabel)
        {
            try
            {
                var versionString = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
                var buildString = DependencyService.Get<IAppVersionAndBuild>().GetBuildNumber();

                appVersionLabel.Text = "Version: " + versionString + ", Build: " + buildString;
            }
            catch
            {
                appVersionLabel.Text = "";
            }
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


        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SignUpPage());
        }
    }
}
