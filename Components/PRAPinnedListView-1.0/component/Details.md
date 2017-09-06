# PRA Pinned Header #
PRAPinnedListView  enables you to use Sticky Headers feature with header click, use this list view to make your app look awesome. You don't need to write lot's of code to use this just few simple steps and you can add it to your app. this contains a generic adapter also that means you also don't need to make a separate list adapter to bind list, just pass your item collection and your list will be bind prestty well. **:)**

# Features #
- Sticky Headers
- Inbuilt Adapter
- Header Click


## PRAListView ##

Ready to use!! Follow me >

- Add PRAPinnedListView reference to app.
- Add PRAListView to your Xaml file (e.g. Main.xaml)

	     <prapinnedlistview.PRAListView
		    android:id="@+id/list"
		    android:layout_width="match_parent"
		    android:layout_height="match_parent" />

- Access in your activity (e.g. MainActivity.cs).
			
			#region Pinned List View

            PRAService.Register<IListItemView<ItemHolder>>(this);
            listView = FindViewById<PRAListView>(Resource.Id.list);
            listView.SetItemSource<ItemHolder>(DataHandler.GetData());
            
            #endregion

You have done your most time taken part here :) Now we just need to pass **Item Header View** and **Item View** these view will be customized as per your need :) Yeppeeeee!! we have done

