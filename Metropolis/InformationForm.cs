using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metropolis
{
    public partial class InformationForm : Form
    {
        public Station station;

        public InformationForm(Station s)
        {
            station = s;
            InitializeComponent();

            btnName.Text = station.Name;
            panel1.BackColor = station.Color;
            panel2.BackColor = station.Color;
            panel3.BackColor = station.Color;
            panel4.BackColor = station.Color;
            btnInfo.BackColor = station.Color;
            
        }

        public List<PictureBox> images = new List<PictureBox>();

        private void InformationForm_Load(object sender, EventArgs e)
        {
            TextBox.ReadOnly = true;
            btnName.BackColor = SchemeEditor.DefColor;
            if (SchemeEditor.Lang == "En")
            {
                this.Text = "Information";
                btnInfo.Text = "Information";
                btnHistory.Text = "History";
                btnRoute.Text = "Infrastructure";
                btnImage.Text = "Image";
            }

            Point start = new Point(TextBox.Left, TextBox.Top + 5);
            int width = TextBox.Width - 10;
            int height = width / 7 *4;

            station.Info = CheckForEmpty(station.Info, btnInfo);
            station.History = CheckForEmpty(station.History, btnHistory);
            station.Route = CheckForEmpty(station.Route, btnRoute);
            TextBox.Text = station.Info;

            foreach (var item in station.Images)
            {
                PictureBox pb = new PictureBox()
                {
                    Location = start,
                    Width = width,
                    Height = height,
                    Visible = false,
                    BackgroundImage = item,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Left = start.X,
                    BorderStyle = BorderStyle.FixedSingle
                };
                start.Y += height + 5;

                Controls.Add(pb);
                images.Add(pb);
            }
        }


        public string CheckForEmpty(string s, Button b)
        {
            bool flag = false;

            for (int i = 0; i < s.Length; i++)
            {
                char a = s[i];
                //Debug.WriteLine(s[i]);
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
                b.Enabled = false;
                s = "";
            }

            return s;
        }


        private void btnName_MouseEnter(object sender, EventArgs e)
        {
            btnName.BackColor = SchemeEditor.MyColor;
        }

        private void btnName_MouseLeave(object sender, EventArgs e)
        {
            btnName.BackColor = SchemeEditor.DefColor;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            TextBox.Visible = true;
            btnInfo.BackColor = station.Color;
            TextBox.Text = station.Info;
            TextBox.SelectionStart = 0;

            btnHistory.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnRoute.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            TextBox.Visible = true;
            btnHistory.BackColor = station.Color;
            TextBox.Text = station.History;
            TextBox.SelectionStart = 0;

            btnInfo.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnRoute.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnRoute_Click(object sender, EventArgs e)
        {
            TextBox.Visible = true;
            btnRoute.BackColor = station.Color;
            TextBox.Text = station.Route;
            TextBox.SelectionStart = 0;

            btnInfo.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnHistory.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            TextBox.Visible = false;
            btnImage.BackColor = station.Color;

            btnInfo.BackColor = Color.White;
            btnHistory.BackColor = Color.White;
            btnRoute.BackColor = Color.White;
            TextBox.Text = "";

            foreach (var item in images)
            {
                item.Visible = true;
                item.BringToFront();
            }

            
        }

        
    }

       
    
}
