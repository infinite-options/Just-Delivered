using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JustDelivered.Models;
using Xamarin.Forms;
using static JustDelivered.Views.ProductsPage;

namespace JustDelivered.Views
{
    public partial class SummaryPage : ContentPage
    {
        public ObservableCollection<SummaryItem> summarySource = new ObservableCollection<SummaryItem>();

        public SummaryPage()
        {
            InitializeComponent();
            BackgroundColor = Color.FromHex("AB000000");
            foreach (ProductionDetails businessDetails in businessSource)
            {
                var item = new SummaryItem();
                item.businessName = businessDetails.bussinessName;
                item.productToGet = new ObservableCollection<ProductDetails>();
                foreach (ProductItem product in businessDetails.productSource)
                {
                    var totalQuantity = 0;
                    foreach (Customer customer in product.customers)
                    {
                        if(customer.borderColor != Color.Red)
                        {
                            totalQuantity += ConvertStringToInt(customer.qty);
                        }
                    }

                    if(totalQuantity != 0)
                    {
                        item.productToGet.Add(new ProductDetails {productImage = product.img, productName = product.title, productQuantity = totalQuantity.ToString() });
                    }
                }

                if(item.productToGet.Count != 0)
                {
                    summarySource.Add(item);
                }
               
            }

            if(summarySource.Count != 0)
            {
                message.Text = "These are the items you are missing. Please go back to each business shown below and collect all the missing items before viewing your deliveries route.";
            }
            else
            {
                message.Text = "Awesome! You collected all the items to be delivered. Please press the 'continue' button to view your deliveries routes.";
            }
            summaryList.ItemsSource = summarySource;
            
        }

        int ConvertStringToInt(string quantity)
        {
            var result = 0;
            try
            {
                result = Int16.Parse(quantity);
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        async void ProcessRequest(System.Object sender, System.EventArgs e)
        {
            
            if (summarySource.Count != 0)
            {
                bool result = await DisplayAlert("Oops", "It seems like you still have some items to collect.", "Continue Anyways", "Collect Missing Items");
                if(!result)
                {
                    await Navigation.PopModalAsync();
                }
                else { 
                    Application.Current.MainPage = new DeliveriesPage();
                }
            }
            else
            {
                Application.Current.MainPage = new DeliveriesPage();
            }
        }

        void NavigateToProductsPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
