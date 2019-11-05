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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SchemeEditor s = new SchemeEditor();

            foreach (var l in s.Controls.OfType<Label>())
            {
                l.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SchemeEditor.showInfrastructure = cbShowInfrastructure.Checked;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            cbShowInfrastructure.Checked = !cbShowInfrastructure.Checked;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.BackColor = SchemeEditor.DefColor;
            }
            catch (Exception)
            {
            }
            cbShowInfrastructure.Checked = SchemeEditor.showInfrastructure;
            cbMyColor.Text = SchemeEditor.MyColor.Name;
            cbDefColor.Text = SchemeEditor.DefColor.Name;
            pSet.BackColor = SchemeEditor.MyColor;

            if (SchemeEditor.Lang == "En")
            {
                this.Text = "Settings";
                lblSet.Text = "Settings and customization";
                lblShowInfrastructure.Text = "Show information about infrastructure \nand routes in historical reference";
                lblMyColor.Text = "Main selection color:";
                lblMyColor.Text = "Additional selection color:";
                lblTime.Text = "Time to change photo on background:";
            }


            cbMyColor.Left = lblMyColor.Left + lblMyColor.Width;
            cbDefColor.Left = lblDefColor.Left + lblDefColor.Width;
        }

        private void cbMyColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SchemeEditor.MyColor = Color.FromName(cbMyColor.SelectedItem.ToString());

            pSet.BackColor = SchemeEditor.MyColor;
        }

        private void cbDefColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SchemeEditor.DefColor = Color.FromName(cbDefColor.SelectedItem.ToString());

            try
            {
                this.BackColor = SchemeEditor.DefColor;
            }
            catch (Exception)
            {
            }
            
        }

        private void cbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            int t = 0;
            switch (cbTime.SelectedIndex)
            {
                case 0:
                    t = 0;
                    break;
                case 1:
                    t = 15;
                    break;
                case 2:
                    t = 30;
                    break;
                case 3:
                    t = 60;
                    break;
                case 4:
                    t = 90;
                    break;
                case 5:
                    t = 120;
                    break;
                case 6:
                    t = 180;
                    break;
                default: t = 30;
                    break;
            }

            SchemeEditor.timeForBack = t;
        }
    }
}
