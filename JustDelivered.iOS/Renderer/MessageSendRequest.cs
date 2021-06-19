using System;
using JustDelivered.Interfaces;
using JustDelivered.iOS.Renderer;
using System.Diagnostics;
using System.IO;
using UIKit;
using Foundation;
using MessageUI;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using JustDelivered.Views;

[assembly: Xamarin.Forms.Dependency(typeof(MessageSendRequest))]
namespace JustDelivered.iOS.Renderer
{
    public class MessageSendRequest: UIViewController, IMessageSendRequest
    {
        public MessageSendRequest()
        {
        }

        public string SendTextMessage()
        {
            Debug.WriteLine("You call SendTextMessage");
            return "SUCCESSFUL";
        }

        public string SendMessage(Stream stream, string[] recipients, string message)
        {

            //bool result = false;
            var ms = new MemoryStream();

            stream.CopyTo(ms);

            //var rotateImage = Image.FromStream(ms);
            //rotateImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //rotateImage.Save(ms, rotateImage.RawFormat);
            //byteArray = memoryStream.ToArray();

            var TargetImageByte = ms.ToArray();
            //var TargetImageByte = stream;
            //var resized = Utils.Image.Extensions.Rotate(TargetImageByte, 180);


            var data = NSData.FromArray(TargetImageByte);
           

			var picture = UIImage.LoadFromData(data);
           
            UIImage d = new UIImage(picture.CGImage,10,UIImageOrientation.Down);
            Debug.WriteLine("ORIENTATION: " + d.Orientation);
            //d.Orientation = UIImageOrientation.Down;
            var photo = d.AsPNG();
            



            //var authorizationController = new ASAuthorizationController(new[] { request });
            //authorizationController.Delegate = this;
            //authorizationController.PresentationContextProvider = this;


            var messageController = new MFMessageComposeViewController();
            
            //messageController.Delegate = this;
            messageController.Finished += MessageController_Finished;
            
            
                //var mailController = new MFMailComposeViewController();
            //mailController.Finished += MailController_Finished;
            //var data = NSData.FromArray(TargetImageByte);
            //var picture = UIImage.LoadFromData(data);
            // ONLY SEND MMS WITH USER THAT CAN SEND MMS
            //this.PresentViewController(messageController, true, null);
            
            //rootVC.PresentViewController(messageController, true, null);

            //this.PresentModalViewController(messageController, false);
            //this.PresentViewController(messageController, true, null);
            
            
            if (MFMessageComposeViewController.CanSendText)
            {


                messageController.Recipients = recipients;
                messageController.Body = message;
                messageController.AddAttachment(data, "kUTTypePNG", "image.png");
                
                //messageController.Finished += MessageController_Finished;
                UIApplication.SharedApplication.Delegate.GetWindow().RootViewController.PresentViewController(messageController, true, null);
                //this.PresentViewController(messageController, true, null);
                //UserDialogs.Instance.HideLoading();
                //result = true;
            }
            else
            {
                return "CANNOT SEND TEXT MESSAGE";
            }

            //Debug.WriteLine("You call SendMessage");
            return "SUCCESSFUL";

        }

        private void MessageController_Finished(object sender, MFMessageComposeResultEventArgs e)
        {
            e.Result.ToString();
            Debug.WriteLine("Message e result: " + e.Result.ToString());
            e.Controller.DismissViewController(true, null);
            VerificationPage.UIMessageDispose(e.Result.ToString());
            //var purchaseId = myPage.GetPurchaseId();
            //UpdateDeliveryStatus(purchaseId);
            //myPage.NextDelivery();
        }

        
    }
}
