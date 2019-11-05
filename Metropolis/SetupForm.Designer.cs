namespace Metropolis
{
    partial class SetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.ImpLines = new System.Windows.Forms.Button();
            this.ImpStations = new System.Windows.Forms.Button();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.ExpStations = new System.Windows.Forms.Button();
            this.EditModeCBox = new System.Windows.Forms.CheckBox();
            this.ImpStationsExpBt = new System.Windows.Forms.Button();
            this.St2LangRes = new System.Windows.Forms.Button();
            this.LangResImp = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ImpLines
            // 
            this.ImpLines.Location = new System.Drawing.Point(18, 18);
            this.ImpLines.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ImpLines.Name = "ImpLines";
            this.ImpLines.Size = new System.Drawing.Size(147, 35);
            this.ImpLines.TabIndex = 0;
            this.ImpLines.Text = "Импорт линий";
            this.ImpLines.UseVisualStyleBackColor = true;
            this.ImpLines.Click += new System.EventHandler(this.ImpLines_Click);
            // 
            // ImpStations
            // 
            this.ImpStations.Location = new System.Drawing.Point(18, 65);
            this.ImpStations.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ImpStations.Name = "ImpStations";
            this.ImpStations.Size = new System.Drawing.Size(147, 35);
            this.ImpStations.TabIndex = 1;
            this.ImpStations.Text = "Импорт станций";
            this.ImpStations.UseVisualStyleBackColor = true;
            this.ImpStations.Click += new System.EventHandler(this.ImpStations_click);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(112, 625);
            this.ProgressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(0, 20);
            this.ProgressLabel.TabIndex = 2;
            // 
            // ExpStations
            // 
            this.ExpStations.Location = new System.Drawing.Point(18, 109);
            this.ExpStations.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ExpStations.Name = "ExpStations";
            this.ExpStations.Size = new System.Drawing.Size(147, 38);
            this.ExpStations.TabIndex = 3;
            this.ExpStations.Text = "Эксп.станций";
            this.ExpStations.UseVisualStyleBackColor = true;
            this.ExpStations.Click += new System.EventHandler(this.ExpStations_Click_1);
            // 
            // EditModeCBox
            // 
            this.EditModeCBox.AutoSize = true;
            this.EditModeCBox.Location = new System.Drawing.Point(227, 29);
            this.EditModeCBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EditModeCBox.Name = "EditModeCBox";
            this.EditModeCBox.Size = new System.Drawing.Size(159, 24);
            this.EditModeCBox.TabIndex = 4;
            this.EditModeCBox.Text = "Редактор карты";
            this.EditModeCBox.UseVisualStyleBackColor = true;
            this.EditModeCBox.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // ImpStationsExpBt
            // 
            this.ImpStationsExpBt.Location = new System.Drawing.Point(18, 157);
            this.ImpStationsExpBt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ImpStationsExpBt.Name = "ImpStationsExpBt";
            this.ImpStationsExpBt.Size = new System.Drawing.Size(147, 54);
            this.ImpStationsExpBt.TabIndex = 5;
            this.ImpStationsExpBt.Text = "Расширенный импорт станций";
            this.ImpStationsExpBt.UseVisualStyleBackColor = true;
            this.ImpStationsExpBt.Click += new System.EventHandler(this.ImpStationsExpBt_Click);
            // 
            // St2LangRes
            // 
            this.St2LangRes.Location = new System.Drawing.Point(18, 289);
            this.St2LangRes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.St2LangRes.Name = "St2LangRes";
            this.St2LangRes.Size = new System.Drawing.Size(147, 58);
            this.St2LangRes.TabIndex = 6;
            this.St2LangRes.Text = "Cтанции в яз ресурсы";
            this.St2LangRes.UseVisualStyleBackColor = true;
            this.St2LangRes.Click += new System.EventHandler(this.St2LangRes_Click);
            // 
            // LangResImp
            // 
            this.LangResImp.Location = new System.Drawing.Point(18, 221);
            this.LangResImp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LangResImp.Name = "LangResImp";
            this.LangResImp.Size = new System.Drawing.Size(147, 58);
            this.LangResImp.TabIndex = 7;
            this.LangResImp.Text = "Расш.имп яз ресурсов";
            this.LangResImp.UseVisualStyleBackColor = true;
            this.LangResImp.Click += new System.EventHandler(this.LangResImp_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Metropolis.Properties.Resources.Настройки;
            this.pictureBox1.Location = new System.Drawing.Point(193, 118);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(240, 229);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(445, 365);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LangResImp);
            this.Controls.Add(this.St2LangRes);
            this.Controls.Add(this.ImpStationsExpBt);
            this.Controls.Add(this.EditModeCBox);
            this.Controls.Add(this.ExpStations);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.ImpStations);
            this.Controls.Add(this.ImpLines);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SetupForm";
            this.Text = "Режим разработчика";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ImpLines;
        private System.Windows.Forms.Button ImpStations;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Button ExpStations;
        private System.Windows.Forms.CheckBox EditModeCBox;
        private System.Windows.Forms.Button ImpStationsExpBt;
        private System.Windows.Forms.Button St2LangRes;
        private System.Windows.Forms.Button LangResImp;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}