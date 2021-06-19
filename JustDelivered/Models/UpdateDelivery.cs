using System;
namespace JustDelivered.Models
{
    public class UpdateDelivery
    {
        public string purchase_uid { get; set; }
        public string cmd { get; set; }
        public string note { get; set; }
    }
}
