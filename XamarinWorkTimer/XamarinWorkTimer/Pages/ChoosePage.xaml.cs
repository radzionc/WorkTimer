using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinWorkTimer;

using Xamarin.Forms;
using XamarinWorkTimer.Pages.Elements;
using XamarinWorkTimer.DataBase;

namespace XamarinWorkTimer.Pages
{
    public partial class ChoosePage : ContentPage
    {
        private const int maxInputLength = 20;
        public ChoosePage()
        {
            InitializeComponent();
            foreach (Item item in g.itemDB.GetAll())
                AddLine(item.NamePK);
        }

        public event EventHandler LineClicked;
        public void OnTextChanged(object sender, EventArgs e)
        {
            if((entry.Text).Length > maxInputLength)
                entry.Text = (entry.Text).Remove(maxInputLength);
        }

        private void EntryInputCompleted(object sender, EventArgs e)
        {
            if (g.itemDB.Get(entry.Text).NamePK == null)
            {
                g.itemDB.Add(new Item { NamePK = entry.Text });
                AddLine(entry.Text);
            }
            entry.Text = string.Empty;
        }

        void AddLine(string name)
        {
            ChooseLine newLine = new ChooseLine(name);
            newLine.LineClicked += (object sender, EventArgs args) =>
            {
                LineClicked.Invoke(sender, EventArgs.Empty);
            };
            newLine.XClicked += delegate
            {
                itemsLayout.Children.Remove(newLine);
                g.itemDB.Delete(name);
            };
            itemsLayout.Children.Add(newLine);
        }
    }
}
