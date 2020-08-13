using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();
        }

        public async void newUserInterestRequestForm(System.Object sender, System.EventArgs e)
        {
            try
            {
                List<string> recipients = new List<string>();
                recipients.Add("pmarathay@gmail.com");

                var message = new EmailMessage();
                message.Body = "Hello Just Delivered, " + Environment.NewLine + Environment.NewLine + "I am interested in becoming a JD driver." + Environment.NewLine + "First Name: " + userFirstName.Text + Environment.NewLine + "Last Name: " + userLastName.Text + Environment.NewLine + "Email Address: " + userEmailAddress.Text; 
                message.To = recipients;
                message.Subject = "Just Delivered New User Request Form";

                await Email.ComposeAsync(message);
                await Navigation.PopAsync();
                await DisplayAlert("Message", "Thank you for your interest in driving for Just Delivered. We will review your information and send you an email shortly.", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }

        }
    }
}
