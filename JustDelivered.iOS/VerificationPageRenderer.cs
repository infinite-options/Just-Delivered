using System;
using JustDelivered.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using JustDelivered.iOS;
using MessageUI;
using UIKit;
using Xamarin.Essentials;
using Foundation;
using System.Diagnostics;
using System.Threading.Tasks;
using AssetsLibrary;
using CoreGraphics;
using System.IO;
using System.Collections.Generic;
using Acr.UserDialogs;
using System.Net.Http;
using System.Text;
using JustDelivered.Models;
using Newtonsoft.Json;

[assembly: ExportRenderer(typeof(VerificationPage), typeof(VerificationPageRenderer))]
namespace JustDelivered.iOS
{
	public class VerificationPageRenderer : PageRenderer
    {
		public async Task<string> SendMessage(Stream stream, string[] recipients, string message)
		{
			//bool result = false;
			var ms = new MemoryStream();
			stream.CopyTo(ms);
			var TargetImageByte = ms.ToArray();

			var data = NSData.FromArray(TargetImageByte);
			var picture = UIImage.LoadFromData(data);

			//var authorizationController = new ASAuthorizationController(new[] { request });
			//authorizationController.Delegate = this;
			//authorizationController.PresentationContextProvider = this;


			//var messageController = new MFMessageComposeViewController();

			////messageController.Delegate = this;
			//messageController.Finished += MessageController_Finished;
			////var mailController = new MFMailComposeViewController();
			////mailController.Finished += MailController_Finished;
			////var data = NSData.FromArray(TargetImageByte);
			////var picture = UIImage.LoadFromData(data);
			//// ONLY SEND MMS WITH USER THAT CAN SEND MMS
			//if (MFMessageComposeViewController.CanSendText)
			//{


			//    messageController.Recipients = recipients;
			//    messageController.Body = message;
			//    messageController.AddAttachment(picture.AsPNG(), "kUTTypePNG", "image.png");

			//    messageController.Finished += MessageController_Finished;

			//    this.PresentViewController(messageController, true, null);
			//    //UserDialogs.Instance.HideLoading();
			//    //result = true;
			//}
			//else
			//{
			//    //result = false;
			//}
			String answer = "fdf";
			return answer;
		}
	}
 //   public class VerificationPageRenderer : PageRenderer
 //   {
 //	VerificationPage myPage;
 //	UIButton messageButton = new UIButton();
 //	UIButton emailButton = new UIButton();
 //       private Stream photoStream;
 //	UIActionSheet actionSheet = new UIActionSheet();
 //	protected override void OnElementChanged(VisualElementChangedEventArgs e)
 //	{
 //		base.OnElementChanged(e);

	//		myPage = (VerificationPage)e.NewElement;

	//		if (e.OldElement != null || Element == null)
	//		{
	//			return;
	//		}

	//		try
	//		{
	//			//SendEmail();
	//			SetupUserInterface();

	//		}
	//		catch (Exception ex)
	//		{
	//			//myPage.CameraUnableToLoad(ex.Message);
	//		}
	//	}

	//	public void SetupUserInterface()
	//	{
	//		var centerButtonX = View.Bounds.GetMidX() - 100f;
	//		var topLeftX = View.Bounds.X + 25;
	//		var topRightX = View.Bounds.Right - 65;
	//		var bottomButtonY = View.Bounds.Bottom - 85;
	//		var topButtonY = View.Bounds.Top + 15;
	//		//var normalAttributedTitle = new NSAttributedString("Take Picture", foregroundColor: UIColor.White);

	//		//liveCameraStream.Frame = new CGRect(0f, -100f, View.Bounds.Width, View.Bounds.Height);

	//		//takePhotoButton.Frame = new CGRect(View.Bounds.GetMidX() - 65f, View.Bounds.Bottom - 45f, 130f, 25);
	//		//takePhotoButton.SetAttributedTitle(normalAttributedTitle, UIControlState.Normal);
	//		//takePhotoButton.Font = UIFont.SystemFontOfSize(15);
	//		//takePhotoButton.Layer.CornerRadius = 20;
	//		//takePhotoButton.BackgroundColor = UIColor.FromRGB(255, 0, 0);

	//		//viewBar.Frame = new CGRect(0f, View.Bounds.Bottom - 45f, View.Bounds.Width, 45);
	//		//viewBar.BackgroundColor = UIColor.White;

