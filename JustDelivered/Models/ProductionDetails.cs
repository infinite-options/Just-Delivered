using System;
using System.Collections.ObjectModel;

namespace JustDelivered.Models
{
    public class ProductionDetails
    {
        public string bussinessName { get; set; }
        public string businessID { get; set; }
        public double totalAmountToPay { get; set; }
        public string totalAmountToPayStr { get; set; }
        
        public ObservableCollection<ProductItem> productSource { get; set; }
        public double viewHeight { get; set; }
    }
}
