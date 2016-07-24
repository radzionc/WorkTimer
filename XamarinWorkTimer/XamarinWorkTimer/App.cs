using System;
using Xamarin.Forms;
using XamarinWorkTimer.Pages;
using XamarinWorkTimer.Pages.Elements;

[assembly: Dependency(typeof(XamarinWorkTimer.App))]

namespace XamarinWorkTimer
{
    public class App : Application
    {
        DatabaseManager databaseManager;
        StartPage startPage;
        ChoosePage choosePage;
        ChooseLine chooseLine;

        DateTime StartTime;

        int preventInterval = 0;
        bool stopTimer = true;
        bool stopTick = false;
        int period;
        public bool InStartPage => MainPage == startPage;
        public App()
        {
            if (!Properties.ContainsKey(gf.slider))
                Properties[gf.slider] = 45;
            period = (int)Properties[gf.slider] * 60;
            if (!Properties.ContainsKey(gf.lastTime))
                Properties[gf.lastTime] = DateTime.Now;

            databaseManager = new DatabaseManager();
            startPage = new StartPage(databaseManager.SummaryTime(), (int)Properties[gf.slider]);
            choosePage = new ChoosePage();
            MainPage = startPage;
            LookForMidnight();

            foreach (DatabaseItem item in databaseManager.GetItems())
                AddChooseLine(item.Name, item.Time);
            choosePage.InputCompleted += (object sender, EventArgs args) =>
            {
                AddItem((string)sender);
            };

            startPage.ChooseButtonClicked += OnChoosePage;
            startPage.StopButtonClicked += Stop;
            startPage.SliderValueChanged += (object sender, EventArgs args) => 
            {
                Properties[gf.slider] = (int)sender;
                period = (int)sender * 60;
            };

            Device.StartTimer(TimeSpan.FromSeconds(0.5), Tick);
        }

        bool Tick()
        {
            if (stopTick) return false;
            LookForMidnight();

            if (!stopTimer)
            {
                int interval = (int)(DateTime.Now - StartTime).TotalSeconds;
                int delay = interval - preventInterval;
                int leftTime = period - interval;
                if (delay > 0)
                {
                    System.Diagnostics.Debug.WriteLine("\n\n\n\n" + delay.ToString() + "\n\n\n\n");
                    if (leftTime <= 0)
                    {
                        delay = period - preventInterval;
                        databaseManager.UpdateItem(chooseLine.Name, delay);
                        Stop(null, EventArgs.Empty);
                    }
                    else
                    {
                        preventInterval = interval;
                        databaseManager.UpdateItem(chooseLine.Name, delay);
                        startPage.updateUI(true, databaseManager.SummaryTime(), leftTime);
                    }
                                      
                }
            }
            return true;
        }

        void Stop(object sender, EventArgs args)
        {
            preventInterval = 0;
            stopTimer = true;
            startPage.updateUI(false, databaseManager.SummaryTime(), period);
        }

        public event EventHandler Midnight;

        private void LookForMidnight()
        {
            if (((DateTime)Properties[gf.lastTime]).Date != DateTime.Today)
            {
                //databaseManager.AddSumary(new DatabaseSummary
                //{
                //    Date = ((DateTime)App.Current.Properties[gf.lastTime]).ToString(gf.dateFormat),
                //    Summary = databaseManager.SummaryTime()
                //});
                databaseManager.cleanInterval();
                databaseManager.CleanTime();

                Midnight?.Invoke(null, EventArgs.Empty);
            }
            Properties[gf.lastTime] = DateTime.Now;
        }

        public void AddItem(string name)
        {
            if (!databaseManager.Contain(name))
            {
                databaseManager.AddItem(new DatabaseItem()
                {
                    Name = name,
                });
                AddChooseLine(name);
            }
        }

        public void OnStartPage()
        {
            MainPage = startPage;
        }

        public void FromChooseToStartPage(ChooseLine line)
        {
            chooseLine = line;
            stopTimer = false;
            StartTime = DateTime.Now;
            preventInterval = 0;
            DependencyService.Get<IReminderService>().Remind(period, "Got it!", "You finish " + chooseLine.Name + "!");
            OnStartPage();
        }

        public void OnChoosePage(object sender, EventArgs args)
        {
            if(chooseLine != null)
                chooseLine.Time = databaseManager.GetItem(chooseLine.Name).Time;
            MainPage = choosePage;
        }

        public void AddChooseLine(string name, int time = 0)
        {
            ChooseLine line = new ChooseLine(name, time);
            line.StartButtonClick += delegate { FromChooseToStartPage(line); };
            line.DeleteButtonClick += delegate { databaseManager.DeleteItem(line.Name); };
            Midnight += delegate { line.Time = 0; };
            choosePage.AddLine(line);
        }

        protected override void OnStart()
        {
            System.Diagnostics.Debug.WriteLine("Start");
        }

        protected override void OnSleep()
        {
            stopTick = true;
            System.Diagnostics.Debug.WriteLine("Sleep");
        }

        protected override void OnResume()
        {
            stopTick = false;

            Tick(); Device.StartTimer(TimeSpan.FromSeconds(0.5), Tick);
            System.Diagnostics.Debug.WriteLine("Resume");
        }
    }
}