	//		messageButton.Frame = new CGRect(centerButtonX, View.Bounds.Bottom - 80f, 200, 45);
	//		messageButton.Layer.CornerRadius = 10;
	//		//messageButton.SetBackgroundImage(UIImage.FromFile("TextIcon.png"), UIControlState.Normal);
	//		messageButton.SetTitle("Take Picture", UIControlState.Normal);
	//		messageButton.SetTitleColor(UIColor.White, UIControlState.Normal);
	//		messageButton.BackgroundColor = UIColor.Red;
	//           messageButton.TouchDown += MessageButton_TouchDown;

	//		//emailButton.Frame = new CGRect(View.Bounds.GetMidX() + 90f, View.Bounds.Bottom - 80f, 45, 45);
	//		emailButton.SetBackgroundImage(UIImage.FromFile("EmailIcon.png"), UIControlState.Normal);

	//		//View.InsertSubview(liveCameraStream, 0);
	//		//View.AddSubview(viewBar);
	//		//View.AddSubview(takePhotoButton);
	//		View.Add(messageButton);
	//		//View.Add(emailButton);
	//           //emailButton.TouchDown += EmailButton_TouchDown;
	//	}

	//       private void EmailButton_TouchDown(object sender, EventArgs e)
	//       {
	//		myPage.ImageButton_Clicked_1(sender,e);
	//		//var messageController = new MFMessageComposeViewController();
	//		//if (MFMessageComposeViewController.CanSendText)
	//		//{
	//		//	messageController.Body = "Here's an image for u 2 n joy.";

	//		//	//Add attachment as NSData, and set the uti
	//		//	//var data = FromUrl("assets-library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG");
	//		//	//var r = FromUrl("/var/mobile/Containers/Data/Application/FE5CA5E1-19A0-4A67-8CF3-17A2582EA36E/Documents/IMG_1611874139913.jpg");

	//		//	//var data = NSData.FromUrl(assets - library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG);
	//		//	//VerificationPage p = new VerificationPage();
	//		//	//ImageSource im = myPage.img();

	//		//	//var array = LoadAssetAsByteArray("assets-library://asset/asset.JPG?id=CAE6D253-EE39-421D-B514-ACB98D38174A&ext=JPG");
	//		//	//ImageSource i = "http://cdn.playbuzz.com/cdn/38402fff-32a3-4e78-a532-41f3a54d04b9/cc513a85-8765-48a5-8481-98740cc6ccdc.jpg";
	//		//	//var d = im.ToUIImage();

	//		//	var array = LoadAssetAsByteArray(myPage.pa());
	//  //             //var d = UIImage.LoadFromData(data);


	//  //             var data = NSData.FromArray(array);
	//  //             var uiimage = UIImage.LoadFromData(data);


	//  //             //messageController.AddAttachment(UIImage.FromFile("IMG_1611873461144.jpg").AsPNG()  , "kUTTypePNG", "image.png");

	//  //             messageController.AddAttachment(uiimage.AsJPEG(), "kUTTypePNG", "image.png");

	//		//	messageController.Finished += MessageController_Finished;

	//		//	this.PresentViewController(messageController, true, null);
	//		//}
	//	}

	//	public void ShowActionSheet()
	//       {
	//		actionSheet.Title = "Select the recipient(s) for this confirmation message";
	//		actionSheet.AddButton("Seller");
	//		actionSheet.AddButton("Customer");
	//		actionSheet.AddButton("Seller And Customer");
	//		actionSheet.AddButton("Cancel");
	//		actionSheet.CancelButtonIndex = 3;
	//		actionSheet.ShowInView(View);
	//           actionSheet.Clicked += ActionSheetClicked;
	//	}

	//       private async void ActionSheetClicked(object sender, UIButtonEventArgs e)
	//       {
	//		var option = "";
	//		if (e.ButtonIndex.ToString().Equals("0"))
	//		{
	//			option = "1";
	//		}
	//		if (e.ButtonIndex.ToString().Equals("1"))
	//		{
	//			option = "2";
	//		}
	//		if (e.ButtonIndex.ToString().Equals("2"))
	//		{
	//			option = "2";
	//		}
	//		if(e.ButtonIndex.ToString() != "3")
	//           {
	//               await WaitAndApologizeAsync();
	//               ProcessConfirmation(option);
	//           }
	//	}

