using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Net;

namespace EGOTaskTretyakov
{
    public class ImagesCollection : Fragment
    {
        private View view;

        ListView imgList;
        public static Fragment GetInstance() { return new ImagesCollection(); }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.ImagesCollection, container, false);
            imgList = view.FindViewById<ListView>(Resource.Id.imgList);

            imgList.Adapter = new ImgCollectListAdapter(Activity, 50);
            return view;
        }
        

      
    }
}