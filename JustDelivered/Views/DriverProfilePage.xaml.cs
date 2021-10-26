using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.Interfaces;
using JustDelivered.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;
namespace JustDelivered.Views
{
    public partial class DriverProfilePage : ContentPage
    {
        public Connect client = new Connect();

        public static SignUpAccount userToSignUp = null;
        public static List<string> businessSelected = new List<string>();
        public static byte[] insurancePicture = null;
        public static string accountString = "";
        public string insuraceExpDate = "";
        public string licenseExpDate = "";

        private AddressAutocomplete addressToValidate = null;
        public ObservableCollection<Models.Item> businesSource = new ObservableCollection<Models.Item>();
        Models.Address addr = new Models.Address();

        Driver driver = new Driver();

        Dictionary<string, object> account = new Dictionary<string, object>();

        public string phoneNum = "";

        public DriverProfilePage()
        {
            InitializeComponent();

            if (userToSignUp != null)
            {
                if (userToSignUp.platform != "DIRECT")
                {
                    firstName.Text = userToSignUp.firstName;
                    lastName.Text = userToSignUp.lastName;
                    email.Text = userToSignUp.email;
                }
                else
                {
                    directSignUp.IsVisible = true;
                }
            }

            SetDatePickers(insuranceExpirationDate, driveLicenseExperirationDate);
            SetAccountKeysValues();
            _= SetUpAccountDetails();
            
        }

        async Task SetUpAccountDetails()
        {
            driver = await client.GetUserProfile(user.id);

            if(driver != null)
            {
                // parse organizations...
                firstName.Text = driver.result[0].driver_first_name == null || driver.result[0].driver_first_name == "NULL" ? null : driver.result[0].driver_first_name;
                lastName.Text = driver.result[0].driver_last_name == null || driver.result[0].driver_last_name == "NULL" ? null : driver.result[0].driver_last_name;
                email.Text = driver.result[0].driver_email == null || driver.result[0].driver_email == "NULL" ? null : driver.result[0].driver_email;
                AddressEntry.Text = driver.result[0].driver_street == null || driver.result[0].driver_street == "NULL" ? null : driver.result[0].driver_street;
                unit.Text = driver.result[0].driver_unit == null || driver.result[0].driver_unit == "NULL" ? null : driver.result[0].driver_unit;
                city.Text = driver.result[0].driver_city == null || driver.result[0].driver_city == "NULL" ? null : driver.result[0].driver_city;
                state.Text = driver.result[0].driver_state == null || driver.result[0].driver_state == "NULL" ? null : driver.result[0].driver_state;
                zipcode.Text = driver.result[0].driver_zip == null || driver.result[0].driver_zip == "NULL" ? null : driver.result[0].driver_zip;
                emergencyFirstName.Text = driver.result[0].emergency_contact_name == null || driver.result[0].emergency_contact_name == "NULL" ? null : driver.result[0].emergency_contact_name;
                emergencyRelationship.Text = driver.result[0].emergency_contact_relationship == null || driver.result[0].emergency_contact_relationship == "NULL" ? null : driver.result[0].emergency_contact_relationship;
                emergencyPhoneNumber.Text = driver.result[0].emergency_contact_phone == null || driver.result[0].emergency_contact_phone == "NULL" ? null : driver.result[0].emergency_contact_phone;
                ssNumber.Text = driver.result[0].driver_ssn == null || driver.result[0].driver_ssn == "NULL" ? null : driver.result[0].driver_ssn;
                carYear.Text = driver.result[0].driver_car_year == null || driver.result[0].driver_car_year == "NULL" ? null : driver.result[0].driver_car_year;
                carModel.Text = driver.result[0].driver_car_model == null || driver.result[0].driver_car_model == "NULL" ? null : driver.result[0].driver_car_model;
                carMake.Text = driver.result[0].driver_car_make == null || driver.result[0].driver_car_make == "NULL" ? null : driver.result[0].driver_car_make;
                insuranceCarrier.Text = driver.result[0].driver_insurance_carrier == null || driver.result[0].driver_insurance_carrier == "NULL" ? null : driver.result[0].driver_insurance_carrier;
                insuranceNumber.Text = driver.result[0].driver_insurance_carrier == null || driver.result[0].driver_insurance_carrier == "NULL" ? null : driver.result[0].driver_insurance_carrier;
                insuranceExpirationDate.Date = driver.result[0].driver_first_name == null || driver.result[0].driver_first_name == "NULL" ? DateTime.Now : DateTime.Parse(driver.result[0].driver_insurance_exp_date);
                driveLicenseNumber.Text = driver.result[0].driver_license == null || driver.result[0].driver_license == "NULL" ? null : driver.result[0].driver_license;
                driveLicenseExperirationDate.Date = driver.result[0].driver_first_name == null || driver.result[0].driver_first_name == "NULL" ? DateTime.Now : DateTime.Parse(driver.result[0].driver_insurance_exp_date);
                accountNumber.Text = driver.result[0].bank_account_info == null || driver.result[0].bank_account_info == "NULL" ? null : driver.result[0].bank_account_info;
                routingNumber.Text = driver.result[0].bank_routing_info == null || driver.result[0].bank_routing_info == "NULL" ? null : driver.result[0].bank_routing_info;
                SetAvailableBusiness();
            }
        }

