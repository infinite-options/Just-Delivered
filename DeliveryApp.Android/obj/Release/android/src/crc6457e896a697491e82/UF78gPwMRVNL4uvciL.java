package crc6457e896a697491e82;


public class UF78gPwMRVNL4uvciL
	extends android.view.OrientationEventListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onOrientationChanged:(I)V:GetOnOrientationChanged_IHandler\n" +
			"";
		mono.android.Runtime.register ("nkSmBSsyJm4eISfkmt.UF78gPwMRVNL4uvciL, Leadtools.Camera.Xamarin.Android", UF78gPwMRVNL4uvciL.class, __md_methods);
	}


	public UF78gPwMRVNL4uvciL (android.content.Context p0)
	{
		super (p0);
		if (getClass () == UF78gPwMRVNL4uvciL.class)
			mono.android.TypeManager.Activate ("nkSmBSsyJm4eISfkmt.UF78gPwMRVNL4uvciL, Leadtools.Camera.Xamarin.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void onOrientationChanged (int p0)
	{
		n_onOrientationChanged (p0);
	}

	private native void n_onOrientationChanged (int p0);

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
