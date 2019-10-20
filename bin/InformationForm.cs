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
            TextBox.Text = station.Info;
        }

        public List<PictureBox> images = new List<PictureBox>();

        private void InformationForm_Load(object sender, EventArgs e)
        {
            Point start = new Point(TextBox.Left + 5, TextBox.Top + 5);
            int width = TextBox.Width - 10;
            int height = width / 7 *4;



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
                    BorderStyle = BorderStyle.FixedSingle
                };
                start.Y += height + 5;

                Controls.Add(pb);
                images.Add(pb);
            }
        }


        private void btnInfo_Click(object sender, EventArgs e)
        {
            btnInfo.BackColor = station.Color;
            TextBox.Text = station.Info;

            btnHistory.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnRoute.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            btnHistory.BackColor = station.Color;
            TextBox.Text = station.History;
            
            btnInfo.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnRoute.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnRoute_Click(object sender, EventArgs e)
        {
            btnRoute.BackColor = station.Color;
            TextBox.Text = station.Route;

            btnInfo.BackColor = Color.White;
            btnImage.BackColor = Color.White;
            btnHistory.BackColor = Color.White;

            foreach (var item in images)
                item.Visible = false;
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
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
