using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Graphics;
using System.Net;
using Android.Support.V7.App;
using System.Threading;

namespace EGOTaskTretyakov
{
    public class ImageChange : Fragment
    {
        public static Fragment GetInstance() { return new ImageChange(); }
        View view;
        Button buttonRotate, buttonFlipV, buttonFlipH;
        ImageView img;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.ImageChange, container, false);
            img = view.FindViewById<ImageView>(Resource.Id.imageForRotation);
            img.BuildDrawingCache(true);
            new Thread(new ThreadStart(delegate
            {
                Activity.RunOnUiThread(() =>
                {
                    Android.App.ProgressDialog progress = Android.App.ProgressDialog.Show(img.Context, "", "Loading image..", true);
                    img.SetImageBitmap(GetPictureFromUrl(string.Format("http://placekitten.com/g/{0}/{1}", img.Width, img.Height)));
                    progress.Dismiss();
                }
                );
            })).Start();

            buttonRotate = view.FindViewById<Button>(Resource.Id.buttonRotate);
            buttonFlipH = view.FindViewById<Button>(Resource.Id.buttonFlipH);
            buttonFlipV = view.FindViewById<Button>(Resource.Id.buttonFlipV);

            //actions on rotate button
            buttonRotate.Click += (sender,e) =>
            {
                img.SetImageBitmap(TransformImage(0));
                img.BuildDrawingCache(true);
            };
            //actions on flip horizontal button
            buttonFlipH.Click += (sender, e) => 
            {
               img.SetImageBitmap(TransformImage(1));
               img.BuildDrawingCache(true);
            };
            //actions on flip vertical button
            buttonFlipV.Click += (sender, e) =>
            {
                img.SetImageBitmap(TransformImage(2));
                img.BuildDrawingCache(true);
            };
            return view;
        }

        public Bitmap TransformImage(int action)
        {
            img.BuildDrawingCache(true);
            Matrix matrix = new Matrix();
            switch (action)
            {
                case 0:
                    matrix.PostRotate(90);
                    break;
                case 1:
                    matrix.PostScale(-1, 1);
                    break;
                case 2:
                    matrix.PostScale(1, -1);
                    break;
            }
            return Bitmap.CreateBitmap(img.GetDrawingCache(true), 0, 0, img.Height, img.Width, matrix, true);
        }

        public Bitmap GetPictureFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (WebException)
            {
                Toast.MakeText(Context,"Unable to load image. Reload page to try again.",ToastLength.Long).Show();
            }
            return imageBitmap;
        }
    }
}