        void SetDatePickers(DatePicker picker1, DatePicker picker2)
        {
            picker1.Date = DateTime.Now;
            picker2.Date = DateTime.Now;
        }

        void SetAccountKeysValues()
        {
            var keys = new[]
            {
                "firstName",
                "lastName",
                "phoneNumber",
                "email",
                "emergencyFirstName",
                "emergencyLastName",
                "emergencyRelationship",
                "emergencyPhoneNumber",
                "ssNumber",
                "carYear",
                "carModel",
                "carMake",
                "insuranceCarrier",
                "insuranceNumber",
                "insuranceExpirationDate",
                "driveLicenseNumber",
                "driveLicenseExperirationDate",
                "accountNumber",
                "routingNumber",
                "insuranceImage",
                "street",
                "unit",
                "city",
                "state",
                "zipcode",
                "organizations",
                "latitude",
                "longitude",
                "platform",
                "driver_uid"
            };

            foreach(string k in keys)
            {
                if(k == "organizations")
                {
                    account.Add(k, new List<string>());
                }
                else
                {
                    account.Add(k, null);
                }
            }

            account["platform"] = user.platform;
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
                    foreach (Models.Item item in data.result.result)
                    {
                        if (driver.result[0].business_id.Contains(item.business_name))
                        {
                            item.businessSelected = true;
                        }
                        else
                        {
                            item.businessSelected = false;
                        }

                        businesSource.Add(item);
                    }

                    
                    
                    BindableLayout.SetItemsSource(organizationView,businesSource);

                    if (businesSource.Count == 0)
                    {
                        organizationMessageLabel.IsVisible = true;
                        organizationMessageLabel.Text = "The are zero organization to select at the moment.";
                    }
                    else
                    {
                        organizationMessageLabel.IsVisible = false;
                    }
                }
                else
                {
                    organizationMessageLabel.IsVisible = true;
                    organizationMessageLabel.Text = "Our system seems to experience a problem. We were not able to retrieve the organizations. Our engineers are working diligently to resolve this issue. ";
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
            var business = (Models.Item)gesture.CommandParameter;

            if (business.businessSelected == false)
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

        async void SaveChanges(System.Object sender, System.EventArgs e)
        {
            var client = new SignUp();
            foreach(string key in account.Keys)
            {
                Debug.WriteLine("KEY: {0}, VALUE: {1}", key, account[key]);
            }


            var businessIDs = "";
            foreach (string id in businessSelected)
            {
                businessIDs += id + ",";
            }

            if (businessIDs != "")
            {
                businessIDs = businessIDs.Remove(businessIDs.Length - 1);
            }

            account["driver_uid"] = user.id;
            account["organizations"] = businessIDs;

            var result = await client.UpdateUserProfile(account);

            Debug.WriteLine("RESULT: " + result);

            if (result)
            {
                await DisplayAlert("Great!", "Your request was succesful!", "OK");
            }
            else
            {
                await DisplayAlert("Oops", "We were not able to update your profile. Please try again.", "OK");
            }


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
            // remove || String.IsNullOrEmpty(address.Text)
            bool result = false;
            if (!(
                 String.IsNullOrEmpty(firstName.Text)
              || String.IsNullOrEmpty(lastName.Text)
              || String.IsNullOrEmpty(phoneNumber.Text)
              || String.IsNullOrEmpty(email.Text)
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
                if (businessSelected.Count != 0)
                {
                    if (insurancePicture != null)
                    {
                        if (addressToValidate != null && addressToValidate.isValidated == true)
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
            if (!(String.IsNullOrEmpty(password1.Text) || String.IsNullOrEmpty(password2.Text)))
            {
                if (password1.Text == password2.Text)
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
            var image = (Image)sender;

            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                if (photo != null)
                {

                    var path = photo.Path;
                    insuranceImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    insurancePicture = File.ReadAllBytes(path);
                    imageFrame.IsVisible = true;

                    SetAccountValues(image.ClassId, File.ReadAllBytes(path));

                    //f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                }
            }
            catch (Exception ex)
            {
                
                SetAccountValues(image.ClassId, null);

                Debug.WriteLine(ex.Message);
                await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                return;
            }
        }

        void OnDateSelectedChaged(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var frame = (Frame)datePicker.Parent;

            if (datePicker.Date != null)
            {
                if (frame.BorderColor == Color.LightGray)
                {
                    frame.BorderColor = Color.Red;

                    datePicker.Date = e.NewDate;
                    datePicker.TextColor = Color.Black;
                }

                SetAccountValues(datePicker.ClassId, datePicker.Date.ToString("yyyy-MM-dd").Trim());
            }
            else
            {
                if (frame.BorderColor == Color.Red)
                {
                    frame.BorderColor = Color.LightGray;
                    datePicker.TextColor = Color.Gray;
                    SetAccountValues(datePicker.ClassId, null);
                }
            }
        }

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
            if (addressToValidate != null && addressToValidate.isValidated == true)
            {
                //addressView.IsVisible = true;

            }
            else
            {
                //addressView.IsVisible = false;
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
                                    //account["street"] = addressToValidate.Street;
                                    unit.Text = addressToValidate.Unit;
                                    city.Text = addressToValidate.City;
                                    state.Text = addressToValidate.State;
                                    zipcode.Text = addressToValidate.ZipCode;


                                    account["street"] = addressToValidate.Street;
                                    account["unit"] = addressToValidate.Unit;
                                    account["city"] = addressToValidate.City;
                                    account["state"] = addressToValidate.State;
                                    account["zipcode"] = addressToValidate.ZipCode;
                                    account["latitude"] = addressToValidate.Latitude.ToString();
                                    account["longitude"] = addressToValidate.Longitude.ToString();


                                    addressToValidate.isValidated = true;
                                }
                                else if (addressStatus == "D")
                                {
                                    var unit1 = await DisplayPromptAsync("It looks like your address is missing its unit number", "Please enter your address unit number in the space below", "OK", "Cancel");
                                    if (unit1 != null)
                                    {
                                        await DisplayAlert("Great!", "Your address is valid. Please continue with your application", "OK");
                                        addressToValidate.Unit = unit1;
                                        //address.Text = addressToValidate.Street;
                                        unit.Text = addressToValidate.Unit;
                                        city.Text = addressToValidate.City;
                                        state.Text = addressToValidate.State;
                                        zipcode.Text = addressToValidate.ZipCode;

                                        account["street"] = addressToValidate.Street;
                                        account["unit"] = addressToValidate.Unit;
                                        account["city"] = addressToValidate.City;
                                        account["state"] = addressToValidate.State;
                                        account["zipcode"] = addressToValidate.ZipCode;
                                        account["latitude"] = addressToValidate.Latitude.ToString();
                                        account["longitude"] = addressToValidate.Longitude.ToString();

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
                    await DisplayAlert("Oops", "Please select an address to validate", "OK");
                }
            }
            catch (Exception errorFindLocalProduceBaseOnLocation)
            {
                Debug.WriteLine(errorFindLocalProduceBaseOnLocation.Message);
            }
        }

        void NavigateToDeliveryPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }

        void EntryOnTextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var frame = (Frame)entry.Parent;

            if (!String.IsNullOrEmpty(entry.Text))
            {
                if(frame.BorderColor == Color.LightGray)
                {
                    frame.BorderColor = Color.Red;
                }

                SetAccountValues(entry.ClassId, entry.Text.Trim());
            }
            else
            {
                if (frame.BorderColor == Color.Red)
                {
                    frame.BorderColor = Color.LightGray;
                    SetAccountValues(entry.ClassId, null);
                }
            }
        }

        void SetAccountValues(string key, object input)
        {
            if (account.ContainsKey(key))
            {
                account[key] = input;
            }
            else
            {
                account.Add(key, input);
            }
        }

        void OnPhoneTextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var frame = (Frame)entry.Parent;

            if (!String.IsNullOrEmpty(entry.Text))
            {
                if (frame.BorderColor == Color.LightGray)
                {
                    frame.BorderColor = Color.Red;
                }
            }
            else
            {
                if (frame.BorderColor == Color.Red)
                {
                    frame.BorderColor = Color.LightGray;
                }
            }

            if (e.NewTextValue != null && e.OldTextValue != null)
            {
                if (e.NewTextValue.Length > e.OldTextValue.Length)
                {

                    var localInput = "";

                    foreach (char i in e.NewTextValue)
                    {
                        if (i != '(' && i != ')' && i != ' ' && i != '-')
                        {
                            localInput += i;
                        }
                    }

                    if (localInput.Length != 0)
                    {
                        var newChar = localInput[localInput.Length - 1];

                        int defaultValue = 0;
                        bool isNumber = int.TryParse(newChar.ToString(), out defaultValue);

                        if (isNumber)
                        {

                            var phone = "(";

                            for (int i = 0; i < localInput.Length; i++)
                            {
                                if (i == 2)
                                {
                                    phone += localInput[i] + ") ";
                                }
                                else if (i == 5)
                                {
                                    phone += localInput[i] + "-";
                                }
                                else
                                {
                                    phone += localInput[i];
                                }
                            }

                            entry.Text = phone;

                            SetAccountValues(entry.ClassId, phone.Trim());

                        }
                        else
                        {
                            DisplayAlert("Oops", "You enter a non numberical value. Please enter 0-9 digits.", "OK");
                            entry.Text = e.OldTextValue;
                        }
                    }
                }
            }
            else
            {
                SetAccountValues(entry.ClassId, null);
            }
        }
    }
}
