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
        public event EventHandler StatisticButtonClicked;
        public event EventHandler ChooseButtonClicked;
        public event EventHandler StopButtonClicked;
        public event EventHandler SliderValueChanged;
        
        public void updateUI(bool timer, int summary, int left)
        {
            if (timer)
            {
                chooseButton.IsEnabled = false;
                statisticButton.IsEnabled = false;
                slider.IsEnabled = false;
                stopButton.IsEnabled = true;
                double complete = ((int)slider.Value - (double)left / 60) / (int)slider.Value;
                stopButton.Text = slider.Value.ToString() + " : " + left.ToString() + " : " + complete.ToString();
                circularText.Update(complete);
            }
            else
            {
                chooseButton.IsEnabled = true;
                statisticButton.IsEnabled = true;
                slider.IsEnabled = true;
                stopButton.IsEnabled = false;
                circularText.Update(1);
            }
            summaryTime.Text = g.strToSec(summary);
            leftTime.Text = g.strToSec(left);
        }

        public StartPage(int summary, int sliderValue)
        {
            InitializeComponent();
            slider.Value = sliderValue;
            updateUI(false, summary, sliderValue * 60);   
        }

        public void OnStatisticButtonClicked(object sender, EventArgs args)
        {
            StatisticButtonClicked?.Invoke(null, EventArgs.Empty);
        }
                    
        public void OnChooseButtonClicked(object sender, EventArgs args)
        {
            ChooseButtonClicked?.Invoke(null, EventArgs.Empty);
        }

        public void OnStopButtonClicked(object sender, EventArgs args)
        {
            StopButtonClicked?.Invoke(null, EventArgs.Empty);
        }

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            SliderValueChanged?.Invoke((int)slider.Value, EventArgs.Empty);
            leftTime.Text = g.strToSec((int)slider.Value * 60);
        }
        
    }

}
