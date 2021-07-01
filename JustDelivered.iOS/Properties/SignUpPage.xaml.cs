using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;
using Item = JustDelivered.Models.Item;

namespace JustDelivered.Views
{
    public partial class SignUpPage : ContentPage
    {
        public static byte[] insurancePhotoBiteArray = null;
        public static string data = "";
        public static SignUpAccount userToSignUp = null;
        public ObservableCollection<Item> businessSource = new ObservableCollection<Item>();
        public static List<string> businessSelected = new List<string>();
        public SignUpPage()
        {
            InitializeComponent();
            if(userToSignUp != null)
            {
                userToSignUp.Print();
                AutoFill(userToSignUp);
                if(userToSignUp.platform == "DIRECT")
                {
                    directAccount.IsVisible = true;
                }
            }
            SetBusinessList();
        }

        async void SetBusinessList()
        {
            var client = new HttpClient();
            var endpointCall = await client.GetAsync(Constant.AvailableBusinessList);
            if (endpointCall.IsSuccessStatusCode)
            {
                var sorceString = await endpointCall.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Business>(sorceString);
              
                foreach(Item item in data.result.result)
                {
                    item.businessSelected = false;
                    businessSource.Add(item);
                    
                }
                businessList.ItemsSource = businessSource;
                businessList.HeightRequest = businessSource.Count * 50;
            }
        }

