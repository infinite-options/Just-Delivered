using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using JustDelivered.Config;
using JustDelivered.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace JustDelivered.Views
{
    public partial class SignUpPage : ContentPage
    {
        public static SignUpAccount userToSignUp = null;
        public static List<string> businessSelected = new List<string>();
        public ObservableCollection<Item> businesSource = new ObservableCollection<Item>();

        public SignUpPage()
        {
            InitializeComponent();
            if(userToSignUp != null)
            {

            }
            SetAvailableBusiness();
        }

        async void SetAvailableBusiness()
        {
            try
            {
                var client = new HttpClient();
                var endpointCall = await client.GetAsync(Constant.AvailableBusinessList);
                if (endpointCall.IsSuccessStatusCode)
                {
                    var contentString = await endpointCall.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Business>(contentString);
                    foreach(Item item in data.result.result)
                    {
                        item.businessSelected = false;
                        businesSource.Add(item);
                    }
                    businessList.ItemsSource = businesSource;
                    businessList.HeightRequest = businesSource.Count * 50;
                }
            }
            catch (Exception issueGettingBusiness)
            {
                Debug.WriteLine("issueGettingBusiness: " + issueGettingBusiness.Message);
            }

        }

        void SelectBusiness(System.Object sender, System.EventArgs e)
        {
            var stack = (StackLayout)sender;
            var gesture = (TapGestureRecognizer)stack.GestureRecognizers[0];
            var business = (Item)gesture.CommandParameter;

            if(business.businessSelected == false)
            {
                business.updateBusinessSelected = true;
                if (!businessSelected.Contains(business.business_uid))
                {
                    businessSelected.Add(business.business_uid);
                }
            }
            else
            {
                business.updateBusinessSelected = false;
                if (businessSelected.Contains(business.business_uid))
                {
                    businessSelected.Remove(business.business_uid);
                }
            }
        }

        void ShowList(System.Object sender, System.EventArgs e)
        {
            showListButton.IsVisible = false;
            businessListView.IsVisible = true;
        }

        void HideList(System.Object sender, System.EventArgs e)
        {
            businessListView.IsVisible = false;
            showListButton.IsVisible = true;
        }

        void Continue(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SubmitSignUpPage(), false);
        }

        void Cancel(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LogInPage();
        }
    }
}
