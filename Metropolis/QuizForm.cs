using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metropolis
{
    public partial class QuizForm : Form
    {
        public QuizForm()
        {
            InitializeComponent();
        }

        int time = 0;
        List<Image> back = new List<Image>();

        private void QuizForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            int halfx = this.Width / 2;
            int halfy = this.Height / 2;
            btn1.Left = halfx - btn1.Width - 8;
            btn3.Left = halfx + 8;
            btn2.Left = btn1.Left;
            btn4.Left = btn3.Left;
            lblQuestion.Left = halfx - lblQuestion.Width / 2;
            btnNext.Left = btn4.Left + btn4.Width - btnNext.Width;

            btn1.Top = halfy;
            btn3.Top = halfy;
            btn2.Top = btn1.Top + btn1.Height + 5;
            btn4.Top = btn3.Top + btn1.Height + 5;
            lblQuestion.Top = this.Height / 4;
            btnNext.Top = btn4.Top + 100;


            if (SchemeEditor.timeForBack != 0)
                timerBack.Enabled = true;

            Random random = new Random();
            back = SchemeEditor.back;

            this.BackgroundImage = back[random.Next(back.Count)];
            NextQuestion();
        }
        

        private void timerBack_Tick(object sender, EventArgs e)
        {
            time++;
            if (time > SchemeEditor.timeForBack)
            {
                Random random = new Random();
                this.BackgroundImage = back[random.Next(back.Count)];
                time = 0;
            }
        }


        private void btn_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
            System.Media.SoundPlayer sp1 = new System.Media.SoundPlayer(Properties.Resources.Wait);
            sp1.Play();
            Thread.Sleep(5500);
            btn1.Enabled = false;
            btn2.Enabled = false;
            btn3.Enabled = false;
            btn4.Enabled = false;

            
            if (b.Text != correct)
            {
                sp = new System.Media.SoundPlayer(Properties.Resources.WA);
                b.BackColor = Color.Orange;
                foreach (var item in Controls.OfType<Button>())
                {
                    if (item.Text == correct)
                    {
                        item.BackColor = Color.Lime;
                        break;
                    }
                }
                sp.Play();
            }
            else
            {
                b.BackColor = Color.Lime;
                sp = new System.Media.SoundPlayer(Properties.Resources.RA);
                sp.Play();
            }

            btnNext.Enabled = true;
            sp.Dispose();
            sp1.Dispose();
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = false;
            btn1.Enabled = true;
            btn2.Enabled = true;
            btn3.Enabled = true;
            btn4.Enabled = true;
            NextQuestion();
        }

        string correct = "";
        public void NextQuestion()
        {
            btnNext.Enabled = false;
            btn1.BackColor = SchemeEditor.DefColor;
            btn2.BackColor = SchemeEditor.DefColor;
            btn3.BackColor = SchemeEditor.DefColor;
            btn4.BackColor = SchemeEditor.DefColor;
            btnNext.BackColor = SchemeEditor.DefColor;

            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Read вопросов невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    Random r = new Random();
                    string plus;

                    if (SchemeEditor.Lang == "Ru")
                    {
                        lblQuestion.Text = "Вопрос:\n";
                        plus = r.Next(1, 31).ToString();
                    }
                    else
                    {
                        lblQuestion.Text = "Question:\n";
                        plus = r.Next(31, 51).ToString();
                    }




                    cmd.CommandText = "select question, v1, v2, v3, v4, correct from QuizTable where num = " + plus;
                    cmd.ExecuteNonQuery();
                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())
                        {
                            lblQuestion.Text += reader.GetString(0);
                            btn1.Text = reader.GetString(1);
                            btn2.Text = reader.GetString(2);
                            btn3.Text = reader.GetString(3);
                            btn4.Text = reader.GetString(4);
                            correct = reader.GetString(5);
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка чтения вопросов: " + ex.Message); }



        }

        private void btn_MouseEnter(object sender, EventArgs e) // персонализация 
        {
            Button btn = sender as Button;
            btn.BackColor = SchemeEditor.MyColor;
        }

        private void btn_MouseLeave(object sender, EventArgs e) // персонализация 
        {
            if (!btnNext.Enabled)
            {
                Button btn = sender as Button;
                btn.BackColor = SchemeEditor.DefColor;
            }
        }

    }
}
