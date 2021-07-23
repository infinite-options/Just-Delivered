using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class UpdateDeliveryRoute
    {
        public string route_id { get; set; }
        public Dictionary<string, List<DeliveryItemToSave>> route { get; set; }
    }
}
