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
        ItemDB itemDB = new ItemDB(g.item);
        DB<Interval> intervalDB = new DB<Interval>(g.interval);
        DB<Sum> sumDB = new DB<Sum>(g.sum);
        StartPage startPage;
        ChoosePage choosePage;
        ChooseLine chooseLine;

        DateTime startTime;
        Interval interval;
        Item item;

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

            foreach (Item item in itemDB.GetAll())
                AddChooseLine(item.NamePK, item.Time);
            choosePage.InputCompleted += (object sender, EventArgs args) =>
            {
                AddItem((string)sender);
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

                    System.Diagnostics.Debug.WriteLine($"!!!\n current = {currentInterval}\n delay = {delay}\n leftTime = {leftTime}\n!!!");
                    item.Time += delay;
                    itemDB.Update(item);
                    interval.Sum += delay;
                    intervalDB.Update(interval);

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

        public event EventHandler Midnight;

        private void LookForMidnight()
        {
            if (((DateTime)Properties[g.lastTime]).Date != DateTime.Today)
            {
                int sum = itemDB.Sum();
                string date = ((DateTime)Properties[g.lastTime]).ToString(g.dateFormat);
                if (sumDB.Get(date).DatePK != null)
                    sumDB.Delete(date);
                sumDB.Add(new Sum { DatePK = date, Value = sum });

                intervalDB.DeleteAll();
                if (stopTimer == false)
                {
                    item.Time = 0;
                    interval.StartPK = DateTime.Now.ToString(g.timeFormat);
                    interval.Name = item.NamePK;
                    interval.Sum = 0;
                }
                itemDB.Clean();

                Midnight?.Invoke(null, EventArgs.Empty);
            }
            Properties[g.lastTime] = DateTime.Now;
        }

        public void AddItem(string name)
        {
            if (itemDB.Get(name).NamePK == null)
            {
                itemDB.Add(new Item() { NamePK = name });
                AddChooseLine(name);
            }
        }

        public void OnStartPage()
        {
            startPage.updateUI(!stopTimer, g.period * 60);
            MainPage = startPage;
        }

        public void FromChooseToStartPage(ChooseLine line)
        {
            chooseLine = line;
            stopTimer = false;
            startTime = DateTime.Now;

            interval.StartPK = startTime.ToString(g.timeFormat);
            interval.Name = chooseLine.Name;
            interval.Sum = 0;
            intervalDB.Add(interval);

            item = itemDB.Get(chooseLine.Name);
            preventInterval = 0;

            DependencyService.Get<IReminderService>().Remind(g.period * 60, "Got it!", "You finish " + chooseLine.Name + "!");
            OnStartPage();
        }

        public void OnChoosePage(object sender, EventArgs args)
        {
            if (chooseLine != null)
                chooseLine.Time = itemDB.Get(chooseLine.Name).Time;
            MainPage = choosePage;
        }
        public void OnStatisticPage(object sender, EventArgs args)
        {
            MainPage = new StatisticPage(intervalDB.GetAll());
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
