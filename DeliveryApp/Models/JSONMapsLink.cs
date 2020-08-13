using System;
using System.Collections.Generic;

namespace DeliveryApp.Models
{
    public class Links
    {
        public string link { get; set; }
    }

    public class MapsLink
    {
        public string message { get; set; }
        public IList<Links> result { get; set; }
    }

}