        void AutoFill(SignUpAccount account)
        {
            firstName.Text = account.firstName;
            lastName.Text = account.lastName;
            email.Text = account.socialEmail;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (ValidateEntries())
            {
                var signUpClient = new SignUp();

                signUpClient.first_name = firstName.Text;
                signUpClient.last_name = lastName.Text;
                signUpClient.business_uid = "";
                signUpClient.driver_hours = "";
                signUpClient.street = address.Text;
                signUpClient.city = city.Text;
                signUpClient.state = state.Text;
                signUpClient.zipcode = zipcode.Text;
                signUpClient.email = email.Text;
                signUpClient.phone = phoneNumber.Text;
                signUpClient.ssn = ssNumber.Text;
                signUpClient.license_num = driverLicense.Text;
                signUpClient.license_exp = "2021-06-01";
                signUpClient.driver_insurance_carrier = insuranceCarrier.Text;
                signUpClient.driver_insurance_num = insuranceNumber.Text;
                signUpClient.driver_insurance_exp_date = "2021-06-01";
                signUpClient.contact_name = emergencyAddress.Text;
                signUpClient.contact_phone = emergencyPhone.Text;
                signUpClient.contact_relation = emergencyRelationship.Text;
                signUpClient.bank_acc_info = accountNumber.Text;
                signUpClient.bank_routing_info = routingNumber.Text;

                if(userToSignUp.platform == "DIRECT")
                {
                    signUpClient.password = password1.Text;
                    signUpClient.social = "NULL";
                    signUpClient.social_id = "NULL";
                    signUpClient.mobile_access_token = "FALSE";
                    signUpClient.mobile_refresh_token = "FALSE";
                    signUpClient.user_access_token = "FALSE";
                    signUpClient.user_refresh_token = "FALSE";
 
                }
                else
                {
                    signUpClient.password = "";
                    signUpClient.mobile_access_token = "FALSE";
                    signUpClient.mobile_refresh_token = "FALSE";
                    signUpClient.social = userToSignUp.platform;
                    signUpClient.user_access_token = userToSignUp.accessToken; ;
                    signUpClient.user_refresh_token = userToSignUp.refreshToken;
                    signUpClient.social_id = userToSignUp.socialID;
                }

                //SignUpUser(signUpClient);

                //user = new User();
                //user.id = "930-000027";
                //user.email = "";
                //user.socialId = "";
                //user.platform = "";
                //user.route_id = "";
                //SaveUser(user);
                data = JsonConvert.SerializeObject(user);

                _ =  Navigation.PushAsync(new SubmitSignUpPage(), false);

            }
            else
            {
                await DisplayAlert("Oops", "Please fill in all entries before submitting your application.", "OK");
            }
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

        string GetBusinessID(string business)
        {
            string result = "";
            if(business == "Serving Fresh")
            {
                result = "200-000001";
            }
            else if (business == "M4ME")
            {
                result = "200-000002";
            }
            return result;
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LogInPage();
        }

        bool ValidateEntries()
        {
            bool result = false;
            if (String.IsNullOrEmpty(firstName.Text)
                || String.IsNullOrEmpty(lastName.Text)
                || String.IsNullOrEmpty(phoneNumber.Text)
                || String.IsNullOrEmpty(address.Text)
                || String.IsNullOrEmpty(city.Text)
                || String.IsNullOrEmpty(state.Text)
                || String.IsNullOrEmpty(zipcode.Text)
                || String.IsNullOrEmpty(emergencyAddress.Text)
                || String.IsNullOrEmpty(emergencyCity.Text)
                || String.IsNullOrEmpty(emergencyState.Text)
                || String.IsNullOrEmpty(emergencyZipcode.Text)
                || String.IsNullOrEmpty(ssNumber.Text)
                || String.IsNullOrEmpty(driverLicense.Text)
                || String.IsNullOrEmpty(driverLicenseExpiration.Text)
                || String.IsNullOrEmpty(accountNumber.Text)
                || String.IsNullOrEmpty(routingNumber.Text))
            {
                result = true;
            }
            return result;
        }

        async Task<bool> SignUpUser(SignUp account)
        {
            try
            {
                var client = new HttpClient();

                var deliveryJSON = JsonConvert.SerializeObject(account);
                Debug.WriteLine("DELIVERY JSON: " + deliveryJSON);
                var content = new StringContent(deliveryJSON, Encoding.UTF8, "application/json");

                var RDSResponse = await client.PostAsync(Constant.SignUpUrl, content);
                Debug.WriteLine("UPDATE DELIVERY STATUS ENDPOINT " + RDSResponse.IsSuccessStatusCode);

            }
            catch (Exception ErrorUpdatingStatus)
            {
                Debug.WriteLine("Exception: " + ErrorUpdatingStatus.Message);
            }
            return true;
        }

        void showListButton_Clicked(System.Object sender, System.EventArgs e)
        {
            showListButton.IsVisible = false;
            businessList.IsVisible = true;
            hideListButton.IsVisible = true;
        }

        void hideListButton_Clicked(System.Object sender, System.EventArgs e)
        {
            hideListButton.IsVisible = false;
            businessList.IsVisible = false;
            showListButton.IsVisible = true;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var stack = (StackLayout)sender;
            var gesture = (TapGestureRecognizer)stack.GestureRecognizers[0];
            var selectedItem = (Item)gesture.CommandParameter;

            if (selectedItem.businessSelected == false)
            {
                selectedItem.updateBusinessSelected = true;
                if (!businessSelected.Contains(selectedItem.business_uid))
                {
                    businessSelected.Add(selectedItem.business_uid);
                }
            }
            else
            {
                selectedItem.updateBusinessSelected = false;
                if (businessSelected.Contains(selectedItem.business_uid))
                {
                    businessSelected.Remove(selectedItem.business_uid);
                }
            }
        }

        public async void TakePicture(System.Object sender, System.EventArgs e)
        {
            string option = await DisplayActionSheet("Select the recipient(s) for this confirmation message", "Cancel", null, new string[] { "Seller", "Customer", "Seller And Customer" });
            Debug.WriteLine("Option: " + option);
            if (option != null && option != "")
            {
                if (option != "Cancel")
                {
                    try
                    {
                        var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                        //if (true)
                        if (photo != null)
                        {
                            //Get the public album path

                            var aPpath = photo.AlbumPath;
                            //p = aPpath;
                            Debug.WriteLine("PATH: " + aPpath);
                            Debug.WriteLine("PHOTO: " + photo.Path);
                            //var message = new SmsMessage("Hello JD", new[] { "4158329643" });
                            //await Sms.ComposeAsync(message);

                            //var smsMessanger = CrossMessaging.Current.SmsMessenger;
                            //if (smsMessanger.CanSendSms)
                            //{
                            //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
                            //}

                            //DependencyService.Get<INativeMessage>().OpenUrl("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");

                            //DependencyService.Get<INativeMessage>().OpenUrl("f");

                            //SendSMS(new[] { "14158329643"}, "Hello everyone! This is JD in development mode");
                            //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);

                            var path = photo.Path;
                            //photoStream = photo.GetStream();

                            var ar = File.ReadAllBytes(path);
                            insurancePhotoBiteArray = ar;
                            //f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                            //f.Rotation = 90;

                            //Bitmap bmp = BitmapFactory.decodeByteArray(byteArray, 0, byteArray.length);
                            //using (var memoryStream = new MemoryStream(t))
                            //{
                            //    var rotateImage = System.Drawing.Image.FromStream(memoryStream);
                            //    rotateImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //    rotateImage.Save(memoryStream, rotateImage.RawFormat);
                            //    byteArray = memoryStream.ToArray();
                            //}



                            //image = "CallIcon.png";


                            //refundItemImage.Scale = 1;

                            //ProcessRequest(option);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                        return;
                    }
                }
            }
        }

    }
}
