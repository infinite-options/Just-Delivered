using System;
namespace JustDelivered.Models
{
    public class EmergencyContact
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public EmergencyContact()
        {

        }

        public EmergencyContact(string first, string second)
        {
            firstName = first;
            lastName = second;
        }
    }
}
