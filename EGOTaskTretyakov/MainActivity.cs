using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace EGOTaskTretyakov
{
    [Activity(Label = "EGOTaskTretyakov", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation =Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {

        protected override int LayoutResource
        {
            get { return Resource.Layout.main; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);

            CustomPagerAdapter adapter = new CustomPagerAdapter(this, SupportFragmentManager);
            pager.Adapter = adapter; // Set adapter to view pager
            tabLayout.SetupWithViewPager(pager); // Setup tablayout with view pager

            for (int i = 0; i < tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                tab.SetCustomView(adapter.GetTabView(i));
                
            }

        }
    }
}

