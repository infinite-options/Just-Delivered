using System;
using System.Collections.Generic;

namespace JustDelivered.Models
{
    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class SaveDrivePath
    {
        public string action { get; set; }
        public string route_id { get; set; }
        public List<Models.Point> directions { get; set; }
    }
}