	//       static async Task WaitAndApologizeAsync()
	//	{
	//		await Task.Delay(500);
	//	}

	//	private void MessageButton_TouchDown(object sender, EventArgs e)
	//	{
	//		ShowActionSheet();
	//	}

	//	public async void ProcessConfirmation(string option)
	//       {
	//		try
	//		{
	//			await WaitAndApologizeAsync();
	//			var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });

	//			if (photo != null)
	//			{
	//				//Get the public album path
	//				var aPpath = photo.AlbumPath;
	//				var path = photo.Path;

	//				photoStream = photo.GetStream();

	//				UserDialogs.Instance.ShowLoading("Processing...\nWe are preparing the confirmation message...");

	//				var purchaseId = myPage.GetPurchaseId();

	//				var client = new HttpClient();
	//				var content = new MultipartFormDataContent();
	//				var purchase_uid = new StringContent(purchaseId, Encoding.UTF8);

	//				var ms = new MemoryStream();
	//				photoStream.CopyTo(ms);
	//				var TargetImageByte = ms.ToArray();
	//				var userImageContent = new ByteArrayContent(TargetImageByte);

	//				content.Add(purchase_uid, "purchase_uid");

	//				// CONTENT, NAME, FILENAME
	//				content.Add(userImageContent, "image", "product_image.png");

	//				var request = new HttpRequestMessage();

	//				request.RequestUri = new Uri("https://rqiber37a4.execute-api.us-west-1.amazonaws.com/dev/api/v2/GetAWSLink");
	//				request.Method = HttpMethod.Post;
	//				request.Content = content;

	//				var response = await client.SendAsync(request);
	//				var messageController = new MFMessageComposeViewController();
	//				messageController.Finished += MessageController_Finished;
	//				var mailController = new MFMailComposeViewController();
	//				mailController.Finished += MailController_Finished;
	//				var data = NSData.FromArray(TargetImageByte);
	//				var picture = UIImage.LoadFromData(data);
	//				// ONLY SEND MMS WITH USER THAT CAN SEND MMS
	//				if (MFMessageComposeViewController.CanSendText)
	//				{
	//					if (option == "1")
	//					{
	//						string[] recipient = new string[2];
	//						recipient[0] = "4084760001";
	//						recipient[1] = "4158329643";
	//						messageController.Recipients = recipient;
	//						var message = "\nHello Prashant, This is Serving Fresh. A package has been delivered.\n";
	//						var missingItems = myPage.GetUndeliveredItems();
	//						if (missingItems == "")
	//						{
	//							messageController.Body = message + "\nBest, \nServing Fresh Team";
	//						}
	//						else
	//						{
	//							message += "\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
	//							messageController.Body = message;
	//						}
	//						messageController.AddAttachment(picture.AsPNG(), "kUTTypePNG", "image.png");

	//						messageController.Finished += MessageController_Finished;

	//						this.PresentViewController(messageController, true, null);
	//						UserDialogs.Instance.HideLoading();
	//					}
	//					else
	//					{
	//						var customerPhone = myPage.GetPhone();
	//						if (customerPhone != "Phone # Not Available")
	//						{
	//							string[] recipient = new string[3];
	//							recipient[0] = "4084760001";
	//							recipient[1] = "4158329643";
	//							recipient[2] = customerPhone;
	//							messageController.Recipients = recipient;

	//							var message = myPage.Message();
	//							var missingItems = myPage.GetUndeliveredItems();
	//							if (missingItems == "")
	//							{
	//								messageController.Body = message + "\nBest, \nServing Fresh Team";
	//							}
	//							else
	//							{
	//								message += "\n\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
	//								messageController.Body = "\n" + message;
	//							}

	//							messageController.AddAttachment(picture.AsPNG(), "kUTTypePNG", "image.png");

	//							messageController.Finished += MessageController_Finished;

	//							this.PresentViewController(messageController, true, null);
	//							UserDialogs.Instance.HideLoading();

	//						}
	//						else
	//						{
	//							var message = myPage.Message();
	//							var missingItems = myPage.GetUndeliveredItems();
	//							var body = "";
	//							if (missingItems == "")
	//							{
	//								body = message + "\n\nBest, \nServing Fresh Team";
	//							}
	//							else
	//							{
	//								message += "\n\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
	//								body = "\n" + message;
	//							}

