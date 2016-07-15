using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer.Pages
{
    public partial class StartPage : ContentPage
    {
        Manager manager;
        
        private int summaryTimeValue;
        public int SummaryTime
        {
            set
            {
                summaryTime.Text = gf.FromSecondsToString(value);
                summaryTimeValue = value;
            }
            get
            {
                return summaryTimeValue;
            }
        }

        private int leftTimeValue;
        public int LeftTime
        {
            set
            {
                leftTimeValue = value;
                if (leftTimeValue < 0)  leftTimeValue = 0;
                leftTime.Text = gf.FromSecondsToString(leftTimeValue);
                if (leftTimeValue == 0) StopButtonClicked(null, EventArgs.Empty);
            }
            get
            {
                return leftTimeValue;
            }
        }

        private bool stopTimer = true;
        public bool StopTimer
        {
            get
            {
                return stopTimer;
            }
            set
            {
                stopTimer = value;

                if (StopTimer == false)
                {
                    chooseButton.IsEnabled = false;
                    statisticButton.IsEnabled = false;
                    slider.IsEnabled = false;

                    stopButton.IsEnabled = true;
                    pauseButton.IsEnabled = true;
                }
                if (StopTimer == true && pauseButton.Text == "pause")
                {
                    chooseButton.IsEnabled = true;
                    statisticButton.IsEnabled = true;
                    slider.IsEnabled = true;

                    stopButton.IsEnabled = false;
                    pauseButton.IsEnabled = false;
                }
            }
        }
     

        public StartPage(Manager pageManager)
        {
            InitializeComponent();
            StopTimer = true;
            this.manager = pageManager;
            slider.Value = (int)App.Current.Properties[gf.slider] + 1;

            manager.Midnight += (object sender, EventArgs args) =>
            {
                SummaryTime = 0;
            };
        }

        public void StatisticButtonClicked(object sender, EventArgs args)
        {
            
        }

        public void ChooseButtonClicked(object sender, EventArgs args)
        {
            manager.OnChoosePage();
        }

        public void PauseButtonClicked(object sender, EventArgs args)
        {
            if (StopTimer == true)
            {
                pauseButton.Text = gf.pause;
                manager.StartTime += DateTime.Now - manager.PauseTime;
            }
            else
            {
                pauseButton.Text = gf.resume;
                manager.PauseTime = DateTime.Now;
            }
            StopTimer = !StopTimer;
        }

        public void StopButtonClicked(object sender, EventArgs args)
        {
            pauseButton.Text = gf.pause;
            LeftTime = (int)slider.Value * 60;
            StopTimer = true;
        }

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            leftTime.Text = gf.FromSecondsToString((int)slider.Value * 60);
            LeftTime = (int)slider.Value * 60;
            App.Current.Properties[gf.slider] = (int)slider.Value;
        }
        
    }

}
