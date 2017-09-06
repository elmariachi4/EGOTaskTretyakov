using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Android.Widget;
using Android.Provider;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Threading;

namespace EGOTaskTretyakov
{
    public class AddressBook : Fragment
    {
        
        public static List<Contact> contList;
        private View view, popupView, emailView;
        ListView addressListView;
        Android.Widget.SearchView searchBox;
        public static Fragment GetInstance() { return new AddressBook(); }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.AddressBook, container, false);
            popupView = inflater.Inflate(Resource.Layout.ActionsOnContact, container, false); //view for actions popup
            emailView = inflater.Inflate(Resource.Layout.MailTemplate, container, false); //view for sending email template

            Android.App.ProgressDialog pd = Android.App.ProgressDialog.Show(Context, "", "Loadind contacts..", false, false);

            new Thread(new ThreadStart(() =>
            {
            searchBox = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchBox);
            searchBox.QueryTextChange += (sender, e) => 
            {
                if (!string.IsNullOrEmpty(e.NewText))
                {
                    List<Contact> newContactList = contList.FindAll(x => x.Name.ToLower().Contains(e.NewText.ToLower())); //filtering query for the list
                    addressListView.Adapter = new AddressBookListAdapter(Activity, newContactList);
                }
                else
                    addressListView.Adapter = new AddressBookListAdapter(Activity, contList);
            };

            addressListView = view.FindViewById<ListView>(Resource.Id.addressBookList);
            addressListView.SetDrawSelectorOnTop(true);
            addressListView.ItemClick += ((sender, e) =>
            {
                PopupWindow popup = CreateActionsPopup(contList[e.Position], e);
                popup.OutsideTouchable = true;
                popup.ShowAtLocation(View, GravityFlags.Center, 0, 0);
            });
            GetContactList();
            addressListView.Adapter = new AddressBookListAdapter(Activity, contList);
            pd.Dismiss();
            })).Start();
            return view;
        }
        
        public List<string> RequestContactData(Contact contact) //used to generate list with actions
        {
            List<string> actionsList = new List<string>();
            string[] numbers = contact.PhoneNumber.Split(';');
            string[] emails = contact.Email.Split(';');
            foreach (string num in numbers)
                if(num!="")
                    actionsList.Add(string.Format("Call {0}",num));
            foreach(string mail in emails)
                if(mail!="")
                    actionsList.Add(string.Format("Send email to {0}",mail));
            return actionsList;
        }

        PopupWindow CreateActionsPopup(Contact contactActions, AdapterView.ItemClickEventArgs e)
        {
            PopupWindow popup = new PopupWindow(Activity.CurrentFocus);
            popup.ContentView = popupView;
            popup.Focusable = true;
            popup.Update();
            popup.OutsideTouchable = true;
            popup.SetBackgroundDrawable(new ColorDrawable(Color.LightGray));
            popup.AnimationStyle = Resource.Style.Animation_AppCompat_Dialog;
            popup.Width = ViewGroup.LayoutParams.WrapContent;
            popup.Height = ViewGroup.LayoutParams.WrapContent;
            ListView actionsListView = popupView.FindViewById<ListView>(Resource.Id.actionsListView);
            actionsListView.Adapter = new AddressBookListAdapter(Activity, RequestContactData(contactActions));
            actionsListView.ItemClick += (sender,eArgs) => 
            {
                List<string> a = RequestContactData(contactActions);
                if (a[eArgs.Position].StartsWith("Call"))
                {
                    popup.Dismiss();
                    var callIntent = new Intent(Intent.ActionCall, 
                        Android.Net.Uri.Parse(string.Format("tel:{0}", 
                        a[eArgs.Position].Substring(5).Trim(' ', '+', '-')))); //definitely not the best optimization concept, but still works
                    StartActivity(callIntent); 
                }
                else
                {
                    PopupWindow mailPopup = new PopupWindow(Activity.CurrentFocus); //creating nested popup, if user choses sending email
                    mailPopup.ContentView = emailView;
                    mailPopup.Focusable = true;
                    mailPopup.Update();
                    mailPopup.OutsideTouchable = true;
                    mailPopup.SetBackgroundDrawable(new ColorDrawable(Color.LightGray));
                    mailPopup.AnimationStyle = Resource.Style.Animation_AppCompat_Dialog;
                    mailPopup.Width = ViewGroup.LayoutParams.WrapContent;
                    mailPopup.Height = ViewGroup.LayoutParams.WrapContent;
                    EditText editTitle = emailView.FindViewById<EditText>(Resource.Id.editMailTitle);
                    EditText editMessage = emailView.FindViewById<EditText>(Resource.Id.editMailMessage);
                    editTitle.Text = string.Format("Hello, {0}", contactActions.Name); //template for title
                    editMessage.Text = string.Format("Dear {0}, ", contactActions.Name);// and message of email
                    Button buttonSend = emailView.FindViewById<Button>(Resource.Id.buttonSend);
                    buttonSend.Click += (_sender, _e) => 
                    {
                        var email = new Intent(Intent.ActionSend);
                        email.PutExtra(Intent.ExtraEmail, a[eArgs.Position]);
                        email.PutExtra(Intent.ExtraSubject, string.Format("Hello, {0}, ", contactActions.Name));
                        email.PutExtra(Intent.ExtraText, string.Format("Dear {0}, ", contactActions.Name));
                        email.SetType("message/rfc822");
                        StartActivity(email); //starting the mail app with given data
                    };
                    emailView.FindViewById<TextView>(Resource.Id.textViewMailHeader).Text = string.Format("Email to {0}",contactActions.Name);
                    mailPopup.ShowAtLocation(view, GravityFlags.Center, 0, 0);
                }
            };
            return popup;
        }

        void GetContactList()
        {
            contList = new List<Contact>();
            ContentResolver cr = Context.ContentResolver;
            var cur = cr.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null);
            if (cur.Count > 0)
            {
                while (cur.MoveToNext())
                {
                    string id = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId));
                    string name = cur.GetString(cur.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                    string phone = cur.GetString(cur.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    string email = "";
                    cr.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null); //query to get the phones
                    var emailCur = cr.Query(ContactsContract.CommonDataKinds.Email.ContentUri,
                                            null, 
                                            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = " + id.ToString(), 
                                            null,
                                            null); // query to get contact's emails
                    while (emailCur.MoveToNext())
                    {
                        email = emailCur.GetString(emailCur.GetColumnIndex(ContactsContract.CommonDataKinds.Email.Address));
                    }
                    if(contList.Exists(v => v.Name == name))
                        {
                        contList.Find(v => v.Name == name).PhoneNumber += ";" + phone;
                        contList.Find(v => v.Name == name).Email += ";" + email;
                        }
                    else 
                        contList.Add(new Contact { Id = Convert.ToInt32(id), Name = name, PhoneNumber = phone, Email = email });
                }
                contList = contList.OrderBy(v => v.Name).ToList(); //setting the list to alphabetical order
            }
        }
    }
}