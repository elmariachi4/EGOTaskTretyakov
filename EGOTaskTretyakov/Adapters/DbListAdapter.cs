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

namespace EGOTaskTretyakov
{
    class DbListAdapter : BaseAdapter
    {
        Activity _activity;
        List<SomeEntity> infoList;

        public DbListAdapter(Activity act, List<SomeEntity> list)
        {
            _activity = act;
            infoList = list;
        }
        

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return infoList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = infoList[position];
            //var view = convertView;
           // if (view == null)
            var view =  _activity.LayoutInflater.Inflate(
                Resource.Layout.DBListItem, parent, false);

            view.FindViewById<TextView>(Resource.Id.dbItemID).Text = item.Id.ToString();
            view.FindViewById<TextView>(Resource.Id.dbItemName).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.dbItemDescription).Text = item.Description;
            view.SetBackgroundColor(item.IsActive ? Color.Argb(150,90,238,90) : Color.Argb(150,211,211,211)); //LightGreen and LightGray colors with mid alpha
            view.FindViewById<TextView>(Resource.Id.dbItemDT).Text = item.Updated.ToString("ddd, dd.MM.yyyy HH:mm:ss");


            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return infoList.Count;
            }
        }
    }
}