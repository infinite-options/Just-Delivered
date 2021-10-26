using System;
namespace JustDelivered.Models
{
    public class ProfileInput
    {
        public string driver_uid { get; set; }

        public ProfileInput()
        {
            driver_uid = "";
        }
    }
}
