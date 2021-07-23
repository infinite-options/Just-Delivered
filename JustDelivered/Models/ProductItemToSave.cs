using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class ProductItemToSave
    {
        public string item_uid { get; set; }
        public IList<CustomerToSave> customers { get; set; }
    }
}
