using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Net;
using Android.Support.V7.Widget;
using System.Threading;
using Android.Graphics.Drawables;

namespace EGOTaskTretyakov
{
    class ImgCollectListAdapter : BaseAdapter
    {

        Activity activity;
        int imgAmount;
        Random rnd = new Random();
        public static List<Bitmap> imgList { get; set; }
        public ImgCollectListAdapter(Activity _activity, int _imgAmount)
        {
            activity = _activity;
            imgAmount = _imgAmount;
            imgList = new List<Bitmap>();
        }

        public override int Count
        {
            get
            {
                return imgAmount;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ImgCollListItem, parent, false);
            var image = view.FindViewById<ImageView>(Resource.Id.imgColl_item);
            Bitmap bmp = null;
            new Thread(new ThreadStart(() => 
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var imageBytes = webClient.DownloadData(string.Format("http://placekitten.com/{0}/{1}/", rnd.Next(100, 300), rnd.Next(100, 300)));
                        if (imageBytes != null && imageBytes.Length > 0)
                            bmp = Bitmap.CreateBitmap(BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length));

                        activity.RunOnUiThread(() => image.SetImageBitmap(bmp));
                        imgList.Add(bmp);
                    }

                }
                catch (WebException)
                {
                    image.SetImageDrawable(new ColorDrawable(Color.Red)); //failed to load
                }
            })).Start();
            


            return view;
        }

        List<Bitmap> GenerateImageList(int amount)
        {
            List<Bitmap> _list = new List<Bitmap>();
            Random rnd = new Random();
            for (int i = 0; i < amount; i++)
            {
                Bitmap bmp = null;
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(string.Format("http://placekitten.com/{0}/{1}/", rnd.Next(100, 300), rnd.Next(100, 300)));
                    if (imageBytes != null && imageBytes.Length > 0)
                        bmp = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
                _list.Add(bmp);
            }
            return _list;
        }

    }
}