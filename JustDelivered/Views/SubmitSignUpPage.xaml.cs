using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using static JustDelivered.Views.SignUpPage;

namespace JustDelivered.Views
{
    public partial class SubmitSignUpPage : ContentPage
    {
        public ObservableCollection<Schedule> scheduleSource = new ObservableCollection<Schedule>();

        public ObservableCollection<PickerTimeHour> hourSource = new ObservableCollection<PickerTimeHour>();
        public ObservableCollection<PickerTimeMinute> minuteSource = new ObservableCollection<PickerTimeMinute>();
        public ObservableCollection<PickerTime> timeSource = new ObservableCollection<PickerTime>();
        public Dictionary<string, List<string>> selectedSchedule = new Dictionary<string, List<string>>();

        public SubmitSignUpPage()
        {
            InitializeComponent();
            SetSchedule(scheduleView);
            SetDictionary();
        }

        void SetSchedule(CollectionView view) 
        {
            SetHours();
            SetMinutes();
            SetTime();

            string[] weekdays = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            foreach(string day in weekdays)
            {
                scheduleSource.Add(new Schedule {
                    day = day,
                    startHour = hourSource,
                    startMinute = minuteSource,
                    startTime = timeSource,
                    endHour = hourSource,
                    endMinute = minuteSource,
                    endTime = timeSource,
                });;
            }
            view.ItemsSource = scheduleSource;
        }

        void SetDictionary()
        {
            string[] weekdays = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            foreach (string day in weekdays)
            {
                selectedSchedule.Add(day, new List<string>());
            }

        }