	//							mailController.SetToRecipients(new[] { "pmarathay@gmail.com", "omarfacio2010@gmail.com", myPage.GetCustomerEmail() });
	//							mailController.SetSubject("Email Confirmation By Just Delivered");
	//							mailController.SetMessageBody(body, false);
	//							mailController.AddAttachmentData(picture.AsJPEG(), "image/png", "image.png");
	//							this.PresentViewController(mailController, true, null);
	//							UserDialogs.Instance.HideLoading();
	//						}
	//					}
	//				}
	//			}
	//			else
	//			{
	//				myPage.CancelOnTakePicture();
	//			}
	//           }
	//           catch (Exception ErrorTakingPicture)
	//           {
	//			Debug.WriteLine(ErrorTakingPicture.Message);
	//           }
	//       }

	//       private void MailController_Finished(object sender, MFComposeResultEventArgs e)
	//       {
	//		e.Controller.DismissViewController(true, null);
	//		var purchaseId = myPage.GetPurchaseId();
	//		UpdateDeliveryStatus(purchaseId);
	//		myPage.NextDelivery();
	//	}

	//	private void MessageController_Finished(object sender, MFMessageComposeResultEventArgs e)
	//	{
	//		e.Controller.DismissViewController(true, null);
	//		var purchaseId = myPage.GetPurchaseId();
	//		UpdateDeliveryStatus(purchaseId);
	//		myPage.NextDelivery();
	//	}

	//	public async void UpdateDeliveryStatus(string purchaseId)
	//	{
	//           try
	//           {
	//			var client = new HttpClient();
	//			var delivery = new UpdateDelivery();

	//			delivery.customer_uid = purchaseId;
	//			delivery.delivery_date = myPage.GetDeliveryDate();
	//			delivery.cmd = "update";

	//			var deliveryJSON = JsonConvert.SerializeObject(delivery);
	//			Debug.WriteLine("DELIVERY JSON: " + deliveryJSON);
	//			var content = new StringContent(deliveryJSON, Encoding.UTF8, "application/json");

	//			var RDSResponse = await client.PostAsync("https://rqiber37a4.execute-api.us-west-1.amazonaws.com/dev/api/v2/UpdateDeliveryStatus", content);
	//			Debug.WriteLine("UPDATE DELIVERY STATUS ENDPOINT " + RDSResponse.IsSuccessStatusCode);

	//           }
	//           catch (Exception ErrorUpdatingStatus)
	//           {
	//			Debug.WriteLine("Exception: " + ErrorUpdatingStatus.Message);
	//           }
	//	}

	//	public void SendEmail()
	//	{
	//		try
	//		{


	//			//{

	//			//	var mailController = new MFMailComposeViewController();

	//			//	mailController.SetToRecipients(new[] { "omarfacio2010@gmail.com"});
	//			//	mailController.SetSubject("Email Confirmation By Just Delivered");
	//			//	mailController.SetMessageBody("HELo", false);
	//			//	//mailController.AddAttachmentData(attachment.AsJPEG(), "image/png", "photo.png");

	//			//	mailController.Finished += (object s, MFComposeResultEventArgs args) =>
	//			//	{
	//			//		args.Controller.DismissViewController(true, null);
	//			//	};

	//			//	this.PresentViewController(mailController, true, null);
	//			//}
	//			//catch (FeatureNotSupportedException ex)
	//			//{
	//			//	//myPage.DisplayException(ex.Message);
	//			//}
	//			//catch (Exception ex)
	//			//{
	//			//	//myPage.DisplayException(ex.Message);
	//			//}
	//			var messageController = new MFMessageComposeViewController();
	//			//
	//			//Verify app can send text message             
	//			if (MFMessageComposeViewController.CanSendText)
	//			{
	//				messageController.Body = "Here's an image for u 2 n joy.";

	//				//Add attachment as NSData, and set the uti
	//				//var data = FromUrl("assets-library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG");
	//				//var r = FromUrl("/var/mobile/Containers/Data/Application/FE5CA5E1-19A0-4A67-8CF3-17A2582EA36E/Documents/IMG_1611874139913.jpg");

	//				//var data = NSData.FromUrl(assets - library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG);
	//				//VerificationPage p = new VerificationPage();
	//				var im = myPage.img();

