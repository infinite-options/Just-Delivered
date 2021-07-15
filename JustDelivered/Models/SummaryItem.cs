using System;
using System.Collections.ObjectModel;

namespace JustDelivered.Models
{
    public class SummaryItem
    {
        public string businessName { get; set; }
        public ObservableCollection<ProductDetails> productToGet { get; set; }
    }
}
