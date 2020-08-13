using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Models
{
    public class Result
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public string apt { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public double Total_Items { get; set; }
        public string delivery_note { get; set; }
        public string delivery_date { get; set; }

        // String first_last_name puts fist and last name as one string.
        public string first_last_name { get { return first_name + " " + last_name; } }

        // String city_state_zipcode put city, state, and zipcode as one string.
        public string city_state_zipcode { get { return city + ", " + state + ", " + zipcode; } }

        // String total_items puts the number of items and its correspoing value as one string.
        // This string also knows how to determine where to add an s or not at the end of the word "item".
        // Need to have one person to order one item. 
        public string total_items
        {
            get
            {
                if (Total_Items == 1)
                {
                    return Total_Items + " Item";
                }
                else
                {
                    return Total_Items + " Items";
                }
            }
        }

        // Here we can also process which orders to process today using the date from the phone.

        // String formats the phone property as (area code) xxx - xxxx.
        public string formated_phone_number
        {
            get
            {   if (phone_number.Length == 10)
                {
                    string formatedPhone = "(" + phone_number.Substring(0, 3) + ") " + phone_number.Substring(3, 3) + " - " + phone_number.Substring(6, 4);
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

    public class Result1
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Result> result { get; set; }
        public string sql { get; set; }
    }

    public class Example
    {
        public string message { get; set; }
        public Result1 result { get; set; }
    }
}
