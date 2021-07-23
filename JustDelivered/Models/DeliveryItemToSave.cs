using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    
    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class DeliveryItemToSave
    {
        public string email { get; set; }
        public List<ItemToSave> items { get; set; }
        public string phone { get; set; }
        public Coordinates coordinates { get; set; }
        public string customer_uid { get; set; }
        public string delivery_zip { get; set; }
        public string purchase_uid { get; set; }
        public string delivery_city { get; set; }
        public string delivery_unit { get; set; }
        public string delivery_state { get; set; }
        public string delivery_status { get; set; }
        public string delivery_street { get; set; }
        public string delivery_last_name { get; set; }
        public string delivery_first_name { get; set; }
        public string start_delivery_date { get; set; }
        public string delivery_instructions { get; set; }
    }
}
