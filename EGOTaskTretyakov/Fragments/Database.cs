using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
namespace EGOTaskTretyakov
{
    public class Database : Fragment
    {
        public static Fragment GetInstance() { return new Database(); }
        PopupWindow popUp;
        View view, popupView;
        List<SomeEntity> infoList  = new List<SomeEntity>();
        ListView dbList;
        Button btAddData, actionButton;
        EditText editName;
        TextInputEditText editDescription;
        Switch isActive;
        private int tempID;
        public static SQLiteConnection dbConnection;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            dbConnection = new SQLiteConnection(System.IO.Path.Combine(dbPath, "Database"));
            dbConnection.CreateTable<SomeEntity>(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK); //creating db for items
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.Database, container, false);
            popupView = inflater.Inflate(Resource.Layout.dbAddItem, container, false);
            //Database page UI
            dbList = view.FindViewById<ListView>(Resource.Id.dbListView);
            dbList.ItemClick += (sender, eArgs) =>
            {
                SomeEntity cellInfo = infoList[eArgs.Position];
                bool isClicked = false;
                popUp = CreateDbPopup("SAVE");
                tempID = Convert.ToInt32(eArgs.View.FindViewById<TextView>(Resource.Id.dbItemID).Text);
                popUp.ContentView.FindViewById<EditText>(Resource.Id.dbAddEditName).Text = eArgs.View.FindViewById<TextView>(Resource.Id.dbItemName).Text;
                popUp.ContentView.FindViewById<EditText>(Resource.Id.dbAddEditDescr).Text = eArgs.View.FindViewById<TextView>(Resource.Id.dbItemDescription).Text;
                if (!isClicked)
                    popUp.ShowAtLocation(View, GravityFlags.Center, 0, 0);
                isClicked = true;
            };
            dbList.ItemLongClick += (sender, eArgs) => 
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(Context);
                AlertDialog alert = dlg.Create();
                dlg.SetMessage("Delete this item?");
                dlg.SetPositiveButton("OK", (_sender, e) => 
                {
                    dbConnection.Delete(infoList[eArgs.Position]);
                    dbConnection.Commit();
                    FillListFromDb();
                    alert.Dismiss();
                    dbList.RefreshDrawableState();
                    Toast.MakeText(Context, "Done!", ToastLength.Short).Show();
                });
                dlg.SetNegativeButton("Cancel", (_sender, e) => { alert.Dismiss(); });
                dlg.Show();
            };

            btAddData = view.FindViewById<Button>(Resource.Id.btnAddData);
            btAddData.Click += (sender,e) => 
            {
                popUp = CreateDbPopup("ADD");
                bool isClicked = false;
                if (!isClicked)
                    popUp.ShowAtLocation(View, GravityFlags.Center, 0, 0);
                isClicked = true;
            };

            FillListFromDb(); //fill list with data from DB
            return view;
        }

        public PopupWindow CreateDbPopup(string action)
        {
            popUp = new PopupWindow(Activity.CurrentFocus);
            popUp.ContentView = popupView;
            popUp.Width = ViewGroup.LayoutParams.MatchParent;
            popUp.Height = ViewGroup.LayoutParams.WrapContent;
            popUp.OutsideTouchable = true;
            popUp.SetBackgroundDrawable(new ColorDrawable(Color.LightGray));
            popUp.Focusable = true;
            popUp.Update();
            //----------------------------
            actionButton = popupView.FindViewById<Button>(Resource.Id.dbActionButton);
            editName = popupView.FindViewById<EditText>(Resource.Id.dbAddEditName);
            editDescription = popupView.FindViewById<TextInputEditText>(Resource.Id.dbAddEditDescr);
            isActive = popupView.FindViewById<Switch>(Resource.Id.dbAddIsActiveSwitch);

            actionButton.Click += delegate
            {
                actionButton.Enabled = false;
                if (editName.Text.Trim(' ') != "" || editDescription.Text.Trim(' ') != "")
                {
                    SomeEntity se = new SomeEntity()
                    {
                        Name = editName.Text,
                        Description = editDescription.Text,
                        IsActive = isActive.Checked,
                        Updated = DateTime.Now
                    };
                    if (action == "ADD")
                        dbConnection.Insert(se);
                    else
                    {
                        se.Id = tempID;
                        dbConnection.Update(se);
                    }
                    popUp.Dismiss();
                    FillListFromDb();
                }
                else
                    Toast.MakeText(Context, "Fill the fields!", ToastLength.Short).Show();
                actionButton.Enabled = true;
            };
            actionButton.Text = action;
            return popUp;
        }

        public void FillListFromDb() //refresh data inside the listview
        {
            infoList.Clear(); //clearing list with data
            var dbQuery = dbConnection.Table<SomeEntity>();
            foreach (var smEnt in dbQuery)
                infoList.Add(smEnt);
            dbList.Adapter = new DbListAdapter(Activity, infoList);
            
        }
    }
}