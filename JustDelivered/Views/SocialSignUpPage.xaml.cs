using System;
using System.Collections.Generic;
using JustDelivered.Models;
using Xamarin.Forms;

namespace JustDelivered.Views
{
    public partial class SocialSignUpPage : ContentPage
    {
        public SignUp newUser = new SignUp();

        public SocialSignUpPage(string id = "", string firstName ="", string lastName = "", string email = "", string accessToken = "", string refreshToken = "", string platform = "" )
        {
            InitializeComponent();
            InitializeUser();
            Intro();
        }

        void InitializeUser()
        {
            newUser.first_name = "";
            newUser.last_name = "";
            newUser.business_uid = "";
            newUser.driver_hours = "";
            newUser.street = "";
            newUser.city = "";
            newUser.state = "";
            newUser.zipcode = "";
            newUser.email = "";
            newUser.phone = "";
            newUser.ssn = "";
            newUser.license_num = "";
            newUser.license_exp = "";
            newUser.insurance = "";
            newUser.contact_name = "";
            newUser.contact_phone = "";
            newUser.contact_relation = "";
            newUser.bank_acc_info = "";
            newUser.bank_routing_info = "";
            newUser.password = "";
            newUser.mobile_access_token = "";
            newUser.mobile_refresh_token = "";
            newUser.user_access_token = "";
            newUser.user_refresh_token = "";
            newUser.social_id = "";
            newUser.social = "";
        }

        async void Intro()
        {
            await DisplayAlert("Message", "It looks like you don't have a Just Delivered account. Please sign up!", "OK");
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LogInPage();
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LogInPage();
        }
    }
}
