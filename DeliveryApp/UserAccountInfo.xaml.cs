using System;
using System.Collections.Generic;
using DeliveryApp.Models;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class UserAccountInfo : ContentPage
    {
        public UserAccountInfo(int deliveries)
        {
            InitializeComponent();

            if(Device.RuntimePlatform == Device.iOS)
            {
                userPhotoButton.Text = "Photo";
                deliveryOrganizations.Text = "Delivering for Serving Now";
                numberOfDeliveries.Text = deliveries + " Total Deliveries";

                startTime.Text = "Start Time: ";
                totalHoursWorked.Text = "Total Hours: ";
                totalMilesDriven.Text = "Total Distance Traveled: ";
                endTime.Text = "End Time: ";
            }
            else
            {
                userPhotoButton.Text = "Photo";
                userPhotoButton.FontSize = 10;

                deliveryOrganizations.Text = "Delivering for Serving Now";
                deliveryOrganizations.FontSize = 20;
                deliveryOrganizations.TextColor = Color.Black;

                numberOfDeliveries.Text = deliveries + " Total Deliveries";
                numberOfDeliveries.TextColor = Color.Black;

                startTime.Text = "Start Time: ";
                startTime.TextColor = Color.Black;

                totalHoursWorked.Text = "Total Hours: ";
                totalHoursWorked.TextColor = Color.Black;

                totalMilesDriven.Text = "Total Distance Traveled: ";
                totalMilesDriven.TextColor = Color.Black;

                endTime.Text = "End Time: ";
                endTime.TextColor = Color.Black;
            }
        }
    }
}
