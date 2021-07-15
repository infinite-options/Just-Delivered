using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace JustDelivered.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string customer_first_name { get; set; }
        public string customer_last_name { get; set; }
        public string customer_uid { get; set; }
        public string customer_address { get; set; }
        public string customer_unit { get; set; }
        public string qty { get; set; }

        public string customerName
        {
            get
            {
                return customer_first_name + " " + customer_last_name;
            }
        }

        public Color borderColor { get; set; }

        public Color backgroundColor { get; set; }

        public Color updateBorderColor
        {
            set
            {
                borderColor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("borderColor"));
            }
        }

        public Color updateBackgroundColor
        {
            set
            {
                backgroundColor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("backgroundColor"));
            }
        }
    }

    public class Details
    {
        public IList<Customer> customers { get; set; }
        public int qty { get; set; }
        public string business_name { get; set; }
        public string business_uid { get; set; }
        public string item { get; set; }
        public string item_uid { get; set; }
        public string item_img { get; set; }
        public string item_unit { get; set; }
        public double item_business_price { get; set; }
    }

    public class ItemsToSort
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Details> result { get; set; }
        public string sql { get; set; }
    }
}
