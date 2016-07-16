using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinWorkTimer.Pages;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer
{
    public class Manager
    {
        App app;
        DatabaseManager databaseManager;
        StartPage startPage;
        ChoosePage choosePage;
        ChooseLine chooseLine;

        public DateTime StartTime { get; set; }
        public DateTime PauseTime { get; set; }
        int preventInterval = 0;

        public Manager()
        {
            databaseManager = new DatabaseManager();
            LookForMidnight();
        }
        public void Initialize(App app,
                               StartPage startPage, 
                               ChoosePage choosePage)
        {
            this.app = app;
            this.startPage = startPage;
            this.choosePage = choosePage;

            foreach (DatabaseItem item in databaseManager.GetItems())
                choosePage.AddLine(item.Name, item.Time);

            startPage.SummaryTime = databaseManager.SummaryTime();

            Device.StartTimer(TimeSpan.FromSeconds(0.5), EternalTimer);
        }
        
        public event EventHandler Midnight;
        private void LookForMidnight()
        {
            if(((DateTime)App.Current.Properties[gf.lastTime]).Date != DateTime.Today)
            {
                databaseManager.AddSumary(new DatabaseSummary
                {
                    Date = ((DateTime)App.Current.Properties[gf.lastTime]).ToString(gf.dateFormat),
                    Summary = databaseManager.SummaryTime()
                });
                databaseManager.cleanInterval();
                databaseManager.CleanTime();
                Midnight?.Invoke(null, EventArgs.Empty);
            }
        }

        private bool EternalTimer()
        {
            LookForMidnight();
            if(startPage.StopTimer == false)
            {
                int interval = (int)(DateTime.Now - StartTime).TotalSeconds;
                int delay = interval - preventInterval;
                if (delay > 0)
                {
                    if(startPage.LeftTime <= delay)
                        delay = startPage.LeftTime;

                    chooseLine.Time += delay;
                    databaseManager.UpdateItem(chooseLine.Name, delay);
                    startPage.SummaryTime += delay;
                    startPage.LeftTime -= delay;
                    preventInterval = interval;
                }
            }

            App.Current.Properties[gf.lastTime] = DateTime.Now;
            return true;
        }

        public void AddItem(string name)
        {
            if (!databaseManager.Contain(name))
            {
                databaseManager.AddItem(new DatabaseItem()
                {
                    Name = name,
                });
                choosePage.AddLine(name);
            }
        }
        public void AddEventsOnLine(ChooseLine cl)
        {
            cl.StartButtonClick += (object sender, EventArgs args) =>
            {
                FromChooseToStartPage(cl);
            };
            cl.DeleteButtonClick += (object sender, EventArgs args) =>
            {
                databaseManager.DeleteItem(cl.Name);
            };
        }
        public void OnStartPage()
        {
            app.MainPage = startPage;
        }
        public void FromChooseToStartPage(ChooseLine line)
        {
            chooseLine = line;
            startPage.StopTimer = false;
            StartTime = DateTime.Now;
            preventInterval = 0;
            OnStartPage();
        }
        public void OnChoosePage()
        {
            app.MainPage = choosePage;
        }
    }
}