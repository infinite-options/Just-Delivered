package crc64c1bdfae6ab592b4d;


public class BV9LtSYftT9yCsPst8
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.PictureCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPictureTaken:([BLandroid/hardware/Camera;)V:GetOnPictureTaken_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPictureCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("KYs3QY9N0I0GADvwR1.BV9LtSYftT9yCsPst8, Leadtools.Camera.Xamarin.Android", BV9LtSYftT9yCsPst8.class, __md_methods);
	}


	public BV9LtSYftT9yCsPst8 ()
	{
		super ();
		if (getClass () == BV9LtSYftT9yCsPst8.class)
			mono.android.TypeManager.Activate ("KYs3QY9N0I0GADvwR1.BV9LtSYftT9yCsPst8, Leadtools.Camera.Xamarin.Android", "", this, new java.lang.Object[] {  });
	}


	public void onPictureTaken (byte[] p0, android.hardware.Camera p1)
	{
		n_onPictureTaken (p0, p1);
	}

	private native void n_onPictureTaken (byte[] p0, android.hardware.Camera p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
