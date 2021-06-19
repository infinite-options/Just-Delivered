using System;
using MapKit;

namespace JustDelivered.iOS
{
	public class CustomMKAnnotationView : MKAnnotationView
	{
		public string Name { get; set; }

		public string Url { get; set; }

		public CustomMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
		{

		}
	}
}
