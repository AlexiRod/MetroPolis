using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metropolis
{
    public partial class HistoryForm : Form
    {
        public Station station;
        public List<PictureBox> images = new List<PictureBox>();

        public HistoryForm(Station s)
        {
            station = s;
            InitializeComponent();

            btnName.Text = station.Name;
            panel1.BackColor = station.Color;
            panel2.BackColor = station.Color;
            panel3.BackColor = station.Color;
            panel4.BackColor = station.Color;

        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1200, 600);
            this.AutoScroll = true;
            TextBox.Size = new Size(this.Width / 2 - 50, this.Height - TextBox.Top - 60);
            btnName.Left = (this.Width - btnName.Width) / 2;

            TextBox.ReadOnly = true;
            if (SchemeEditor.Lang == "En")
            {
                this.Text = "Historical information";
            }

            Point start = new Point(TextBox.Left + TextBox.Width + 5, TextBox.Top + 1);
            int width = this.Width - (TextBox.Left + TextBox.Width) - 10;
            int height = width / 7 * 4;

            station.Info = CheckForEmpty(station.Info, 'i');
            station.History = CheckForEmpty(station.History, 'h');
            station.Route = CheckForEmpty(station.Route, 'r');
            TextBox.Text = station.History;
            if (SchemeEditor.showInfrastructure)
                TextBox.Text += "\n\n" + station.Route;

            PictureBox lastPb = null;
            foreach (var item in station.Images)
            {
                PictureBox pb = new PictureBox()
                {
                    Location = start,
                    Width = width,
                    Height = height,
                    Visible = true,
                    BackgroundImage = item,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BorderStyle = BorderStyle.FixedSingle
                };
                start.Y += height + 5;

                lastPb = pb;
                Controls.Add(pb);
                images.Add(pb);
            }

            if (lastPb != null)
            {
                TextBox.Height = lastPb.Top + lastPb.Height - TextBox.Top;
            }
            this.Width += 55;
        }

        public string CheckForEmpty(string s, char c)
        {
            bool flag = false;

            for (int i = 0; i < s.Length; i++)
            {
                
                if (s[i] != ' ' && s[i] != '\r' && s[i] != '\n')
                {
                    flag = true;
                    break;
                }
                else
                {
                    s = s.Remove(i, 1);
                    i--;
                }
            }

            if (!flag)
            {
                if (SchemeEditor.Lang == "En")
                {
                    s = "There is no ";
                    if (c == 'i')
                        s += "actual information ";
                    if (c == 'h')
                        s += "actual historical information ";
                    if (c == 'r')
                        s += "actual information about infrastructure ";

                    s += "for this station.\r\n\r\n";
                }
                else
                {
                    s = "Нет актуальной ";
                    if (c == 'i')
                        s += "основной информации ";
                    if (c == 'h')
                        s += "исторической информации ";
                    if (c == 'r')
                        s += "информации об инфраструктуре ";

                    s += "для данной станции.\r\n\r\n";
                }
            }

            return s;
        }

    }
}
