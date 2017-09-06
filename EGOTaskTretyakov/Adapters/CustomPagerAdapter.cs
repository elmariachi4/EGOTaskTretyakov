using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Java.Lang;

namespace EGOTaskTretyakov
{
    class CustomPagerAdapter : FragmentStatePagerAdapter
    {
        const int PAGE_AMOUNT = 4;
        private string[] tabTitles = { "Address Book", "Images Collection", "Database", "Image Change" };
        readonly Context context;

        public CustomPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomPagerAdapter(Context context, FragmentManager fm) : base(fm)
        {
            this.context = context;
        }

        public override int Count
        {
            get { return PAGE_AMOUNT; }
        }

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return AddressBook.GetInstance();
                case 1:
                    return ImagesCollection.GetInstance();
                case 2:
                    return Database.GetInstance();
                case 3:
                    return ImageChange.GetInstance();
            }
            return null;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return CharSequence.ArrayFromStringArray(tabTitles)[position];
        }

        public View GetTabView(int position)
        {
            var tv = (TextView)LayoutInflater.From(context).Inflate(Resource.Layout.TabView, null);
            tv.Text = tabTitles[position];
            return tv;
        }
    }
}