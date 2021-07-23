using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace JustDelivered.Models
{
    public class ProductItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string item_uid { get; set; }
        public string img { get; set; }
        public Color color { get; set; }
        public string title { get; set; }
        public int quantityInt { get; set; }
        public string quantityStr { get; set; }
        public  IList<Customer> customers { get;set; }
        public bool isEnable { get; set; }
        public string sortedStatusIcon { get; set; }

        public bool isEnableUpdate
        {
            set
            {
                isEnable = value;
                PropertyChanged(this, new PropertyChangedEventArgs("isEnable"));
            }
        }

        public string sortedStatusIconUpdate
        {
            set
            {
                sortedStatusIcon = value;
                PropertyChanged(this, new PropertyChangedEventArgs("sortedStatusIcon"));
            }
        }
    }


}
