using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using JustDelivered.Interfaces;
using JustDelivered.Models;
using Newtonsoft.Json;
using Plugin.Messaging;
using Xamarin.Essentials;

using Xamarin.Forms;
using static JustDelivered.Views.DeliveriesPage;

using System.Net.Http;
using System.Text;
using Acr.UserDialogs;

namespace JustDelivered.Views
{
    public partial class VerificationPage : ContentPage
    {
        int CurrentIndex = 0;
        public static DeliveryInfo LocalDelivery = new DeliveryInfo();
        Stream photoStream = null;
        byte[] t = null;
        //public  ImageSource image = "http://cdn.playbuzz.com/cdn/38402fff-32a3-4e78-a532-41f3a54d04b9/cc513a85-8765-48a5-8481-98740cc6ccdc.jpg";
        public Image f = new Image();
        public string p;
        public ObservableCollection<DisplayItem> Items = new ObservableCollection<DisplayItem>();
        private byte[] byteArray;

        public VerificationPage(DeliveryInfo Delivery, int Index)
        {
            InitializeComponent();
            Debug.WriteLine("CONFIRMATION PAGE");
            Debug.WriteLine("CUSTOMER NAME: " + Delivery.name);
            Debug.WriteLine("DELIVERY STATUS: " + Delivery.status);

            CustomerName.Text = Delivery.name;
            CustomerAddress.Text = Delivery.house_address;
            if(Delivery.delivery_instructions != null && Delivery.delivery_instructions == "")
            {
                DeliveryInstructions.Text += Delivery.delivery_instructions;
            }
            else
            {
                DeliveryInstructions.Text  += "No delivery instructions for this order";
            }

            Debug.WriteLine("PHONE (PARSHED) FROM LINE 24: " + Delivery.parsedPhone);
            Debug.WriteLine("PHONE  FROM LINE 25: " + Delivery.phone);
            CurrentIndex = Index;
            LocalDelivery = Delivery;

            var items = JsonConvert.DeserializeObject<ObservableCollection<DeliveryItem>>(Delivery.delivery_items);
            int i = 0;
            foreach (DeliveryItem item in items)
            {
                var el = new DisplayItem();
                el.img = item.img;
                el.title = item.name + " (" + item.unit + ") ";
                el.itemName = item.name;
                el.quantity = item.qty.ToString();
                el.opacityValue = 1;
                el.index = i;
                i++;
                Items.Add(el);
            }

            ItemList.ItemsSource = Items;
        }

        public VerificationPage()
        {
            InitializeComponent();


            CustomerName.Text = DeliveriesPage.delivery.name;
            CustomerAddress.Text = DeliveriesPage.delivery.house_address;
            if (DeliveriesPage.delivery.delivery_instructions != null && DeliveriesPage.delivery.delivery_instructions == "")
            {
                DeliveryInstructions.Text += DeliveriesPage.delivery.delivery_instructions;
            }
            else
            {
                DeliveryInstructions.Text += "No delivery instructions for this order";
            }

            //Debug.WriteLine("PHONE (PARSHED) FROM LINE 24: " + Delivery.parsedPhone);
            //Debug.WriteLine("PHONE  FROM LINE 25: " + Delivery.phone);
            //CurrentIndex = Index;
            //LocalDelivery = Delivery;

            var items = JsonConvert.DeserializeObject<ObservableCollection<DeliveryItem>>(DeliveriesPage.delivery.delivery_items);
            int i = 0;
            foreach (DeliveryItem item in items)
            {
                var el = new DisplayItem();
                el.img = item.img;
                el.title = item.name + " (" + item.unit + ") ";
                el.itemName = item.name;
                el.quantity = item.qty.ToString();
                el.opacityValue = 1;
                el.index = i;
                i++;
                if(item.qty > 1)
                {
                    el.color = Color.Red;
                }
                else
                {
                    el.color = Color.Black;
                }
                Items.Add(el);
            }

            ItemList.ItemsSource = Items;
        }


