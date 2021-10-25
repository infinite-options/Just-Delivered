using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using JustDelivered.Config;
using Newtonsoft.Json;
using Xamarin.Forms;
using JustDelivered.Models;
using System.Collections.ObjectModel;
using static JustDelivered.Views.DeliveriesPage;
using System.Text;
using System.Threading.Tasks;

namespace JustDelivered.Views
{
    public partial class ProductsPage : ContentPage
    {
        public static ObservableCollection<ProductionDetails> businessSource = new ObservableCollection<ProductionDetails>();
        public static ProductItem productSelected = null;
        public static Dictionary<string, ProductionDetails> businesses = new Dictionary<string, ProductionDetails>();
        public static string isProductionSave = null;

        public ProductsPage()
        {
            InitializeComponent();
            GetProducts();
        }

        async void GetProducts()
        {
            try
            {
                isProductionSave = "FALSE";
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

                // LIVE
                string date = currentDate.ToString("yyyy-MM-dd");

                // TEST
                //string date = currentDate.ToString("2021-08-22");

                var client = new HttpClient();
                var endpointCall = await client.GetAsync(Constant.AllProductToBeSorted + date + "," + user.id);

                if (endpointCall.IsSuccessStatusCode)
                {
                    var contentString = await endpointCall.Content.ReadAsStringAsync();
                    Debug.WriteLine(contentString);
                    var data = JsonConvert.DeserializeObject<ItemsToSort>(contentString);

                    foreach (Details details in data.result)
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

                            var tempAmount = businesses[details.business_uid].totalAmountToPay;
                            businesses[details.business_uid].totalAmountToPay = tempAmount + details.qty * details.item_business_price;
                            businesses[details.business_uid].totalAmountToPayStr = (tempAmount + details.qty * details.item_business_price).ToString("N2");
                            businesses[details.business_uid].productSource.Add(new ProductItem
                            {
                                color = color,
                                quantityInt = details.qty,
                                quantityStr = details.qty.ToString(),
                                title = details.item,
                                img = details.item_img,
                                customers = details.customers,
                                item_uid = details.item_uid,
                                isEnable = false,
                                sortedStatusIcon = "",
                                amountToPayPerItem = (details.qty * details.item_business_price).ToString("N2")
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
                                totalAmountToPay = details.qty * details.item_business_price,
                                totalAmountToPayStr = (details.qty * details.item_business_price).ToString("N2"),
                                productSource = new ObservableCollection<ProductItem>()
                            });

                            businesses[details.business_uid].productSource.Add(new ProductItem
                            {
                                color = color,
                                quantityInt = details.qty,
                                quantityStr = details.qty.ToString(),
                                title = details.item,
                                img = details.item_img,
                                customers = details.customers,
                                item_uid = details.item_uid,
                                isEnable = false,
                                sortedStatusIcon = "",
                                amountToPayPerItem = (details.qty * details.item_business_price).ToString("N2")
                            });

                        }
                    }

                    foreach (string ID in businesses.Keys)
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

                        double height = (double)(Math.Ceiling(rows) * 160);
                        businesses[ID].viewHeight = height;
                        businessSource.Add(businesses[ID]);
                    }

                    var result = await GetSavedProduction();

