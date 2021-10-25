using System;
namespace JustDelivered.Models
{
    public class Account
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string driver_uid { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class FastSignUpAccount
    {
        public string message { get; set; }
        public int code { get; set; }
        public string sql { get; set; }
        public Account result { get; set; }
    }
}
