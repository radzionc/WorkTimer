using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinWorkTimer;

namespace XamarinWorkTimer.Pages
{
    public partial class StartPage : ContentPage
    {
        private int summaryTimeValue;
        private int leftTimeValue;
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
        public int LeftTime
        {
            set
            {
                leftTimeValue = value;
                leftTime.Text = gf.FromSecondsToString(leftTimeValue);
            }
            get
            {
                return leftTimeValue;
            }
        }


        public event EventHandler StatisticButtonClicked;
        public event EventHandler ChooseButtonClicked;
        public event EventHandler PauseButtonClicked;
        public event EventHandler StopButtonClicked;

        public void TimerUI()
        {
            chooseButton.IsEnabled = false;
            statisticButton.IsEnabled = false;
            slider.IsEnabled = false;

            stopButton.IsEnabled = true;
            pauseButton.IsEnabled = true;
        }

        public void NoTimerUI()
        {
            chooseButton.IsEnabled = true;
            statisticButton.IsEnabled = true;
            slider.IsEnabled = true;

            stopButton.IsEnabled = false;
            pauseButton.IsEnabled = false;

            pauseButton.Text = gf.pause;
            LeftTime = (int)slider.Value * 60;
        }

        public StartPage()
        {
            InitializeComponent();
            NoTimerUI();
            slider.Value = (int)App.Current.Properties[gf.slider] + 1;       
        }

        public void OnStatisticButtonClicked(object sender, EventArgs args)
        {
            StatisticButtonClicked?.Invoke(null, EventArgs.Empty);
        }
                    
        public void OnChooseButtonClicked(object sender, EventArgs args)
        {
            ChooseButtonClicked?.Invoke(null, EventArgs.Empty);
        }

        public void OnPauseButtonClicked(object sender, EventArgs args)
        {
            pauseButton.Text = (pauseButton.Text == gf.pause) ? gf.resume : gf.pause;
            PauseButtonClicked?.Invoke(null, EventArgs.Empty);
        }

        public void OnStopButtonClicked(object sender, EventArgs args)
        {
            StopButtonClicked?.Invoke(null, EventArgs.Empty);
        }

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            LeftTime = (int)slider.Value * 60;
            App.Current.Properties[gf.slider] = (int)slider.Value;
        }
        
    }

}
