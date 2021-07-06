using System;
namespace JustDelivered.Models
{
    public class SignUp
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string business_uid { get; set; }
        public string referral_source { get; set; }
        public string driver_hours { get; set; }
        public string street { get; set; }
        public string unit { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string ssn { get; set; }
        public string license_num { get; set; }
        public string license_exp { get; set; }
        public string driver_car_year { get; set; }
        public string driver_car_model { get; set; }
        public string driver_car_make { get; set; }
        public string driver_insurance_carrier { get; set; }
        public string driver_insurance_num { get; set; }
        public string driver_insurance_exp_date { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_relation { get; set; }
        public string bank_acc_info { get; set; }
        public string bank_routing_info { get; set; }
        public string password { get; set; }
        public string mobile_access_token { get; set; }
        public string mobile_refresh_token { get; set; }
        public string user_access_token { get; set; }
        public string user_refresh_token { get; set; }
        public string social_id { get; set; }
        public string social { get; set; }
    }
}
