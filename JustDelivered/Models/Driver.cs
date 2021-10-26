using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class Driver
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Profile> result { get; set; }
        public string sql { get; set; }
    }

    public class Profile
    {
        public string driver_uid { get; set; }
        public string driver_first_name { get; set; }
        public string driver_last_name { get; set; }
        public string business_id { get; set; }
        public string referral_source { get; set; }
        public string driver_available_hours { get; set; }
        public object driver_scheduled_hours { get; set; }
        public string driver_street { get; set; }
        public string driver_unit { get; set; }
        public string driver_city { get; set; }
        public string driver_state { get; set; }
        public string driver_zip { get; set; }
        public string driver_latitude { get; set; }
        public string driver_longitude { get; set; }
        public string driver_phone_num { get; set; }
        public string driver_email { get; set; }
        public object driver_phone_num2 { get; set; }
        public string driver_ssn { get; set; }
        public string driver_license { get; set; }
        public string driver_license_exp { get; set; }
        public string driver_insurance_carrier { get; set; }
        public string driver_insurance_num { get; set; }
        public string driver_insurance_exp_date { get; set; }
        public string driver_insurance_picture { get; set; }
        public string emergency_contact_name { get; set; }
        public string emergency_contact_phone { get; set; }
        public string emergency_contact_relationship { get; set; }
        public string bank_routing_info { get; set; }
        public string bank_account_info { get; set; }
        public string password_salt { get; set; }
        public string password_hashed { get; set; }
        public string password_algorithm { get; set; }
        public string driver_created_at { get; set; }
        public object email_verified { get; set; }
        public string social_id { get; set; }
        public string user_social_media { get; set; }
        public string user_access_token { get; set; }
        public string user_refresh_token { get; set; }
        public string mobile_access_token { get; set; }
        public string mobile_refresh_token { get; set; }
        public string social_timestamp { get; set; }
        public string user_guid_device_id_notification { get; set; }
        public string driver_car_year { get; set; }
        public string driver_car_model { get; set; }
        public string driver_car_make { get; set; }
    }
}
