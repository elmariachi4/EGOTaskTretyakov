package md5ebc8a5f0964c6e1bff3acb18eccd9d46;


public class PRADataSetObserver
	extends android.database.DataSetObserver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onChanged:()V:GetOnChangedHandler\n" +
			"n_onInvalidated:()V:GetOnInvalidatedHandler\n" +
			"";
		mono.android.Runtime.register ("PRAPinnedListView.PRADataSetObserver, PRAPinnedListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PRADataSetObserver.class, __md_methods);
	}


	public PRADataSetObserver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PRADataSetObserver.class)
			mono.android.TypeManager.Activate ("PRAPinnedListView.PRADataSetObserver, PRAPinnedListView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onChanged ()
	{
		n_onChanged ();
	}

	private native void n_onChanged ();


	public void onInvalidated ()
	{
		n_onInvalidated ();
	}

	private native void n_onInvalidated ();

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