        public async void TakePicture(System.Object sender, System.EventArgs e)
        {
            string option = await DisplayActionSheet("Select the recipient(s) for this confirmation message", "Cancel", null, new string[] {"Seller", "Customer", "Seller And Customer"});
            Debug.WriteLine("Option: " + option);
            if (option != null && option != "")
            {
                if (option != "Cancel")
                {
                    try
                    {
                        var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                        //if (true)
                        if (photo != null)
                        {
                            //Get the public album path
                            
                            var aPpath = photo.AlbumPath;
                            p = aPpath;
                            Debug.WriteLine("PATH: " + aPpath);
                            Debug.WriteLine("PHOTO: " + photo.Path);
                            //var message = new SmsMessage("Hello JD", new[] { "4158329643" });
                            //await Sms.ComposeAsync(message);

                            //var smsMessanger = CrossMessaging.Current.SmsMessenger;
                            //if (smsMessanger.CanSendSms)
                            //{
                            //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
                            //}

                            //DependencyService.Get<INativeMessage>().OpenUrl("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");

                            //DependencyService.Get<INativeMessage>().OpenUrl("f");

                            //SendSMS(new[] { "14158329643"}, "Hello everyone! This is JD in development mode");
                            //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);

                            var path = photo.Path;
                            photoStream = photo.GetStream();
                            
                            var ar = File.ReadAllBytes(path);
                            t = ar;
                            f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                            //f.Rotation = 90;

                            //Bitmap bmp = BitmapFactory.decodeByteArray(byteArray, 0, byteArray.length);
                            //using (var memoryStream = new MemoryStream(t))
                            //{
                            //    var rotateImage = System.Drawing.Image.FromStream(memoryStream);
                            //    rotateImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            //    rotateImage.Save(memoryStream, rotateImage.RawFormat);
                            //    byteArray = memoryStream.ToArray();
                            //}



                            //image = "CallIcon.png";


                            //refundItemImage.Scale = 1;

                            ProcessRequest(option);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                        return;
                    }
                }
            }
        }

       
        async void ProcessRequest(string option)
        {
            var recipients = new List<string>();
            var names = new List<string>();
            string method = null;
            if(option == "Seller")
            {
                recipients.Add("4084760001");
                recipients.Add("4158329643");
                names.Add("Prashant");
                method = "Message";
            }
            else if (option == "Customer")
            {
                names.Add(DeliveriesPage.delivery.name);
                if (delivery.parsedPhone != "Phone # Not Available")
                {
                    recipients.Add(delivery.parsedPhone);
                    method = "Message";
                }
                else
                {
                    recipients.Add(delivery.email);
                    method = "Email";
                }
            }
            else if (option == "Seller And Customer")
            {
                names.Add("Prashant");
                names.Add(DeliveriesPage.delivery.name);
                if (delivery.parsedPhone != "Phone # Not Available")
                {
                    recipients.Add("4084760001");
                    recipients.Add("4158329643");
                    recipients.Add(delivery.parsedPhone);
                    method = "Message";
                }
                else
                {
                    recipients.Add("pmarathay@gmail.com");
                    recipients.Add("omarfacio2010@gmail.com");
                    recipients.Add(delivery.email);
                    method = "Email";
                }
            }

            if (method == "Message")
            {
                if(Device.RuntimePlatform == Device.iOS)
                {
                    // call ios interface to send message
                    UserDialogs.Instance.ShowLoading("Preparing the confirmation message...");
                    await WaitAndApologizeAsync();
                    UserDialogs.Instance.HideLoading();
                    string test = DependencyService.Get<IMessageSendRequest>().SendMessage(photoStream, recipients.ToArray(), GetMessageContent(names.ToArray()));
                    if(test == "CANNOT SEND TEXT MESSAGE")
                    {
                        var result = await SendSMS(recipients.ToArray(), GetMessageContent(names.ToArray()));

                        if (!list.Contains(DeliveriesPage.delivery.purchase_uid))
                        {
                            list.Add(DeliveriesPage.delivery.purchase_uid);
                        }
                        //deliveryList.Remove(DeliveriesPage.delivery);

                        DeliveriesPage.delivery.status = "Status: Delivered";
                        //deliveryList.Add(DeliveriesPage.delivery);
                        Application.Current.MainPage = new DeliveriesPage("");
                    }

                }
                else
                {
                    var result = await SendSMS(recipients.ToArray(), GetMessageContent(names.ToArray()));
                    if (!list.Contains(DeliveriesPage.delivery.purchase_uid))
                    {
                        list.Add(DeliveriesPage.delivery.purchase_uid);
                    }
                    //deliveryList.Remove(DeliveriesPage.delivery);

                    DeliveriesPage.delivery.status = "Status: Delivered";
                    //deliveryList.Add(DeliveriesPage.delivery);
                    Application.Current.MainPage = new DeliveriesPage("");
                }
            }
            else
            {
                ComposeEmail(recipients, GetMessageContent(names.ToArray()));
                //deliveryList.Remove(DeliveriesPage.delivery);

                DeliveriesPage.delivery.status = "Status: Delivered";
                //deliveryList.Add(DeliveriesPage.delivery);
                if (!list.Contains(DeliveriesPage.delivery.purchase_uid))
                {
                    list.Add(DeliveriesPage.delivery.purchase_uid);
                }
                Application.Current.MainPage = new DeliveriesPage("");
            }

            _ = SavePhoto(t, DeliveriesPage.delivery.purchase_uid);
            _ = UpdateDeliveryStatus(DeliveriesPage.delivery.purchase_uid);
        }

        public static void UIMessageDispose(string result)
        {

            if(result == "Sent")
            {
                //deliveryList.Remove(DeliveriesPage.delivery);

                DeliveriesPage.delivery.status = "Status: Delivered";
                //deliveryList.Add(DeliveriesPage.delivery);

                Application.Current.MainPage = new DeliveriesPage("");
            }
            else if (result == "Cancelled")
            {

            }
            else
            {
                //deliveryList.Remove(DeliveriesPage.delivery);

                DeliveriesPage.delivery.status = "Status: Delivered";
                //deliveryList.Add(DeliveriesPage.delivery);

                Application.Current.MainPage = new DeliveriesPage("");
            }
        }

        async Task<bool> SavePhoto(byte [] TargetImageByte,  string purchaseId)
        {
            var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var purchase_uid = new StringContent(purchaseId, Encoding.UTF8);
            var userImageContent = new ByteArrayContent(TargetImageByte);

            content.Add(purchase_uid, "purchase_uid");

            // CONTENT, NAME, FILENAME
            content.Add(userImageContent, "image", "product_image.png");

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri("https://0ig1dbpx3k.execute-api.us-west-1.amazonaws.com/dev/api/v2/GetAWSLink");
            request.Method = HttpMethod.Post;
            request.Content = content;

            var response = await client.SendAsync(request);
            return true;
        }

        async Task<bool> UpdateDeliveryStatus(string purchaseId)
        {
            try
            {
                var client = new HttpClient();
                var delivery = new UpdateDelivery();

                delivery.purchase_uid = purchaseId;
                delivery.cmd = "Delivered";
                delivery.note = "";

                var deliveryJSON = JsonConvert.SerializeObject(delivery);
                Debug.WriteLine("DELIVERY JSON: " + deliveryJSON);
                var content = new StringContent(deliveryJSON, Encoding.UTF8, "application/json");

                var RDSResponse = await client.PostAsync("https://0ig1dbpx3k.execute-api.us-west-1.amazonaws.com/dev/api/v2/UpdateDeliveryStatus", content);
                Debug.WriteLine("UPDATE DELIVERY STATUS ENDPOINT " + RDSResponse.IsSuccessStatusCode);

            }
            catch (Exception ErrorUpdatingStatus)
            {
                Debug.WriteLine("Exception: " + ErrorUpdatingStatus.Message);
            }
            return true;
        }


        string GetMessageContent(string[] names)
        {
            string message = "Hello ";

            foreach(string name in names)
            {
                message += name + ", ";
            }
            message = message.Remove(message.Length - 2) + "! \n\n";
            message += "This is Serving Fresh. Your package has been delivered!";

            var missingItems = GetUndeliveredItems();
            if(missingItems == "")
            {
                message += "\nBest, \nServing Fresh Team";
            }
            else
            {
                message += "\n\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
            }

            return message;
        }


        static async Task WaitAndApologizeAsync()
        {
            await Task.Delay(1000);
        }

        public async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name="Photo1.png" });
                //if (true)
                if (photo != null)
                {
                    //Get the public album path
                    var aPpath = photo.AlbumPath;
                    p = aPpath;
                    Debug.WriteLine("PATH: " + aPpath);
                    Debug.WriteLine("PHOTO: " + photo.Path);
                    //var message = new SmsMessage("Hello JD", new[] { "4158329643" });
                    //await Sms.ComposeAsync(message);

                    //var smsMessanger = CrossMessaging.Current.SmsMessenger;
                    //if (smsMessanger.CanSendSms)
                    //{
                    //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
                    //}

                    //DependencyService.Get<INativeMessage>().OpenUrl("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");

                    //DependencyService.Get<INativeMessage>().OpenUrl("f");

                    //SendSMS(new[] { "14158329643"}, "Hello everyone! This is JD in development mode");
                    //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);

                    var path = photo.Path;
                    photoStream = photo.GetStream();
                    f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    
                    //image = "CallIcon.png";
                    

                    //refundItemImage.Scale = 1;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                return;
            }

        }


        public  ImageSource img()
        {
            return f.Source;
        }

        public string pa()
        {
            return p;
        }

        public async Task<bool> SendSMS(string[] recipients, string message)
        {
            try
            {
                var textMessage = new SmsMessage(message, recipients);
                await Sms.ComposeAsync(textMessage);
                return true;
            }
            catch
            {
                await DisplayAlert("Oops","Unfortunaly, your device or mobile plan doesn't support sending SMS or MMS text messages.","OK");
                return false;
            }
        }

        public async void ComposeEmail(List<string> recipients, string message)
        {

            try
            {
                var email = new EmailMessage
                {
                    Subject = "Email Confirmation By Just Delivered",
                    Body = message,
                    To = recipients,
                };


                await Email.ComposeAsync(email);

            }
            catch
            {

            }

        }

        public async void ImageButton_Clicked_1(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("This feature is in development", "Feature coming soon!", "OK");
            //var smsMessanger = CrossMessaging.Current.SmsMessenger;
            //if (smsMessanger.CanSendSms)
            //{
            //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
            //}
        }

        public string GetPhone()
        {
            return LocalDelivery.parsedPhone;
        }

        public string Message()
        {
            return "Hello " + LocalDelivery.name + "! This is Serving Fresh. Your package has been delivered!"; 
        }

        public async void CancelOnTakePicture()
        {
            await DisplayAlert("Ooops", "You clicked on cancel and a picture was not take. Please take a picture to continue. Thank you!", "OK");
        }

        public async void Send()
        {



            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });
                if (true)
                //if (photo != null)
                {
                    //Get the public album path
                    var aPpath = photo.AlbumPath;

                    Debug.WriteLine("PATH: 151" + aPpath);
                    Debug.WriteLine("PHOTO: " + photo.Path);
                    //var message = new SmsMessage("Hello JD", new[] { "4158329643" });
                    //await Sms.ComposeAsync(message);

                    //var smsMessanger = CrossMessaging.Current.SmsMessenger;
                    //if (smsMessanger.CanSendSms)
                    //{
                    //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
                    //}

                    //DependencyService.Get<INativeMessage>().OpenUrl("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");

                    //DependencyService.Get<INativeMessage>().OpenUrl("f");

                    //SendSMS(new[] { "14158329643"}, "Hello everyone! This is JD in development mode");
                    //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);

                    var path = photo.Path;
                    photoStream = photo.GetStream();
                    f.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                    //image = "CallIcon.png";


                    //refundItemImage.Scale = 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                return;
            }



        }

        public void NextDelivery()
        {
            //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var stack = (StackLayout)sender;
            Debug.WriteLine("OPACITY VALUE: " + stack.Opacity);
            var index = Int16.Parse(stack.ClassId.ToString());
            if (stack.Opacity == 1)
            {
                Items[index].updateOpacity(0.3);
            }
            else
            {
                Items[index].updateOpacity(1.0);
            }
        }

        public string GetUndeliveredItems()
        {
            var listString = "";
            foreach (DisplayItem item in Items)
            {
                if(item.opacityValue == 1)
                {
                    listString += item.itemName + ", ";
                }
            }
            try
            {
                listString = listString.Remove(listString.Length - 2) + ".";
            }
            catch
            {
                listString = listString.TrimEnd();
            }

            return listString;
        }

        public string GetCustomerEmail()
        {
            return LocalDelivery.email;
        }

        public string GetPurchaseId()
        {
            return LocalDelivery.purchase_uid;
        }

        public string GetCustomerId()
        {
            return LocalDelivery.customer_uid;
        }

        public string GetDeliveryDate()
        {
            var date = DateTime.Parse(LocalDelivery.delivery_date);
            return date.ToString("yyyy-MM-dd 10:00:00");
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new DeliveriesPage("");
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
        }
    }
}
