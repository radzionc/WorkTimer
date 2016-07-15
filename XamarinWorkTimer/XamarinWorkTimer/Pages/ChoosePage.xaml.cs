using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer.Pages
{
    public partial class ChoosePage : ContentPage
    {
        Manager manager;
        private const int maxInputLength = 12;
        public ChoosePage(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        public void AddLine(string name, int time = 0)
        {
            ChooseLine newLine = new ChooseLine(name, time);
            manager.AddEventsOnLine(newLine);
            manager.Midnight += (object sender, EventArgs args) =>
            {
                newLine.Time = 0;
            };
            newLine.DeleteButtonClick += (object sender, EventArgs args) =>
            {
                itemsLayout.Children.Remove(newLine);
            };
            itemsLayout.Children.Add(newLine);
        }

        public void OnTextChanged(object sender, EventArgs e)
        {
            if((entry.Text).Length > maxInputLength)
                entry.Text = (entry.Text).Remove(maxInputLength);
        }

        private void EntryInputCompleted(object sender, EventArgs e)
        {
            manager.AddItem(entry.Text);
            entry.Text = string.Empty;
        }

    }
}
