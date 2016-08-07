using System;
using Xamarin.Forms;
using XamarinWorkTimer.Pages;
using XamarinWorkTimer.Pages.Elements;
using XamarinWorkTimer.DataBase;

[assembly: Dependency(typeof(XamarinWorkTimer.App))]

namespace XamarinWorkTimer
{
    public partial class App : Application
    {
        StartPage startPage;
        ChoosePage choosePage;

        DateTime startTime;
        Interval interval;
        string name;

        int preventInterval = 0;
        bool stopTimer = true;
        bool stopTick = false;
        public bool InStartPage => MainPage == startPage;
        void PropertiesChecking()
        {
            if (!Properties.ContainsKey(g.slider))
                Properties[g.slider] = 46;
            if (!Properties.ContainsKey(g.lastTime))
                Properties[g.lastTime] = DateTime.Now;
        }
        public App()
        {

            InitializeComponent();

            PropertiesChecking();
            LookForMidnight();
            startPage = new StartPage();
            choosePage = new ChoosePage();
            MainPage = startPage;

            choosePage.LineClicked += (object sender, EventArgs args) =>
            {
                name = (string)sender;
                FromChooseToStartPage();
            };

            startPage.ChooseLabelClicked += OnChoosePage;
            startPage.StatisticLabelClicked += OnStatisticPage;
            startPage.StopLabelClicked += Stop;

            Device.StartTimer(TimeSpan.FromSeconds(0.5), Tick);
        }

        bool Tick()
        {
            if (stopTick) return false;
            LookForMidnight();

            if (!stopTimer)
            {
                int currentInterval = (int)(DateTime.Now - startTime).TotalSeconds;
                int delay = currentInterval - preventInterval;
                int leftTime = g.period * 60 - currentInterval;
                if (delay > 0)
                {
                    if (leftTime <= 0)
                        delay = g.period * 60 - preventInterval;

                    interval.Sum += delay;
                    g.intervalDB.Update(interval);

                    if (leftTime > 0)
                    {
                        preventInterval = currentInterval;
                        startPage.updateUI(!stopTimer, leftTime);
                    }
                    else Stop(null, EventArgs.Empty);

                }
            }
            return true;
        }

        void Stop(object sender, EventArgs args)
        {
            preventInterval = 0;
            stopTimer = true;
            //////Weird bag////////
            MainPage = choosePage;
            OnStartPage();
            //////Weird bag////////
            DependencyService.Get<IReminderService>().Cancel();
        }

        private void LookForMidnight()
        {
            if (((DateTime)Properties[g.lastTime]).Date != DateTime.Today)
            {
                int sum = g.TodaySum();
                string date = ((DateTime)Properties[g.lastTime]).ToString(g.dateFormat);
                if (g.sumDB.Get(date).DatePK != null)
                    g.sumDB.Delete(date);
                g.sumDB.Add(new Sum { DatePK = date, Value = sum });

                g.intervalDB.DeleteAll();
                if (stopTimer == false)
                {
                    interval.StartPK = DateTime.Now.ToString(g.timeFormat);
                    interval.Name = name;
                    interval.Sum = 0;
                }
            }
            Properties[g.lastTime] = DateTime.Now;
        }

        public void OnStartPage()
        {
            startPage.updateUI(!stopTimer, g.period * 60);
            MainPage = startPage;
        }

        public void FromChooseToStartPage()
        {
            stopTimer = false;
            startTime = DateTime.Now;

            interval.StartPK = startTime.ToString(g.timeFormat);
            interval.Name = name;
            interval.Sum = 0;
            g.intervalDB.Add(interval);

            preventInterval = 0;

            DependencyService.Get<IReminderService>().Remind(g.period * 60, "Got it!", "You finish " + name + "!");
            OnStartPage();
        }

        public void OnChoosePage(object sender, EventArgs args)
        {
            MainPage = choosePage;
        }
        public void OnStatisticPage(object sender, EventArgs args)
        {
            MainPage = new StatisticPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            stopTick = true;
        }

        protected override void OnResume()
        {
            stopTick = false;

            Tick(); Device.StartTimer(TimeSpan.FromSeconds(0.5), Tick);
            System.Diagnostics.Debug.WriteLine("Resume");
        }
    }
}
