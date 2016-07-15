using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamarinWorkTimer;
using XamarinWorkTimer.Pages;

[assembly: Dependency(typeof(XamarinWorkTimer.App))]

namespace XamarinWorkTimer
{
    public class App : Application
    {
        StartPage startPage;
        ChoosePage choosePage;
        Manager pageManager;
        public App()
        {
            gf.PropertiesCheck();
            pageManager = new Manager();
                startPage = new StartPage(pageManager);
                choosePage = new ChoosePage(pageManager);
            pageManager.Initialize(this, startPage, choosePage);

            MainPage = startPage;
        }

        public bool OnStartPage()
        {
            if (MainPage == startPage)
                return true;
            MainPage = startPage;
            return false;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
