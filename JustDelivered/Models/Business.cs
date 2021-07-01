using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JustDelivered.Models
{
    public class Business
    {
        public string message { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Item> result { get; set; }
        public string sql { get; set; }
    }

    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string business_uid { get; set; }
        public DateTime business_created_at { get; set; }
        public string business_name { get; set; }
        public string business_type { get; set; }
        public string business_desc { get; set; }
        public object business_association { get; set; }
        public string business_contact_first_name { get; set; }
        public string business_contact_last_name { get; set; }
        public string business_phone_num { get; set; }
        public string business_phone_num2 { get; set; }
        public string business_email { get; set; }
        public string business_hours { get; set; }
        public string business_accepting_hours { get; set; }
        public string business_delivery_hours { get; set; }
        public string business_address { get; set; }
        public string business_unit { get; set; }
        public string business_city { get; set; }
        public string business_state { get; set; }
        public string business_zip { get; set; }
        public string business_longitude { get; set; }
        public string business_latitude { get; set; }
        public string business_EIN { get; set; }
        public string business_WAUBI { get; set; }
        public string business_license { get; set; }
        public string business_USDOT { get; set; }
        public string bus_notification_approval { get; set; }
        public int can_cancel { get; set; }
        public int delivery { get; set; }
        public int reusable { get; set; }
        public string business_image { get; set; }
        public string business_password { get; set; }
        public string bus_guid_device_id_notification { get; set; }
        public double platform_fee { get; set; }
        public double transaction_fee { get; set; }
        public double revenue_sharing { get; set; }
        public double profit_sharing { get; set; }
        public bool businessSelected { get; set; }

        public bool updateBusinessSelected
        {
            set
            {
                businessSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("businessSelected"));
            }
        }
    }  
}
