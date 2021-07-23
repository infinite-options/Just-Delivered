using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class SavedProduction
    {
        public string action { get; set; }
        public string route_id { get; set; }
    }

    public class Production
    {
        public string sorted_produce { get; set; }
    }

    public class StoredProduction
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Production> result { get; set; }
        public string sql { get; set; }
    }
}
