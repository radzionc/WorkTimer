using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public partial class ChooseLine : StackLayout
    {
        private int time;

        public int Time
        {
            get{ return time; }
            set
            {
                time = value;
                
                if (time > 0)
                {
                    deleteButton.BackgroundColor = Color.Transparent;
                    deleteButton.BorderColor = Color.Transparent;
                    deleteButton.IsEnabled = false;
                    deleteButton.Text = g.strToSec(time);
                }
                else
                {
                    deleteButton.BackgroundColor = Color.Default;
                    deleteButton.BorderColor = Color.Default;
                    deleteButton.Text = "delete";
                    deleteButton.IsEnabled = true;
                }
            }
        }

        public string Name { get; private set; }
        public ChooseLine(string name, int time)
        {
            InitializeComponent();
            Time = time;
            Name = name;
            nameLabel.Text = Name;
        }

        public event EventHandler StartButtonClick;
        public void StartButtonClicked(object sender, EventArgs args)
        {
            //BackgroundColor = Color.Lime;
            StartButtonClick?.Invoke(this, args);
        }
        
        public event EventHandler DeleteButtonClick;
        public void DeleteButtonClicked(object sender, EventArgs args)
        {
            DeleteButtonClick?.Invoke(this, args);
        }
    }
}
