using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamarinWorkTimer.Pages.Elements
{
    public class CircularText : AbsoluteLayout
    {
        Label[] labels;
        const int length = 50;
        string text = new string('|', length);
        public void Update(double complete)
        {
            int len = (int)Math.Ceiling(complete * length);
            text = new string('|', len) + new string(' ', length - len);
            for (int index = 0; index < text.Length; index++)
            {
                labels[index].Text = text[index].ToString();
            }
        }
        public CircularText()
        {
            SizeChanged += (sender, args) =>
            {
                LayOutLabels();
            };
            labels = new Label[text.Length];
            double fontSize = 32;
            int countSized = 0;
            for (int index = 0; index < text.Length; index++)
            {
                char ch = text[index];
                Label label = new Label
                {
                    Text = ch == ' ' ? "-" : ch.ToString(),
                    Opacity = ch == ' ' ? 0 : 1,
                    FontSize = fontSize,
                };
                label.SizeChanged += (sender, args) =>
                {
                    if (++countSized >= labels.Length)
                        LayOutLabels();
                };
                labels[index] = label;
                Children.Add(label);
            }
        }
        void LayOutLabels()
        {
            // Calculate the total width of the Labels.
            double totalWidth = 0;
            foreach (Label label in labels)
            {
                totalWidth += label.Width;
            }
            // From that, get a radius of the circle to center of Labels.
            double radius = totalWidth / 2 / Math.PI + labels[0].Height / 2;
            Point center = new Point(Width / 2, Height / 2);
            double angle = 0;
            for (int index = 0; index < labels.Length; index++)
            {
                Label label = labels[index];
                // Set the position of the Label.
                double x = center.X + radius * Math.Sin(angle) - label.Width / 2;
                double y = center.Y - radius * Math.Cos(angle) - label.Height / 2;
                SetLayoutBounds(label, new Rectangle(x, y, AutoSize, AutoSize));
                // Set the rotation of the Label.
                label.Rotation = 360 * angle / 2 / Math.PI;
                // Increment the rotation angle.
                if (index < labels.Length - 1)
                {
                    angle += 2 * Math.PI * (label.Width + labels[index + 1].Width) / 2 / totalWidth;
                }
            }
        }
    }
}
