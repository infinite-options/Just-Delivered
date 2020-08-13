using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryApp.Models;
using Xamarin.Forms;

namespace DeliveryApp
{
    public partial class ScannerPage : ContentPage
    {
        public ScannerPage(ServingNowList deliveryData, int num, ServingNowList deliveryListCopy, int index, bool scannerOn)
        {
            InitializeComponent();
        }
        private async Task ScanBarcode()
        {
            // var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            // var r = await scanner.Scan();

            try
            {
                ZXing.Mobile.MobileBarcodeScanner scanner = new ZXing.Mobile.MobileBarcodeScanner();

                scanner.FlashButtonText = "Flash";
                scanner.TopText = "Scan Package";
                scanner.BottomText = "";

                var result = await scanner.Scan();
                string barCode = result.ToString();

                if (barCode != "")
                {
                    // verify that the barcode from the scanner matches our database records
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!", ex.Message, "OK");
            }
        }
    }
}