        void SetHours()
        {
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    string value = "";
                    if (i <= 9)
                    {
                        value = "0" + i;
                    }
                    else
                    {
                        value = i + "";
                    }
                    hourSource.Add(new PickerTimeHour { hour = value });
                }
                
            }
            catch (Exception setHourIssue)
            {
                Debug.WriteLine("Error: " + setHourIssue.Message);
            }
            
        }

        void SetMinutes()
        {
            try
            {
                minuteSource.Add(new PickerTimeMinute { minute = "00" });
                minuteSource.Add(new PickerTimeMinute { minute = "15" });
                minuteSource.Add(new PickerTimeMinute { minute = "30" });
                minuteSource.Add(new PickerTimeMinute { minute = "45" });
                
            }
            catch (Exception setHourIssue)
            {
                Debug.WriteLine("Error: " + setHourIssue.Message);
            }
           
        }

       void  SetTime()
        {
            try
            {
                timeSource.Add(new PickerTime { time = "AM" });
                timeSource.Add(new PickerTime { time = "PM" });
               
            }
            catch (Exception setHourIssue)
            {
                Debug.WriteLine("Error: " + setHourIssue.Message);
            }
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PopAsync(false);
        }

        void hourList_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
            var list = (CollectionView)sender;
            list.ScrollTo(hourSource[e.CenterItemIndex]);
        }

        void minuteList_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
            var list = (CollectionView)sender;
            list.ScrollTo(minuteSource[e.CenterItemIndex]);
        }

        void timeList_Scrolled(System.Object sender, Xamarin.Forms.ItemsViewScrolledEventArgs e)
        {
            var list = (CollectionView)sender;
            list.ScrollTo(timeSource[e.CenterItemIndex]);
        }

        async Task<bool> ProcessRequest()
        {
            var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var scheduleToSubmit = new ScheduleToSubmit();

            scheduleToSubmit.sunday = selectedSchedule["Sunday"];
            scheduleToSubmit.monday = selectedSchedule["Monday"];
            scheduleToSubmit.tuesday = selectedSchedule["Tuesday"];
            scheduleToSubmit.wednesday = selectedSchedule["Wednesday"];
            scheduleToSubmit.thursday = selectedSchedule["Thursday"];
            scheduleToSubmit.friday = selectedSchedule["Friday"];
            scheduleToSubmit.saturday = selectedSchedule["Saturday"];

            var scheduleToSubmitString = JsonConvert.SerializeObject(scheduleToSubmit);
            var businessIDs = "";
            foreach(string id in businessSelected)
            {
                businessIDs += id + ",";
            }

            if(businessIDs != "")
            {
                businessIDs = businessIDs.Remove(businessIDs.Length - 1);
            }

            var account = JsonConvert.DeserializeObject<SignUp>(accountString);

            var first_name = new StringContent(account.first_name, Encoding.UTF8);
            var last_name = new StringContent(account.last_name, Encoding.UTF8);
            var business_uid = new StringContent(businessIDs, Encoding.UTF8);
            var driver_hours = new StringContent(scheduleToSubmitString, Encoding.UTF8);
            var street = new StringContent(account.street, Encoding.UTF8);
            var city = new StringContent(account.city, Encoding.UTF8);
            var state = new StringContent(account.state, Encoding.UTF8);
            var zipcode = new StringContent(account.zipcode, Encoding.UTF8);
            var email = new StringContent(account.email, Encoding.UTF8);
            var phone = new StringContent(account.phone, Encoding.UTF8);
            var ssn = new StringContent(account.ssn, Encoding.UTF8);
            var license_num = new StringContent(account.license_num, Encoding.UTF8);
            var license_exp = new StringContent(account.license_exp, Encoding.UTF8);
            var driver_car_year = new StringContent(account.driver_car_year, Encoding.UTF8);
            var driver_car_model = new StringContent(account.driver_car_model, Encoding.UTF8);
            var driver_car_make = new StringContent(account.driver_car_make, Encoding.UTF8);
            var driver_insurance_carrier = new StringContent(account.driver_insurance_carrier, Encoding.UTF8);
            var driver_insurance_num = new StringContent(account.driver_insurance_num, Encoding.UTF8);
            var driver_insurance_exp_date = new StringContent(account.driver_insurance_exp_date, Encoding.UTF8);
            var contact_name = new StringContent(account.contact_name, Encoding.UTF8);
            var contact_phone = new StringContent(account.contact_phone, Encoding.UTF8);
            var contact_relation = new StringContent(account.contact_relation, Encoding.UTF8);
            var bank_acc_info = new StringContent(account.bank_acc_info, Encoding.UTF8);
            var bank_routing_info = new StringContent(account.bank_routing_info, Encoding.UTF8);
            var password = new StringContent(account.password, Encoding.UTF8);
            var social = new StringContent(account.social, Encoding.UTF8);
            var mobile_access_token = new StringContent(account.mobile_access_token, Encoding.UTF8);
            var mobile_refresh_token = new StringContent(account.mobile_refresh_token, Encoding.UTF8);
            var user_access_token = new StringContent(account.user_access_token, Encoding.UTF8);
            var user_refresh_token = new StringContent(account.user_refresh_token, Encoding.UTF8);
            var social_id = new StringContent(account.social_id, Encoding.UTF8);
            var userImageContent = new ByteArrayContent(insurancePicture);

            // CONTENT, NAME
            content.Add(first_name, "first_name");
            content.Add(last_name, "last_name");
            content.Add(business_uid, "business_uid");
            content.Add(driver_hours, "driver_hours");
            content.Add(street, "street");
            content.Add(city, "city");
            content.Add(state, "state");
            content.Add(zipcode, "zipcode");
            content.Add(email, "email");
            content.Add(phone, "phone");
            content.Add(ssn, "ssn");
            content.Add(license_num, "license_num");
            content.Add(license_exp, "license_exp");
            content.Add(driver_car_year, "driver_car_year");
            content.Add(driver_car_model, "driver_car_model");
            content.Add(driver_car_make, "driver_car_make");
            content.Add(driver_insurance_carrier, "driver_insurance_carrier");
            content.Add(driver_insurance_num, "driver_insurance_num");
            content.Add(driver_insurance_exp_date, "driver_insurance_exp_date");
            content.Add(contact_name, "contact_name");
            content.Add(contact_phone, "contact_phone");
            content.Add(contact_relation, "contact_relation");
            content.Add(bank_acc_info, "bank_acc_info");
            content.Add(bank_routing_info, "bank_routing_info");
            content.Add(password, "password");
            content.Add(social, "social");
            content.Add(mobile_access_token, "mobile_access_token");
            content.Add(mobile_refresh_token, "mobile_refresh_token");
            content.Add(user_access_token, "user_access_token");
            content.Add(user_refresh_token, "user_refresh_token");
            content.Add(social_id, "social_id");

            // CONTENT, NAME, FILENAME
            content.Add(userImageContent, "driver_insurance_picture", "product_image.png");

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(Constant.SignUpUrl);
            request.Method = HttpMethod.Post;
            request.Content = content;

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("contentString: " + contentString);
                return true;
            }
            return false;
        }

        async void SubmitApplication(System.Object sender, System.EventArgs e)
        {
            var result = await ProcessRequest();
            if (result)
            {
                await DisplayAlert("Congratulations!", "Your application is in process. We will notify you of your result via email.", "OK");
                Application.Current.MainPage = new LogInPage();
            }
            else
            {
                await DisplayAlert("Oops", "Unfortunately, we weren't able to sign you up. Please try again later.", "OK");
            }
        }
    }
}
