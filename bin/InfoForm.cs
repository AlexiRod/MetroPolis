using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace var3
{
    public partial class InfoForm : Form
    {
        private Button button1;

        public InfoForm(Metropolis.Station station)
        {
            InitializeComponent();

            btnName.Text = station.Name;
            if (station.Images != null)
            {
                int  i = station.Images.Count;
                Image i1  = null, i2 = null;
                bool b_FileFound = true;
                if (i > 0) LineLogo.Image = station.Images[0];
                try
                {   // создадим образ из файла в поддиректории IMG 1_ - код москвы Расширение jpg. _1 в конце - порядковый номер
                    i1 = Image.FromFile(".\\img\\1_" + station.Button.st_line_id.ToString() + "_"+
                        station.Button.st_id.ToString() + "_1.jpg");
                }
                catch(System.IO.FileNotFoundException e)
                    { b_FileFound = false; }
                if (b_FileFound)   // если первый файл был найден, то поищем второй
                {
                    try
                    {   // создадим образ из файла в поддиректории IMG 1_ - код москвы Расширение jpg. _1 в конце - порядковый номер
                        i2 = Image.FromFile(".\\img\\1_" + station.Button.st_line_id.ToString() + "_" +
                            station.Button.st_id.ToString() + "_2.jpg");
                    }
                    catch (System.IO.FileNotFoundException e)
                    { b_FileFound = false; }
                }
                if (i1 != null) pB1.Image = i1;
                if (i2 != null) pB2.Image = i2;
                //    if (i > 1) pB1.Image = station.Images[1];
                //    if (i > 2) pB2.Image = station.Images[2]; 
            }
            panel1.BackColor = station.Color;
            panel2.BackColor = station.Color;
            panel3.BackColor = station.Color;
            panel4.BackColor = station.Color;
            if (true/*language == Rus*/)
                TextBox.Text = station.RusText;
            else
                TextBox.Text = station.EngText;
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            if (pB1 != null) pB1.Top = TextBox.Top + TextBox.Height + 5;
            if (pB2 != null && pB1 != null) pB2.Top = pB1.Top;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            //using (Graphics graphics = ((RichTextBox)sender).CreateGraphics())
            //{
            //    ((RichTextBox)sender).Width = TextRenderer.MeasureText(((RichTextBox)sender).Text, ((RichTextBox)sender).Font).Width;
            //    ((RichTextBox)sender).Height = TextRenderer.MeasureText(((RichTextBox)sender).Text, ((RichTextBox)sender).Font).Height;
            //}
          
        }

 
           

    

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InfoForm
            // 
            this.ClientSize = new System.Drawing.Size(176, 176);
            this.Name = "InfoForm";
            this.Load += new System.EventHandler(this.InfoForm_Load_1);
            this.ResumeLayout(false);

        }

        private void InfoForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