	//				var array = LoadAssetAsByteArray("assets-library://asset/asset.JPG?id=CAE6D253-EE39-421D-B514-ACB98D38174A&ext=JPG");
	//				//ImageSource i = "http://cdn.playbuzz.com/cdn/38402fff-32a3-4e78-a532-41f3a54d04b9/cc513a85-8765-48a5-8481-98740cc6ccdc.jpg";
	//				var d = im.ToUIImage();

	//				//var data = NSData.FromArray(array);
	//				//var uiimage = UIImage.LoadFromData(data);


	//				//messageController.AddAttachment(UIImage.FromFile("IMG_1611873461144.jpg").AsPNG()  , "kUTTypePNG", "image.png");

	//				messageController.AddAttachment(d.Result.AsJPEG(), "kUTTypePNG", "image.png");

	//				messageController.Finished += MessageController_Finished;


	//				this.PresentViewController(messageController, true, null);
	//			}
	//		}
	//		catch (Exception p)
	//		{
	//			Debug.WriteLine("ERROR: " + p.Message);
	//		}
	//	}

	//	public Byte[] LoadAssetAsByteArray(string uri)
	//	{
	//		var nsUrl = new NSUrl(uri);
	//		var asset = new ALAssetsLibrary();
	//		Byte[] imageByteArray = new Byte[0];
	//		UIImage image;

	//		asset.AssetForUrl(nsUrl, (ALAsset obj) => {

	//			var assetRep = obj.DefaultRepresentation;
	//			var cGImage = assetRep.GetFullScreenImage();
	//			image = new UIImage(cGImage);
	//			// get as Byte[]
	//			imageByteArray = new Byte[image.AsPNG().Length];
	//			//imageView.Image = image;
	//		},
	//		(NSError err) => {
	//			Console.WriteLine(err);
	//		});

	//		return imageByteArray;
	//	}

	//	UIImage FromUrl(string uri)
	//	{
	//		using (var url = new NSUrl(uri))
	//		using (var data = NSData.FromUrl(url))
	//			return UIImage.LoadFromData(data);
	//	}


	//	// RECYCLE CODE
	//	//public void R()
	//	//      {
	//	//          var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true, Name = "Photo1.png" });

	//	//          if (photo != null)
	//	//          {
	//	//              //Get the public album path
	//	//              var aPpath = photo.AlbumPath;

	//	//              Debug.WriteLine("PATH: " + aPpath);
	//	//              Debug.WriteLine("PHOTO: " + photo.Path);
	//	//              //var message = new SmsMessage("Hello JD", new[] { "4158329643" });
	//	//              //await Sms.ComposeAsync(message);

	//	//              //var smsMessanger = CrossMessaging.Current.SmsMessenger;
	//	//              //if (smsMessanger.CanSendSms)
	//	//              //{
	//	//              //    smsMessanger.SendSms("14158329643", "Welcome to Xamarin.Forms");
	//	//              //}

	//	//              //DependencyService.Get<INativeMessage>().OpenUrl("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");

	//	//              //DependencyService.Get<INativeMessage>().OpenUrl("f");

	//	//              //SendSMS(new[] { "14158329643"}, "Hello everyone! This is JD in development mode");
	//	//              //Application.Current.MainPage = new DeliveriesPage(CurrentIndex);

	//	//              var path = photo.Path;
	//	//              photoStream = photo.GetStream();
	//	//              //ImageSource.FromStream(() => { return photo.GetStream(); });


	//	//              UserDialogs.Instance.ShowLoading("Processing...\nWe are preparing the confirmation message...");
	//	//              var purchaseId = myPage.GetPurchaseId();
	//	//              Debug.WriteLine("PURCHASE ID: " + purchaseId);
	//	//              // var userMessage = message.Text;
	//	//              // var userPhone = "4158329643";
	//	//              // var userImage = PhotoImage.Source;

	//	//              HttpClient client = new HttpClient();
	//	//              MultipartFormDataContent content = new MultipartFormDataContent();
	//	//              StringContent purchase_uid = new StringContent(purchaseId, Encoding.UTF8);

	//	//              var ms = new MemoryStream();
	//	//              photoStream.CopyTo(ms);
	//	//              byte[] TargetImageByte = ms.ToArray();
	//	//              ByteArrayContent userImageContent = new ByteArrayContent(TargetImageByte);

	//	//              content.Add(purchase_uid, "purchase_uid");

	//	//              // CONTENT, NAME, FILENAME
	//	//              content.Add(userImageContent, "image", "product_image.png");

