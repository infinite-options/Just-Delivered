using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JustDelivered.Models
{
    public class ProductionDetailsToSave
    {
        public Dictionary<string, ProductToSave> productSource { get; set; }
    }
}
