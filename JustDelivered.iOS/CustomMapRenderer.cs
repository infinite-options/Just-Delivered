using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoreGraphics;
using JustDelivered.Controls;
using JustDelivered.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace JustDelivered.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<CustomPin> customPins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                //throw new Exception("Custom pin not found");
            }
            else
            {
                if (annotationView == null)
                {
                    annotationView = new CustomMKAnnotationView(annotation, customPin.Name == null ? "" : customPin.Name);

                    UILabel label = new UILabel();

                    label.Text = customPin.Number == null ? "" : customPin.Number;
                    label.TextColor = UIColor.Black;

                    label.Frame = new CGRect(0, 0, 20, 20);
                    label.TextAlignment = UITextAlignment.Center;

                    UIView f = new UIView();
                    f.Frame = new CGRect(5, 5, 20, 20);

                    f.Layer.CornerRadius = 10;
                    f.BackgroundColor = UIColor.White;

                    f.AddSubview(label);

                    //f.Center = new CGPoint(label.Frame.Size.Width / 2, label.Frame.Size.Height / 2);

                    Debug.WriteLine("INPUT COLOR: " + customPin.Color);
                    Debug.WriteLine("GET COLOR: " + GetPin(customPin.Color));
                    annotationView.Image = UIImage.FromFile(GetPin(customPin.Color));




                    //annotationView.CalloutOffset = new CGPoint(0, 0);
                    //annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
                    //annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                    //((CustomMKAnnotationView)annotationView).Name = customPin.Name;
                    //((CustomMKAnnotationView)annotationView).Url = customPin.Url;

                    annotationView.AddSubview(f);

                }
                annotationView.CanShowCallout = true;
            }


            //annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
            

            return annotationView;
        }

        private void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            //customPinView = new UIView();

            //if (customView.Name.Equals("Xamarin"))
            //{
            //    customPinView.Frame = new CGRect(0, 0, 200, 84);
            //    var image = new UIImageView(new CGRect(0, 0, 200, 84));
            //    image.Image = UIImage.FromFile("xamarin.png");
            //    customPinView.AddSubview(image);
            //    customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
            //    e.View.AddSubview(customPinView);
            //}
        }

        private void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            //CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
            //if (!string.IsNullOrWhiteSpace(customView.Url))
            //{
            //    UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
            //}
        }

        private void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            if (!e.View.Selected)
            {
                customPinView.RemoveFromSuperview();
                customPinView.Dispose();
                customPinView = null;
            }
        }

        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        string GetPin(string color)
        {
            string result = "";
            if(color == "Black")
            {
                result = "blackPin.png";
            }
            else if (color == "Red")
            {
                result = "redPin.png";
            }
            else if (color == "Green")
            {
                result = "greenPin.png";
            }
            else if (color == "Gray")
            {
                result = "grayPin.png";
            }
            return result;
        }
    }
}
