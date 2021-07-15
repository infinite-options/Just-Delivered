using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace JustDelivered.Models
{
    public class ProductItem
    {
        public string img { get; set; }
        public Color color { get; set; }
        public string title { get; set; }
        public int quantityInt { get; set; }
        public string quantityStr { get; set; }
        public  IList<Customer> customers { get;set; }
    }
}
