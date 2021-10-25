using System;
using System.Diagnostics;

namespace JustDelivered.Models
{
    public class User
    {
        public string id;
        public string email;
        public string socialId;
        public string platform;
        public string route_id;
        public DateTime sessionTime;

        public User()
        {
            id = "";
            email = "";
            socialId = "";
            platform = "";
            route_id = "";
            sessionTime = new DateTime();
        }

        public void PrintUser()
        {
            Debug.WriteLine("Driver ID: " + id);
            Debug.WriteLine("Driver email: " + email);
            Debug.WriteLine("Driver socialId: " + socialId);
            Debug.WriteLine("Driver platform: " + platform);
            Debug.WriteLine("Driver route_id: " + route_id);
        }
    }
}
