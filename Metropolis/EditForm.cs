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
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbLogin.Text == "Admin" && tbPassword.Text == "123")
            {
                SchemeEditor.isEditor = true;
                if (SchemeEditor.Lang == "En")
                    MessageBox.Show("Developer mode is now available to you!");
                else
                    MessageBox.Show("Теперь вам доступен режим разработчика!");
            }
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            btnAccept.BackColor = SchemeEditor.DefColor;

            if (SchemeEditor.Lang == "En")
            {
                lblLogin.Text = "Login";
                lblPassword.Text = "Password";
                btnAccept.Text = "Accept";
            }

        }

        private void btnAccept_MouseEnter(object sender, EventArgs e)
        {
            btnAccept.BackColor = SchemeEditor.MyColor;
        }

        private void btnAccept_MouseLeave(object sender, EventArgs e)
        {
            btnAccept.BackColor = SchemeEditor.DefColor;
        }
    }
}
