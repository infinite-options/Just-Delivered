using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Models
{
    public class Item
    {

        public int route_id { get; set; }
        public int driver_id { get; set; }
        public string name { get; set; }
        public string house_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string delivery_instructions { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string latitude { get; set; } 
        public string longitude { get; set; }

        public string ParsedPhone
        {
            get
            {
                if (phone.Length != 12)
                {
                    return "(XXX) XXX - XXXX";
                }
                else if (phone.Length == 12)
                {
                    string formatedPhone = "(" + phone.Substring(0, 3) + ") " + phone.Substring(4, 3) + " - " + phone.Substring(8, 4);
                    return formatedPhone;
                }
                else
                {
                    string noPhoneNumber = "Phone # Not Available";
                    return noPhoneNumber;
                }
            }
        }

        public string phoneToCall
        {
            get
            {
                if (phone.Length != 12)
                {
                    return "(XXX) XXX - XXXX";
                }
                else if (phone.Length == 12)
                {
                    string formatedPhone = phone.Substring(0, 3) + phone.Substring(4, 3) + phone.Substring(8, 4);
                    return formatedPhone;
                }
                else
                {
                    string noPhoneNumber = "Phone # Not Available";
                    return noPhoneNumber;
                }
            }
        }

    }

    public class Response
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Item> result { get; set; }
        public string sql { get; set; }
    }

    public class Customers
    {
        public string message { get; set; }
        public Response result { get; set; }
    }
}