	//	//              var request = new HttpRequestMessage();

	//	//              request.RequestUri = new Uri("https://rqiber37a4.execute-api.us-west-1.amazonaws.com/dev/api/v2/GetAWSLink");
	//	//              request.Method = HttpMethod.Post;
	//	//              request.Content = content;

	//	//              //UserDialogs.Instance.ShowLoading("Sending your request...");

	//	//              //HttpResponseMessage response = await client.SendAsync(request);
	//	//              //Debug.WriteLine("This is the response from request.isSuccess: " + response.IsSuccessStatusCode);



	//	//              var messageController = new MFMessageComposeViewController();
	//	//              //
	//	//              //Verify app can send text message             
	//	//              if (MFMessageComposeViewController.CanSendText)
	//	//              {

	//	//                  if (option == "1")
	//	//                  {
	//	//                      string[] recipient = new string[2];
	//	//                      recipient[0] = "4084760001";
	//	//                      recipient[1] = "4158329643";
	//	//                      messageController.Recipients = recipient;
	//	//                      var message = "\nHello Prashant, This is Serving Fresh. A package has been delivered.\n";
	//	//                      var missingItems = myPage.GetUndeliveredItems();
	//	//                      if (missingItems == "")
	//	//                      {
	//	//                          messageController.Body = message + "\n\nBest, \nServing Fresh Team";

	//	//                      }
	//	//                      else
	//	//                      {
	//	//                          message += "\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
	//	//                          messageController.Body = message;
	//	//                      }

	//	//                  }
	//	//                  else
	//	//                  {
	//	//                      var customer = myPage.GetPhone();
	//	//                      string[] recipient = new string[3];
	//	//                      recipient[0] = "4084760001";
	//	//                      recipient[1] = "4158329643";
	//	//                      recipient[2] = customer;
	//	//                      messageController.Recipients = recipient;
	//	//                      var message = myPage.Message();
	//	//                      var missingItems = myPage.GetUndeliveredItems();
	//	//                      if (missingItems == "")
	//	//                      {
	//	//                          messageController.Body = message + "\n\nBest, \nServing Fresh Team";

	//	//                      }
	//	//                      else
	//	//                      {
	//	//                          message += "\n\nThese are the items that weren't delivered: " + missingItems + "\n\nWe will bring them in the next delivery.\n\nBest, \nServing Fresh Team";
	//	//                          messageController.Body = "\n" + message;
	//	//                      }
	//	//                  }



	//	//                  //Add attachment as NSData, and set the uti
	//	//                  //var data = FromUrl("assets-library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG");
	//	//                  //var r = FromUrl("/var/mobile/Containers/Data/Application/FE5CA5E1-19A0-4A67-8CF3-17A2582EA36E/Documents/IMG_1611874139913.jpg");

	//	//                  //var data = NSData.FromUrl(assets - library://asset/asset.JPG?id=5FD1FC79-9962-4C80-81AB-24E1BD51211D&ext=JPG);
	//	//                  //VerificationPage p = new VerificationPage();
	//	//                  //var im = myPage.img();
	//	//                  //var ms = new MemoryStream();
	//	//                  //photoStream.CopyTo(ms);
	//	//                  //byte[] TargetImageByte = ms.ToArray();
	//	//                  //ImageSource i = "http://cdn.playbuzz.com/cdn/38402fff-32a3-4e78-a532-41f3a54d04b9/cc513a85-8765-48a5-8481-98740cc6ccdc.jpg";

	//	//                  //ImageSource i = ImageSource.FromStream(() => { return photo.GetStream(); });
	//	//                  //var d = im.ToUIImage();

	//	//                  var data = NSData.FromArray(TargetImageByte);
	//	//                  var uiimage = UIImage.LoadFromData(data);


	//	//                  //messageController.AddAttachment(UIImage.FromFile("IMG_1611873461144.jpg").AsPNG()  , "kUTTypePNG", "image.png");

	//	//                  messageController.AddAttachment(uiimage.AsPNG(), "kUTTypePNG", "image.png");

	//	//                  messageController.Finished += MessageController_Finished;

	//	//                  this.PresentViewController(messageController, true, null);
	//	//                  UserDialogs.Instance.HideLoading();
	//	//                  //
	//	//                  //Verify app can send text message             


	//	//              }

	//	//          }

	//	//      }
	//}
}
