using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using JustDelivered.Config;
using JustDelivered.Interfaces;
using JustDelivered.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace JustDelivered.Views
{
    public partial class SignUpPage : ContentPage
    {
        public static SignUpAccount userToSignUp = null;

        public static List<string> businessSelected = new List<string>();
        public static byte[] insurancePicture = null;
        public static string accountString = "";
        public string insuraceExpDate = "";
        public string licenseExpDate = "";
        private AddressAutocomplete addressToValidate = null;

        public ObservableCollection<Item> businesSource = new ObservableCollection<Item>();

        public SignUpPage()
        {
            InitializeComponent();
            if(userToSignUp != null)
            {
                if(userToSignUp.platform != "DIRECT")
                {
                    firstName.Text = userToSignUp.firstName;
                    lastName.Text = userToSignUp.lastName;
                    email.Text = userToSignUp.socialEmail;
                }
                else
                {
                    directSignUp.IsVisible = true;
                }
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

        async void Continue(System.Object sender, System.EventArgs e)
        {
            if (ValidateEntries())
            {
                if(userToSignUp.platform == "DIRECT")
                {
                    if (!ValidatePassword())
                    {
                        await DisplayAlert("Oops!", "Please enter a password and make sure they match.", "OK");
                        return;
                    }

                }

                var account = new SignUp();
                account.first_name = firstName.Text.Trim();
                account.last_name = lastName.Text.Trim();
                account.business_uid = "";
                account.driver_hours = "";
                account.street = address.Text.Trim();
                account.unit = unit.Text == null ? "" : unit.Text;
                account.city = city.Text.Trim();
                account.state = state.Text.Trim();
                account.email = email.Text.Trim();
                account.zipcode = zipcode.Text.Trim();
                account.phone = phoneNumber.Text.Trim();
                account.ssn = ssNumber.Text.Trim();
                account.license_num = driveLicenseNumber.Text.Trim();
                account.license_exp = licenseExpDate.Trim();
                account.driver_car_year = carYear.Text.Trim();
                account.driver_car_model = carModel.Text.Trim();
                account.driver_car_make = carMake.Text.Trim();
                account.driver_insurance_carrier = insuranceCarrier.Text.Trim();
                account.driver_insurance_num = insuranceNumber.Text.Trim();
                account.driver_insurance_exp_date = insuraceExpDate.Trim();
                account.contact_name = emergencyFirstName.Text.Trim() + " " + emergencyLastName.Text;
                account.contact_phone = emergencyPhoneNumber.Text.Trim();
                account.contact_relation = emergencyRelationship.Text.Trim();
                account.bank_acc_info = accountNumber.Text.Trim();
                account.bank_routing_info = routingNumber.Text.Trim();
                account.latitude = addressToValidate.Latitude.ToString();
                account.longitude = addressToValidate.Longitude.ToString();
                account.referral_source = GetDeviceInformation() + GetAppVersion();

                if (userToSignUp.platform == "DIRECT")
                {
                    account.password = password1.Text.Trim();
                    account.mobile_access_token = "FALSE";
                    account.mobile_refresh_token = "FALSE";
                    account.user_access_token = "FALSE";
                    account.user_refresh_token = "FALSE";
                    account.social = "NULL";
                    account.social_id = "NULL";

                }
                else
                {
                    account.password = "";
                    account.mobile_access_token = userToSignUp.accessToken;
                    account.mobile_refresh_token = userToSignUp.refreshToken;
                    account.user_access_token = "FALSE";
                    account.user_refresh_token = "FALSE";
                    account.social = userToSignUp.platform;
                    account.social_id = userToSignUp.socialID;
                }
                accountString = JsonConvert.SerializeObject(account);
                Debug.WriteLine("ACCOUNT: " + accountString);
                _ = Navigation.PushAsync(new SubmitSignUpPage(), false);
            }
            else
            {
                await DisplayAlert("Oops!", "Please check that you have filled all the entries in the application. In addition, don't forget to select an organization and take a picture of your insurance card. Thanks!", "OK");
            }
            //Navigation.PushAsync(new SubmitSignUpPage(), false);
        }

        public static string GetAppVersion()
        {

            string versionStr = "";
            string buildStr = "";
            try
            {
                versionStr = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
                buildStr = DependencyService.Get<IAppVersionAndBuild>().GetBuildNumber();
            }
            catch
            {

            }

            return versionStr + ", " + buildStr;
        }

        public static string GetDeviceInformation()
        {
            var device = "";
            if (Device.RuntimePlatform == Device.Android)
            {
                device = "MOBILE: ANDROID, ";
            }
            else
            {
                device = "MOBILE: IOS, ";
            }
            return device;
        }



        bool ValidateEntries()
        {
          
            bool result = false;
            if(!(
                 String.IsNullOrEmpty(firstName.Text)
              || String.IsNullOrEmpty(lastName.Text)
              || String.IsNullOrEmpty(phoneNumber.Text)
              || String.IsNullOrEmpty(email.Text)
              || String.IsNullOrEmpty(address.Text)
              || String.IsNullOrEmpty(city.Text)
              || String.IsNullOrEmpty(state.Text)
              || String.IsNullOrEmpty(zipcode.Text)
              || String.IsNullOrEmpty(emergencyFirstName.Text)
              || String.IsNullOrEmpty(emergencyLastName.Text)
              || String.IsNullOrEmpty(emergencyRelationship.Text)
              || String.IsNullOrEmpty(emergencyPhoneNumber.Text)
              || String.IsNullOrEmpty(ssNumber.Text)
              || String.IsNullOrEmpty(carYear.Text)
              || String.IsNullOrEmpty(carModel.Text)
              || String.IsNullOrEmpty(carMake.Text)
              || String.IsNullOrEmpty(insuranceCarrier.Text)
              || String.IsNullOrEmpty(insuranceNumber.Text)
              || String.IsNullOrEmpty(insuraceExpDate)
              || String.IsNullOrEmpty(driveLicenseNumber.Text)
              || String.IsNullOrEmpty(licenseExpDate)
              || String.IsNullOrEmpty(accountNumber.Text)
              || String.IsNullOrEmpty(routingNumber.Text)
              )
              )
            {
                if(businessSelected.Count != 0)
                {
                    if(insurancePicture != null)
                    {
                        if(addressToValidate != null && addressToValidate.isValidated == true)
                        {
                            result = true; 
                        }
                    }
                }

            }
            return result;


              //|| String.IsNullOrEmpty(emergencyAddress.Text)
              //|| String.IsNullOrEmpty(emergencyCity.Text)
              //|| String.IsNullOrEmpty(emergencyUnit.Text)
              //|| String.IsNullOrEmpty(emergencyState.Text)
              //|| String.IsNullOrEmpty(emergencyZipcode.Text)
        }

        bool ValidatePassword()
        {
            bool result = false;
            if(!(String.IsNullOrEmpty(password1.Text) || String.IsNullOrEmpty(password2.Text))){
                if(password1 == password2)
                {
                    result = true;
                }
            }
            return result;
        }

        void Cancel(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new LogInPage();
        }

        async void TakePicture(System.Object sender, System.EventArgs e)
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                if (photo != null)
                {
                    var path = photo.Path;
                    insurancePicture = File.ReadAllBytes(path);
                 
                    //f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                return;
            }
        }

        void InsuranceExpirationDate_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            insuraceExpDate = e.NewDate.ToString("yyyy-MM-dd");
        }

        void DriveLicenseExpirationDate_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            licenseExpDate = e.NewDate.ToString("yyyy-MM-dd");
        }

        Models.Address addr = new Models.Address();

        async void OnAddressChanged(object sender, EventArgs eventArgs)
        {
            if (!String.IsNullOrEmpty(AddressEntry.Text))
            {
                if (addressToValidate != null)
                {
                    if (addressToValidate.Street != AddressEntry.Text)
                    {
                        addressList.ItemsSource = await addr.GetPlacesPredictionsAsync(AddressEntry.Text);
                        addressEntryFocused(sender, eventArgs);
                    }
                }
                else
                {
                    addressList.ItemsSource = await addr.GetPlacesPredictionsAsync(AddressEntry.Text);
                    addressEntryFocused(sender, eventArgs);
                }
            }
            else
            {
                addressEntryUnfocused(sender, eventArgs);
                addressToValidate = null;
            }
        }

        void addressEntryFocused(object sender, EventArgs eventArgs)
        {
            if (!String.IsNullOrEmpty(AddressEntry.Text))
            {
                addr.addressEntryFocused(addressList, addressFrame);
            }
        }

        void addressEntryUnfocused(object sender, EventArgs eventArgs)
        {
            addr.addressEntryUnfocused(addressList, addressFrame);
            if(addressToValidate != null && addressToValidate.isValidated == true)
            {
                addressView.IsVisible = true;
               
            }
            else
            {
                addressView.IsVisible = false;
            }
        }

        async void addressSelected(System.Object sender, SelectedItemChangedEventArgs e)
        {
            AddressEntry.TextChanged -= OnAddressChanged;
            addressToValidate = addr.addressSelected(addressList, AddressEntry, addressFrame);
            addressToValidate.isValidated = false;
            AddressEntry.Text = addressToValidate.Street;
            string zipcode = await addr.getZipcode(addressToValidate.PredictionID);
            if (zipcode != null)
            {
                addressToValidate.ZipCode = zipcode;
            }
            AddressEntry.TextChanged += OnAddressChanged;
            ValidateAddress(sender, e);
        }

        async void ValidateAddress(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (addressToValidate != null)
                {
                    if (!String.IsNullOrEmpty(AddressEntry.Text))
                    {
                        var client = new AddressValidation();
                        var addressStatus = client.ValidateAddressString(addressToValidate.Street, addressToValidate.Unit, addressToValidate.City, addressToValidate.State, addressToValidate.ZipCode);

                        if (addressStatus != null)
                        {
                            var location = await client.ConvertAddressToGeoCoordiantes(addressToValidate.Street, addressToValidate.City, addressToValidate.State);
                            if (location != null)
                            {
                                
                                addressToValidate.Latitude = location.Latitude;
                                addressToValidate.Longitude = location.Longitude;
                               

                                if (addressStatus == "Y" || addressStatus == "S")
                                {
                                    await DisplayAlert("Great!", "Your address is valid. Please continue with your application", "OK");
                                    address.Text = addressToValidate.Street;
                                    unit.Text = addressToValidate.Unit;
                                    city.Text = addressToValidate.City;
                                    state.Text = addressToValidate.State;
                                    zipcode.Text = addressToValidate.ZipCode;
                                    addressToValidate.isValidated = true;
                                }
                                else if (addressStatus == "D")
                                {
                                    var unit1 = await DisplayPromptAsync("It looks like your address is missing its unit number", "Please enter your address unit number in the space below", "OK", "Cancel");
                                    if (unit1 != null)
                                    {
                                        await DisplayAlert("Great!", "Your address is valid. Please continue with your application", "OK");
                                        addressToValidate.Unit = unit1;
                                        address.Text = addressToValidate.Street;
                                        unit.Text = addressToValidate.Unit;
                                        city.Text = addressToValidate.City;
                                        state.Text = addressToValidate.State;
                                        zipcode.Text = addressToValidate.ZipCode;
                                        addressToValidate.isValidated = true;
                                    }
                                    return;
                                }
                            }
                            else
                            {
                                await DisplayAlert("Oops", "We weren't able to find your address", "OK");
                            }

                         
                        }
                        else
                        {
                            await DisplayAlert("Oops", "The address you enter is not valid. Please enter another address to continue.", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Oops","Please select an address to validate","OK");
                }
            }
            catch (Exception errorFindLocalProduceBaseOnLocation)
            {
                Debug.WriteLine(errorFindLocalProduceBaseOnLocation.Message);
            }
        }

    }
}
