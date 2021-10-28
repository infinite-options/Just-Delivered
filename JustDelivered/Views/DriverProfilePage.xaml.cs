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

        List<string> businessSelected = new List<string>();
        byte[] insurancePicture = null;



        AddressAutocomplete addressToValidate  = new AddressAutocomplete();
        ObservableCollection<Models.Item> businesSource = new ObservableCollection<Models.Item>();
        Address addr = new Address();

        Driver driver = new Driver();

        Dictionary<string, object> account = new Dictionary<string, object>();

        bool displayAddress = false;
        AddressValidation addressValidationClient = new AddressValidation();

        public DriverProfilePage()
        {
            try
            {
                InitializeComponent();

                //if (userToSignUp != null)
                //{
                //    if (userToSignUp.platform != "DIRECT")
                //    {
                //        firstName.Text = userToSignUp.firstName;
                //        lastName.Text = userToSignUp.lastName;
                //        email.Text = userToSignUp.email;
                //    }
                //    else
                //    {
                //        directSignUp.IsVisible = true;
                //    }
                //}

                SetAccountKeysValues();
                SetUpAccountDetails();
                addressValidationClient.InitializeMap(MapView);
            }
            catch (Exception s)
            {
                Debug.WriteLine(s.Message);
            }
        }

        async void SetUpAccountDetails()
        {


            driver = await client.GetUserProfile(user.id);

            if(driver != null)
            {
                if (driver.result.Count != 0)
                {


                    EmergencyContact emergencyContact = new EmergencyContact();
                    string emergencyContactString = SetDataBaseOnDriverProfile(driver.result[0].emergency_contact_name);

                    if (emergencyContactString != null)
                    {
                        emergencyContact = ParseEmergencyContact(emergencyContactString);
                    }

                    if (SetDataBaseOnDriverProfile(driver.result[0].driver_street) == null)
                    {
                        displayAddress = true;
                    }

                    if(SetDataBaseOnDriverProfile(driver.result[0].driver_first_name) != null)
                    {
                        FirstNameEntry.IsEnabled = false;
                    }

                    if (SetDataBaseOnDriverProfile(driver.result[0].driver_last_name) != null)
                    {
                        LastNameEntry.IsEnabled = false;
                    }

                    if (SetDataBaseOnDriverProfile(driver.result[0].driver_street) != null)
                    {
                        double defaultResult = 0;

                        addressToValidate.isValidated = true;
                        addressToValidate.Street = SetDataBaseOnDriverProfile(driver.result[0].driver_street);

                        if (Double.TryParse(driver.result[0].driver_latitude, out defaultResult))
                        {
                            addressToValidate.Latitude = Double.Parse(driver.result[0].driver_latitude);
                        }

                        if (Double.TryParse(driver.result[0].driver_latitude, out defaultResult))
                        {
                            addressToValidate.Longitude = Double.Parse(driver.result[0].driver_longitude);
                        }


                        addressValidationClient.SetPinOnMap(MapView, new Xamarin.Essentials.Location(addressToValidate.Latitude, addressToValidate.Longitude), addressToValidate.Street);

                        //public void InitializeMap()
                        //{
                        //    map.MapType = MapType.Street;
                        //    Position point = new Position(37.334789, -121.888138);
                        //    var mapSpan = new MapSpan(point, 5, 5);
                        //    map.MoveToRegion(mapSpan);
                        //    map.Pins.Clear();
                        //}
                    }

                    SetEntry(FirstNameEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_first_name));
                    SetEntry(LastNameEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_last_name));
                    SetEntry(EmailEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_email));
                    SetEntry(StreetEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_street));
                    SetEntry(UnitEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_unit));
                    SetEntry(CityEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_city));
                    SetEntry(StateEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_state));
                    SetEntry(ZipcodeEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_zip));
                    SetEntry(ContactFirstNameEntry, SetDataBaseOnDriverProfile(emergencyContact.firstName));
                    SetEntry(ContactLastNameEntry, SetDataBaseOnDriverProfile(emergencyContact.lastName));
                    SetEntry(ContactRelationshipEntry, SetDataBaseOnDriverProfile(driver.result[0].emergency_contact_relationship));
                    SetEntry(CarYearEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_car_year));
                    SetEntry(CarModelEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_car_model));
                    SetEntry(carMakeEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_car_make));
                    SetEntry(InsuranceCarrierEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_insurance_carrier));
                    SetEntry(InsuranceNumberEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_insurance_num));
                    SetEntry(AccountNumberEntry, SetDataBaseOnDriverProfile(driver.result[0].bank_account_info));
                    SetEntry(RoutingNumberEntry, SetDataBaseOnDriverProfile(driver.result[0].bank_routing_info));
                    SetEntry(SSNEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_ssn));
                    SetEntry(DriveLicenseNumberEntry, SetDataBaseOnDriverProfile(driver.result[0].driver_license));
                    OnPhoneTextChanged(PhoneEntry, SetEntry(SetDataBaseOnDriverProfile(driver.result[0].driver_phone_num)));
                    OnPhoneTextChanged(ContactPhoneEntry, SetEntry(SetDataBaseOnDriverProfile(driver.result[0].emergency_contact_phone)));

                    SetPickerDate(InsuranceDatePicker, SetDataBaseOnDriverProfile(driver.result[0].driver_insurance_exp_date));
                    SetPickerDate(DriveLicenseDatePicker, SetDataBaseOnDriverProfile(driver.result[0].driver_license_exp));

                    SetInsuranceImage(SetDataBaseOnDriverProfile(driver.result[0].driver_insurance_picture));

                    SetAvailableBusiness(SetDataBaseOnDriverProfile(driver.result[0].business_id));
                }
            }
        }

        void SetPickerDate(DatePicker picker, string data)
        {
            if (data != null)
            {
                OnDateSelectedChaged(picker, SetDateBaseOn(SetTimeBaseOnProfile(SetDataBaseOnDriverProfile(driver.result[0].driver_insurance_exp_date))));
            }
            else
            {
                InsuranceDatePicker.Date = DateTime.Now;
            }
        }

        void SetInsuranceImage(string image)
        {
            if(image != null)
            {
                InsuranceImage.Source = image;
            }
        }

        TextChangedEventArgs SetEntry(string data)
        {
            return new TextChangedEventArgs(null, data);
        }

        void SetEntry(Entry element, string data)
        {
            if(data != null)
            {
                element.Text = data;
            }
        }

        void SetDatePicker(DatePicker element, string data)
        {
            
        }

        EmergencyContact ParseEmergencyContact(string contact)
        {
            EmergencyContact result = null;

            result = JsonConvert.DeserializeObject<EmergencyContact>(contact);

            return result;
        }

        DateTime SetTimeBaseOnProfile(string property)
        {
            DateTime result = new DateTime();

            try
            {
                if (property != null && property != "NULL" && property != "")
                {
                    result = DateTime.Parse(property + " " + "00:00:00");
                }
            }
            catch
            {

            }

            return result;
        }

        DateChangedEventArgs SetDateBaseOn(DateTime date)
        {
            return new DateChangedEventArgs(DateTime.Now, date);
        }


        string SetDataBaseOnDriverProfile(string property)
        {
            string result = null;

            try
            {
                result = property == null || property == "NULL"  ||  property == "0000-00-00" ? null : property;
            }
            catch
            {

            }

            return result;
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
                "driver_uid",
                "social_id",
                "password",
                "referal",
                "schedule"
            };

            foreach(string k in keys)
            {
                account.Add(k, null);
            }

            account["platform"] = user.platform;
        }

        async void SetAvailableBusiness(string organizations)
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

                        if (organizations != null && organizations != "" && organizations != "NULL")
                        {
                            if (organizations.Contains(item.business_uid))
                            {
                                item.businessSelected = true;
                                businessSelected.Add(item.business_uid);
                            }
                            else
                            {
                                item.businessSelected = false;
                            }
                        }
                        else
                        {
                            item.businessSelected = false;
                        }

                        businesSource.Add(item);
                    }

                    if (businesSource.Count == 0)
                    {
                        organizationMessageLabel.IsVisible = true;
                        organizationMessageLabel.Text = "The are zero organization to select at the moment.";
                    }
                    else
                    {
                        organizationMessageLabel.IsVisible = false;
                    }

                    BindableLayout.SetItemsSource(organizationView, businesSource);
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

            var businessIDs = "";
            foreach (string id in businessSelected)
            {
                businessIDs += id + ",";
            }


            if (businessIDs != "")
            {
                account["organizations"] = "";
                businessIDs = businessIDs.Remove(businessIDs.Length - 1);
            }

            account["street"] = addressToValidate.Street;
            account["latitude"] = addressToValidate.Latitude.ToString();
            account["longitude"] = addressToValidate.Longitude.ToString();
            account["social_id"] = driver == null && driver.result.Count == 0 ? null : driver.result[0].social_id;
            account["platform"] = user.platform;
            account["driver_uid"] = user.id;
            account["organizations"] = businessIDs;
            account["referal"] = driver == null && driver.result.Count == 0 ? null: driver.result[0].referral_source;
            account["schedule"] = driver == null && driver.result.Count == 0 ? null : driver.result[0].driver_available_hours;

            foreach (string key in account.Keys)
            {
                Debug.WriteLine("KEY: {0}, VALUE: {1}", key, account[key]);
            }

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



        //bool ValidateEntries()
        //{
        //    // remove || String.IsNullOrEmpty(address.Text)
        //    bool result = false;
        //    if (!(
        //         String.IsNullOrEmpty(firstName.Text)
        //      || String.IsNullOrEmpty(lastName.Text)
        //      || String.IsNullOrEmpty(phoneNumber.Text)
        //      || String.IsNullOrEmpty(email.Text)
        //      || String.IsNullOrEmpty(city.Text)
        //      || String.IsNullOrEmpty(state.Text)
        //      || String.IsNullOrEmpty(zipcode.Text)
        //      || String.IsNullOrEmpty(emergencyFirstName.Text)
        //      || String.IsNullOrEmpty(emergencyLastName.Text)
        //      || String.IsNullOrEmpty(emergencyRelationship.Text)
        //      || String.IsNullOrEmpty(emergencyPhoneNumber.Text)
        //      || String.IsNullOrEmpty(ssNumber.Text)
        //      || String.IsNullOrEmpty(carYear.Text)
        //      || String.IsNullOrEmpty(carModel.Text)
        //      || String.IsNullOrEmpty(carMake.Text)
        //      || String.IsNullOrEmpty(insuranceCarrier.Text)
        //      || String.IsNullOrEmpty(insuranceNumber.Text)
        //      || String.IsNullOrEmpty(insuraceExpDate)
        //      || String.IsNullOrEmpty(driveLicenseNumber.Text)
        //      || String.IsNullOrEmpty(licenseExpDate)
        //      || String.IsNullOrEmpty(accountNumber.Text)
        //      || String.IsNullOrEmpty(routingNumber.Text)
        //      )
        //      )
        //    {
        //        if (businessSelected.Count != 0)
        //        {
        //            if (insurancePicture != null)
        //            {
        //                if (addressToValidate != null && addressToValidate.isValidated == true)
        //                {
        //                    result = true;
        //                }
        //            }
        //        }

        //    }
        //    return result;


        //    //|| String.IsNullOrEmpty(emergencyAddress.Text)
        //    //|| String.IsNullOrEmpty(emergencyCity.Text)
        //    //|| String.IsNullOrEmpty(emergencyUnit.Text)
        //    //|| String.IsNullOrEmpty(emergencyState.Text)
        //    //|| String.IsNullOrEmpty(emergencyZipcode.Text)
        //}

        bool ValidatePassword()
        {
            bool result = false;
            if (!(String.IsNullOrEmpty(Password1Entry.Text) || String.IsNullOrEmpty(Password1Entry.Text)))
            {
                if (Password1Entry.Text == Password1Entry.Text)
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
            var image = (Button)sender;

            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                if (photo != null)
                {

                    var path = photo.Path;
                    InsuranceImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
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
            var entry = (Entry)sender;
            var frame = (Frame)entry.Parent;

            if (displayAddress)
            {
                if (!String.IsNullOrEmpty(StreetEntry.Text))
                {
                    if (frame.BorderColor == Color.LightGray)
                    {
                        frame.BorderColor = Color.Red;
                    }

                    if (addressToValidate != null)
                    {
                        if (addressToValidate.Street != StreetEntry.Text)
                        {
                            AddressListView.ItemsSource = await addr.GetPlacesPredictionsAsync(StreetEntry.Text);
                            addressEntryFocused(sender, eventArgs);
                        }
                    }
                    else
                    {
                        AddressListView.ItemsSource = await addr.GetPlacesPredictionsAsync(StreetEntry.Text);
                        addressEntryFocused(sender, eventArgs);
                    }
                }
                else
                {
                    if (frame.BorderColor == Color.Red)
                    {
                        frame.BorderColor = Color.LightGray;
                        SetAccountValues(entry.ClassId, null);
                    }

                    addressEntryUnfocused(sender, eventArgs);
                    addressToValidate = null;
                }
            }
            else
            {
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
                        SetAccountValues(entry.ClassId, null);
                    }
                }
            }
        }

        void addressEntryFocused(object sender, EventArgs eventArgs)
        {
          
            if (!String.IsNullOrEmpty(StreetEntry.Text))
            {
                displayAddress = true;
                addr.addressEntryFocused(AddressListView, addressFrame);
            }
        }

        void addressEntryUnfocused(object sender, EventArgs eventArgs)
        {
            addr.addressEntryUnfocused(AddressListView, addressFrame);
        }

        async void addressSelected(System.Object sender, SelectedItemChangedEventArgs e)
        {
            StreetEntry.TextChanged -= OnAddressChanged;
            addressToValidate = addr.addressSelected(AddressListView, StreetEntry, addressFrame);
            addressToValidate.isValidated = false;
            StreetEntry.Text = addressToValidate.Street;
            string zipcode = await addr.getZipcode(addressToValidate.PredictionID);
            if (zipcode != null)
            {
                addressToValidate.ZipCode = zipcode;
            }
            StreetEntry.TextChanged += OnAddressChanged;
            ValidateAddress(sender, e);
        }

        async void ValidateAddress(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (addressToValidate != null)
                {
                    if (!String.IsNullOrEmpty(StreetEntry.Text))
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
                                    StreetEntry.Text = addressToValidate.Street;
                                    UnitEntry.Text = addressToValidate.Unit;
                                    CityEntry.Text = addressToValidate.City;
                                    StateEntry.Text = addressToValidate.State;
                                    ZipcodeEntry.Text = addressToValidate.ZipCode;

                                    account["street"] = addressToValidate.Street;
                                    account["latitude"] = addressToValidate.Latitude.ToString();
                                    account["longitude"] = addressToValidate.Longitude.ToString();

                                    addressToValidate.isValidated = true;

                                    addressValidationClient.SetPinOnMap(MapView, new Xamarin.Essentials.Location(addressToValidate.Latitude, addressToValidate.Longitude), addressToValidate.Street);
                                }
                                else if (addressStatus == "D")
                                {
                                    var unit1 = await DisplayPromptAsync("It looks like your address is missing its unit number", "Please enter your address unit number in the space below", "OK", "Cancel");
                                    if (unit1 != null)
                                    {
                                        await DisplayAlert("Great!", "Your address is valid. Please continue with your application", "OK");
                                        addressToValidate.Unit = unit1;
                                        StreetEntry.Text = addressToValidate.Street;
                                        UnitEntry.Text = addressToValidate.Unit;
                                        CityEntry.Text = addressToValidate.City;
                                        StateEntry.Text = addressToValidate.State;
                                        ZipcodeEntry.Text = addressToValidate.ZipCode;

                                        account["street"] = addressToValidate.Street;
                                        account["latitude"] = addressToValidate.Latitude.ToString();
                                        account["longitude"] = addressToValidate.Longitude.ToString();

                                        addressToValidate.isValidated = true;

                                        addressValidationClient.SetPinOnMap(MapView, new Xamarin.Essentials.Location(addressToValidate.Latitude, addressToValidate.Longitude), addressToValidate.Street);
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

            if (e.NewTextValue != null)
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

                            SetAccountValues(entry.ClassId, localInput.Trim());

                        }
                        else
                        {
                            DisplayAlert("Oops", "You enter a non numberical value. Please enter 0-9 digits.", "OK");
                            entry.Text = e.OldTextValue;
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
