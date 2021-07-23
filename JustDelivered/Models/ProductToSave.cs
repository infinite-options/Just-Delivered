using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class ProductToSave
    {
        public string isEnable { get; set; }
        public string icon { get; set; }
        public List<CustomerToSave> customersSource { get; set; }
    }
}
