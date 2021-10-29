using System;
using System.Collections.Generic;
using JustDelivered.Models;
using Xamarin.Forms;
using static JustDelivered.Views.SignUpPage;

namespace JustDelivered.Views
{
    public partial class DriverCredentialsVerificationPage : ContentPage
    {
        string email = "";
        string cEmail = "";
        string password = "";
        string cPassword = "";

        public DriverCredentialsVerificationPage(string email, string password)
        {
            InitializeComponent();
            OnTextChange(EmailEntry, new TextChangedEventArgs(null, email));
            OnTextChange(PasswordEntry,new TextChangedEventArgs(null, password));
        }

        void OnTextChange(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var frame = (Frame)entry.Parent;

            if (!String.IsNullOrEmpty(e.NewTextValue))
            {
                if (frame.BorderColor == Color.LightGray)
                {
                    frame.BorderColor = Color.Red;
                }
                entry.Text = e.NewTextValue;
                SetData(entry.ClassId, e.NewTextValue);
            }
            else
            {
                if (frame.BorderColor == Color.Red)
                {
                    frame.BorderColor = Color.LightGray;
                    SetData(entry.ClassId, "");
                }
            }
        }

        void SetData(string classId, string data)
        {
            if(classId == "email")
            {
                email = data.ToLower().Trim();
            }
            else if (classId == "confirmationEmail")
            {
                cEmail = data.ToLower().Trim();
            }
            else if (classId == "password")
            {
                password = data.ToLower().Trim();
            }
            else if (classId == "confirmationPassword")
            {
                cPassword = data.ToLower().Trim();
            }
        }

        async void VerifyCredentials(System.Object sender, System.EventArgs e)
        {
            if(CheckAllEntriesAreFilled(email, cEmail, password, cPassword))
            {
                if (CheckEmailsEntriesAreFilled(email, cEmail))
                {
                    if (CheckPasswordsEntriesAreFilled(password, cPassword))
                    {
                        if (email == cEmail)
                        {
                            if (password == cPassword)
                            {
                                await DisplayAlert("Great!", "Your information is correct!. We are creating an account for you. ","OK");

                                userToSignUp.password = password;
                                userToSignUp.email = email;

                                var client = new SignUp();
                                var result = await client.FastSignUp(userToSignUp);

                                if (result)
                                {
                                    Application.Current.MainPage = new DeliveriesPage();
                                }
                                else
                                {
                                    await DisplayAlert("Oops", "We were not able to create an account. Please try again.", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Oops", "The passwords entered do not match. Please check they are the same. Thank you!", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Oops", "The emails entered do not match. Please check they are the same. Thank you!", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Oops", "One of your password entries is empty. Please fill it in with a valid password.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Oops", "One of your email entries is empty. Please fill it in with a valid email", "OK");
                }
            }
            else
            {
                await DisplayAlert("Oops", "It looks like one of the fields is empty. Please fill all entries.", "OK");
            }
        }

        void NavigateToPreviousPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        bool CheckAllEntriesAreFilled(string email, string cEmail, string password, string cPassword)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(cEmail) && !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(cPassword))
            {
                result = true;
            }

            return result;
        }

        bool CheckEmailsEntriesAreFilled(string email, string cEmail)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(cEmail))
            {
                result = true;
            }

            return result;
        }

        bool CheckPasswordsEntriesAreFilled(string password, string cPassword)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(cPassword))
            {
                result = true;
            }

            return result;
        }
    }
}
