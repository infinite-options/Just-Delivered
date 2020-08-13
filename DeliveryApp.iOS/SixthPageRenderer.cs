using System;
using System.Threading.Tasks;
using AVFoundation; 
using DeliveryApp; 
using DeliveryApp.iOS; 
using CoreGraphics; 
using Foundation; 
using MessageUI;
using UIKit; 
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS; 

[assembly: ExportRenderer(typeof(SixthPage), typeof(SixthPageRenderer))]
namespace DeliveryApp.iOS
{
    public class SixthPageRenderer : PageRenderer
    {
		AVCaptureSession captureSession = new AVCaptureSession();
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;

		UIView liveCameraStream = new UIView();
		UIView viewBar = new UIView();

		UIImage image = new UIImage();

		UIButton takePhotoButton = new UIButton();
		UIButton messageButton = new UIButton();
		UIButton emailButton = new UIButton();
		UIButton nextDeliveryButton = new UIButton();

		UIActionSheet actionSheet = new UIActionSheet();

		// Bool flags to take or retake mechanism
		bool cameraButtonClick = false;
		bool retakePictureClick = false;
		bool deliveryButtonClick = false;

		SixthPage myPage;

		// This function overrides elements of the native renderer
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            myPage = (SixthPage)e.NewElement;

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
				SetupUserInterface();
				SetupEventHandlers();
			}
            catch (Exception ex)
            {
				myPage.CameraUnableToLoad(ex.Message);
			}
        }

        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void ViewDidLoad()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
			base.ViewDidLoad();

			SetupUserInterface();
			SetupEventHandlers();
			AuthorizeCameraUse();
			SetupLiveCameraStream();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		// This function authorize the useage of the camera
		public async void AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}

		// This function sets camera stream 
        [Obsolete]
        public void SetupLiveCameraStream()
		{
			captureSession = new AVCaptureSession();

			var viewLayer = liveCameraStream.Layer;
			var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession);

			videoPreviewLayer.Frame = liveCameraStream.Bounds;
			liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			
			ConfigureCameraForDevice(captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};

			captureSession.AddOutput(stillImageOutput);
			captureSession.AddInput(captureDeviceInput);
			captureSession.StartRunning();
		}

		// This function captures a photo
		public async void CapturePhoto()
		{
			
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
			var image = new UIImage(jpegImageAsNsData);

			this.image = image;

			captureSession.StopRunning();

			retakePictureClick = true;
			var normalAttributedTitle = new NSAttributedString("Retake Picture", foregroundColor: UIColor.White);
			takePhotoButton.SetAttributedTitle(normalAttributedTitle, UIControlState.Normal);
			
		}

		// This function configure the camera
        public void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}

		// This function gets the camera's orientation
        public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}
			return null;
		}

		// This function sets up the camera interface
		public void SetupUserInterface()
		{
			var centerButtonX = View.Bounds.GetMidX() - 35f;
			var topLeftX = View.Bounds.X + 25;
			var topRightX = View.Bounds.Right - 65;
			var bottomButtonY = View.Bounds.Bottom - 85;
			var topButtonY = View.Bounds.Top + 15;
			var normalAttributedTitle = new NSAttributedString("Take Picture", foregroundColor: UIColor.White);

			liveCameraStream.Frame = new CGRect(0f, -100f, View.Bounds.Width, View.Bounds.Height);

			takePhotoButton.Frame = new CGRect(View.Bounds.GetMidX() - 65f, View.Bounds.Bottom - 45f, 130f, 25);
			takePhotoButton.SetAttributedTitle(normalAttributedTitle, UIControlState.Normal);
			takePhotoButton.Font = UIFont.SystemFontOfSize(15);
			takePhotoButton.Layer.CornerRadius = 20;
			takePhotoButton.BackgroundColor = UIColor.FromRGB(255, 0, 0);

			viewBar.Frame = new CGRect(0f, View.Bounds.Bottom - 45f, View.Bounds.Width, 45);
			viewBar.BackgroundColor = UIColor.White;

			messageButton.Frame = new CGRect(View.Bounds.GetMidX() - 130f, View.Bounds.Bottom - 45f, 45, 45);
			messageButton.SetBackgroundImage(UIImage.FromFile("textMessageIcon.png"), UIControlState.Normal);

			emailButton.Frame = new CGRect(View.Bounds.GetMidX() + 90f, View.Bounds.Bottom - 45f, 45, 45);
			emailButton.SetBackgroundImage(UIImage.FromFile("sendEmailButton.png"), UIControlState.Normal);

			View.InsertSubview(liveCameraStream,0);
			View.AddSubview(viewBar);
			View.AddSubview(takePhotoButton);
			View.Add(messageButton);
			View.Add(emailButton);
		}

		// This function sets up the event handlers for every button on the camera
		public void SetupEventHandlers()
		{
            takePhotoButton.TouchUpInside += (object sender, EventArgs e) =>
			{
                if (!cameraButtonClick && !retakePictureClick)
                {
                    if (captureSession.Running)
                    {
						CapturePhoto();
						myPage.UpdateMessage("Picture is ready to send!");
						myPage.UpdateSubMessage("Now send a text or an email to " + myPage.GetName());
                    }
                    else
                    {
						myPage.CameraUnableToLoad("");
                    }
				}
				else if(!cameraButtonClick && retakePictureClick)
                {
					var normalAttributedTitle = new NSAttributedString("Take Picture", foregroundColor: UIColor.White);

					myPage.UpdateMessage("Take picture of delivery");
					myPage.UpdateSubMessage("");

					captureSession.StartRunning();

					retakePictureClick = false;
					takePhotoButton.SetAttributedTitle(normalAttributedTitle, UIControlState.Normal);
				}
			};

			messageButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				TextConfirmation("Send Text Message To");
				SetUpTitle();
			};

			emailButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				if(image != null)
                {
					EmailConfirmation("Send Confirmation Email To", image);
					SetUpTitle();
				}
                else
                {
					myPage.WarningMessage();
                }
			};

			nextDeliveryButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// Limit to click multiple times on the next delivery button
                if (!deliveryButtonClick)
                {
                    if (!myPage.state)
                    {
						myPage.ReturnButton();
						deliveryButtonClick = true;
                    }
                    else
                    {
						myPage.ReturnHome();
						deliveryButtonClick = true;
					}
				}
			};
		}

		// This function sets the title of return button
		public void SetUpTitle()
        {
			var normalAttributedTitle = new NSAttributedString("Next Delivery!", foregroundColor: UIColor.White);

			myPage.UpdateMessage("Congratulations! Delivery Completed");
			myPage.UpdateSubMessage("");

			nextDeliveryButton.Frame = new CGRect(View.Bounds.GetMidX() - 70f, View.Bounds.Bottom - 45f, 140f, 25);

			if (!myPage.state)
			{
				normalAttributedTitle = new NSAttributedString("Next Delivery!", foregroundColor: UIColor.White);
			}
			else
			{
				normalAttributedTitle = new NSAttributedString("Back To Main Page", foregroundColor: UIColor.White);
			}

			nextDeliveryButton.SetAttributedTitle(normalAttributedTitle, UIControlState.Normal);
			nextDeliveryButton.Font = UIFont.SystemFontOfSize(15);
			nextDeliveryButton.Layer.CornerRadius = 20;
			nextDeliveryButton.BackgroundColor = UIColor.FromRGB(50, 205, 50);

			View.AddSubview(nextDeliveryButton);
			cameraButtonClick = false;
		}

		// This function gets current customer's first name
		public string GetFirstName()
		{
			string firstName = "";

			for (int i = 0; i < myPage.GetName().Length; i++)
			{
				if ((int)myPage.GetName()[i] != (int)'.')
				{
					firstName += myPage.GetName()[i];
				}
				else
				{
					break;
				}
			}
			return firstName;
		}

		// This function prompts users with a menu to send a text message to
		public void TextConfirmation(string title)
        {
			actionSheet.AddButton("Seller");
			actionSheet.AddButton("Customer");
			actionSheet.AddButton("Seller And Customer");
			actionSheet.AddButton("Cancel");
			actionSheet.CancelButtonIndex = 3;
			
			actionSheet.Clicked += delegate (object a, UIButtonEventArgs b) {
				if (b.ButtonIndex.ToString().Equals("0"))
				{
					SendMMS("sms://open?addresses=4084760001,4158329643&body=Hello%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");
				}
				if (b.ButtonIndex.ToString().Equals("1"))
				{
					SendMMS("sms:/" + "/" + "open?addresses=" + myPage.GetPhone() + "&body=Hello%20" + GetFirstName() + ",%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");
				}
				if (b.ButtonIndex.ToString().Equals("2"))
				{
					SendMMS("sms:/" + "/" + "open?addresses=4084760001,4158329643," + myPage.GetPhone() + "&body=Hello%20" + GetFirstName() + "%20and%20Prashant,%20This%20is%20Just%20Delivered.%20We%20just%20delivered%20your%20package%21");
				}
			};
			actionSheet.ShowInView(View);
		}

		// This function prompts users with a menu to send a email message to
		private void EmailConfirmation(string title, UIImage picture)
		{
			actionSheet = new UIActionSheet(title);

			actionSheet.AddButton("Seller");
			actionSheet.AddButton("Customer");
			actionSheet.AddButton("Seller And Customer");
			actionSheet.AddButton("Cancel");

			actionSheet.CancelButtonIndex = 3;

			actionSheet.Clicked += delegate (object a, UIButtonEventArgs b) {
				if (b.ButtonIndex.ToString().Equals("0"))
				{
					SendEmail(new string[] { "pmarathay@gmail.com", "omarfacio2010@gmail.com" }, "Hello Prashant, Just Delivered Driver completed a delivery for "+ myPage.GetName(), picture);

				}
				if (b.ButtonIndex.ToString().Equals("1"))
				{
					SendEmail(new string[] { myPage.GetEmail() }, "Hello "+ myPage.GetName()+", This is Just Delivered. We just delivered your package!", picture);

				}
				if (b.ButtonIndex.ToString().Equals("2"))
				{
					SendEmail(new string[] { myPage.GetEmail(), "pmarathay@gmail.com", "omarfacio2010@gmail.com" }, "Hello "+myPage.GetName()+" and Prashant, This is Just Delivered. We just delivered your package!", picture);
				}
			};
			actionSheet.ShowInView(View);
		}

		// This function call the navite sms app to send MMS message
		public void SendMMS(string url)
        {
			UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(url));
		}

		// This function only sends a text message
		public async Task SendSMS(string[] recipients, string message)
        {
            try
            {
                var textMessage = new SmsMessage(message, recipients);
                await Sms.ComposeAsync(textMessage);
            }
            catch (FeatureNotSupportedException ex)
            {
				myPage.DisplayException(ex.Message);
            }
            catch (Exception ex)
            {
				myPage.DisplayException(ex.Message);
			}
        }

		// This function builds email confirmation with attachment
		private void SendEmail(string[] recipients, string messageBody, UIImage attachment)
        {
			try
			{
				var mailController = new MFMailComposeViewController();

				mailController.SetToRecipients(recipients);
				mailController.SetSubject("Email Confirmation By Just Delivered");
				mailController.SetMessageBody(messageBody, false);
				mailController.AddAttachmentData(attachment.AsJPEG(), "image/png", "photo.png");

				mailController.Finished += (object s, MFComposeResultEventArgs args) =>
				{
					args.Controller.DismissViewController(true, null);
				};

				this.PresentViewController(mailController, true, null);
			}
			catch (FeatureNotSupportedException ex)
			{
				myPage.DisplayException(ex.Message);
			}
			catch (Exception ex)
			{
				myPage.DisplayException(ex.Message);
			}
		}
	}
}
