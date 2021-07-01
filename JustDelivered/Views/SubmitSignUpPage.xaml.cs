using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.Models;
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

        public SubmitSignUpPage()
        {
            InitializeComponent();
            SetSchedule(scheduleView);
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

        //async Task<bool> SavePhoto()
        //{
        //    var client = new HttpClient();
        //    var content = new MultipartFormDataContent();
        //    //var purchase_uid = new StringContent(purchaseId, Encoding.UTF8);
        //    var userImageContent = new ByteArrayContent(insurancePhotoBiteArray);

        //    //content.Add(purchase_uid, "purchase_uid");

        //    // CONTENT, NAME, FILENAME
        //    content.Add(userImageContent, "driver_insurance_picture", "product_image.png");

        //    var request = new HttpRequestMessage();

        //    request.RequestUri = new Uri(Constant.SignUpUrl);
        //    request.Method = HttpMethod.Post;
        //    request.Content = content;

        //    var response = await client.SendAsync(request);
        //    return true;
        //}
    }
}
