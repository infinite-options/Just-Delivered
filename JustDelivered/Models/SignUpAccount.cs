using System;
using System.Diagnostics;

namespace JustDelivered.Models
{
    public class SignUpAccount
    {
        public string socialID { get; set; }
        public string socialEmail { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string profilePicture { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string platform { get; set; }

        public SignUpAccount()
        {
            socialID = "";
            socialEmail = "";
            firstName = "";
            lastName = "";
            profilePicture = "";
            accessToken = "";
            refreshToken = "";
            platform = "";
        }

        public void Print()
        {
            Debug.WriteLine("socialID: " + socialID);
            Debug.WriteLine("socialEmail: " + socialEmail);
            Debug.WriteLine("firstName: " + firstName);
            Debug.WriteLine("lastName: " + lastName);
            Debug.WriteLine("profilePicture: " + profilePicture);
            Debug.WriteLine("accessToken: " + accessToken);
            Debug.WriteLine("refreshToken: " + refreshToken);
            Debug.WriteLine("platform: " + platform);
        }
    }
}
