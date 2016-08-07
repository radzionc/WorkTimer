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
        public event EventHandler StatisticLabelClicked;
        public event EventHandler ChooseLabelClicked;
        public event EventHandler StopLabelClicked;

        public void updateUI(bool timer, int left)
        {
            if (timer)
            {
                statisticLabel.IsEnabled = false;
                statisticLabel.IsVisible = false;
                slider.IsEnabled = false;
                slider.IsVisible = false;
                StopOrChoose.Text = "Stop";

                double complete = ((int)slider.Value - (double)left / 60) / (int)slider.Value;
                circularText.Update(complete);
            }
            else
            {
                statisticLabel.IsEnabled = true;
                statisticLabel.IsVisible = true;
                slider.IsEnabled = true;
                slider.IsVisible = true;
                StopOrChoose.Text = "Choose Job?";

                circularText.Update(1);
            }

            leftTime.Text = g.SecToStr(left);
        }

        public StartPage()
        {
            InitializeComponent();
            slider.Value = g.period; 
            updateUI(false, g.period * 60);
            statisticLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    StatisticLabelClicked?.Invoke(null, EventArgs.Empty);
                })
            });
            StopOrChoose.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    if(StopOrChoose.Text == g.stop)
                        StopLabelClicked?.Invoke(null, EventArgs.Empty);
                    else
                        ChooseLabelClicked?.Invoke(null, EventArgs.Empty);
                })
            });
        }

        public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            leftTime.Text = g.SecToStr((int)slider.Value * 60);
            g.period = (int)slider.Value;
        }
        
    }

}
