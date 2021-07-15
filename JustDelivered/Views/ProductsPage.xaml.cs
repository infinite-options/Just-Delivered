using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using JustDelivered.Config;
using Newtonsoft.Json;
using Xamarin.Forms;
using JustDelivered.Models;
using System.Collections.ObjectModel;

namespace JustDelivered.Views
{
    public partial class ProductsPage : ContentPage
    {
        public static ObservableCollection<ProductionDetails> businessSource = new ObservableCollection<ProductionDetails>();
        public static ProductItem productSelected = null;
        public static Dictionary<string, ProductionDetails> businesses = new Dictionary<string, ProductionDetails>();

        public ProductsPage()
        {
            InitializeComponent();
            GetProducts();
        }

        async void GetProducts()
        {
            businessSource.Clear();
            businesses.Clear();
            var currentDate = DateTime.Now;

            for (int i = 0; i < 7; i++)
            {
                if (currentDate.DayOfWeek == DayOfWeek.Wednesday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    break;
                }
                currentDate = currentDate.AddDays(1);
            }

            //socialLogInPost.email = user.email;
            //socialLogInPost.social_id = user.socialId;

            //Debug.WriteLine("Current Date: " + currentDate.ToString("yyyy-MM-dd"));
            // LIVE
            //string date = currentDate.ToString("yyyy-MM-dd");

            // TEST
            string date = currentDate.ToString("2021-07-18");

            var client = new HttpClient();
            var endpointCall = await client.GetAsync(Constant.AllProductToBeSorted + date);

            if (endpointCall.IsSuccessStatusCode)
            {
                var contentString = await endpointCall.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ItemsToSort>(contentString);

                foreach(Details details in data.result)
                {

                    if (businesses.ContainsKey(details.business_uid))
                    {
                        var color = Color.Black;
                        if (details.qty > 1)
                        {
                            color = Color.Red;
                        }
                        foreach (Customer customer in details.customers)
                        {
                            customer.borderColor = Color.Black;
                            customer.backgroundColor = Color.White;
                        }

                        businesses[details.business_uid].productSource.Add(new ProductItem
                        {
                            color = color,
                            quantityInt = details.qty,
                            quantityStr = details.qty.ToString(),
                            title = details.item,
                            img = details.item_img,
                            customers = details.customers

                        });
                    }
                    else
                    {
                        var color = Color.Black;
                        if (details.qty > 1)
                        {
                            color = Color.Red;
                        }
                        foreach (Customer customer in details.customers)
                        {
                            customer.borderColor = Color.Black;
                            customer.backgroundColor = Color.White;
                        }

                        businesses.Add(details.business_uid, new ProductionDetails
                        {
                            businessID = details.business_uid,
                            bussinessName = details.business_name,
                            productSource = new ObservableCollection<ProductItem>()
                        });

                        businesses[details.business_uid].productSource.Add(new ProductItem
                        {
                            color = color,
                            quantityInt = details.qty,
                            quantityStr = details.qty.ToString(),
                            title = details.item,
                            img = details.item_img,
                            customers = details.customers

                        });
                    }
                }

                foreach(string ID in businesses.Keys)
                {
                    float size = businesses[ID].productSource.Count;
                    float rows = 0;
                    float d = 4;
                    if (size != 0)
                    {
                        if (size > 0 && size <= 4)
                        {
                             rows = 1;
                        }
                        else
                        {
                            rows = size / d;
                        }
                    }

                    double height = (double)(Math.Ceiling(rows) * 130);
                    businesses[ID].viewHeight = height;
                    businessSource.Add(businesses[ID]);
                }

                ItemList.ItemsSource = businessSource;
            }
        }

        void SelectItemToSort(System.Object sender, System.EventArgs e)
        {
            var view = (StackLayout)sender;
            var recognizer = (TapGestureRecognizer)view.GestureRecognizers[0];
            var product = (ProductItem)recognizer.CommandParameter;

            productSelected = product;

            Navigation.PushModalAsync(new CustomersPage());
        }

        void NavigateToSummaryPage(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SummaryPage());
        }
    }
}
