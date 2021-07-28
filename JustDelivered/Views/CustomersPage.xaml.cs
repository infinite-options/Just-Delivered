using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JustDelivered.Models;
using Xamarin.Forms;
using static JustDelivered.Views.ProductsPage;

namespace JustDelivered.Views
{
    public partial class CustomersPage : ContentPage
    {
        ObservableCollection<Customer> customerSource = new ObservableCollection<Customer>();

        public CustomersPage()
        {
            InitializeComponent();
            BackgroundColor = Color.FromHex("AB000000");
            if (productSelected != null)
            {
                img.Source = productSelected.img;
                quantity.Text = productSelected.quantityStr;
                title.Text = productSelected.title;

                foreach (Customer customer in productSelected.customers)
                {
                    customerSource.Add(customer);
                }

                customerList.ItemsSource = customerSource;
            }
        }

        void CheckOffCustomer(System.Object sender, System.EventArgs e)
        {
            var view = (Frame)sender;
            var recognizer = (TapGestureRecognizer)view.GestureRecognizers[0];
            var customer = (Customer)recognizer.CommandParameter;

            if(customer.borderColor == Color.Black)
            {
                customer.updateBorderColor = Color.Red;
                customer.updateBackgroundColor = Color.FromHex("#54C66A");
            }
            else
            {
                customer.updateBorderColor = Color.Black;
                customer.updateBackgroundColor = Color.White;
            }
        }

        void NavigateBackToProductsPage(System.Object sender, System.EventArgs e)
        {
            if (productSelected != null)
            {
                if (AllCustomersNotSorted(customerSource))
                {
                    productSelected.isEnableUpdate = false;
                    productSelected.sortedStatusIconUpdate = "";
                }
                else
                {
                    productSelected.isEnableUpdate = true;

                    if (AllCustomersSorted(customerSource))
                    {
                        productSelected.sortedStatusIconUpdate = "completedIcon.png";
                    }
                    else
                    {
                        productSelected.sortedStatusIconUpdate = "inprogressYellowIcon.png";
                    }
                }
            }

            Navigation.PopModalAsync();
        }

        bool AllCustomersNotSorted(IList<Customer> customers)
        {
            var status = true;

            foreach(Customer customer in customers)
            {
                if(customer.borderColor == Color.Red)
                {
                    status = false;
                    break;
                }
            }

            return status;
        }

        bool AllCustomersSorted(IList<Customer> customers)
        {
            var status = true;

            foreach (Customer customer in customers)
            {
                if (customer.borderColor == Color.Black)
                {
                    status = false;
                    break;
                }
            }

            return status;
        }
    }
}
