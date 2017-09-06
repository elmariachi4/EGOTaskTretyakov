using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
namespace EGOTaskTretyakov
{
    public class DatabaseEntityInfo : Fragment
    {
        View view;
        Bundle args;
        Button actionButton;
        EditText editName;
        TextInputEditText editDescription;
        Switch isActive;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.dbAddItem, container, false);
            actionButton = view.FindViewById<Button>(Resource.Id.dbActionButton);
            editName = view.FindViewById<EditText>(Resource.Id.dbAddEditName);
            editDescription = view.FindViewById<TextInputEditText>(Resource.Id.dbAddEditDescr);
            isActive = view.FindViewById<Switch>(Resource.Id.dbAddIsActiveSwitch);
            actionButton.Text = args.GetString("action") == "add" ? "ADD" : "SAVE";
            actionButton.Click += ActionButton_Click;
            return view;
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            if (editName.Text.Trim(' ') != "" || editDescription.Text.Trim(' ') != "")
            {
                SomeEntity se = new SomeEntity()
                {
                    Name = editName.Text,
                    Description = editDescription.Text,
                    IsActive = isActive.Selected,
                    Updated = DateTime.Now
                };
              //  if(args.GetString("action")=="add")
            }
               
        }
    }
}