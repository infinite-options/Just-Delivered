package crc6485704ca69e78e604;


public class jliB5PnCcZ8AydE4G2
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.PictureCallback,
		android.hardware.Camera.PreviewCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPictureTaken:([BLandroid/hardware/Camera;)V:GetOnPictureTaken_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPictureCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onPreviewFrame:([BLandroid/hardware/Camera;)V:GetOnPreviewFrame_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPreviewCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SuI9CIDpvY5L35ZJQj.jliB5PnCcZ8AydE4G2, Leadtools.Camera.Xamarin.Android", jliB5PnCcZ8AydE4G2.class, __md_methods);
	}


	public jliB5PnCcZ8AydE4G2 ()
	{
		super ();
		if (getClass () == jliB5PnCcZ8AydE4G2.class)
			mono.android.TypeManager.Activate ("SuI9CIDpvY5L35ZJQj.jliB5PnCcZ8AydE4G2, Leadtools.Camera.Xamarin.Android", "", this, new java.lang.Object[] {  });
	}


	public void onPictureTaken (byte[] p0, android.hardware.Camera p1)
	{
		n_onPictureTaken (p0, p1);
	}

	private native void n_onPictureTaken (byte[] p0, android.hardware.Camera p1);


	public void onPreviewFrame (byte[] p0, android.hardware.Camera p1)
	{
		n_onPreviewFrame (p0, p1);
	}

	private native void n_onPreviewFrame (byte[] p0, android.hardware.Camera p1);

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