                    if (result != null)
                    {
                        foreach (string ID in businesses.Keys)
                        {
                            foreach (ProductItem product in businesses[ID].productSource)
                            {
                                if (result.ContainsKey(ID))
                                {
                                    var savedProduction = result[ID].productSource;
                                    if (savedProduction.Count != 0)
                                    {
                                        if (savedProduction.ContainsKey(product.item_uid))
                                        {
                                            if (savedProduction[product.item_uid].isEnable == "FALSE")
                                            {
                                                product.isEnableUpdate = false;
                                            }
                                            else if (savedProduction[product.item_uid].isEnable == "TRUE")
                                            {
                                                product.isEnableUpdate = true;
                                            }

                                            product.sortedStatusIconUpdate = savedProduction[product.item_uid].icon;

                                            var savedCustomers = savedProduction[product.item_uid].customersSource;

                                            foreach (CustomerToSave savedCustomer in savedCustomers)
                                            {
                                                foreach (Customer customer in product.customers)
                                                {
                                                    if (savedCustomer.customer_uid == customer.customer_uid)
                                                    {
                                                        customer.borderColor = Color.Red;
                                                        customer.backgroundColor = Color.FromHex("#54C66A");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ItemList.ItemsSource = businessSource;
                }
            }
            catch (Exception issueOnGetProducts)
            {
               await DisplayAlert("Oops",issueOnGetProducts.Message, "OK");
            }
        }

        async Task<Dictionary<string, ProductionDetailsToSave>> GetSavedProduction()
        {
            try
            {
                Dictionary<string, ProductionDetailsToSave> data = null;

                if (routeID != "")
                {
                    var client = new HttpClient();
                    var postClient = new SavedProduction();

                    postClient.action = "get";
                    postClient.route_id = routeID;

                    var contentString = JsonConvert.SerializeObject(postClient);
                    var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                    var endpointCall = await client.PostAsync(Constant.SaveSortedProducts, content);

                    Debug.WriteLine("JSON TO GET SAVED PRODUCTION: " + contentString);
                    Debug.WriteLine("ENDPOINT CALL RESULT: " + endpointCall.IsSuccessStatusCode);

                    if (endpointCall.IsSuccessStatusCode)
                    {
                        var returnedContentString = await endpointCall.Content.ReadAsStringAsync();

                        Debug.WriteLine("RESULT CONTENT: " + returnedContentString);

                        var result = JsonConvert.DeserializeObject<StoredProduction>(returnedContentString);

                        if (result.result.Count != 0)
                        {
                            if (result.result[0].sorted_produce != null)
                            {
                                data = JsonConvert.DeserializeObject<Dictionary<string, ProductionDetailsToSave>>(result.result[0].sorted_produce);
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception issueOnGetSavedProduction)
            {
                await DisplayAlert("Oops",issueOnGetSavedProduction.Message,"OK");
                return null;
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
            try
            {
                var sortedProducts = new Dictionary<string, ProductionDetailsToSave>();

                foreach (string ID in businesses.Keys)
                {
                    if (!sortedProducts.ContainsKey(ID))
                    {
                        var productionToSave = new ProductionDetailsToSave()
                        {
                            productSource = new Dictionary<string, ProductToSave>()
                        };

                        foreach (ProductItem product in businesses[ID].productSource)
                        {
                            var productDetailsToSave = new ProductToSave();

                            if (product.isEnable == true)
                            {
                                productDetailsToSave.isEnable = "TRUE";
                            }
                            else if (product.isEnable == false)
                            {
                                productDetailsToSave.isEnable = "FALSE";
                            }
                            productDetailsToSave.icon = product.sortedStatusIcon;

                            var customerList = new List<CustomerToSave>();

                            foreach (Customer customer in product.customers)
                            {
                                var customerToSave = new CustomerToSave();
                                customerToSave.customer_first_name = customer.customer_first_name;
                                customerToSave.customer_last_name = customer.customer_last_name;
                                customerToSave.customer_uid = customer.customer_uid;
                                customerToSave.qty = customer.qty;

                                if (customer.borderColor == Color.Red)
                                {
                                    customerList.Add(customerToSave);
                                }
                            }

                            productDetailsToSave.customersSource = customerList;

                            productionToSave.productSource.Add(product.item_uid, productDetailsToSave);
                        }

                        sortedProducts.Add(ID, productionToSave);
                    }
                }

                SaveChanges(sortedProducts);
                isProductionSave = "TRUE";
                Navigation.PushModalAsync(new SummaryPage());
            }
            catch(Exception issueOnNavigateToSummaryPage) 
            {
                Application.Current.MainPage.DisplayAlert("Oops", issueOnNavigateToSummaryPage.Message, "OK");
            }
        }

        public static void UpdateSavedProductsWhenClosingApp()
        {
            try
            {
                var sortedProducts = new Dictionary<string, ProductionDetailsToSave>();

                foreach (string ID in businesses.Keys)
                {
                    if (!sortedProducts.ContainsKey(ID))
                    {
                        var productionToSave = new ProductionDetailsToSave()
                        {
                            productSource = new Dictionary<string, ProductToSave>()
                        };

                        foreach (ProductItem product in businesses[ID].productSource)
                        {
                            var productDetailsToSave = new ProductToSave();

                            if (product.isEnable == true)
                            {
                                productDetailsToSave.isEnable = "TRUE";
                                productDetailsToSave.icon = product.sortedStatusIcon;
                            }
                            else if (product.isEnable == false)
                            {
                                productDetailsToSave.isEnable = "FALSE";
                                productDetailsToSave.icon = "";
                            }

                            var customerList = new List<CustomerToSave>();

                            foreach (Customer customer in product.customers)
                            {
                                var customerToSave = new CustomerToSave();
                                customerToSave.customer_first_name = customer.customer_first_name;
                                customerToSave.customer_last_name = customer.customer_last_name;
                                customerToSave.customer_uid = customer.customer_uid;
                                customerToSave.qty = customer.qty;

                                if (customer.borderColor == Color.Red)
                                {
                                    customerList.Add(customerToSave);
                                }
                            }

                            productDetailsToSave.customersSource = customerList;

                            productionToSave.productSource.Add(product.item_uid, productDetailsToSave);
                        }

                        sortedProducts.Add(ID, productionToSave);
                    }
                }

                SaveChanges(sortedProducts);
            }catch(Exception issueOnUpdateSavedProductsWhenClosingApp)
            {
                Application.Current.MainPage.DisplayAlert("Oops", issueOnUpdateSavedProductsWhenClosingApp.Message, "OK");
            }
        }

        public static async void SaveChanges(Dictionary<string, ProductionDetailsToSave> production)
        {
            try
            {
                if (production != null && routeID != "")
                {
                    var data = new SortedItemsToSave();

                    data.action = "post";
                    data.sorted_produce = production;
                    data.route_id = routeID;

                    var contentString = JsonConvert.SerializeObject(data);
                    var content = new StringContent(contentString, Encoding.UTF8, "application/json");

                    var client = new HttpClient();
                    var endpointCall = await client.PostAsync(Constant.SaveSortedProducts, content);

                    //Debug.WriteLine("JSON TO SEND: " + contentString);
                    //Debug.WriteLine("CALL STATUS: " + endpointCall.IsSuccessStatusCode);
                }
            }
            catch (Exception issueOnSaveChanges)
            {
                await Application.Current.MainPage.DisplayAlert("Oops",issueOnSaveChanges.Message,"OK");
            }
        }
    }
}
