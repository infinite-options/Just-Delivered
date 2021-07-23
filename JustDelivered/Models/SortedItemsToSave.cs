using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class SortedItemsToSave
    {
        public string action { get; set; }
        public Dictionary<string, ProductionDetailsToSave> sorted_produce { get; set; }
        public string route_id { get; set; }
    }
}
