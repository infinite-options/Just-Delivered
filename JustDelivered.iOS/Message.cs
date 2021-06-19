using System;
using System.Diagnostics;
using Foundation;
using JustDelivered.Interfaces;
using JustDelivered.iOS;
using MessageUI;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Message))]
namespace JustDelivered.iOS
{
    public class Message : PageRenderer, INativeMessage
	{
        public void OpenUrl(string url)
        {

            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(url));
            //var mailController = new MFMailComposeViewController();

            //mailController.SetToRecipients(new[] { "omarfacio2010@gmail.com" });
            //mailController.SetSubject("Email Confirmation By Just Delivered");
            //mailController.SetMessageBody("HELo", false);
            ////mailController.AddAttachmentData(attachment.AsJPEG(), "image/png", "photo.png");

            //mailController.Finished += (object s, MFComposeResultEventArgs args) =>
            //{
            //	args.Controller.DismissViewController(true, null);
            //};

            //this.PresentViewController(mailController, true, null);
        }

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			//myPage = (ConfirmationPage)e.NewElement;

			if (e.OldElement != null || Element == null)
			{
				return;
			}

			try
			{
				//SendEmail();
				//Debug.WriteLine("");

			}
			catch (Exception ex)
			{
				//myPage.CameraUnableToLoad(ex.Message);
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
	}
}
