using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinWorkTimer;
using Xamarin.Forms;
using XamarinWorkTimer.Pages.Elements;

namespace XamarinWorkTimer.Pages
{
    public partial class ChoosePage : ContentPage
    {
        private const int maxInputLength = 12;
        public ChoosePage()
        {
            InitializeComponent();
        }

        public event EventHandler InputCompleted;

        public void AddLine(ChooseLine chooseLine)
        {
            chooseLine.DeleteButtonClick += delegate { itemsLayout.Children.Remove(chooseLine); };
            itemsLayout.Children.Add(chooseLine);
        }
        public void OnTextChanged(object sender, EventArgs e)
        {
            if((entry.Text).Length > maxInputLength)
                entry.Text = (entry.Text).Remove(maxInputLength);
        }

        private void EntryInputCompleted(object sender, EventArgs e)
        {
            InputCompleted.Invoke(entry.Text, EventArgs.Empty);
            entry.Text = string.Empty;
        }

    }
}
