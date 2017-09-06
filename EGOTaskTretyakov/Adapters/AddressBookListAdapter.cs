using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace EGOTaskTretyakov
{
    class AddressBookListAdapter : BaseAdapter
    {

        Activity activity;
        List<string> actionsList; //if it's needed to use adapter for actions popup
        List<Contact> contactList; //if contact list uses the adapter.
        public AddressBookListAdapter(Activity _activity, List<Contact> _contactList)
        {
            activity = _activity;
            contactList = _contactList;
        }
        public AddressBookListAdapter(Activity _activity, List<string> _actionsList)
        {
            activity = _activity;
            actionsList = _actionsList;
        }
        
        public override int Count
        {
            get
            {
                return actionsList == null ? contactList.Count : actionsList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;//not used
        }

        public void RefreshList(List<Contact> newList)
        {
            contactList.Clear();
            contactList = newList;
            NotifyDataSetChanged();
        }
        public override long GetItemId(int position)
        {
            return 0; //not used
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(
                Resource.Layout.AddressBookListItem, parent, false);
            var contactName = view.FindViewById<TextView>(Resource.Id.ContactName);
            contactName.Text = actionsList == null ? contactList[position].Name : actionsList[position]; //adapter is used for different purposes, so don't pay attention to its name.
            return view;
        }
    }
      public class Contact //contact class and its parameters
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
    }