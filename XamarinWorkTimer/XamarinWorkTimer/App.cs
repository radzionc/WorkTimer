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
        DateTime PauseTime;
        int preventInterval;
        bool stopTimer = true;
        public bool InStartPage => MainPage == startPage;
        public App()
        {
            gf.PropertiesCheck();
            databaseManager = new DatabaseManager();
            startPage = new StartPage();
            choosePage = new ChoosePage();
            MainPage = startPage;
            LookForMidnight();

            foreach (DatabaseItem item in databaseManager.GetItems())
                AddChooseLine(item.Name, item.Time);
            choosePage.InputCompleted += (object sender, EventArgs args) =>
            {
                AddItem((string)sender);
            };

            startPage.SummaryTime = databaseManager.SummaryTime();
            startPage.ChooseButtonClicked += OnChoosePage;
            startPage.StopButtonClicked += Stop;
            startPage.PauseButtonClicked += Pause;

            Device.StartTimer(TimeSpan.FromSeconds(0.5), EternalTimer);
        }

        private bool EternalTimer()
        {
            LookForMidnight();
            if (stopTimer == false)
            {
                int interval = (int)(DateTime.Now - StartTime).TotalSeconds;
                int delay = interval - preventInterval;
                if (delay > 0)
                {
                    if (startPage.LeftTime <= delay)
                    {
                        delay = startPage.LeftTime;
                        Stop(null, EventArgs.Empty);
                    }
                    else
                        preventInterval = interval;

                    databaseManager.UpdateItem(chooseLine.Name, delay);

                    chooseLine.Time += delay;
                    startPage.SummaryTime += delay;
                    startPage.LeftTime -= delay;
                }
            }

            App.Current.Properties[gf.lastTime] = DateTime.Now;
            return true;
        }

        void Stop(object sender, EventArgs args)
        {
            preventInterval = 0;
            stopTimer = true;
            startPage.NoTimerUI();
        }

        void Pause(object sender, EventArgs args)
        {
            stopTimer = !stopTimer;
            if (stopTimer)
                PauseTime = DateTime.Now;
            else
                StartTime += DateTime.Now - PauseTime;
        }

        public event EventHandler Midnight;

        private void LookForMidnight()
        {
            if (((DateTime)App.Current.Properties[gf.lastTime]).Date != DateTime.Today)
            {
                databaseManager.AddSumary(new DatabaseSummary
                {
                    Date = ((DateTime)App.Current.Properties[gf.lastTime]).ToString(gf.dateFormat),
                    Summary = databaseManager.SummaryTime()
                });
                databaseManager.cleanInterval();
                databaseManager.CleanTime();

                startPage.SummaryTime = 0;
                Midnight?.Invoke(null, EventArgs.Empty);
            }
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
            OnStartPage();
        }

        public void OnChoosePage(object sender, EventArgs args)
        {
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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
