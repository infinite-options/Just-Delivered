using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using DeliveryApp;
using DeliveryApp.Droid;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content.PM;
using Android;
using Plugin.Messaging;

[assembly: ExportRenderer(typeof(SixthPage), typeof(SixthPageRenderer))]
namespace DeliveryApp.Droid
{
    public class SixthPageRenderer : PageRenderer, TextureView.ISurfaceTextureListener
    {
        global::Android.Hardware.Camera camera;
        global::Android.Widget.Button takePhotoButton;
        global::Android.Widget.Button retakePhotoButton;
        global::Android.Widget.Button textMessageButton;
        global::Android.Widget.Button sendEmailButton;

        global::Android.Views.View view;

        Activity activity;
        CameraFacing cameraType;
        TextureView textureView;
        SurfaceTexture surfaceTexture;

        bool picturetaken = false;
        bool messageOrEmailSent = false;

        SixthPage myPage;

        byte[] bitmapData;

        public SixthPageRenderer(Context context) : base(context)
        {
        }

        // This function overrides the native renderer
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
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
                AddView(view);
            }
            catch (Exception ex)
            {
                myPage.DisplayException(ex.Message);
            }
        }

        // This function sets the camera page interface
        public void SetupUserInterface()
        {
            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
            cameraType = CameraFacing.Back;

            textureView = view.FindViewById<TextureView>(Resource.Id.textureView);
            textureView.SurfaceTextureListener = this;
        }

        // This function sets the event handlers for all the buttons on the camera page
        public void SetupEventHandlers()
        {
            takePhotoButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.takePhotoButton);
            takePhotoButton.Click += TakePhoto;

            retakePhotoButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.retakePhotoButton);
            retakePhotoButton.Click += RetakePhoto;

            textMessageButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.textMessageButton);
            textMessageButton.Click += SendMMS;

            sendEmailButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.sendEmailButton);
            sendEmailButton.Click += SendEmail;
        }

        // This function capture photo
        public void TakePhoto(object sender, EventArgs e)
        {
            camera.StopPreview();
            picturetaken = true;
            myPage.UpdateMessage("Picture is ready to send!");
            myPage.UpdateSubMessage("Now send a text or an email to " + myPage.GetName());

            var image = textureView.Bitmap;

            try
            {
                var absolutePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
                var folderPath = absolutePath + "/Camera";
                var filePath = System.IO.Path.Combine(folderPath, string.Format("photo_{0}.jpg", Guid.NewGuid()));

                var fileStream = new FileStream(filePath, FileMode.Create);
                using (var stream = new MemoryStream())
                {
                    image.Compress(Bitmap.CompressFormat.Png, 50, stream);
                    bitmapData = stream.ToArray();
                }

                fileStream.Close();

                var intent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
                var file = new Java.IO.File(filePath);
                var uri = Android.Net.Uri.FromFile(file);
            }
            catch (Exception ex)
            {
                myPage.DisplayException(ex.Message);
            }
        }

        // This function retakes photo
        private void RetakePhoto(object sender, EventArgs e)
        {
            if (!picturetaken && !messageOrEmailSent)
            {
                myPage.WarningMessage();
            }

            if (messageOrEmailSent && !myPage.state)
            {
                myPage.ReturnButton();
            }

            if (messageOrEmailSent && myPage.state)
            {
                myPage.ReturnHome();
            }

            if (picturetaken)
            {
                camera.StartPreview();
                picturetaken = false;
            }
        }

        // This function sends current delivery customer an email with attachment
        public void SendEmail(object sender, EventArgs e)
        {
            if (picturetaken)
            {
                messageOrEmailSent = true;
                myPage.EmailMessageAndroid(bitmapData);
                myPage.UpdateMessage("Congratulations! Delivery Completed");
                myPage.UpdateSubMessage("");
                retakePhotoButton.Text = "Next Delivery!";
            }
            else
            {
                myPage.WarningMessage();
            }
        }

        // This function sends current delivery customer a MMS message
        private void SendMMS(object sender, EventArgs e)
        {
            myPage.TextMessageAndroid();
            if (myPage.state)
            {
                retakePhotoButton.Text = "Back To Main Page";
            }
            else
            {
                retakePhotoButton.Text = "Next Delivery!";
            }

            myPage.UpdateMessage("Congratulations! Delivery Completed");
            myPage.UpdateSubMessage("");
            messageOrEmailSent = true;
        }

        // This function prepares the camera to take a photo
        public void PrepareAndStartCamera()
        {
            camera.StopPreview();

            var display = activity.WindowManager.DefaultDisplay;
            if (display.Rotation == SurfaceOrientation.Rotation0)
            {
                camera.SetDisplayOrientation(90);
            }

            if (display.Rotation == SurfaceOrientation.Rotation270)
            {
                camera.SetDisplayOrientation(180);
            }

            camera.StartPreview();
        }

        // This function prepares the layout of the interface
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        // This function sets the texture of the interface
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            try
            {
                camera = global::Android.Hardware.Camera.Open((int)cameraType);
                textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);
                surfaceTexture = surface;

                camera.SetPreviewTexture(surface);
                PrepareAndStartCamera();
            }
            catch (Exception ex)
            {
                myPage.CameraAccess(ex.Message);
            }

        }

        // This function removes the texture of interface
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            camera.StopPreview();
            camera.Release();
            return true;
        }

        // This function changes the size of texture the interface
        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            PrepareAndStartCamera();
        }

        // This function updates the texture of the interface
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            
        }
    }
}
