using System;
using Xamarin.Forms;
using XamarinWorkTimer.Pages;
using XamarinWorkTimer.Pages.Elements;
using XamarinWorkTimer.DataBase;

[assembly: Dependency(typeof(XamarinWorkTimer.App))]

namespace XamarinWorkTimer
{
    public class App : Application
    {
        ItemDB itemDB = new ItemDB(gf.database);
        DB<Interval> intervalDB = new DB<Interval>(gf.intervalsDatabase);
        DB<Summary> summaryDB = new DB<Summary>(gf.summaryDatabase);
        StartPage startPage;
        ChoosePage choosePage;
        ChooseLine chooseLine;

        DateTime startTime;
        Interval interval;
        Item item;

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
            
            LookForMidnight();
            startPage = new StartPage(itemDB.Sum(), (int)Properties[gf.slider]);
            choosePage = new ChoosePage();
            MainPage = startPage;

            foreach (Item item in itemDB.GetAll())
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
                int currentInterval = (int)(DateTime.Now - startTime).TotalSeconds;
                int delay = currentInterval - preventInterval;
                int leftTime = period - currentInterval;
                if (delay > 0)
                {
                    if (leftTime <= 0)
                        delay = period - preventInterval;

                    item.Time += delay;
                    itemDB.Update(item);
                    interval.Sum += delay;
                    intervalDB.Update(interval);

                    if (leftTime > 0)
                    {
                        preventInterval = currentInterval;
                        startPage.updateUI(true, itemDB.Sum(), leftTime);
                    }
                                      
                }
            }
            return true;
        }

        void Stop(object sender, EventArgs args)
        {
            preventInterval = 0;
            stopTimer = true;
            startPage.updateUI(false, itemDB.Sum(), period);
        }

        public event EventHandler Midnight;

        private void LookForMidnight()
        {
            if (((DateTime)Properties[gf.lastTime]).Date != DateTime.Today)
            {
                int sum = itemDB.Sum();
                string date = ((DateTime)Properties[gf.lastTime]).ToString(gf.dateFormat);
                if (summaryDB.Contain(date))
                    summaryDB.Delete(date);
                summaryDB.Add(new Summary { Date = date, Sum = sum});

                intervalDB.DeleteAll();
                if(stopTimer == false)
                {
                    item.Time = 0;
                    interval.Start = DateTime.Now.ToString(gf.timeFormat);
                    interval.Name = item.Name;
                    interval.Sum = 0;
                }
                itemDB.Clean();

                Midnight?.Invoke(null, EventArgs.Empty);
            }
            Properties[gf.lastTime] = DateTime.Now;
        }

        public void AddItem(string name)
        {
            if (!itemDB.Contain(name))
            {
                itemDB.Add(new Item(){ Name = name });
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
            startTime = DateTime.Now;

            interval.Start = startTime.ToString(gf.timeFormat);
            interval.Name = chooseLine.Name;
            interval.Sum = 0;
            intervalDB.Add(interval);

            item = itemDB.Get(chooseLine.Name);
            preventInterval = 0;

            DependencyService.Get<IReminderService>().Remind(period, "Got it!", "You finish " + chooseLine.Name + "!");
            OnStartPage();
        }

        public void OnChoosePage(object sender, EventArgs args)
        {
            if(chooseLine != null)
                chooseLine.Time = itemDB.Get(chooseLine.Name).Time;
            MainPage = choosePage;
        }

        public void AddChooseLine(string name, int time = 0)
        {
            ChooseLine line = new ChooseLine(name, time);
            line.StartButtonClick += delegate { FromChooseToStartPage(line); };
            line.DeleteButtonClick += delegate { itemDB.Delete(line.Name); };
            Midnight += delegate { line.Time = 0; };
            choosePage.AddLine(line);
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
